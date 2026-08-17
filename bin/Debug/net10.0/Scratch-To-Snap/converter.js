/*
 * Scratch (.sb3) -> Snap! (.xml) converter.
 *
 * Works in both:
 *   - Browsers  (loads JSZip from a <script> tag; exposes window.ScratchToSnap)
 *   - Node.js   (require("jszip"); exports module.exports)
 *
 * Public API:
 *   ScratchToSnap.convert(arrayBuffer)  -> Promise<{ xml, warnings }>
 *
 * Only .sb3 is supported (the modern Scratch format). .sb / .sb2 need
 * scratch-vm to upconvert; keep this file dependency-light and skip them.
 */

(function (root, factory) {
  if (typeof module === "object" && module.exports) {
    module.exports = factory(require("jszip"));
  } else {
    root.ScratchToSnap = factory(root.JSZip);
  }
})(typeof self !== "undefined" ? self : this, function (JSZip) {
  "use strict";

  // =========================================================================
  // 1. XML builder
  // =========================================================================

  function XmlNode(tag, attrs, children) {
    this.tag = tag;
    this.attrs = attrs || {};
    this.children = children || [];
  }
  XmlNode.prototype.add = function (child) {
    this.children.push(child);
    return this;
  };
  XmlNode.prototype.toString = function (indent) {
    indent = indent || 0;
    var pad = repeat("  ", indent);
    var attrStr = "";
    for (var k in this.attrs) {
      var v = this.attrs[k];
      if (v === undefined || v === null) continue;
      attrStr += " " + k + '="' + escapeAttr(String(v)) + '"';
    }
    if (this.children.length === 0) return pad + "<" + this.tag + attrStr + "/>";
    if (this.children.length === 1 && typeof this.children[0] === "string") {
      return pad + "<" + this.tag + attrStr + ">" + escapeText(this.children[0]) + "</" + this.tag + ">";
    }
    var parts = [];
    for (var i = 0; i < this.children.length; i++) {
      var c = this.children[i];
      if (typeof c === "string") parts.push(repeat("  ", indent + 1) + escapeText(c));
      else parts.push(c.toString(indent + 1));
    }
    return pad + "<" + this.tag + attrStr + ">\n" + parts.join("\n") + "\n" + pad + "</" + this.tag + ">";
  };

  function el(tag, attrs) {
    var node = new XmlNode(tag, attrs || {}, []);
    for (var i = 2; i < arguments.length; i++) node.children.push(arguments[i]);
    return node;
  }
  function repeat(s, n) { var out = ""; for (var i = 0; i < n; i++) out += s; return out; }
  function escapeAttr(s) {
    return s.replace(/&/g, "&amp;").replace(/"/g, "&quot;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  }
  function escapeText(s) {
    return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  }

  // =========================================================================
  // 2. Base64 (works in both environments)
  // =========================================================================

  function bytesToBase64(bytes) {
    if (typeof Buffer !== "undefined") return Buffer.from(bytes).toString("base64");
    var binary = "";
    var chunk = 0x8000;
    for (var i = 0; i < bytes.length; i += chunk) {
      binary += String.fromCharCode.apply(null, bytes.subarray(i, i + chunk));
    }
    return btoa(binary);
  }

  var MIME_BY_EXT = {
    svg: "image/svg+xml", png: "image/png", jpg: "image/jpeg", jpeg: "image/jpeg",
    gif: "image/gif", bmp: "image/bmp", wav: "audio/wav", mp3: "audio/mpeg",
  };

  // =========================================================================
  // 3. SB3 parser -> intermediate representation (IR)
  // =========================================================================

  async function fileToDataUrl(zip, name) {
    var file = zip.file(name);
    if (!file) return "";
    var ext = (name.split(".").pop() || "").toLowerCase();
    var mime = MIME_BY_EXT[ext] || "application/octet-stream";
    var buf = await file.async("uint8array");
    return "data:" + mime + ";base64," + bytesToBase64(buf);
  }

  async function readSvgViewBoxOffset(zip, name) {
    var file = zip.file(name);
    if (!file) return null;
    var text = await file.async("string");
    var m = text.match(/viewBox\s*=\s*["']\s*([-\d.eE]+)\s+([-\d.eE]+)\s+([-\d.eE]+)\s+([-\d.eE]+)/);
    if (!m) return null;
    var x = parseFloat(m[1]), y = parseFloat(m[2]);
    if (!isFinite(x) || !isFinite(y)) return null;
    return { x: x, y: y };
  }

  async function parseSb3(arrayBuffer) {
    var zip = await JSZip.loadAsync(arrayBuffer);
    var projectFile = zip.file("project.json");
    if (!projectFile) throw new Error("Missing project.json in .sb3");
    var json = JSON.parse(await projectFile.async("string"));

    var targets = [];
    for (var ti = 0; ti < json.targets.length; ti++) {
      var t = json.targets[ti];

      var costumes = [];
      for (var ci = 0; ci < t.costumes.length; ci++) {
        var c = t.costumes[ci];
        var dataUrl = await fileToDataUrl(zip, c.md5ext);
        var ext = (c.md5ext.split(".").pop() || "").toLowerCase();
        var rcx = c.rotationCenterX || 0;
        var rcy = c.rotationCenterY || 0;
        if (ext === "svg") {
          var offset = await readSvgViewBoxOffset(zip, c.md5ext);
          if (offset) { rcx -= offset.x; rcy -= offset.y; }
        }
        costumes.push({ name: c.name, dataUrl: dataUrl, rotationCenterX: rcx, rotationCenterY: rcy });
      }

      var sounds = [];
      for (var si = 0; si < t.sounds.length; si++) {
        var s = t.sounds[si];
        sounds.push({ name: s.name, dataUrl: await fileToDataUrl(zip, s.md5ext) });
      }

      var variables = [];
      for (var vk in (t.variables || {})) {
        var v = t.variables[vk];
        variables.push({ name: v[0], value: v[1] });
      }
      var lists = [];
      for (var lk in (t.lists || {})) {
        var l = t.lists[lk];
        lists.push({ name: l[0], items: l[1] });
      }

      var blocks = t.blocks || {};
      var scripts = parseScripts(blocks);

      targets.push({
        name: t.name, isStage: !!t.isStage,
        x: t.x || 0, y: t.y || 0, direction: t.direction == null ? 90 : t.direction,
        size: t.size == null ? 100 : t.size, visible: t.visible !== false,
        draggable: !!t.draggable, rotationStyle: t.rotationStyle || "all around",
        costumes: costumes, currentCostume: t.currentCostume || 0,
        sounds: sounds, variables: variables, lists: lists, scripts: scripts,
      });
    }

    var stage = null, sprites = [];
    for (var i = 0; i < targets.length; i++) {
      if (targets[i].isStage) stage = targets[i];
      else sprites.push(targets[i]);
    }
    return { stage: stage, sprites: sprites, warnings: [] };
  }

  function parseScripts(blocks) {
    var scripts = [];
    for (var id in blocks) {
      var b = blocks[id];
      if (!b || Array.isArray(b)) continue;
      if (!b.topLevel) continue;
      if (b.shadow) continue;
      var stack = buildStack(id, blocks);
      scripts.push({ x: b.x || 0, y: b.y || 0, blocks: stack });
    }
    return scripts;
  }

  function buildStack(startId, blocks) {
    var out = [], cur = startId, seen = {};
    while (cur && !seen[cur]) {
      seen[cur] = true;
      var b = blocks[cur];
      if (!b || Array.isArray(b)) break;
      out.push(buildBlockIR(cur, blocks));
      cur = b.next;
    }
    return out;
  }

  function buildBlockIR(id, blocks) {
    var b = blocks[id];
    var inputs = {}, branches = {};
    for (var name in (b.inputs || {})) {
      var raw = b.inputs[name];
      if (name === "SUBSTACK" || name === "SUBSTACK2") {
        var bid = extractBlockId(raw);
        branches[name] = bid ? buildStack(bid, blocks) : [];
        continue;
      }
      inputs[name] = extractInputValue(raw, blocks);
    }
    var fields = {};
    for (var fname in (b.fields || {})) {
      var fraw = b.fields[fname];
      if (Array.isArray(fraw) && fraw.length > 0) fields[fname] = String(fraw[0] == null ? "" : fraw[0]);
    }
    var mutation;
    if (b.mutation) {
      mutation = {
        proccode: b.mutation.proccode || "",
        argumentIds: safeJsonArray(b.mutation.argumentids),
        argumentNames: safeJsonArray(b.mutation.argumentnames),
        argumentDefaults: safeJsonArray(b.mutation.argumentdefaults),
        warp: b.mutation.warp === true || b.mutation.warp === "true",
      };
    }
    return { opcode: b.opcode, inputs: inputs, fields: fields, branches: branches, mutation: mutation };
  }

  function safeJsonArray(s) {
    if (!s) return [];
    try { var v = JSON.parse(s); return Array.isArray(v) ? v.map(String) : []; }
    catch (e) { return []; }
  }
  function extractBlockId(raw) {
    if (!Array.isArray(raw)) return null;
    return typeof raw[1] === "string" ? raw[1] : null;
  }
  function extractInputValue(raw, blocks) {
    if (!Array.isArray(raw)) return "";
    var inner = raw[1];
    if (typeof inner === "string" && blocks[inner]) {
      var child = blocks[inner];
      if (child.shadow && isMenuOpcode(child.opcode)) {
        var keys = Object.keys(child.fields || {});
        if (keys.length && Array.isArray(child.fields[keys[0]])) {
          return menuValue(String(child.fields[keys[0]][0] == null ? "" : child.fields[keys[0]][0]));
        }
      }
      return buildBlockIR(inner, blocks);
    }
    if (Array.isArray(inner)) return inlineValue(inner);
    return "";
  }
  function inlineValue(value) {
    var type = Number(value[0]), payload = value[1];
    if (type === 12) return { kind: "variable", name: String(payload == null ? "" : payload) };
    if (type === 13) return { kind: "list", name: String(payload == null ? "" : payload) };
    return String(payload == null ? "" : payload);
  }
  function menuValue(value) {
    return value.indexOf("_") === 0 ? { kind: "special", name: value } : value;
  }
  function isMenuOpcode(op) {
    if (/_menu$/.test(op)) return true;
    return [
      "looks_costume", "looks_backdrops", "sound_sounds_menu",
      "sensing_touchingobjectmenu", "sensing_distancetomenu", "sensing_keyoptions",
      "sensing_of_object_menu", "motion_goto_menu", "motion_glideto_menu",
      "motion_pointtowards_menu", "control_create_clone_of_menu",
      "pen_menu_colorParam", "music_menu_DRUM", "music_menu_INSTRUMENT",
      "videoSensing_menu_ATTRIBUTE", "videoSensing_menu_SUBJECT", "videoSensing_menu_VIDEO_STATE",
    ].indexOf(op) !== -1;
  }

  // =========================================================================
  // 4. Snap! writer
  // =========================================================================

  // Simple 1:1 opcode -> Snap selector map (ordered slot names).
  var simpleMap = {
    motion_movesteps: { selector: "forward", slots: ["STEPS"] },
    motion_turnright: { selector: "turn", slots: ["DEGREES"] },
    motion_turnleft: { selector: "turnLeft", slots: ["DEGREES"] },
    motion_pointindirection: { selector: "setHeading", slots: ["DIRECTION"] },
    motion_gotoxy: { selector: "gotoXY", slots: ["X", "Y"] },
    motion_glidesecstoxy: { selector: "doGlide", slots: ["SECS", "X", "Y"] },
    motion_changexby: { selector: "changeXPosition", slots: ["DX"] },
    motion_setx: { selector: "setXPosition", slots: ["X"] },
    motion_changeyby: { selector: "changeYPosition", slots: ["DY"] },
    motion_sety: { selector: "setYPosition", slots: ["Y"] },
    motion_ifonedgebounce: { selector: "bounceOffEdge" },
    motion_xposition: { selector: "xPosition" },
    motion_yposition: { selector: "yPosition" },
    motion_direction: { selector: "direction" },

    looks_sayforsecs: { selector: "doSayFor", slots: ["MESSAGE", "SECS"] },
    looks_say: { selector: "bubble", slots: ["MESSAGE"] },
    looks_thinkforsecs: { selector: "doThinkFor", slots: ["MESSAGE", "SECS"] },
    looks_think: { selector: "doThink", slots: ["MESSAGE"] },
    looks_show: { selector: "show" },
    looks_hide: { selector: "hide" },
    looks_switchcostumeto: { selector: "doSwitchToCostume", slots: ["COSTUME"] },
    looks_nextcostume: { selector: "doWearNextCostume" },
    looks_switchbackdropto: { selector: "doSwitchToCostume", slots: ["BACKDROP"] },
    looks_switchbackdroptoandwait: { selector: "doSwitchToCostume", slots: ["BACKDROP"] },
    looks_nextbackdrop: { selector: "doWearNextCostume" },
    looks_changesizeby: { selector: "changeScale", slots: ["CHANGE"] },
    looks_setsizeto: { selector: "setScale", slots: ["SIZE"] },
    looks_size: { selector: "getScale" },

    sound_play: { selector: "playSound", slots: ["SOUND_MENU"] },
    sound_playuntildone: { selector: "doPlaySoundUntilDone", slots: ["SOUND_MENU"] },
    sound_stopallsounds: { selector: "doStopAllSounds" },
    sound_changevolumeby: { selector: "changeVolume", slots: ["VOLUME"] },
    sound_setvolumeto: { selector: "setVolume", slots: ["VOLUME"] },
    sound_volume: { selector: "getVolume" },

    pen_clear: { selector: "clear" },
    pen_stamp: { selector: "doStamp" },
    pen_penDown: { selector: "down" },
    pen_penUp: { selector: "up" },
    pen_setPenColorToColor: { selector: "setColor", slots: ["COLOR"] },
    pen_changePenSizeBy: { selector: "changeSize", slots: ["SIZE"] },
    pen_setPenSizeTo: { selector: "setSize", slots: ["SIZE"] },

    event_whenflagclicked: { selector: "receiveGo" },
    event_whenkeypressed: { selector: "receiveKey", slots: ["KEY_OPTION"] },
    event_whenthisspriteclicked: { selector: "receiveInteraction" },
    event_whenstageclicked: { selector: "receiveInteraction" },
    event_whenbroadcastreceived: { selector: "receiveMessage", slots: ["BROADCAST_OPTION"] },
    event_whenbackdropswitchesto: { selector: "receiveMessage", slots: ["BACKDROP"] },
    event_broadcast: { selector: "doBroadcast", slots: ["BROADCAST_INPUT"] },
    event_broadcastandwait: { selector: "doBroadcastAndWait", slots: ["BROADCAST_INPUT"] },

    control_wait: { selector: "doWait", slots: ["DURATION"] },
    control_wait_until: { selector: "doWaitUntil", slots: ["CONDITION"] },
    control_stop: { selector: "doStopThis", slots: ["STOP_OPTION"] },
    control_delete_this_clone: { selector: "removeClone" },

    sensing_askandwait: { selector: "doAsk", slots: ["QUESTION"] },
    sensing_answer: { selector: "getLastAnswer" },
    sensing_keypressed: { selector: "reportKeyPressed", slots: ["KEY_OPTION"] },
    sensing_mousedown: { selector: "reportMouseDown" },
    sensing_mousex: { selector: "reportMouseX" },
    sensing_mousey: { selector: "reportMouseY" },
    sensing_timer: { selector: "getTimer" },
    sensing_resettimer: { selector: "doResetTimer" },
    sensing_touchingcolor: { selector: "reportTouchingColor", slots: ["COLOR"] },
    sensing_username: { selector: "reportUsername" },

    operator_add: { selector: "reportSum", slots: ["NUM1", "NUM2"] },
    operator_subtract: { selector: "reportDifference", slots: ["NUM1", "NUM2"] },
    operator_multiply: { selector: "reportProduct", slots: ["NUM1", "NUM2"] },
    operator_divide: { selector: "reportQuotient", slots: ["NUM1", "NUM2"] },
    operator_random: { selector: "reportRandom", slots: ["FROM", "TO"] },
    operator_gt: { selector: "reportGreaterThan", slots: ["OPERAND1", "OPERAND2"] },
    operator_lt: { selector: "reportLessThan", slots: ["OPERAND1", "OPERAND2"] },
    operator_equals: { selector: "reportEquals", slots: ["OPERAND1", "OPERAND2"] },
    operator_and: { selector: "reportAnd", slots: ["OPERAND1", "OPERAND2"] },
    operator_or: { selector: "reportOr", slots: ["OPERAND1", "OPERAND2"] },
    operator_not: { selector: "reportNot", slots: ["OPERAND"] },
    operator_join: { selector: "reportJoinWords", slots: ["STRING1", "STRING2"] },
    operator_letter_of: { selector: "reportLetter", slots: ["LETTER", "STRING"] },
    operator_length: { selector: "reportStringSize", slots: ["STRING"] },
    operator_mod: { selector: "reportModulus", slots: ["NUM1", "NUM2"] },
    operator_round: { selector: "reportRound", slots: ["NUM"] },
  };

  // ----- render context -----
  function newCtx() {
    return {
      procDefs: [], procArgScope: {}, unknownOpcodes: {},
      autoBlocks: {},
    };
  }
  function ensureHelper(ctx, spec, argNames, argDefaults, body, type) {
    if (ctx.autoBlocks[spec]) return spec;
    ctx.autoBlocks[spec] = true;
    ctx.procDefs.push({
      spec: spec, argNames: argNames, argDefaults: argDefaults,
      body: [], bodyPrebuilt: body, warp: false,
      category: "other", type: type || "command",
    });
    return spec;
  }
  function helperReporter(ctx, spec, value) {
    ensureHelper(ctx, spec, [], [], [el("block", { s: "doReport" }, value)], "reporter");
    return el("custom-block", { s: spec, scope: "local" });
  }
  function variableReporter(name) {
    return el("block", { var: name });
  }
  function optionLiteral(value) {
    return el("l", {}, el("option", {}, value));
  }
  function boolLiteral(value) {
    return el("l", {}, el("bool", {}, value ? "true" : "false"));
  }
  function myAttribute(name) {
    return el("block", { s: "reportGet" }, optionLiteral(name));
  }

  // ----- special-case handlers -----
  var handlers = {
    motion_goto: function (b, ctx) {
      return el("block", { s: "doGotoObject" }, targetMenu(b.inputs.TO, ctx, "mouse-pointer"));
    },
    motion_pointtowards: function (b, ctx) {
      return el("block", { s: "doFaceTowards" }, targetMenu(b.inputs.TOWARDS, ctx, "mouse-pointer"));
    },
    motion_setrotationstyle: function (b, ctx) {
      return el("block", { s: "doSetVar" }, optionLiteral("my rotation style"),
        el("l", {}, String(rotationStyleToNumber(b.fields.STYLE))));
    },
    motion_glideto: function (b, ctx) {
      var secs = argOrLiteral(b.inputs.SECS, ctx, "1");
      var to = b.inputs.TO;
      if (to && typeof to === "object" && to.kind === "special") {
        if (to.name === "_random_") {
          return el("block", { s: "doGlide" }, secs,
            el("block", { s: "reportRandom" }, el("l", {}, "-240"), el("l", {}, "240")),
            el("block", { s: "reportRandom" }, el("l", {}, "-180"), el("l", {}, "180")));
        }
        if (to.name === "_mouse_") {
          return el("block", { s: "doGlide" }, secs,
            el("block", { s: "reportMouseX" }), el("block", { s: "reportMouseY" }));
        }
      }
      var target = targetMenu(to, ctx, "mouse-pointer");
      return el("block", { s: "doGlide" }, secs,
        el("block", { s: "reportAttributeOf" }, optionLiteral("x position"), target),
        el("block", { s: "reportAttributeOf" }, optionLiteral("y position"), cloneNode(target)));
    },

    looks_costumenumbername: function (b, ctx) {
      if ((b.fields.NUMBER_NAME || "number") === "name") {
        return el("block", { s: "reportAttributeOf" }, optionLiteral("costume name"), optionLiteral("myself"));
      }
      return el("block", { s: "getCostumeIdx" });
    },
    looks_backdropnumbername: function (b, ctx) {
      if ((b.fields.NUMBER_NAME || "number") === "name") {
        return el("block", { s: "reportAttributeOf" }, optionLiteral("costume name"), el("l", {}, "Background"));
      }
      return el("block", { s: "reportAttributeOf" }, optionLiteral("costume #"), el("l", {}, "Background"));
    },
    looks_changeeffectby: function (b, ctx) {
      return el("block", { s: "changeEffect" }, el("l", {}, mapEffect(b.fields.EFFECT)),
        argOrLiteral(b.inputs.CHANGE, ctx, "25"));
    },
    looks_seteffectto: function (b, ctx) {
      return el("block", { s: "setEffect" }, el("l", {}, mapEffect(b.fields.EFFECT)),
        argOrLiteral(b.inputs.VALUE, ctx, "0"));
    },
    looks_cleargraphiceffects: function () { return el("block", { s: "clearEffects" }); },
    looks_gotofrontback: function (b) {
      if ((b.fields.FRONT_BACK || "front") === "front") return el("block", { s: "comeToFront" });
      return el("block", { s: "goBack" }, el("l", {}, "9999"));
    },
    looks_goforwardbackwardlayers: function (b, ctx) {
      var num = argOrLiteral(b.inputs.NUM, ctx, "1");
      if ((b.fields.FORWARD_BACKWARD || "backward") === "forward") {
        return el("block", { s: "goBack" },
          el("block", { s: "reportDifference" }, el("l", {}, "0"), num));
      }
      return el("block", { s: "goBack" }, num);
    },

    control_repeat: function (b, ctx) {
      return el("block", { s: "doRepeat" }, argOrLiteral(b.inputs.TIMES, ctx, "10"),
        branch(b.branches.SUBSTACK, ctx));
    },
    control_forever: function (b, ctx) {
      return el("block", { s: "doForever" }, branch(b.branches.SUBSTACK, ctx));
    },
    control_if: function (b, ctx) {
      return el("block", { s: "doIf" }, argOrLiteral(b.inputs.CONDITION, ctx, "false"),
        branch(b.branches.SUBSTACK, ctx));
    },
    control_if_else: function (b, ctx) {
      return el("block", { s: "doIfElse" }, argOrLiteral(b.inputs.CONDITION, ctx, "false"),
        branch(b.branches.SUBSTACK, ctx), branch(b.branches.SUBSTACK2, ctx));
    },
    control_repeat_until: function (b, ctx) {
      return el("block", { s: "doUntil" }, argOrLiteral(b.inputs.CONDITION, ctx, "false"),
        branch(b.branches.SUBSTACK, ctx));
    },
    control_create_clone_of: function (b, ctx) {
      return el("block", { s: "createClone" }, argOrLiteral(b.inputs.CLONE_OPTION, ctx, "_myself_"));
    },
    control_start_as_clone: function () { return el("block", { s: "receiveOnClone" }); },

    sensing_of: function (b, ctx) {
      var prop = mapSensingProperty(b.fields.PROPERTY || "");
      return el("block", { s: "reportAttributeOf" },
        prop.option ? optionLiteral(prop.value) : el("l", {}, prop.value),
        argOrLiteral(b.inputs.OBJECT, ctx, "Background"));
    },
    sensing_current: function (b) {
      var which = (b.fields.CURRENTMENU || "YEAR").toLowerCase();
      return el("block", { s: "reportDate" }, el("l", {}, which));
    },
    sensing_loudness: function () {
      return el("block", { s: "reportAudio" }, el("l", {}, "volume"));
    },
    sensing_username: function (b, ctx) {
      return helperReporter(ctx, "username", el("l", {}, ""));
    },
    sensing_setdragmode: function (b, ctx) {
      return el("block", { s: "doSetVar" }, optionLiteral("my draggable?"),
        boolLiteral((b.fields.DRAG_MODE || "draggable") === "draggable"));
    },
    sensing_touchingobject: function (b, ctx) {
      return el("block", { s: "reportTouchingObject" },
        targetMenu(b.inputs.TOUCHINGOBJECTMENU, ctx, "mouse-pointer"));
    },
    sensing_distanceto: function (b, ctx) {
      return el("block", { s: "reportDistanceTo" },
        targetMenu(b.inputs.DISTANCETOMENU, ctx, "mouse-pointer"));
    },

    data_setvariableto: function (b, ctx) {
      return el("block", { s: "doSetVar" }, el("l", {}, b.fields.VARIABLE || ""),
        argOrLiteral(b.inputs.VALUE, ctx, "0"));
    },
    data_changevariableby: function (b, ctx) {
      return el("block", { s: "doChangeVar" }, el("l", {}, b.fields.VARIABLE || ""),
        argOrLiteral(b.inputs.VALUE, ctx, "1"));
    },
    data_showvariable: function (b) { return el("block", { s: "doShowVar" }, el("l", {}, b.fields.VARIABLE || "")); },
    data_hidevariable: function (b) { return el("block", { s: "doHideVar" }, el("l", {}, b.fields.VARIABLE || "")); },
    data_variable: function (b) { return variableReporter(b.fields.VARIABLE || ""); },

    data_listcontents: function (b) { return variableReporter(b.fields.LIST || ""); },
    data_addtolist: function (b, ctx) {
      return el("block", { s: "doAddToList" }, argOrLiteral(b.inputs.ITEM, ctx, ""),
        variableReporter(b.fields.LIST || ""));
    },
    data_deleteoflist: function (b, ctx) {
      return el("block", { s: "doDeleteFromList" }, argOrLiteral(b.inputs.INDEX, ctx, "1"),
        variableReporter(b.fields.LIST || ""));
    },
    data_insertatlist: function (b, ctx) {
      return el("block", { s: "doInsertInList" }, argOrLiteral(b.inputs.ITEM, ctx, ""),
        argOrLiteral(b.inputs.INDEX, ctx, "1"), variableReporter(b.fields.LIST || ""));
    },
    data_replaceitemoflist: function (b, ctx) {
      return el("block", { s: "doReplaceInList" }, argOrLiteral(b.inputs.INDEX, ctx, "1"),
        variableReporter(b.fields.LIST || ""), argOrLiteral(b.inputs.ITEM, ctx, ""));
    },
    data_itemoflist: function (b, ctx) {
      return el("block", { s: "reportListItem" }, argOrLiteral(b.inputs.INDEX, ctx, "1"),
        variableReporter(b.fields.LIST || ""));
    },
    data_lengthoflist: function (b) {
      return el("block", { s: "reportListLength" }, variableReporter(b.fields.LIST || ""));
    },
    data_listcontainsitem: function (b, ctx) {
      return el("block", { s: "reportListContainsItem" }, variableReporter(b.fields.LIST || ""),
        argOrLiteral(b.inputs.ITEM, ctx, ""));
    },
    data_deletealloflist: function (b) {
      return el("block", { s: "doDeleteFromList" }, el("l", {}, "all"),
        variableReporter(b.fields.LIST || ""));
    },
    data_showlist: function (b) { return el("block", { s: "doShowVar" }, el("l", {}, b.fields.LIST || "")); },
    data_hidelist: function (b) { return el("block", { s: "doHideVar" }, el("l", {}, b.fields.LIST || "")); },

    operator_mathop: function (b, ctx) {
      return el("block", { s: "reportMonadic" }, el("l", {}, b.fields.OPERATOR || "sqrt"),
        argOrLiteral(b.inputs.NUM, ctx, "0"));
    },
    // Snap has no text-contains primitive; emulate via split length > 1.
    operator_contains: function (b, ctx) {
      return el("block", { s: "reportGreaterThan" },
        el("block", { s: "reportListLength" },
          el("block", { s: "reportTextSplit" },
            argOrLiteral(b.inputs.STRING1, ctx, ""),
            argOrLiteral(b.inputs.STRING2, ctx, ""))),
        el("l", {}, "1"));
    },

    procedures_call: function (b, ctx) {
      var proccode = (b.mutation && b.mutation.proccode) || "unknown";
      var ids = (b.mutation && b.mutation.argumentIds) || [];
      var spec = snapCallSpecFromProccode(proccode);
      var node = el("custom-block", { s: spec, scope: "local" });
      for (var i = 0; i < ids.length; i++) node.add(argOrLiteral(b.inputs[ids[i]], ctx, ""));
      return node;
    },
    argument_reporter_string_number: function (b) {
      return el("block", { var: b.fields.VALUE || "" });
    },
    argument_reporter_boolean: function (b) {
      return el("block", { var: b.fields.VALUE || "" });
    },
  };

  function buildBlock(block, ctx) {
    var handler = handlers[block.opcode];
    if (handler) return handler(block, ctx);
    var spec = simpleMap[block.opcode];
    if (spec) {
      var node = el("block", { s: spec.selector });
      var slots = spec.slots || [];
      for (var i = 0; i < slots.length; i++) {
        var slot = slots[i], v = block.inputs[slot];
        if (v !== undefined) node.add(buildArg(v, ctx));
        else if (block.fields[slot] !== undefined) node.add(el("l", {}, block.fields[slot]));
        else node.add(el("l", {}, ""));
      }
      return node;
    }
    ctx.unknownOpcodes[block.opcode] = true;
    return el("block", { s: "reportJoinWords" },
      el("l", {}, "[unconverted: " + block.opcode + "]"), el("l", {}, ""));
  }

  function buildArg(arg, ctx) {
    if (arg == null) return el("l", {}, "");
    if (typeof arg === "object") {
      if (arg.kind) {
        switch (arg.kind) {
          case "variable": return variableReporter(arg.name);
          case "list": return variableReporter(arg.name);
          case "special": return el("l", {}, translateSpecial(arg.name));
          case "option": return el("l", {}, arg.value);
        }
      }
      return buildBlock(arg, ctx);
    }
    return el("l", {}, String(arg));
  }

  function translateSpecial(name) {
    switch (name) {
      case "_mouse_": return "mouse-pointer";
      case "_random_": return "random position";
      case "_edge_": return "edge";
      case "_myself_": return "myself";
      case "_stage_": return "Background";
      default: return name;
    }
  }

  function targetMenu(arg, ctx, fallback) {
    if (arg == null || arg === "") return el("l", {}, fallback);
    if (typeof arg === "object" && arg.kind === "special") return el("l", {}, translateSpecial(arg.name));
    return buildArg(arg, ctx);
  }

  function cloneNode(node) {
    var attrs = {}; for (var k in node.attrs) attrs[k] = node.attrs[k];
    var copy = el(node.tag, attrs);
    for (var i = 0; i < node.children.length; i++) {
      var c = node.children[i];
      copy.add(typeof c === "string" ? c : cloneNode(c));
    }
    return copy;
  }

  function argOrLiteral(arg, ctx, fallback) {
    if (arg == null || arg === "") return el("l", {}, fallback);
    return buildArg(arg, ctx);
  }

  function branch(stack, ctx) {
    var node = el("script", {});
    var s = stack || [];
    for (var i = 0; i < s.length; i++) node.add(buildBlock(s[i], ctx));
    return node;
  }

  function snapSpecFromProccode(proccode, argNames) {
    var i = 0;
    return proccode.replace(/%[sb]/g, function (marker) {
      var name = argNames[i] || ("arg" + (i + 1));
      i++;
      return "%'" + name + "'";
    });
  }
  function snapCallSpecFromProccode(proccode) {
    return (proccode || "unknown").replace(/%s/g, "%s").replace(/%b/g, "%b");
  }
  function inputTypesFromProccode(proccode, count) {
    var types = [], m, re = /%[sb]/g;
    while ((m = re.exec(proccode || ""))) types.push(m[0] === "%b" ? "%b" : "%s");
    while (types.length < count) types.push("%s");
    return types;
  }

  function mapSensingProperty(property) {
    var p = String(property || "").toLowerCase();
    if (p === "backdrop #" || p === "backdrop number") return { option: true, value: "costume #" };
    if (p === "backdrop name") return { option: true, value: "costume name" };
    if (p === "costume #" || p === "costume number") return { option: true, value: "costume #" };
    if (p === "costume name") return { option: true, value: "costume name" };
    if (p === "x position" || p === "y position" || p === "direction" || p === "size" ||
        p === "volume" || p === "width" || p === "height") return { option: true, value: p };
    return { option: false, value: p };
  }

  function mapRotationStyle(style) {
    if (style === "left-right") return "left-right";
    if (style === "don't rotate") return "don't rotate";
    return "full";
  }
  function mapEffect(name) {
    var key = (name || "").toLowerCase();
    var known = ["color", "fisheye", "whirl", "pixelate", "mosaic", "brightness", "ghost"];
    return known.indexOf(key) !== -1 ? key : "ghost";
  }
  function rotationStyleToNumber(style) {
    if (style === "left-right") return 2;
    if (style === "don't rotate") return 0;
    return 1;
  }

  // ----- top-level project XML -----
  function projectToSnapXml(project) {
    var unknown = {};
    var stageNode = buildStage(project, unknown);
    for (var k in unknown) project.warnings.push(k);
    var notes = "Converted from Scratch to Snap! by scratch-to-snap.js.";
    if (project.warnings.length) notes += "\nUnconverted opcodes: " + project.warnings.join(", ");
    var root = el("project", { name: "Project", app: "Snapinator", version: "1" },
      el("notes", {}, notes), el("thumbnail", {}), stageNode, buildVariables(project.stage));
    return root.toString();
  }

  function buildStage(project, unknown) {
    var ctx = newCtx(); ctx.unknownOpcodes = unknown;
    var stage = project.stage;
    var stageNode = el("stage", {
      name: "Background", width: 480, height: 360,
      costume: stage.currentCostume + 1, color: "255,255,255,1",
      tempo: 60, threadsafe: false, volume: 100, pan: 0,
      lines: "round", ternary: false, hyperops: true, codify: false,
      inheritance: true, sublistIDs: false, scheduled: true, id: "1",
    });
    stageNode.add(buildCostumes(stage));
    stageNode.add(buildSounds(stage));
    stageNode.add(el("variables", {}));
    stageNode.add(buildScripts(stage, ctx));
    stageNode.add(buildBlocksSection(ctx));
    var sprites = el("sprites", {});
    for (var i = 0; i < project.sprites.length; i++) {
      sprites.add(buildSprite(project.sprites[i], i + 2, unknown));
    }
    stageNode.add(sprites);
    return stageNode;
  }

  function buildSprite(sprite, id, unknown) {
    var ctx = newCtx(); ctx.unknownOpcodes = unknown;
    var node = el("sprite", {
      name: sprite.name, idx: id,
      x: sprite.x, y: sprite.y,
      heading: sprite.direction, scale: sprite.size / 100,
      rotation: rotationStyleToNumber(sprite.rotationStyle),
      draggable: sprite.draggable === true,
      costume: sprite.currentCostume + 1,
      color: "80,80,80,1", pen: "tip", id: String(id),
    });
    if (!sprite.visible) node.attrs.hidden = true;
    node.add(buildCostumes(sprite));
    node.add(buildSounds(sprite));
    node.add(buildVariables(sprite));
    node.add(buildScripts(sprite, ctx));
    node.add(buildBlocksSection(ctx));
    return node;
  }

  function buildCostumes(target) {
    var list = el("list", {});
    var costumes = el("costumes", {}, list);
    for (var i = 0; i < target.costumes.length; i++) {
      var c = target.costumes[i];
      list.add(el("item", {},
        el("costume", {
          name: c.name,
          "center-x": c.rotationCenterX || 0,
          "center-y": c.rotationCenterY || 0,
          image: c.dataUrl,
        })));
    }
    return costumes;
  }

  function buildSounds(target) {
    var list = el("list", {});
    var sounds = el("sounds", {}, list);
    for (var i = 0; i < target.sounds.length; i++) {
      var s = target.sounds[i];
      list.add(el("item", {}, el("sound", { name: s.name, sound: s.dataUrl })));
    }
    return sounds;
  }

  function buildVariables(target) {
    var node = el("variables", {});
    for (var i = 0; i < target.variables.length; i++) {
      var v = target.variables[i];
      node.add(el("variable", { name: v.name }, el("l", {}, String(v.value))));
    }
    for (var j = 0; j < target.lists.length; j++) {
      var list = target.lists[j];
      var listNode = el("variable", { name: list.name });
      var listVal = el("list", {});
      for (var k = 0; k < list.items.length; k++) {
        listVal.add(el("item", {}, el("l", {}, String(list.items[k]))));
      }
      listNode.add(listVal);
      node.add(listNode);
    }
    return node;
  }

  function buildScripts(target, ctx) {
    var scripts = el("scripts", {});
    for (var i = 0; i < target.scripts.length; i++) {
      var script = target.scripts[i];
      if (script.blocks[0] && script.blocks[0].opcode === "procedures_definition") {
        collectProcDef(script, ctx);
      }
    }
    for (var j = 0; j < target.scripts.length; j++) {
      var s = target.scripts[j];
      if (s.blocks[0] && s.blocks[0].opcode === "procedures_definition") continue;
      var scriptNode = el("script", { x: s.x, y: s.y });
      for (var b = 0; b < s.blocks.length; b++) scriptNode.add(buildBlock(s.blocks[b], ctx));
      scripts.add(scriptNode);
    }
    return scripts;
  }

  function collectProcDef(script, ctx) {
    var def = script.blocks[0];
    if (!def) return;
    var proto = def.inputs.custom_block;
    if (!proto || typeof proto !== "object" || proto.kind) return;
    var m = proto.mutation; if (!m) return;
    ctx.procDefs.push({
      spec: snapSpecFromProccode(m.proccode, m.argumentNames),
      argNames: m.argumentNames, argDefaults: m.argumentDefaults,
      argTypes: inputTypesFromProccode(m.proccode, m.argumentNames.length),
      body: script.blocks.slice(1), warp: !!m.warp,
    });
  }

  function buildBlocksSection(ctx) {
    var node = el("blocks", {});
    for (var i = 0; i < ctx.procDefs.length; i++) {
      var def = ctx.procDefs[i];
      var scope = {};
      for (var a = 0; a < (def.argNames || []).length; a++) scope[def.argNames[a]] = true;
      ctx.procArgScope = scope;
      var bd = el("block-definition", {
        s: def.spec, type: def.type || "command", category: def.category || "other",
      });
      bd.add(el("header", {}));
      bd.add(el("code", {}));
      bd.add(el("translations", {}));
      if ((def.argNames || []).length) {
        var inputs = el("inputs", {});
        for (var k = 0; k < def.argNames.length; k++) {
          inputs.add(el("input", { type: (def.argTypes && def.argTypes[k]) || "%s" }, (def.argDefaults && def.argDefaults[k]) || ""));
        }
        bd.add(inputs);
      }
      var bodyScript = el("script", {});
      if (def.bodyPrebuilt) {
        for (var p = 0; p < def.bodyPrebuilt.length; p++) bodyScript.add(def.bodyPrebuilt[p]);
      } else {
        for (var q = 0; q < def.body.length; q++) bodyScript.add(buildBlock(def.body[q], ctx));
      }
      bd.add(bodyScript);
      node.add(bd);
      ctx.procArgScope = {};
    }
    return node;
  }

  // =========================================================================
  // 5. Public API
  // =========================================================================

  async function convert(arrayBuffer) {
    var project = await parseSb3(arrayBuffer);
    var xml = projectToSnapXml(project);
    return { xml: xml, warnings: project.warnings };
  }

  return { convert: convert };
});

// =========================================================================
// CLI entrypoint: `node converter.js <input.sb3> [output.xml]`
// Only runs when invoked directly with Node, not when required or loaded
// in a browser (typeof require / module guards keep browsers happy).
// =========================================================================
if (typeof require !== "undefined" && typeof module !== "undefined" && require.main === module) {
  (async function runCli() {
    var fs = require("fs");
    var args = process.argv.slice(2);
    if (args.length === 0 || args[0] === "-h" || args[0] === "--help") {
      console.log("Usage: node converter.js <input.sb3> [output.xml]");
      process.exit(args.length === 0 ? 1 : 0);
    }
    var inputPath = args[0];
    var outputPath = args[1] || inputPath.replace(/\.sb3?$/i, "") + ".xml";
    if (!fs.existsSync(inputPath)) {
      console.error("Input not found: " + inputPath);
      process.exit(1);
    }
    var buf = fs.readFileSync(inputPath);
    var ab = buf.buffer.slice(buf.byteOffset, buf.byteOffset + buf.byteLength);
    try {
      var out = await module.exports.convert(ab);
      fs.writeFileSync(outputPath, out.xml, "utf8");
      console.log("Wrote " + outputPath + " (" + Math.round(out.xml.length / 1024) + " KB)");
      if (out.warnings.length) {
        console.log("Unconverted opcodes: " + out.warnings.join(", "));
      }
    } catch (e) {
      console.error(e && e.stack ? e.stack : e);
      process.exit(1);
    }
  })();
}
