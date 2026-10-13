using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>Sammelt, was nicht umgesetzt werden konnte (damit man es nach der Konvertierung sieht).</summary>
public sealed class ConversionReport
{
    public readonly SortedDictionary<string, int> Unsupported = new();
    public int Scripts;
    public int IgnoredScripts;
    public int CustomBlocks;
    public void Add(string selector) => Unsupported[selector] = Unsupported.TryGetValue(selector, out var n) ? n + 1 : 1;
}

/// <summary>Ein eigener Block (block-definition) und der Name der C#-Methode, in die er übersetzt wird.</summary>
public sealed class CustomInfo
{
    public string Method = "";
    public string Type = "command";                 // command | reporter | predicate
    public List<string> Params = new();
    public JNode Def = null!;
    public SnapObject? Owner;                       // null = global
}

/// <summary>Findet eigene Blöcke anhand ihrer Spezifikation ("move %n steps" und "move %'n' steps" sind dasselbe).</summary>
public sealed class CustomRegistry
{
    readonly Dictionary<string, CustomInfo> map = new();
    public IEnumerable<CustomInfo> All => map.Values;

    public static string Norm(string spec) => Regex.Replace(spec, @"%'[^']*'|%[A-Za-z]+", "%");

    static string Key(SnapObject? owner, string spec) => (owner?.Name ?? "") + "\0" + Norm(spec);

    public void Add(SnapObject? owner, JNode def, NameRegistry names)
    {
        string spec = def.Attr("s");
        var info = new CustomInfo
        {
            Method = names.Get("Cb", Key(owner, spec).Replace('\0', '_')),
            Type = def.Attr("type", "command"),
            Def = def,
            Owner = owner
        };
        foreach (Match m in Regex.Matches(spec, @"%'([^']*)'")) info.Params.Add(m.Groups[1].Value);
        map[Key(owner, spec)] = info;
    }

    public CustomInfo? Find(SnapObject? obj, string callSpec) =>
        (obj != null && map.TryGetValue(Key(obj, callSpec), out var local)) ? local
        : map.TryGetValue(Key(null, callSpec), out var shared) ? shared : null;
}

/// <summary>Typisierter C#-Ausdruck: n = double, s = string, b = bool, o = object.</summary>
readonly record struct Ex(string Code, char T);

/// <summary>
/// Übersetzt den Block-Baum eines Skripts rekursiv in C#. Eine Stelle pro Block (switch-Tabelle),
/// keine Textsuche im JSON. Alles Gemeinsame steckt in der Laufzeitklasse Rt / Sprite.
/// Variablen aus Parametern, Skriptvariablen und Ring-Parametern werden zu C#-Variablen (Closures funktionieren),
/// alle anderen Namen werden zur Laufzeit gesucht (Sprite-lokal vor global).
/// </summary>
public sealed class BlockTranslator
{
    static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    sealed class RingCtx
    {
        public string Args = "";
        public bool NoFormals;
        public int Empty;
    }

    readonly SnapObject? obj;
    readonly string field;                  // C#-Feldname des Objekts (nur für Hut-Registrierungen)
    readonly ConversionReport report;
    readonly CustomRegistry customs;
    readonly List<Dictionary<string, string>> scopes = new();
    readonly Stack<RingCtx> rings = new();
    StringBuilder sb = new();
    string me = "me";
    int uid;
    int retKind;                            // 0 = Task-Methode, 1 = Task<object?>-Methode, 2 = Ring-Lambda

    public BlockTranslator(SnapObject? obj, string spriteField, ConversionReport report, CustomRegistry customs)
    {
        this.obj = obj;
        field = spriteField;
        this.report = report;
        this.customs = customs;
    }

    // ---------- Skripte mit Hut ----------

    /// <summary>Gibt null zurück, wenn das Skript keinen unterstützten Hut-Block hat.</summary>
    public (string Method, string Registration)? TranslateScript(JNode script, string methodName)
    {
        var first = script.Kids.FirstOrDefault(k => k.Tag == "block");
        if (first == null) return null;

        string hat = first.Attr("s");
        sb = new StringBuilder();
        scopes.Clear(); scopes.Add(new Dictionary<string, string>());
        rings.Clear(); me = "me"; retKind = 0;

        string reg;
        switch (hat)
        {
            case "receiveGo": reg = $"Rt.OnGo({field}, {methodName});"; break;
            case "receiveKey": reg = $"Rt.OnKey({field}, {Esc(HatArg(first))}, {methodName});"; break;
            case "receiveMessage": reg = $"Rt.OnMessage({field}, {Esc(HatArg(first))}, {methodName});"; break;
            case "receiveInteraction":
            {
                string kind = HatArg(first);
                reg = $"Rt.OnInteraction({field}, {Esc(kind == "" ? "clicked" : kind)}, {methodName});";
                break;
            }
            case "receiveOnClone": reg = $"Rt.OnClone({field}, {methodName});"; break;
            case "receiveCondition":
            {
                string ring = MakeRing(first, false, first.Kids.Count > 0 ? first.Kids[0] : null).Code;
                reg = $"Rt.OnCondition({field}, {ring}, {methodName});";
                break;
            }
            default:
                if (hat.StartsWith("receive")) report.Add(hat);
                report.IgnoredScripts++;
                return null;                              // freie Blöcke ohne Hut werden in Snap! nie ausgeführt
        }

        report.Scripts++;
        var body = new StringBuilder();
        body.AppendLine($"    private async Task {methodName}(Sprite me)");
        body.AppendLine("    {");
        body.AppendLine("        await Task.CompletedTask;");
        Blocks(script.Kids.Where(k => k.Tag == "block").Skip(1), 2);
        body.Append(sb);
        body.AppendLine("    }");
        return (body.ToString(), reg);
    }

