using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>Sammelt, was nicht umgesetzt werden konnte (damit man es nach der Konvertierung sieht).</summary>
public sealed class ConversionReport
{
    public readonly SortedDictionary<string, int> Unsupported = new();
    public int Scripts;
    public int IgnoredScripts;
    public void Add(string selector) => Unsupported[selector] = Unsupported.TryGetValue(selector, out var n) ? n + 1 : 1;
}

/// <summary>Typisierter C#-Ausdruck: n = double, s = string, b = bool, o = object.</summary>
readonly record struct Ex(string Code, char T);

/// <summary>
/// Übersetzt den Block-Baum eines Skripts rekursiv in C#. Eine Stelle pro Block (Tabelle per switch),
/// keine Textsuche im JSON. Alles Gemeinsame steckt in der Laufzeitklasse Rt, nicht im erzeugten Code.
/// </summary>
public sealed class BlockTranslator
{
    static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    readonly SnapObject obj;
    readonly string me;                    // C#-Feldname des Objekts
    readonly ConversionReport report;
    readonly HashSet<string> locals;
    readonly StringBuilder sb = new();
    int loop;

    public BlockTranslator(SnapObject obj, string spriteField, ConversionReport report)
    {
        this.obj = obj;
        me = spriteField;
        this.report = report;
        locals = new HashSet<string>(obj.Locals.Select(v => v.Name));
    }

    // ---------- Skript ----------

    /// <summary>Gibt null zurück, wenn das Skript keinen unterstützten Hut-Block hat.</summary>
    public (string Method, string Registration)? TranslateScript(JNode script, string methodName)
    {
        var first = script.Kids.FirstOrDefault(k => k.Tag == "block");
        if (first == null) return null;

        string hat = first.Attr("s");
        string reg;
        switch (hat)
        {
            case "receiveGo": reg = $"Rt.Run({methodName});"; break;
            case "receiveKey": reg = $"Rt.OnKey({Esc(HatArg(first))}, {methodName});"; break;
            case "receiveMessage": reg = $"Rt.OnMessage({Esc(HatArg(first))}, {methodName});"; break;
            case "receiveInteraction": reg = $"Rt.OnClick({me}, {methodName});"; break;
            default:
                if (hat.StartsWith("receive")) report.Add(hat);
                report.IgnoredScripts++;
                return null;                              // freie Blöcke ohne Hut werden in Snap! nie ausgeführt
        }

        report.Scripts++;
        sb.Clear();
        sb.AppendLine($"    private async Task {methodName}()");
        sb.AppendLine("    {");
        sb.AppendLine("        await Task.CompletedTask;");
        Blocks(script.Kids.Where(k => k.Tag == "block").Skip(1), 2);
        sb.AppendLine("    }");
        return (sb.ToString(), reg);
    }

    static string HatArg(JNode hat)
    {
        string t = hat.Kids.Count > 0 ? LitText(hat.Kids[0]) : "";
        return t == "any message" || t == "any key" ? "*" : t;
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
        if (script != null && script.Tag == "script") Blocks(script.Kids, ind);
    }

    JNode? Kid(JNode n, int i) => i < n.Kids.Count ? n.Kids[i] : null;