    /// <summary>Methode für einen eigenen Block.</summary>
    public string TranslateCustom(CustomInfo ci)
    {
        sb = new StringBuilder();
        scopes.Clear(); scopes.Add(new Dictionary<string, string>());
        rings.Clear(); me = "me";
        bool command = ci.Type == "command";
        retKind = command ? 0 : 1;
        report.CustomBlocks++;

        var ps = new List<string>();
        foreach (var name in ci.Params) ps.Add("object? " + Declare(name));

        var head = new StringBuilder();
        head.Append("    private async Task").Append(command ? "" : "<object?>").Append(' ').Append(ci.Method)
            .Append("(Sprite me").Append(ps.Count > 0 ? ", " + string.Join(", ", ps) : "").AppendLine(")");
        head.AppendLine("    {");
        head.AppendLine("        await Task.CompletedTask;");
        var script = ci.Def.First("script");
        if (script != null) Blocks(script.Kids.Where(k => k.Tag == "block"), 2);
        head.Append(sb);
        if (!command) head.AppendLine("        return null;");
        head.AppendLine("    }");
        return head.ToString();
    }

    static string HatArg(JNode hat)
    {
        string t = hat.Kids.Count > 0 ? LitText(hat.Kids[0]) : "";
        return t == "any message" || t == "any key" || t == "any" ? "*" : t;
    }

    // ---------- Gültigkeitsbereiche ----------

    string Declare(string name)
    {
        string id = "v" + (++uid) + "_" + NameRegistry.Sanitize(name);
        scopes[^1][name] = id;
        return id;
    }

    string? Lookup(string name)
    {
        for (int i = scopes.Count - 1; i >= 0; i--)
            if (scopes[i].TryGetValue(name, out var id)) return id;
        return null;
    }

    // ---------- Anweisungen ----------

    void L(int ind, string code) => sb.Append(' ', ind * 4).AppendLine(code);

    void Todo(int ind, string selector)
    {
        report.Add(selector);
        L(ind, "// TODO: Block '" + selector.Replace('\n', ' ').Replace('\r', ' ') + "' wird noch nicht unterstützt");
    }

    void Blocks(IEnumerable<JNode> blocks, int ind)
    {
        foreach (var b in blocks) Stmt(b, ind);
    }

    void Body(JNode? script, int ind)
    {
        scopes.Add(new Dictionary<string, string>());
        if (script != null && script.Tag == "script") Blocks(script.Kids, ind);
        scopes.RemoveAt(scopes.Count - 1);
    }

    JNode? Kid(JNode n, int i) => i < n.Kids.Count ? n.Kids[i] : null;

    string Ret() => retKind == 0 ? "return;" : "return null;";

    string ArgList(JNode? list) =>
        list == null || list.Tag != "list" || list.Kids.Count == 0
            ? "new List<object?>()"
            : "new List<object?> { " + string.Join(", ", list.Kids.Select(k => Expr(k).Code)) + " }";

    JNode? ListKid(JNode n, int start) => n.Kids.Skip(start).FirstOrDefault(k => k.Tag == "list");

    void Stmt(JNode b, int ind)
    {
        if (b.Tag == "custom-block") { CustomCall(b, ind); return; }
        if (b.Tag != "block") { Todo(ind, "<" + b.Tag + ">"); return; }
        string s = b.Attr("s");
        if (s == "") return;                                  // einzelne Variable im Stapel: nichts zu tun

        // Verzögert und nur einmal ausgewertet: Skript- und Ring-Slots dürfen nicht doppelt übersetzt werden.
        Ex? c0 = null, c1 = null, c2 = null;
        Ex e0() { c0 ??= X(b, 0); return c0.Value; }
        Ex e1() { c1 ??= X(b, 1); return c1.Value; }
        Ex e2() { c2 ??= X(b, 2); return c2.Value; }

        switch (s)
        {
            // ----- Bewegung -----
            case "forward": L(ind, $"{me}.Move({N(e0())});"); break;
            case "turn": L(ind, $"{me}.Turn({N(e0())});"); break;
            case "turnLeft": L(ind, $"{me}.Turn(-({N(e0())}));"); break;
            case "setHeading": L(ind, $"{me}.SetHeading({N(e0())});"); break;
            case "gotoXY": L(ind, $"{me}.GoTo({N(e0())}, {N(e1())});"); break;
            case "doGotoObject": L(ind, $"Rt.GoToObject({me}, {e0().Code});"); break;
            case "doGlide": L(ind, $"await {me}.Glide({N(e0())}, {N(e1())}, {N(e2())});"); break;
            case "changeXPosition": L(ind, $"{me}.GoTo({me}.X + {N(e0())}, {me}.Y);"); break;
            case "setXPosition": L(ind, $"{me}.GoTo({N(e0())}, {me}.Y);"); break;
            case "changeYPosition": L(ind, $"{me}.GoTo({me}.X, {me}.Y + {N(e0())});"); break;
            case "setYPosition": L(ind, $"{me}.GoTo({me}.X, {N(e0())});"); break;
            case "bounceOffEdge": L(ind, $"{me}.BounceOffEdge();"); break;
            case "doFaceTowards": L(ind, $"Rt.FaceTowards({me}, {e0().Code});"); break;

            // ----- Aussehen -----
            case "doSayFor": case "doThinkFor": L(ind, $"await {me}.SayFor({e0().Code}, {N(e1())});"); break;
            case "bubble": case "doThink": case "doSay": L(ind, $"{me}.Say({e0().Code});"); break;
            case "show": L(ind, $"{me}.Show();"); break;
            case "hide": L(ind, $"{me}.Hide();"); break;
            case "setScale": L(ind, $"{me}.SetScale({N(e0())} / 100d);"); break;
            case "changeScale": L(ind, $"{me}.SetScale({me}.Scale + {N(e0())} / 100d);"); break;
            case "doSwitchToCostume": L(ind, $"{me}.SwitchCostume({e0().Code});"); break;
            case "doWearNextCostume": L(ind, $"{me}.NextCostume();"); break;
            case "setEffect": L(ind, $"{me}.SetEffect({S(e0())}, {N(e1())});"); break;
            case "changeEffect": L(ind, $"{me}.ChangeEffect({S(e0())}, {N(e1())});"); break;
            case "clearEffects": L(ind, $"{me}.ClearEffects();"); break;
            case "comeToFront": L(ind, $"{me}.ToFront();"); break;
            case "goBack": L(ind, $"{me}.Back({N(e0())});"); break;

            // ----- Klang -----
            case "playSound": L(ind, $"Rt.PlaySound({me}, {S(e0())});"); break;
            case "doPlaySoundAtRate": L(ind, $"Rt.PlaySound({me}, {S(e0())});"); break;
            case "doPlaySoundUntilDone": L(ind, $"await Rt.PlaySoundAsync({me}, {S(e0())});"); break;
            case "doStopAllSounds": L(ind, "Rt.StopSounds();"); break;
            case "setVolume": L(ind, $"Rt.Volume = {N(e0())};"); break;
            case "changeVolume": L(ind, $"Rt.Volume += {N(e0())};"); break;
            case "setPan": L(ind, $"Rt.Pan = {N(e0())};"); break;
            case "changePan": L(ind, $"Rt.Pan += {N(e0())};"); break;
            case "doPlayNote": L(ind, $"await Rt.PlayNote({N(e0())}, {N(e1())});"); break;
            case "doRest": L(ind, $"await Rt.Rest({N(e0())});"); break;
            case "doSetTempo": L(ind, $"Rt.Tempo = {N(e0())};"); break;
            case "doChangeTempo": L(ind, $"Rt.Tempo += {N(e0())};"); break;
            case "doSetInstrument": break;                        // nur Sinuston, Instrument egal

            // ----- Stift -----
            case "clear": L(ind, "Rt.ClearPen();"); break;
            case "down": L(ind, $"{me}.PenDown = true;"); break;
            case "up": L(ind, $"{me}.PenDown = false;"); break;
            case "setColor": L(ind, $"{me}.SetPenColor({S(e0())});"); break;
            case "setSize": L(ind, $"{me}.PenSize = Math.Max(0, {N(e0())});"); break;
            case "changeSize": L(ind, $"{me}.PenSize = Math.Max(0, {me}.PenSize + {N(e0())});"); break;
            case "doStamp": L(ind, $"{me}.Stamp();"); break;
            case "setHue": L(ind, $"{me}.SetPenDim(\"hue\", {N(e0())}, false);"); break;
            case "changeHue": L(ind, $"{me}.SetPenDim(\"hue\", {N(e0())}, true);"); break;
            case "setBrightness": L(ind, $"{me}.SetPenDim(\"brightness\", {N(e0())}, false);"); break;
            case "changeBrightness": L(ind, $"{me}.SetPenDim(\"brightness\", {N(e0())}, true);"); break;
            case "setPenColorDimension": L(ind, $"{me}.SetPenDim({S(e0())}, {N(e1())}, false);"); break;
            case "changePenColorDimension": L(ind, $"{me}.SetPenDim({S(e0())}, {N(e1())}, true);"); break;

            // ----- Steuerung -----
            case "doWait": L(ind, $"await Rt.Wait({N(e0())});"); break;
            case "doWarp":
                L(ind, "{"); Body(Kid(b, 0), ind + 1); L(ind, "}");
                break;
            case "doForever":
                L(ind, "while (true)"); L(ind, "{");
                Body(Kid(b, 0), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            case "doRepeat":
            {
                int k = ++uid;
                L(ind, $"for (int i{k} = 0, n{k} = (int)Math.Round({N(e0())}); i{k} < n{k}; i{k}++)"); L(ind, "{");
                Body(Kid(b, 1), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            }
            case "doUntil":
                L(ind, $"while (!({B(e0())}))"); L(ind, "{");
                Body(Kid(b, 1), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            case "doFor":
            {
                int k = ++uid;
                string name = Kid(b, 0) is { Tag: "l" } nameNode ? LitText(nameNode) : "i";
                Ex lo = X(b, 1), hi = X(b, 2);
                L(ind, $"for (double c{k} = {N(lo)}, e{k} = {N(hi)}, s{k} = c{k} <= e{k} ? 1 : -1; s{k} > 0 ? c{k} <= e{k} : c{k} >= e{k}; c{k} += s{k})");
                L(ind, "{");
                scopes.Add(new Dictionary<string, string>());
                string id = Declare(name);
                L(ind + 1, $"object? {id} = c{k};");
                Body(Kid(b, 3), ind + 1);
                scopes.RemoveAt(scopes.Count - 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            }
            case "doForEach":
            {
                string name = Kid(b, 0) is { Tag: "l" } nameNode ? LitText(nameNode) : "item";
                Ex list = X(b, 1);
                scopes.Add(new Dictionary<string, string>());
                string id = Declare(name);
                L(ind, $"foreach (object? {id} in Rt.Items({list.Code}))"); L(ind, "{");
                Body(Kid(b, 2), ind + 1);
                scopes.RemoveAt(scopes.Count - 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            }
            case "doWaitUntil": L(ind, $"while (!({B(e0())})) await Rt.Frame();"); break;
            case "doIf":
                L(ind, $"if ({B(e0())})"); L(ind, "{"); Body(Kid(b, 1), ind + 1); L(ind, "}");
                break;
            case "doIfElse":
                L(ind, $"if ({B(e0())})"); L(ind, "{"); Body(Kid(b, 1), ind + 1); L(ind, "}");
                L(ind, "else"); L(ind, "{"); Body(Kid(b, 2), ind + 1); L(ind, "}");
                break;
            case "doBroadcast": L(ind, $"Rt.Broadcast({S(e0())});"); break;
            case "doBroadcastAndWait": L(ind, $"await Rt.BroadcastAndWait({S(e0())});"); break;
            case "doStopThis":
            {
                string what = b.Kids.Count > 0 ? LitText(b.Kids[0]) : "all";
                if (what.StartsWith("all but") || what.StartsWith("other")) L(ind, "Rt.StopOthers();");
                else if (what.StartsWith("all")) { L(ind, "Rt.StopAll();"); L(ind, Ret()); }
                else L(ind, Ret());
                break;
            }
            case "doReport": L(ind, retKind == 0 ? "return;" : $"return {e0().Code};"); break;
            case "doRun": L(ind, $"await Rt.Call({me}, {e0().Code}, {ArgList(ListKid(b, 1))});"); break;
            case "fork": L(ind, $"Rt.Fork({me}, {e0().Code}, {ArgList(ListKid(b, 1))});"); break;
            case "doTellTo": L(ind, $"await Rt.TellTo({me}, {e0().Code}, {e1().Code}, {ArgList(ListKid(b, 2))});"); break;
            case "createClone": L(ind, $"Rt.CreateClone({me}, {S(e0())});"); break;
            case "removeClone": L(ind, $"{me}.RemoveClone();"); L(ind, Ret()); break;
            case "doPauseAll": Todo(ind, s); break;
            case "doSwitchToScene": Todo(ind, s); break;
            case "doSetGlobalFlag": L(ind, $"Rt.SetFlag({S(e0())}, {B(e1())});"); break;
            case "doSetFastTracking": L(ind, $"Rt.SetFlag(\"turbo mode\", {B(e0())});"); break;

            // ----- Fühlen -----
            case "doAsk": L(ind, $"await Rt.Ask({S(e0())});"); break;
            case "doResetTimer": L(ind, "Rt.ResetTimer();"); break;
            case "doSetVideoTransparency": case "doRecord": case "playFreq": case "stopFreq": Todo(ind, s); break;

            // ----- Variablen und Listen -----
            case "doDeclareVariables":
                foreach (var v in (ListKid(b, 0)?.Kids ?? new List<JNode>()).Where(k => k.Tag == "l"))
                    L(ind, $"object? {Declare(LitText(v))} = 0d;");
                break;
            case "doSetVar":
            case "doChangeVar":
            {
                var nameNode = Kid(b, 0);
                bool literal = nameNode != null && nameNode.Tag == "l";
                string name = literal ? LitText(nameNode!) : "";
                if (literal && name.StartsWith("my "))
                {
                    if (s == "doSetVar") L(ind, $"Rt.SetMy({me}, {Esc(name.Substring(3))}, {e1().Code});");
                    else Todo(ind, s + " (" + name + ")");
                    break;
                }
                string? id = literal ? Lookup(name) : null;
                if (id != null)
                    L(ind, s == "doSetVar" ? $"{id} = {e1().Code};" : $"{id} = Rt.N({id}) + {N(e1())};");
                else
                {
                    string key = literal ? Esc(name) : S(e0());
                    L(ind, s == "doSetVar" ? $"Rt.SetVar({me}, {key}, {e1().Code});" : $"Rt.ChangeVar({me}, {key}, {N(e1())});");
                }
                break;
            }
            case "doShowVar": case "doHideVar":
            {
                string name = Kid(b, 0) is { Tag: "l" } nn ? LitText(nn) : "";
                L(ind, $"Rt.ShowVar({me}, {Esc(name)}, {(s == "doShowVar" ? "true" : "false")});");
                break;
            }
            case "doAddToList": L(ind, $"Rt.ListAdd({e1().Code}, {e0().Code});"); break;
            case "doDeleteFromList": L(ind, $"Rt.ListDelete({e1().Code}, {e0().Code});"); break;
            case "doInsertInList": L(ind, $"Rt.ListInsert({e2().Code}, {e0().Code}, {e1().Code});"); break;
            case "doReplaceInList": L(ind, $"Rt.ListReplace({e1().Code}, {e0().Code}, {e2().Code});"); break;

            default: Todo(ind, s); break;
        }
    }

    void CustomCall(JNode b, int ind)
    {
        var info = customs.Find(obj, b.Attr("s"));
        if (info == null) { Todo(ind, "custom-block " + b.Attr("s")); return; }
        string call = $"{info.Method}({me}{CustomArgs(b, info)})";
        L(ind, info.Type == "command" ? $"await {call};" : $"_ = await {call};");
    }

    string CustomArgs(JNode b, CustomInfo info)
    {
        var args = new List<string>();
        for (int i = 0; i < info.Params.Count; i++)
            args.Add(i < b.Kids.Count ? Expr(b.Kids[i]).Code : "\"\"");
        return args.Count == 0 ? "" : ", " + string.Join(", ", args);
    }

    // ---------- Ausdrücke ----------

    static string N(Ex e) => e.T == 'n' ? e.Code : $"Rt.N({e.Code})";
    static string S(Ex e) => e.T == 's' ? e.Code : $"Rt.S({e.Code})";
    static string B(Ex e) => e.T == 'b' ? e.Code : $"Rt.B({e.Code})";

    Ex X(JNode parent, int i) => i < parent.Kids.Count ? Expr(parent.Kids[i]) : new Ex("\"\"", 's');

    /// <summary>Text eines &lt;l&gt;-Knotens; bei &lt;option&gt; der Text der Option.</summary>
    static string LitText(JNode n) => n.Kids.Count > 0 ? n.Kids[0].Text : n.Text;

    static Ex Lit(string text)
    {
        if (text.Length > 0 && text.Trim() == text &&
            double.TryParse(text, NumberStyles.Float, Inv, out var d) && double.IsFinite(d))
            return new Ex(d.ToString("R", Inv) + "d", 'n');
        return new Ex(Esc(text), 's');
    }

    Ex Expr(JNode n)
    {
        switch (n.Tag)
        {
            case "l":
                if (n.Kids.Count > 0 && n.Kids[0].Tag == "bool")
                    return new Ex(n.Kids[0].Text.Trim().ToLowerInvariant() == "true" ? "true" : "false", 'b');
                if (n.Kids.Count == 0 && n.Text.Length == 0 && rings.Count > 0 && rings.Peek().NoFormals)
                {
                    var r = rings.Peek();
                    return new Ex($"Rt.Slot({r.Args}, {r.Empty++})", 'o');    // leerer Platz in einem Ring = impliziter Parameter
                }
                return Lit(LitText(n));
            case "bool":
                return new Ex(n.Text.Trim().ToLowerInvariant() == "true" ? "true" : "false", 'b');
            case "color":
                return new Ex(Esc(n.Text), 's');
            case "list":
                return new Ex(ListOf(n.Kids), 'o');
            case "script":
                return MakeRing(n, true, n);                  // Skript direkt in einem Slot = Ring
            case "autolambda":
                return n.Kids.Count > 0 ? Expr(n.Kids[0]) : new Ex("\"\"", 's');
            case "custom-block":
            {
                var info = customs.Find(obj, n.Attr("s"));
                if (info == null) { report.Add("custom-block " + n.Attr("s")); return new Ex("\"\"", 's'); }
                return new Ex($"(await {info.Method}({me}{CustomArgs(n, info)}))", 'o');
            }
            case "block":
            {
                string v = n.Attr("var");
                if (v != "")
                {
                    string? id = Lookup(v);
                    return new Ex(id ?? $"Rt.GetVar({me}, {Esc(v)})", 'o');
                }
                return Reporter(n);
            }
            default:
                report.Add("<" + n.Tag + ">");
                return new Ex("0d", 'n');
        }
    }

    string ListOf(IEnumerable<JNode> items)
    {
        var parts = items.Select(k => Expr(k).Code).ToList();
        return parts.Count == 0 ? "Rt.NewList()" : $"Rt.ListOf({string.Join(", ", parts)})";
    }

    List<JNode> Inputs(JNode n) =>
        n.Kids.Count == 1 && n.Kids[0].Tag == "list" ? n.Kids[0].Kids : n.Kids;

    /// <summary>Rechenoperation; Zahlen direkt, sonst über Rt.Arith (das auch Listen elementweise verrechnet).</summary>
    static Ex Arith(string op, Ex a, Ex b) =>
        a.T == 'n' && b.T == 'n' && (op is "+" or "-" or "*" or "/")
            ? new Ex($"({a.Code} {op} {b.Code})", 'n')
            : new Ex($"Rt.Arith({Esc(op)}, {a.Code}, {b.Code})", 'o');

    Ex FoldArith(JNode n, string op)
    {
        var parts = Inputs(n).Select(Expr).ToList();
        if (parts.Count == 0) return new Ex("0d", 'n');
        Ex acc = parts[0];
        for (int i = 1; i < parts.Count; i++) acc = Arith(op, acc, parts[i]);
        return acc;
    }

    Ex FoldBool(JNode n, string op)
    {
        var parts = Inputs(n).Select(Expr).Select(B).ToList();
        return parts.Count == 0 ? new Ex("false", 'b') : new Ex("(" + string.Join(op, parts) + ")", 'b');
    }

    /// <summary>Baut aus einem Ring-Block (reifyScript/reifyReporter/reifyPredicate) einen Rt.Ring.</summary>
    Ex MakeRing(JNode n, bool command, JNode? bodyNode)
    {
        var formals = n.Kids.Where(k => k.Tag == "list").SelectMany(k => k.Kids)
                       .Select(k => k.Tag == "l" ? LitText(k) : "").Where(s => s != "").ToList();
        if (n.Tag == "script") formals.Clear();

        if (bodyNode != null && bodyNode.Tag == "autolambda") bodyNode = bodyNode.Kids.FirstOrDefault();

        int id = ++uid;
        string meN = "me" + id, argsN = "args" + id;
        string savedMe = me; var savedSb = sb; int savedRet = retKind;
        me = meN; sb = new StringBuilder(); retKind = 2;
        scopes.Add(new Dictionary<string, string>());
        rings.Push(new RingCtx { Args = argsN, NoFormals = formals.Count == 0 });

        for (int i = 0; i < formals.Count; i++)
            L(3, $"object? {Declare(formals[i])} = Rt.Arg({argsN}, {i});");

        if (command)
        {
            Body(bodyNode, 3);
            L(3, "return null;");
        }
        else
        {
            Ex e = bodyNode == null ? new Ex("\"\"", 's') : Expr(bodyNode);
            L(3, $"return (object?)({e.Code});");
        }

        rings.Pop();
        scopes.RemoveAt(scopes.Count - 1);
        string text = sb.ToString();
        sb = savedSb; me = savedMe; retKind = savedRet;
        return new Ex($"Rt.MakeRing(async ({meN}, {argsN}) =>\n        {{\n{text}        }})", 'o');
    }

    Ex Reporter(JNode n)
    {
        string s = n.Attr("s");

        // Ringe zuerst: ihr Inhalt wird in einem eigenen Kontext übersetzt (nicht vorab im äußeren).
        if (s == "reifyScript") return MakeRing(n, true, n.Kids.FirstOrDefault(k => k.Tag == "script"));
        if (s == "reifyReporter" || s == "reifyPredicate") return MakeRing(n, false, n.Kids.FirstOrDefault(k => k.Tag != "list"));

        Ex? ca = null, cb = null, cc = null;
        Ex a() { ca ??= X(n, 0); return ca.Value; }
        Ex b() { cb ??= X(n, 1); return cb.Value; }
        Ex c() { cc ??= X(n, 2); return cc.Value; }

        switch (s)
        {
            // ----- Rechnen -----
            case "reportSum": case "reportVariadicSum": return FoldArith(n, "+");
            case "reportProduct": case "reportVariadicProduct": return FoldArith(n, "*");
            case "reportDifference": return Arith("-", a(), b());
            case "reportQuotient": return Arith("/", a(), b());
            case "reportPower": return Arith("^", a(), b());
            case "reportModulus": return Arith("%", a(), b());
            case "reportMin": case "reportVariadicMin": return FoldArith(n, "min");
            case "reportMax": case "reportVariadicMax": return FoldArith(n, "max");
            case "reportAtan2": return new Ex($"(Math.Atan2({N(a())}, {N(b())}) * 180d / Math.PI)", 'n');
            case "reportRandom": return new Ex($"Rt.Random({N(a())}, {N(b())})", 'n');
            case "reportRound": return new Ex($"Math.Round({N(a())}, MidpointRounding.AwayFromZero)", 'n');
            case "reportMonadic": return new Ex($"Rt.Monadic({S(a())}, {N(b())})", 'n');

            // ----- Vergleichen, Logik -----
            case "reportLessThan": case "reportVariadicLessThan": return new Ex($"Rt.Lt({a().Code}, {b().Code})", 'b');
            case "reportGreaterThan": case "reportVariadicGreaterThan": return new Ex($"Rt.Lt({b().Code}, {a().Code})", 'b');
            case "reportLessThanOrEquals": case "reportVariadicLessThanOrEquals": return new Ex($"(!Rt.Lt({b().Code}, {a().Code}))", 'b');
            case "reportGreaterThanOrEquals": case "reportVariadicGreaterThanOrEquals": return new Ex($"(!Rt.Lt({a().Code}, {b().Code}))", 'b');
            case "reportEquals": case "reportVariadicEquals": return new Ex($"Rt.Eq({a().Code}, {b().Code})", 'b');
            case "reportNotEquals": case "reportVariadicNotEquals": return new Ex($"(!Rt.Eq({a().Code}, {b().Code}))", 'b');
            case "reportIsIdentical": return new Ex($"Rt.Identical({a().Code}, {b().Code})", 'b');
            case "reportNot": return new Ex($"(!({B(a())}))", 'b');
            case "reportAnd": case "reportVariadicAnd": return FoldBool(n, " && ");
            case "reportOr": case "reportVariadicOr": return FoldBool(n, " || ");
            case "reportBoolean": return a();
            case "reportIfElse": return new Ex($"({B(a())} ? (object?)({b().Code}) : (object?)({c().Code}))", 'o');

            // ----- Text -----
            case "reportJoinWords":
            {
                var parts = Inputs(n).Select(Expr).Select(S).ToList();
                return parts.Count == 0 ? new Ex("\"\"", 's') : new Ex($"string.Concat({string.Join(", ", parts)})", 's');
            }
            case "reportLetter": return new Ex($"Rt.Letter({N(a())}, {S(b())})", 's');
            case "reportStringSize": return new Ex($"((double)({S(a())}).Length)", 'n');
            case "reportTextSplit": return new Ex($"Rt.Split({S(a())}, {S(b())})", 'o');
            case "reportUnicode": return new Ex($"Rt.Unicode({S(a())})", 'n');
            case "reportUnicodeAsLetter": return new Ex($"Rt.UnicodeAsLetter({N(a())})", 's');
            case "reportTypeOf": return new Ex($"Rt.TypeOf({a().Code})", 's');
            case "reportIsA": return new Ex($"(Rt.TypeOf({a().Code}) == {S(b())})", 'b');

            // ----- Bewegung, Aussehen -----
            case "xPosition": return new Ex($"{me}.X", 'n');
            case "yPosition": return new Ex($"{me}.Y", 'n');
            case "direction": return new Ex($"{me}.Heading", 'n');
            case "getScale": return new Ex($"({me}.Scale * 100d)", 'n');
            case "getCostumeIdx": return new Ex($"((double){me}.Costume)", 'n');
            case "reportShown": return new Ex($"{me}.Visible", 'b');
            case "getEffect": return new Ex($"{me}.GetEffect({S(a())})", 'n');
            case "reportGetImageAttribute": return new Ex($"Rt.CostumeAttr({me}, {S(a())}, {b().Code})", 'o');

            // ----- Klang, Stift -----
            case "getVolume": return new Ex("Rt.Volume", 'n');
            case "getPan": return new Ex("Rt.Pan", 'n');
            case "getTempo": return new Ex("Rt.Tempo", 'n');
            case "getPenDown": return new Ex($"{me}.PenDown", 'b');
            case "getPenAttribute": return new Ex($"{me}.GetPenAttr({S(a())})", 'n');
            case "getHue": return new Ex($"{me}.GetPenAttr(\"hue\")", 'n');
            case "getBrightness": return new Ex($"{me}.GetPenAttr(\"brightness\")", 'n');

            // ----- Fühlen -----
            case "reportMouseX": return new Ex("Rt.MouseX", 'n');
            case "reportMouseY": return new Ex("Rt.MouseY", 'n');
            case "reportMouseDown": return new Ex("Rt.MouseDown", 'b');
            case "reportKeyPressed": return new Ex($"Rt.IsKeyDown({S(a())})", 'b');
            case "reportTouchingObject": return new Ex($"Rt.Touching({me}, {S(a())})", 'b');
            case "reportDistanceTo": return new Ex($"Rt.Distance({me}, {a().Code})", 'n');
            case "reportAttributeOf": return new Ex($"Rt.AttributeOf({me}, {a().Code}, {b().Code})", 'o');
            case "reportGet": return new Ex($"Rt.My({me}, {S(a())})", 'o');
            case "reportObject": return new Ex($"Rt.ObjectNamed({S(a())})", 'o');
            case "reportDate": return new Ex($"Rt.DateOf({S(a())})", 'n');
            case "getTimer": return new Ex("Rt.Timer", 'n');
            case "getLastAnswer": return new Ex("Rt.Answer", 's');
            case "getLastMessage": return new Ex("Rt.LastMessage", 's');
            case "reportURL":                                              // bewusst kein Netzwerkzugriff in erzeugten Spielen
                report.Add(s);
                return new Ex("\"\"", 's');
            case "reportAudio": return new Ex("0d", 'n');                     // kein Mikrofon: immer Stille
            case "reportGlobalFlag": return new Ex($"Rt.Flag({S(a())})", 'b');
            case "reportIsFastTracking": return new Ex("Rt.Flag(\"turbo mode\")", 'b');
            case "reportTouchingColor":
            case "reportColorIsTouchingColor":
                report.Add(s);
                return new Ex("false", 'b');

            // ----- Steuerung, Ringe -----
            case "evaluate": return new Ex($"(await Rt.Call({me}, {a().Code}, {ArgList(ListKid(n, 1))}))", 'o');
            case "reportAskFor": return new Ex($"(await Rt.AskFor({me}, {a().Code}, {b().Code}, {ArgList(ListKid(n, 2))}))", 'o');
            case "newClone": return new Ex($"Rt.CreateClone({me}, {S(a())})", 'o');

            // ----- Listen -----
            case "reportNewList": return new Ex(ListOf(Inputs(n)), 'o');
            case "reportNumbers": return new Ex($"Rt.Numbers({N(a())}, {N(b())})", 'o');
            case "reportCONS": return new Ex($"Rt.Cons({a().Code}, {b().Code})", 'o');
            case "reportCDR": return new Ex($"Rt.Cdr({a().Code})", 'o');
            case "reportListLength": return new Ex($"Rt.ListLen({a().Code})", 'n');
            case "reportListItem": return new Ex($"Rt.ListItem({a().Code}, {b().Code})", 'o');
            case "reportListContainsItem": return new Ex($"Rt.ListContains({a().Code}, {b().Code})", 'b');
            case "reportListIndex": return new Ex($"Rt.ListIndexOf({a().Code}, {b().Code})", 'n');
            case "reportListIsEmpty": return new Ex($"(Rt.ListLen({a().Code}) == 0)", 'b');
            case "reportListAttribute": return new Ex($"Rt.ListAttr({S(a())}, {b().Code})", 'o');
            case "reportConcatenatedLists": return new Ex($"Rt.Concat({a().Code})", 'o');
            case "reportMap": return new Ex($"(await Rt.Map({me}, {a().Code}, {b().Code}))", 'o');
            case "reportKeep": return new Ex($"(await Rt.Keep({me}, {a().Code}, {b().Code}))", 'o');
            case "reportFindFirst": return new Ex($"(await Rt.FindFirst({me}, {a().Code}, {b().Code}))", 'o');
            case "reportCombine": return new Ex($"(await Rt.Combine({me}, {a().Code}, {b().Code}))", 'o');

            default:
                report.Add(s);
                return new Ex("0d", 'n');
        }
    }

    // ---------- Hilfen ----------

    /// <summary>C#-Code für einen Anfangswert (Zahl oder Text).</summary>
    public static string LiteralCode(string text) => Lit(text).Code;

    public static string Esc(string s)
    {
        var o = new StringBuilder("\"");
        foreach (char c in s)
        {
            switch (c)
            {
                case '\\': o.Append("\\\\"); break;
                case '"': o.Append("\\\""); break;
                case '\n': o.Append("\\n"); break;
                case '\r': o.Append("\\r"); break;
                case '\t': o.Append("\\t"); break;
                default:
                    if (c < 32 || c == '\u2028' || c == '\u2029') o.Append("\\u").Append(((int)c).ToString("x4"));
                    else o.Append(c);
                    break;
            }
        }
        return o.Append('"').ToString();
    }
}