    void Stmt(JNode b, int ind)
    {
        if (b.Tag != "block") { Todo(ind, "<" + b.Tag + ">"); return; }
        string s = b.Attr("s");
        if (s == "") return;                                  // einzelne Variable im Stapel: nichts zu tun

        Ex e0 = X(b, 0), e1 = X(b, 1), e2 = X(b, 2);

        switch (s)
        {
            // Bewegung
            case "forward": L(ind, $"{me}.Move({N(e0)});"); break;
            case "turn": L(ind, $"{me}.Turn({N(e0)});"); break;
            case "turnLeft": L(ind, $"{me}.Turn(-({N(e0)}));"); break;
            case "setHeading": L(ind, $"{me}.SetHeading({N(e0)});"); break;
            case "gotoXY": L(ind, $"{me}.GoTo({N(e0)}, {N(e1)});"); break;
            case "doGotoObject": L(ind, $"Rt.GoToObject({me}, {S(e0)});"); break;
            case "doGlide": L(ind, $"await {me}.Glide({N(e0)}, {N(e1)}, {N(e2)});"); break;
            case "changeXPosition": L(ind, $"{me}.GoTo({me}.X + {N(e0)}, {me}.Y);"); break;
            case "setXPosition": L(ind, $"{me}.GoTo({N(e0)}, {me}.Y);"); break;
            case "changeYPosition": L(ind, $"{me}.GoTo({me}.X, {me}.Y + {N(e0)});"); break;
            case "setYPosition": L(ind, $"{me}.GoTo({me}.X, {N(e0)});"); break;
            case "bounceOffEdge": L(ind, $"{me}.BounceOffEdge();"); break;
            case "doFaceTowards": L(ind, $"Rt.FaceTowards({me}, {S(e0)});"); break;

            // Aussehen
            case "doSayFor": case "doThinkFor": L(ind, $"await {me}.SayFor({e0.Code}, {N(e1)});"); break;
            case "bubble": case "doThink": case "doSay": L(ind, $"{me}.Say({e0.Code});"); break;
            case "show": L(ind, $"{me}.Show();"); break;
            case "hide": L(ind, $"{me}.Hide();"); break;
            case "setScale": L(ind, $"{me}.SetScale({N(e0)} / 100d);"); break;
            case "changeScale": L(ind, $"{me}.SetScale({me}.Scale + {N(e0)} / 100d);"); break;
            case "doSwitchToCostume": L(ind, $"{me}.SwitchCostume({e0.Code});"); break;
            case "doWearNextCostume": L(ind, $"{me}.NextCostume();"); break;
            case "setEffect": L(ind, $"{me}.SetEffect({S(e0)}, {N(e1)});"); break;
            case "changeEffect": L(ind, $"{me}.ChangeEffect({S(e0)}, {N(e1)});"); break;
            case "clearEffects": L(ind, $"{me}.ClearEffects();"); break;
            case "comeToFront": L(ind, $"{me}.ToFront();"); break;
            case "goBack": L(ind, $"{me}.Back({N(e0)});"); break;

            // Klang
            case "playSound": L(ind, $"Rt.PlaySound({me}, {S(e0)});"); break;
            case "doPlaySoundUntilDone": L(ind, $"await Rt.PlaySoundAsync({me}, {S(e0)});"); break;
            case "doStopAllSounds": L(ind, "Rt.StopSounds();"); break;
            case "setVolume": L(ind, $"Rt.Volume = {N(e0)};"); break;
            case "changeVolume": L(ind, $"Rt.Volume += {N(e0)};"); break;

            // Steuerung
            case "doWait": L(ind, $"await Rt.Wait({N(e0)});"); break;
            case "doForever":
                L(ind, "while (true)"); L(ind, "{");
                Body(Kid(b, 0), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            case "doRepeat":
            {
                int k = ++loop;
                L(ind, $"for (int i{k} = 0, n{k} = (int)Math.Round({N(e0)}); i{k} < n{k}; i{k}++)"); L(ind, "{");
                Body(Kid(b, 1), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            }
            case "doUntil":
                L(ind, $"while (!({B(e0)}))"); L(ind, "{");
                Body(Kid(b, 1), ind + 1);
                L(ind + 1, "await Rt.Frame();"); L(ind, "}");
                break;
            case "doWaitUntil": L(ind, $"while (!({B(e0)})) await Rt.Frame();"); break;
            case "doIf":
                L(ind, $"if ({B(e0)})"); L(ind, "{"); Body(Kid(b, 1), ind + 1); L(ind, "}");
                break;
            case "doIfElse":
                L(ind, $"if ({B(e0)})"); L(ind, "{"); Body(Kid(b, 1), ind + 1); L(ind, "}");
                L(ind, "else"); L(ind, "{"); Body(Kid(b, 2), ind + 1); L(ind, "}");
                break;
            case "doBroadcast": L(ind, $"Rt.Broadcast({S(e0)});"); break;
            case "doBroadcastAndWait": L(ind, $"await Rt.BroadcastAndWait({S(e0)});"); break;
            case "doStopThis":
            {
                string what = b.Kids.Count > 0 ? LitText(b.Kids[0]) : "all";
                if (what.StartsWith("all") && !what.StartsWith("all but")) L(ind, "Rt.StopAll();");
                L(ind, "return;");
                break;
            }
            case "doResetTimer": L(ind, "Rt.ResetTimer();"); break;

            // Variablen und Listen
            case "doSetVar":
            case "doChangeVar":
            {
                var nameNode = Kid(b, 0);
                string name = nameNode != null && nameNode.Tag == "l" ? LitText(nameNode) : "";
                if (name.StartsWith("my ")) { Todo(ind, s + " (" + name + ")"); break; }
                string key = nameNode != null && nameNode.Tag == "l" ? Esc(VarKey(name)) : S(e0);
                L(ind, s == "doSetVar" ? $"Rt.Set({key}, {e1.Code});" : $"Rt.Change({key}, {N(e1)});");
                break;
            }
            case "doShowVar": case "doHideVar": Todo(ind, s); break;      // Variablen-Anzeige gibt es im Ergebnis nicht
            case "doAddToList": L(ind, $"Rt.ListAdd({e1.Code}, {e0.Code});"); break;
            case "doDeleteFromList": L(ind, $"Rt.ListDelete({e1.Code}, {e0.Code});"); break;
            case "doInsertInList": L(ind, $"Rt.ListInsert({e2.Code}, {e0.Code}, {e1.Code});"); break;
            case "doReplaceInList": L(ind, $"Rt.ListReplace({e1.Code}, {e0.Code}, {e2.Code});"); break;

            default: Todo(ind, s); break;
        }
    }

    // ---------- Ausdrücke ----------

    static string N(Ex e) => e.T == 'n' ? e.Code : $"Rt.N({e.Code})";
    static string S(Ex e) => e.T == 's' ? e.Code : $"Rt.S({e.Code})";
    static string B(Ex e) => e.T == 'b' ? e.Code : $"Rt.B({e.Code})";

    Ex X(JNode parent, int i) => i < parent.Kids.Count ? Expr(parent.Kids[i]) : new Ex("\"\"", 's');

    string VarKey(string name) => locals.Contains(name) ? obj.Name + "::" + name : name;

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
                return Lit(LitText(n));
            case "bool":
                return new Ex(n.Text.Trim().ToLowerInvariant() == "true" ? "true" : "false", 'b');
            case "list":
                return new Ex("Rt.NewList()", 'o');
            case "block":
                if (n.Attr("var") != "") return new Ex($"Rt.Get({Esc(VarKey(n.Attr("var")))})", 'o');
                return Reporter(n);
            default:
                report.Add("<" + n.Tag + ">");
                return new Ex("0d", 'n');
        }
    }

    List<JNode> Inputs(JNode n) =>
        n.Kids.Count == 1 && n.Kids[0].Tag == "list" ? n.Kids[0].Kids : n.Kids;

    Ex Fold(JNode n, string op, char type)
    {
        var parts = Inputs(n).Select(Expr).Select(e => type == 'n' ? N(e) : B(e)).ToList();
        if (parts.Count == 0) return type == 'n' ? new Ex("0d", 'n') : new Ex("false", 'b');
        return new Ex("(" + string.Join(op, parts) + ")", type);
    }

    Ex Reporter(JNode n)
    {
        string s = n.Attr("s");
        Ex a = X(n, 0), b = X(n, 1);

        switch (s)
        {
            case "reportSum": case "reportVariadicSum": return Fold(n, " + ", 'n');
            case "reportProduct": case "reportVariadicProduct": return Fold(n, " * ", 'n');
            case "reportDifference": return new Ex($"({N(a)} - {N(b)})", 'n');
            case "reportQuotient": return new Ex($"({N(a)} / {N(b)})", 'n');
            case "reportModulus": return new Ex($"Rt.Mod({N(a)}, {N(b)})", 'n');
            case "reportRandom": return new Ex($"Rt.Random({N(a)}, {N(b)})", 'n');
            case "reportRound": return new Ex($"Math.Round({N(a)}, MidpointRounding.AwayFromZero)", 'n');
            case "reportMonadic": return new Ex($"Rt.Monadic({S(a)}, {N(b)})", 'n');

            case "reportLessThan": case "reportVariadicLessThan": return new Ex($"({N(a)} < {N(b)})", 'b');
            case "reportGreaterThan": case "reportVariadicGreaterThan": return new Ex($"({N(a)} > {N(b)})", 'b');
            case "reportLessThanOrEquals": return new Ex($"({N(a)} <= {N(b)})", 'b');
            case "reportGreaterThanOrEquals": return new Ex($"({N(a)} >= {N(b)})", 'b');
            case "reportEquals": case "reportVariadicEquals": return new Ex($"Rt.Eq({a.Code}, {b.Code})", 'b');
            case "reportNotEquals": return new Ex($"(!Rt.Eq({a.Code}, {b.Code}))", 'b');
            case "reportNot": return new Ex($"(!({B(a)}))", 'b');
            case "reportAnd": case "reportVariadicAnd": return Fold(n, " && ", 'b');
            case "reportOr": case "reportVariadicOr": return Fold(n, " || ", 'b');
            case "reportBoolean": return a;

            case "reportJoinWords":
            {
                var parts = Inputs(n).Select(Expr).Select(S).ToList();
                return parts.Count == 0 ? new Ex("\"\"", 's') : new Ex($"string.Concat({string.Join(", ", parts)})", 's');
            }
            case "reportLetter": return new Ex($"Rt.Letter({N(a)}, {S(b)})", 's');
            case "reportStringSize": return new Ex($"((double)({S(a)}).Length)", 'n');
            case "reportTextSplit": return new Ex($"Rt.Split({S(a)}, {S(b)})", 'o');

            case "xPosition": return new Ex($"{me}.X", 'n');
            case "yPosition": return new Ex($"{me}.Y", 'n');
            case "direction": return new Ex($"{me}.Heading", 'n');
            case "getScale": return new Ex($"({me}.Scale * 100d)", 'n');
            case "getCostumeIdx": return new Ex($"((double){me}.Costume)", 'n');
            case "reportShown": return new Ex($"{me}.Visible", 'b');

            case "reportMouseX": return new Ex("Rt.MouseX", 'n');
            case "reportMouseY": return new Ex("Rt.MouseY", 'n');
            case "reportMouseDown": return new Ex("Rt.MouseDown", 'b');
            case "reportKeyPressed": return new Ex($"Rt.IsKeyDown({S(a)})", 'b');
            case "reportTouchingObject": return new Ex($"Rt.Touching({me}, {S(a)})", 'b');
            case "reportDistanceTo": return new Ex($"Rt.Distance({me}, {S(a)})", 'n');
            case "reportDate": return new Ex($"Rt.DateOf({S(a)})", 'n');
            case "getTimer": return new Ex("Rt.Timer", 'n');
            case "getLastAnswer": return new Ex("Rt.Answer", 's');
            case "getVolume": return new Ex("Rt.Volume", 'n');

            case "reportListLength": return new Ex($"Rt.ListLen({a.Code})", 'n');
            case "reportListItem": return new Ex($"Rt.ListItem({a.Code}, {b.Code})", 'o');
            case "reportListContainsItem": return new Ex($"Rt.ListContains({a.Code}, {b.Code})", 'b');
            case "reportListIndex": return new Ex($"Rt.ListIndexOf({a.Code}, {b.Code})", 'n');

            case "reportTouchingColor":
            case "reportColorIsTouchingColor":
                report.Add(s);
                return new Ex("false", 'b');
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
