namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Die Laufzeitbibliothek, die in jedes erzeugte Projekt kopiert wird (als Runtime.cs).
/// Alles, was mehrere Blöcke brauchen, steht hier EINMAL; der erzeugte Code pro Block ist nur ein Aufruf.
/// __NS__ wird durch den Projektnamen ersetzt.
/// </summary>
public static class RuntimeTemplate
{
    public const string Source = """
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace __NS__;

/// <summary>Ein Snap!-Ring (Lambda): Befehls-, Funktions- oder Prädikat-Ring.</summary>
public sealed class Ring
{
    public readonly Func<Sprite, List<object?>, Task<object?>> Fn;
    public Ring(Func<Sprite, List<object?>, Task<object?>> fn) { Fn = fn; }
}

public static class Rt
{
    public static Canvas Stage = null!;
    public static Canvas PenLayer = null!;
    public static double StageW = 480, StageH = 360;
    public static readonly Dictionary<string, object?> Vars = new();
    public static readonly Dictionary<string, Sprite> Sprites = new();
    public static Sprite? StageSprite;
    public static double MouseX, MouseY, Volume = 100, Pan, Tempo = 60;
    public static bool MouseDown;
    public static string Answer = "", LastMessage = "";
    public static Sprite? Dragging;

    sealed class Handler { public Sprite Owner = null!; public Func<Sprite, Task> F = null!; }
    static readonly Dictionary<string, List<Handler>> handlers = new();
    static readonly HashSet<string> keysDown = new();
    static readonly Dictionary<string, bool> flags = new();
    static readonly AsyncLocal<int> generation = new();
    static readonly AsyncLocal<Sprite?> self = new();
    static int stopGeneration;
    static DateTime timerStart = DateTime.UtcNow;

    public static string AssetPath(string file) => System.IO.Path.Combine(AppContext.BaseDirectory, "Assets", file);

    // ---------- Aufbau ----------

    public static Sprite AddSprite(string name, bool isStage)
    {
        var s = new Sprite(name, isStage);
        Sprites[name] = s;
        if (isStage) StageSprite = s;
        return s;
    }

    public static void Attach(Window window, Canvas stage, double w, double h)
    {
        Stage = stage; StageW = w; StageH = h;
        PenLayer = new Canvas { Width = w, Height = h, IsHitTestVisible = false };
        stage.PointerMoved += (_, e) =>
        {
            var p = e.GetPosition(stage);
            MouseX = p.X - StageW / 2;
            MouseY = StageH / 2 - p.Y;
            Dragging?.GoTo(MouseX, MouseY);
        };
        stage.PointerPressed += (_, _) => MouseDown = true;
        stage.PointerReleased += (_, _) =>
        {
            MouseDown = false;
            if (Dragging != null) { var d = Dragging; Dragging = null; FireFor("int:dropped", d); }
        };
        window.KeyDown += (_, e) =>
        {
            string k = KeyName(e.Key);
            if (keysDown.Add(k)) { FireAll("key:" + k); FireAll("key:*"); }
        };
        window.KeyUp += (_, e) => keysDown.Remove(KeyName(e.Key));

        watchPanel = new StackPanel { Spacing = 2 };
        watchPanel.ZIndex = 2000;
        Canvas.SetLeft(watchPanel, 4);
        Canvas.SetTop(watchPanel, 4);
        stage.Children.Add(watchPanel);
        watchTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        watchTimer.Tick += (_, _) => { foreach (var wt in watchers) wt.Text.Text = wt.Label + ": " + S(wt.Get()); };
        watchTimer.Start();
    }

    static string KeyName(Key k)
    {
        if (k == Key.Space) return "space";
        if (k == Key.Up) return "up arrow";
        if (k == Key.Down) return "down arrow";
        if (k == Key.Left) return "left arrow";
        if (k == Key.Right) return "right arrow";
        if (k == Key.Enter) return "enter";
        if (k == Key.Escape) return "esc";
        if (k >= Key.D0 && k <= Key.D9) return ((int)k - (int)Key.D0).ToString(CultureInfo.InvariantCulture);
        return k.ToString().ToLowerInvariant();
    }

    public static bool IsKeyDown(string name)
    {
        name = name.ToLowerInvariant();
        return name == "any" || name == "any key" ? keysDown.Count > 0 : keysDown.Contains(name);
    }

    // ---------- Skripte, Ereignisse ----------

    public static void Run(Sprite me, Func<Sprite, Task> f) => _ = RunAsync(me, f);

    static async Task RunAsync(Sprite me, Func<Sprite, Task> f)
    {
        generation.Value = stopGeneration;
        self.Value = me;
        try { await f(me); }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Console.Error.WriteLine(ex); }
    }

    static void Check()
    {
        if (generation.Value != stopGeneration) throw new OperationCanceledException();
        var s = self.Value;
        if (s != null && s.Deleted) throw new OperationCanceledException();
    }

    public static void StopAll() { stopGeneration++; StopSounds(); }

    public static void StopOthers() { stopGeneration++; generation.Value = stopGeneration; }

    static void Reg(string key, Sprite owner, Func<Sprite, Task> f)
    {
        if (!handlers.TryGetValue(key, out var list)) handlers[key] = list = new List<Handler>();
        list.Add(new Handler { Owner = owner, F = f });
    }

    public static void OnGo(Sprite owner, Func<Sprite, Task> f) => Run(owner, f);
    public static void OnMessage(Sprite owner, string m, Func<Sprite, Task> f) => Reg("msg:" + m, owner, f);
    public static void OnKey(Sprite owner, string k, Func<Sprite, Task> f) => Reg("key:" + k.ToLowerInvariant(), owner, f);
    public static void OnInteraction(Sprite owner, string kind, Func<Sprite, Task> f) => Reg("int:" + kind, owner, f);
    public static void OnClone(Sprite owner, Func<Sprite, Task> f) => Reg("clone", owner, f);

    static void FireAll(string key)
    {
        if (!handlers.TryGetValue(key, out var list)) return;
        foreach (var h in list.ToArray())
            foreach (var inst in h.Owner.Instances()) Run(inst, h.F);
    }

    public static void FireFor(string key, Sprite inst)
    {
        if (!handlers.TryGetValue(key, out var list)) return;
        foreach (var h in list.ToArray())
            if (h.Owner == inst.Original) Run(inst, h.F);
    }

    public static void Broadcast(string m)
    {
        LastMessage = m;
        FireAll("msg:" + m);
        FireAll("msg:*");
    }

    public static async Task BroadcastAndWait(string m)
    {
        LastMessage = m;
        var tasks = new List<Task>();
        foreach (var key in new[] { "msg:" + m, "msg:*" })
            if (handlers.TryGetValue(key, out var list))
                foreach (var h in list.ToArray())
                    foreach (var inst in h.Owner.Instances()) tasks.Add(RunAsync(inst, h.F));
        await Task.WhenAll(tasks);
        Check();
    }

    /// <summary>"Wenn Bedingung": wird jede Frame geprüft, das Skript läuft nicht doppelt pro Sprite.</summary>
    public static void OnCondition(Sprite owner, Ring cond, Func<Sprite, Task> f) => _ = Watch(owner, cond, f);

    static async Task Watch(Sprite owner, Ring cond, Func<Sprite, Task> f)
    {
        var running = new HashSet<Sprite>();
        while (true)
        {
            await Task.Delay(16);
            foreach (var inst in owner.Instances())
            {
                if (running.Contains(inst)) continue;
                bool ok;
                try { ok = B(await cond.Fn(inst, new List<object?>())); } catch { ok = false; }
                if (!ok) continue;
                running.Add(inst);
                _ = RunWatched(inst, f, running);
            }
        }
    }

    static async Task RunWatched(Sprite inst, Func<Sprite, Task> f, HashSet<Sprite> running)
    {
        try { await RunAsync(inst, f); }
        finally { running.Remove(inst); }
    }

    public static async Task Frame()
    {
        Check();
        await Task.Delay(16);
        Check();
    }

    public static async Task Wait(double secs)
    {
        Check();
        if (secs > 0) await Task.Delay(TimeSpan.FromSeconds(secs)); else await Task.Delay(1);
        Check();
    }

    public static double Timer => (DateTime.UtcNow - timerStart).TotalSeconds;
    public static void ResetTimer() => timerStart = DateTime.UtcNow;

    public static bool Flag(string name) => flags.TryGetValue(name, out var b) && b;
    public static void SetFlag(string name, bool value) => flags[name] = value;

    // ---------- Ringe ----------

    public static Ring MakeRing(Func<Sprite, List<object?>, Task<object?>> fn) => new Ring(fn);

    public static object? Arg(List<object?> args, int i) => i < args.Count ? args[i] : null;

    /// <summary>Leerer Platz in einem Ring ohne Parameter: ein Argument füllt alle, sonst der Reihe nach.</summary>
    public static object? Slot(List<object?> args, int k)
    {
        if (args.Count == 0) return "";
        if (args.Count == 1) return args[0];
        return k < args.Count ? args[k] : "";
    }

    public static async Task<object?> Call(Sprite me, object? ring, List<object?> args)
    {
        if (ring is Ring r) return await r.Fn(me, args);
        return ring;
    }

    public static void Fork(Sprite me, object? ring, List<object?> args) =>
        Run(me, async s => { await Call(s, ring, args); });

    static Sprite Who(Sprite me, object? target) => target as Sprite ?? ObjectNamed(S(target)) ?? me;

    public static async Task TellTo(Sprite me, object? target, object? ring, List<object?> args) =>
        await Call(Who(me, target), ring, args);

    public static async Task<object?> AskFor(Sprite me, object? target, object? ring, List<object?> args) =>
        await Call(Who(me, target), ring, args);

    public static async Task<object?> Map(Sprite me, object? ring, object? list)
    {
        var res = new List<object?>();
        foreach (var x in Items(list)) res.Add(await Call(me, ring, new List<object?> { x }));
        return res;
    }

    public static async Task<object?> Keep(Sprite me, object? ring, object? list)
    {
        var res = new List<object?>();
        foreach (var x in Items(list)) if (B(await Call(me, ring, new List<object?> { x }))) res.Add(x);
        return res;
    }

    public static async Task<object?> FindFirst(Sprite me, object? ring, object? list)
    {
        foreach (var x in Items(list)) if (B(await Call(me, ring, new List<object?> { x }))) return x;
        return "";
    }

    public static async Task<object?> Combine(Sprite me, object? list, object? ring)
    {
        var items = Items(list);
        if (items.Count == 0) return 0d;
        object? acc = items[0];
        for (int i = 1; i < items.Count; i++) acc = await Call(me, ring, new List<object?> { acc, items[i] });
        return acc;
    }

    // ---------- Klone ----------

    public static Sprite CreateClone(Sprite me, string target)
    {
        Sprite src = target == "_myself_" || target == "myself" || target == "" ? me : (ObjectNamed(target) ?? me);
        var c = src.MakeClone();
        FireFor("clone", c);
        return c;
    }

    public static Sprite? ObjectNamed(string name)
    {
        if (Sprites.TryGetValue(name, out var s)) return s;
        if (name == "Stage" || name == "Background") return StageSprite;
        return null;
    }

    // ---------- Variablen ----------

    public static object? GetVar(Sprite me, string name)
    {
        if (me.Vars.TryGetValue(name, out var v)) return v;
        if (Vars.TryGetValue(name, out v)) return v;
        return 0d;
    }

    public static void SetVar(Sprite me, string name, object? value)
    {
        if (me.Vars.ContainsKey(name)) me.Vars[name] = value; else Vars[name] = value;
    }

    public static void ChangeVar(Sprite me, string name, double delta) => SetVar(me, name, N(GetVar(me, name)) + delta);

    // Variablen-Anzeige (Watcher) oben links auf der Bühne
    sealed class Watcher { public string Label = ""; public Func<object?> Get = null!; public TextBlock Text = new(); public Border Box = new(); }
    static readonly List<Watcher> watchers = new();
    static StackPanel? watchPanel;
    static DispatcherTimer? watchTimer;

    public static void ShowVar(Sprite me, string name, bool show)
    {
        string label = (me.Vars.ContainsKey(name) ? me.Name + " " : "") + name;
        var existing = watchers.FirstOrDefault(w => w.Label == label);
        if (!show)
        {
            if (existing != null) { watchPanel?.Children.Remove(existing.Box); watchers.Remove(existing); }
            return;
        }
        if (existing != null || watchPanel == null) return;
        var wt = new Watcher { Label = label, Get = () => GetVar(me, name) };
        wt.Text.Text = label;
        wt.Text.Foreground = Brushes.Black;
        wt.Box.Background = Brushes.Orange;
        wt.Box.CornerRadius = new CornerRadius(4);
        wt.Box.Padding = new Thickness(5, 1);
        wt.Box.Child = wt.Text;
        watchers.Add(wt);
        watchPanel.Children.Add(wt.Box);
    }

    // ---------- Werte ----------

    public static bool TryNum(object? o, out double d)
    {
        switch (o)
        {
            case double x: d = x; return true;
            case int i: d = i; return true;
            case string s when s.Trim().Length > 0 &&
                               double.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var v):
                d = v; return true;
            default: d = 0; return false;
        }
    }

    public static double N(object? o)
    {
        if (o is bool b) return b ? 1 : 0;
        return TryNum(o, out var d) ? d : 0;
    }

    public static string S(object? o) => o switch
    {
        null => "",
        string s => s,
        double d => d.ToString("R", CultureInfo.InvariantCulture),
        bool b => b ? "true" : "false",
        Sprite sp => sp.Name,
        List<object?> l => string.Join(" ", l.Select(S)),
        _ => Convert.ToString(o, CultureInfo.InvariantCulture) ?? ""
    };

    public static bool B(object? o) => o switch
    {
        bool b => b,
        string s => s.Equals("true", StringComparison.OrdinalIgnoreCase),
        double d => d != 0,
        null => false,
        _ => true
    };

    public static bool Eq(object? a, object? b)
    {
        if (a is bool || b is bool) return B(a) == B(b);
        if (a is List<object?> la && b is List<object?> lb)
            return la.Count == lb.Count && la.Zip(lb, Eq).All(same => same);
        if (TryNum(a, out var x) && TryNum(b, out var y)) return x == y;
        return string.Equals(S(a), S(b), StringComparison.OrdinalIgnoreCase);
    }

    public static bool Identical(object? a, object? b) =>
        a is List<object?> || b is List<object?> || a is Sprite || b is Sprite ? ReferenceEquals(a, b) : Eq(a, b);

    public static bool Lt(object? a, object? b)
    {
        if (TryNum(a, out var x) && TryNum(b, out var y)) return x < y;
        return string.Compare(S(a), S(b), StringComparison.OrdinalIgnoreCase) < 0;
    }

    /// <summary>Rechnen; Listen werden elementweise verrechnet (Snap!-Hyperblöcke).</summary>
    public static object? Arith(string op, object? a, object? b)
    {
        if (a is List<object?> la)
        {
            var r = new List<object?>();
            if (b is List<object?> lb)
            {
                int n = Math.Min(la.Count, lb.Count);
                for (int i = 0; i < n; i++) r.Add(Arith(op, la[i], lb[i]));
            }
            else foreach (var x in la) r.Add(Arith(op, x, b));
            return r;
        }
        if (b is List<object?> lb2)
        {
            var r2 = new List<object?>();
            foreach (var y in lb2) r2.Add(Arith(op, a, y));
            return r2;
        }
        double p = N(a), q = N(b);
        return op switch
        {
            "+" => p + q,
            "-" => p - q,
            "*" => p * q,
            "/" => p / q,
            "^" => Math.Pow(p, q),
            "%" => q == 0 ? 0 : ((p % q) + q) % q,
            "min" => Math.Min(p, q),
            "max" => Math.Max(p, q),
            _ => 0d
        };
    }

    public static double Random(double a, double b)
    {
        double lo = Math.Min(a, b), hi = Math.Max(a, b);
        if (lo == Math.Floor(lo) && hi == Math.Floor(hi))
            return System.Random.Shared.NextInt64((long)lo, (long)hi + 1);
        return lo + System.Random.Shared.NextDouble() * (hi - lo);
    }

    public static double Monadic(string f, double x) => f switch
    {
        "abs" => Math.Abs(x),
        "neg" => -x,
        "ceiling" => Math.Ceiling(x),
        "floor" => Math.Floor(x),
        "sqrt" => Math.Sqrt(x),
        "sin" => Math.Sin(x * Math.PI / 180),
        "cos" => Math.Cos(x * Math.PI / 180),
        "tan" => Math.Tan(x * Math.PI / 180),
        "asin" => Math.Asin(x) * 180 / Math.PI,
        "acos" => Math.Acos(x) * 180 / Math.PI,
        "atan" => Math.Atan(x) * 180 / Math.PI,
        "ln" => Math.Log(x),
        "log" => Math.Log10(x),
        "lg" => Math.Log2(x),
        "e^" => Math.Exp(x),
        "10^" => Math.Pow(10, x),
        "2^" => Math.Pow(2, x),
        _ => x
    };

    public static string Letter(double idx, string s)
    {
        int i = (int)idx;
        return i >= 1 && i <= s.Length ? s[i - 1].ToString() : "";
    }

    public static double Unicode(string s) => s.Length == 0 ? 0 : char.ConvertToUtf32(s, 0);

    public static string UnicodeAsLetter(double n)
    {
        int cp = (int)n;
        return cp >= 0 && cp <= 0x10FFFF && !(cp >= 0xD800 && cp <= 0xDFFF) ? char.ConvertFromUtf32(cp) : "";
    }

    public static string TypeOf(object? o) => o switch
    {
        null => "nothing",
        double => "number",
        int => "number",
        bool => "Boolean",
        string s => TryNum(s, out _) ? "number" : "text",
        List<object?> => "list",
        Ring => "command",
        Sprite => "sprite",
        _ => "text"
    };

    public static List<object?> Split(string s, string delim)
    {
        string[] parts = delim switch
        {
            "" => s.Select(c => c.ToString()).ToArray(),
            "line" => s.Split('\n'),
            "tab" => s.Split('\t'),
            "cr" => s.Split('\r'),
            _ => s.Split(delim)
        };
        return parts.Select(p => (object?)p).ToList();
    }

    public static double DateOf(string what)
    {
        var n = DateTime.Now;
        return what switch
        {
            "year" => n.Year,
            "month" => n.Month,
            "date" => n.Day,
            "day of week" => (int)n.DayOfWeek + 1,
            "hour" => n.Hour,
            "minute" => n.Minute,
            "second" => n.Second,
            "time in milliseconds" => new DateTimeOffset(n).ToUnixTimeMilliseconds(),
            _ => 0
        };
    }

    // ---------- Listen ----------

    public static List<object?> NewList() => new();
    public static List<object?> ListOf(params object?[] items) => new(items);
    static List<object?>? L(object? o) => o as List<object?>;
    public static List<object?> Items(object? list) => list is List<object?> l ? new List<object?>(l) : new List<object?>();
    public static double ListLen(object? list) => L(list)?.Count ?? 0;
    public static void ListAdd(object? list, object? item) => L(list)?.Add(item);
    public static bool ListContains(object? list, object? item) => L(list)?.Any(x => Eq(x, item)) ?? false;

    public static double ListIndexOf(object? item, object? list)
    {
        var l = L(list);
        return l == null ? 0 : l.FindIndex(x => Eq(x, item)) + 1;
    }

    static int Index(object? idx, int count)
    {
        string k = S(idx);
        if (k == "last") return count;
        if (k == "random") return count == 0 ? 0 : (int)Random(1, count);
        return (int)N(idx);
    }

    public static object? ListItem(object? idx, object? list)
    {
        var l = L(list);
        if (l == null) return "";
        if (idx is List<object?> many) return many.Select(ix => ListItem(ix, list)).ToList();
        int i = Index(idx, l.Count);
        return i >= 1 && i <= l.Count ? l[i - 1] : "";
    }

    public static void ListDelete(object? list, object? idx)
    {
        var l = L(list);
        if (l == null) return;
        if (S(idx) == "all") { l.Clear(); return; }
        int i = Index(idx, l.Count);
        if (i >= 1 && i <= l.Count) l.RemoveAt(i - 1);
    }

    public static void ListInsert(object? list, object? item, object? idx)
    {
        var l = L(list);
        if (l == null) return;
        int i = Index(idx, l.Count + 1);
        l.Insert(Math.Clamp(i - 1, 0, l.Count), item);
    }

    public static void ListReplace(object? list, object? idx, object? item)
    {
        var l = L(list);
        if (l == null) return;
        int i = Index(idx, l.Count);
        if (i >= 1 && i <= l.Count) l[i - 1] = item;
    }

    public static List<object?> Numbers(double start, double end)
    {
        var r = new List<object?>();
        if (start <= end) for (double x = start; x <= end; x++) r.Add(x);
        else for (double x = start; x >= end; x--) r.Add(x);
        return r;
    }

    public static List<object?> Cons(object? item, object? list)
    {
        var r = new List<object?> { item };
        r.AddRange(Items(list));
        return r;
    }

    public static List<object?> Cdr(object? list) => Items(list).Skip(1).ToList();

    public static List<object?> Concat(object? lists)
    {
        var r = new List<object?>();
        foreach (var l in Items(lists)) r.AddRange(Items(l));
        return r;
    }

    static void Flatten(object? o, List<object?> into)
    {
        if (o is List<object?> l) foreach (var x in l) Flatten(x, into);
        else into.Add(o);
    }

    static string Json(object? o) => o switch
    {
        null => "null",
        List<object?> l => "[" + string.Join(",", l.Select(Json)) + "]",
        bool b => b ? "true" : "false",
        double d => d.ToString("R", CultureInfo.InvariantCulture),
        _ => "\"" + S(o).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n") + "\""
    };

    public static object? ListAttr(string attr, object? list)
    {
        var items = Items(list);
        switch (attr)
        {
            case "length": return (double)items.Count;
            case "rank": return items.Count > 0 && items[0] is List<object?> ? 2d : 1d;
            case "dimensions": return new List<object?> { (double)items.Count };
            case "flatten": { var r = new List<object?>(); foreach (var x in items) Flatten(x, r); return r; }
            case "columns":
            {
                int cols = items.Select(x => Items(x).Count).DefaultIfEmpty(0).Max();
                var r = new List<object?>();
                for (int c = 0; c < cols; c++) r.Add(items.Select(row => ListItem((double)(c + 1), row)).ToList());
                return r;
            }
            case "uniques":
            {
                var r = new List<object?>();
                foreach (var x in items) if (!r.Any(y => Eq(x, y))) r.Add(x);
                return r;
            }
            case "sorted":
            {
                var r = new List<object?>(items);
                r.Sort((x, y) => Lt(x, y) ? -1 : Lt(y, x) ? 1 : 0);
                return r;
            }
            case "shuffled": return items.OrderBy(_ => System.Random.Shared.Next()).ToList();
            case "reverse": { items.Reverse(); return items; }
            case "lines": return string.Join("\n", items.Select(S));
            case "csv":
                return string.Join("\n", items.Select(row => row is List<object?> cells ? string.Join(",", cells.Select(S)) : S(row)));
            case "json": return Json(items);
            default: return new List<object?>();
        }
    }

    // ---------- Sprites untereinander ----------

    public static void GoToObject(Sprite me, object? target)
    {
        string t = S(target);
        if (t == "random position")
            me.GoTo(Random(-StageW / 2, StageW / 2), Random(-StageH / 2, StageH / 2));
        else if (t == "mouse-pointer")
            me.GoTo(MouseX, MouseY);
        else if (target is Sprite sp) me.GoTo(sp.X, sp.Y);
        else if (Sprites.TryGetValue(t, out var o)) me.GoTo(o.X, o.Y);
    }

    static bool Locate(object? target, out double x, out double y)
    {
        x = y = 0;
        string t = S(target);
        if (t == "mouse-pointer") { x = MouseX; y = MouseY; return true; }
        var o = target as Sprite ?? ObjectNamed(t);
        if (o == null) return false;
        x = o.X; y = o.Y;
        return true;
    }

    public static void FaceTowards(Sprite me, object? target)
    {
        if (Locate(target, out var x, out var y))
            me.SetHeading(Math.Atan2(x - me.X, y - me.Y) * 180 / Math.PI);
    }

    public static double Distance(Sprite me, object? target) =>
        Locate(target, out var x, out var y) ? Math.Sqrt((x - me.X) * (x - me.X) + (y - me.Y) * (y - me.Y)) : 0;

    public static bool Touching(Sprite me, string target)
    {
        if (!me.Visible) return false;
        var box = me.Box();
        switch (target)
        {
            case "edge":
                return box.Left < 0 || box.Top < 0 || box.Right > StageW || box.Bottom > StageH;
            case "mouse-pointer":
                return box.Contains(new Point(MouseX + StageW / 2, StageH / 2 - MouseY));
            default:
                var other = ObjectNamed(target);
                return other != null && other.Instances().Any(i => i != me && i.Visible && box.Intersects(i.Box()));
        }
    }

    public static object? AttributeOf(Sprite me, object? attr, object? target)
    {
        var t = S(target) == "myself" ? me : (target as Sprite ?? ObjectNamed(S(target)));
        if (t == null) return 0d;
        string a = S(attr);
        switch (a)
        {
            case "x position": return t.X;
            case "y position": return t.Y;
            case "direction": return t.Heading;
            case "costume #": return (double)t.Costume;
            case "costume name": return t.CostumeName();
            case "size": return t.Scale * 100;
            case "volume": return Volume;
            case "balance": return Pan;
            case "width": return t.Box().Width;
            case "height": return t.Box().Height;
            case "left": return t.Box().Left - StageW / 2;
            case "right": return t.Box().Right - StageW / 2;
            case "top": return StageH / 2 - t.Box().Top;
            case "bottom": return StageH / 2 - t.Box().Bottom;
        }
        if (t.Vars.TryGetValue(a, out var v)) return v;
        if (Vars.TryGetValue(a, out v)) return v;
        return 0d;
    }

    public static object? My(Sprite me, string attr)
    {
        switch (attr)
        {
            case "name": return me.Name;
            case "temporary?": return me.IsClone;
            case "draggable?": return me.Draggable;
            case "rotation style": return (double)me.RotationStyle;
            case "pen down?": return me.PenDown;
            case "other sprites": return Sprites.Values.Where(s => !s.IsStage && s != me.Original).Cast<object?>().ToList();
            default: return AttributeOf(me, attr, me);
        }
    }

    public static void SetMy(Sprite me, string attr, object? value)
    {
        switch (attr)
        {
            case "rotation style": me.SetRotationStyle((int)N(value)); break;
            case "draggable?": me.Draggable = B(value); break;
            default: Console.Error.WriteLine("'my " + attr + "' kann nicht gesetzt werden."); break;
        }
    }

    public static object? CostumeAttr(Sprite me, string attr, object? which) => me.CostumeAttr(attr, which);

    // ---------- Frage ----------

    public static async Task Ask(string prompt)
    {
        Check();
        var done = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var box = new TextBox { MinWidth = 220 };
        var label = new TextBlock { Text = prompt, Foreground = Brushes.Black, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center };
        var row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
        row.Children.Add(label);
        row.Children.Add(box);
        var holder = new Border { Background = Brushes.White, Padding = new Thickness(6), Width = StageW, Child = row };
        holder.ZIndex = 3000;
        Canvas.SetLeft(holder, 0);
        Canvas.SetTop(holder, StageH - 44);
        box.KeyDown += (_, e) => { if (e.Key == Key.Enter) done.TrySetResult(box.Text ?? ""); };
        Stage.Children.Add(holder);
        Dispatcher.UIThread.Post(() => box.Focus());
        try { Answer = await done.Task; }
        finally { Stage.Children.Remove(holder); }
        Check();
    }

    // ---------- Stift ----------

    public static void ClearPen() => PenLayer.Children.Clear();

    public static void DrawLine(double x1, double y1, double x2, double y2, Color c, double size)
    {
        if (x1 == x2 && y1 == y2) return;
        PenLayer.Children.Add(new Line
        {
            StartPoint = new Point(StageW / 2 + x1, StageH / 2 - y1),
            EndPoint = new Point(StageW / 2 + x2, StageH / 2 - y2),
            Stroke = new SolidColorBrush(c),
            StrokeThickness = Math.Max(1, size),
            StrokeLineCap = PenLineCap.Round
        });
    }

    // ---------- Klang (LibVLC, optional: ohne VLC bleibt das Projekt stumm) ----------

    static LibVLCSharp.Shared.LibVLC? vlc;
    static bool vlcFailed;
    static readonly List<LibVLCSharp.Shared.MediaPlayer> players = new();

    static LibVLCSharp.Shared.MediaPlayer? StartPath(string path)
    {
        if (vlcFailed) return null;
        try
        {
            var fi = new FileInfo(path);
            if (!fi.Exists || fi.Length < 64) return null;              // leere Datei: nichts abzuspielen
            if (vlc == null) { LibVLCSharp.Shared.Core.Initialize(); vlc = new LibVLCSharp.Shared.LibVLC("--quiet"); }
            var media = new LibVLCSharp.Shared.Media(vlc, path, LibVLCSharp.Shared.FromType.FromPath);
            var p = new LibVLCSharp.Shared.MediaPlayer(media) { Volume = (int)Math.Clamp(Volume, 0, 100) };
            players.Add(p);
            p.Play();
            return p;
        }
        catch (Exception ex)
        {
            vlcFailed = true;
            Console.Error.WriteLine("Audio nicht verfügbar: " + ex.Message);
            return null;
        }
    }

    static LibVLCSharp.Shared.MediaPlayer? StartSound(Sprite me, string name)
    {
        string? file = me.SoundFile(name) ?? Sprites.Values.Select(s => s.SoundFile(name)).FirstOrDefault(f => f != null);
        return file == null ? null : StartPath(AssetPath(file));
    }

    public static void PlaySound(Sprite me, string name) => StartSound(me, name);

    public static async Task PlaySoundAsync(Sprite me, string name)
    {
        var p = StartSound(me, name);
        if (p == null) return;
        var done = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        p.EndReached += (_, _) => done.TrySetResult();
        p.Stopped += (_, _) => done.TrySetResult();
        await Task.WhenAny(done.Task, Task.Delay(TimeSpan.FromMinutes(5)));
        Check();
    }

    public static void StopSounds()
    {
        foreach (var p in players.ToArray()) { try { p.Stop(); } catch { } }
        players.Clear();
    }

    // Noten: ein Sinuston wird einmal erzeugt und für gleiche Töne wiederverwendet.
    static readonly Dictionary<string, string> noteFiles = new();

    static string NoteFile(double freq, double secs)
    {
        string key = freq.ToString("0.##", CultureInfo.InvariantCulture) + "_" + secs.ToString("0.###", CultureInfo.InvariantCulture);
        if (noteFiles.TryGetValue(key, out var existing) && File.Exists(existing)) return existing;
        int rate = 22050, n = Math.Max(1, (int)(secs * rate));
        string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "snapnote_" + key + ".wav");
        using (var bw = new BinaryWriter(File.Create(path)))
        {
            bw.Write(Encoding.ASCII.GetBytes("RIFF")); bw.Write(36 + n * 2); bw.Write(Encoding.ASCII.GetBytes("WAVE"));
            bw.Write(Encoding.ASCII.GetBytes("fmt ")); bw.Write(16); bw.Write((short)1); bw.Write((short)1);
            bw.Write(rate); bw.Write(rate * 2); bw.Write((short)2); bw.Write((short)16);
            bw.Write(Encoding.ASCII.GetBytes("data")); bw.Write(n * 2);
            int fade = Math.Min(n / 2, rate / 200);
            for (int i = 0; i < n; i++)
            {
                double env = i < fade ? (double)i / fade : i > n - fade ? (double)(n - i) / fade : 1;
                bw.Write((short)(Math.Sin(2 * Math.PI * freq * i / rate) * 0.4 * env * short.MaxValue));
            }
        }
        noteFiles[key] = path;
        return path;
    }

    public static async Task PlayNote(double note, double beats)
    {
        double secs = beats * 60 / Math.Max(1, Tempo);
        StartPath(NoteFile(440 * Math.Pow(2, (note - 69) / 12), secs));
        await Wait(secs);
    }

    public static Task Rest(double beats) => Wait(beats * 60 / Math.Max(1, Tempo));
}

/// <summary>
/// Scratch-Grafikeffekte (Farbe, Helligkeit, Fischauge, Wirbel, Verpixeln, Mosaik) auf den Kostümpixeln.
/// Jede Effektkombination wird nur EINMAL berechnet und wiederverwendet; "ghost" läuft über die Transparenz.
/// </summary>
public static class Fx
{
    static readonly Dictionary<string, SkiaSharp.SKColor[]> sources = new();
    static readonly Dictionary<string, (int W, int H)> sizes = new();
    static readonly Dictionary<string, Bitmap> cache = new();

    public static Bitmap? Apply(string file, double color, double brightness, double fisheye, double whirl, double pixelate, double mosaic)
    {
        string key = file + "|" + string.Join("|", new[] { color, brightness, fisheye, whirl, pixelate, mosaic }
            .Select(v => Math.Round(v, 1).ToString(CultureInfo.InvariantCulture)));
        if (cache.TryGetValue(key, out var hit)) return hit;

        if (!sources.TryGetValue(file, out var src))
        {
            using var decoded = SkiaSharp.SKBitmap.Decode(Rt.AssetPath(file));
            if (decoded == null) return null;
            src = decoded.Pixels;
            sources[file] = src;
            sizes[file] = (decoded.Width, decoded.Height);
        }
        var (w, h) = sizes[file];
        var dst = new SkiaSharp.SKColor[w * h];

        double block = Math.Max(1, Math.Abs(pixelate) / 10);
        int tiles = mosaic == 0 ? 1 : Math.Clamp((int)Math.Round((Math.Abs(mosaic) + 10) / 10), 1, 512);
        double fishPower = Math.Max(0, (fisheye + 100) / 100);
        double whirlAngle = -whirl * Math.PI / 180;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                double u = (x + 0.5) / w, v = (y + 0.5) / h;

                if (tiles > 1) { u = (u * tiles) % 1.0; v = (v * tiles) % 1.0; }
                if (pixelate != 0)
                {
                    u = (Math.Floor(u * w / block) + 0.5) * block / w;
                    v = (Math.Floor(v * h / block) + 0.5) * block / h;
                }
                if (whirl != 0)
                {
                    double ox = u - 0.5, oy = v - 0.5;
                    double factor = Math.Max(1 - Math.Sqrt(ox * ox + oy * oy) / 0.5, 0);
                    double a = whirlAngle * factor * factor;
                    double sin = Math.Sin(a), cos = Math.Cos(a);
                    u = 0.5 + cos * ox + sin * oy;
                    v = 0.5 - sin * ox + cos * oy;
                }
                if (fisheye != 0)
                {
                    double vx = (u - 0.5) / 0.5, vy = (v - 0.5) / 0.5;
                    double len = Math.Sqrt(vx * vx + vy * vy);
                    if (len > 0)
                    {
                        double r = Math.Pow(Math.Min(len, 1), fishPower) * Math.Max(1, len);
                        u = 0.5 + r * vx / len * 0.5;
                        v = 0.5 + r * vy / len * 0.5;
                    }
                }

                int sx = (int)Math.Floor(u * w), sy = (int)Math.Floor(v * h);
                if (sx < 0 || sy < 0 || sx >= w || sy >= h) { dst[y * w + x] = SkiaSharp.SKColors.Transparent; continue; }

                var px = src[sy * w + sx];
                if (px.Alpha == 0) { dst[y * w + x] = px; continue; }

                double red = px.Red / 255.0, green = px.Green / 255.0, blue = px.Blue / 255.0;
                if (color != 0)
                {
                    RgbToHsv(red, green, blue, out double hue, out double sat, out double val);
                    // Wie in Scratch: fast graue/schwarze Pixel bekommen eine Mindestfarbe, damit der Effekt sichtbar wird.
                    if (val < 0.055) { hue = 0; sat = 1; val = 0.055; }
                    else if (sat < 0.09) { hue = 0; sat = 0.09; }
                    hue = (hue + color / 200.0) % 1.0;
                    if (hue < 0) hue += 1;
                    HsvToRgb(hue, sat, val, out red, out green, out blue);
                }
                if (brightness != 0)
                {
                    double d = brightness / 100.0;
                    red = Math.Clamp(red + d, 0, 1);
                    green = Math.Clamp(green + d, 0, 1);
                    blue = Math.Clamp(blue + d, 0, 1);
                }
                dst[y * w + x] = new SkiaSharp.SKColor((byte)Math.Round(red * 255), (byte)Math.Round(green * 255), (byte)Math.Round(blue * 255), px.Alpha);
            }
        }

        using var bmp = new SkiaSharp.SKBitmap(w, h);
        bmp.Pixels = dst;
        using var img = SkiaSharp.SKImage.FromBitmap(bmp);
        using var data = img.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
        using var ms = new MemoryStream(data.ToArray());
        var result = new Bitmap(ms);
        if (cache.Count > 300) cache.Clear();
        cache[key] = result;
        return result;
    }

    static void RgbToHsv(double r, double g, double b, out double h, out double s, out double v)
    {
        double max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b)), d = max - min;
        v = max;
        s = max == 0 ? 0 : d / max;
        if (d == 0) { h = 0; return; }
        if (max == r) h = ((g - b) / d) % 6;
        else if (max == g) h = (b - r) / d + 2;
        else h = (r - g) / d + 4;
        h /= 6;
        if (h < 0) h += 1;
    }

    static void HsvToRgb(double h, double s, double v, out double r, out double g, out double b)
    {
        double hh = h * 6, c = v * s, x = c * (1 - Math.Abs(hh % 2 - 1)), m = v - c;
        if (hh < 1) { r = c; g = x; b = 0; }
        else if (hh < 2) { r = x; g = c; b = 0; }
        else if (hh < 3) { r = 0; g = c; b = x; }
        else if (hh < 4) { r = 0; g = x; b = c; }
        else if (hh < 5) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }
        r += m; g += m; b += m;
    }
}

public sealed class Sprite
{
    public readonly string Name;
    public readonly bool IsStage;
    public double X, Y, Heading = 90, Scale = 1;
    public bool Visible = true;
    public int Costume = 1;
    public bool Draggable;
    public bool Deleted;
    public Sprite Original;
    public readonly Dictionary<string, object?> Vars = new();
    public readonly Image View = new();
    public int RotationStyle { get; private set; } = 1;

    // Stift
    public bool PenDown;
    public double PenSize = 1;
    double penHue, penSat, penBri, penAlpha = 100;

    readonly List<(string Name, Bitmap Bmp, double Cx, double Cy, string FileName)> costumes;
    readonly Dictionary<string, string> sounds;
    readonly Dictionary<string, double> effects = new();
    readonly List<Sprite> clones = new();
    readonly Border bubble = new();
    readonly TextBlock bubbleText = new();

    public bool IsClone => Original != this;

    public Sprite(string name, bool isStage)
    {
        Name = name;
        IsStage = isStage;
        Original = this;
        costumes = new();
        sounds = new();
        InitUi();
    }

    // Klon: teilt Kostüme und Sounds mit dem Original, hat aber eigene Variablen und Position.
    Sprite(Sprite proto)
    {
        Name = proto.Name;
        IsStage = proto.IsStage;
        Original = proto.Original;
        costumes = proto.costumes;
        sounds = proto.sounds;
        X = proto.X; Y = proto.Y; Heading = proto.Heading; Scale = proto.Scale;
        Visible = proto.Visible; Costume = proto.Costume; RotationStyle = proto.RotationStyle; Draggable = proto.Draggable;
        PenDown = proto.PenDown; PenSize = proto.PenSize;
        penHue = proto.penHue; penSat = proto.penSat; penBri = proto.penBri; penAlpha = proto.penAlpha;
        foreach (var kv in proto.Vars) Vars[kv.Key] = kv.Value;
        foreach (var kv in proto.effects) effects[kv.Key] = kv.Value;
        InitUi();
    }

    void InitUi()
    {
        bubbleText.TextWrapping = TextWrapping.Wrap;
        bubbleText.MaxWidth = 160;
        bubbleText.Foreground = Brushes.Black;
        bubble.Child = bubbleText;
        bubble.Background = Brushes.White;
        bubble.BorderBrush = Brushes.Gray;
        bubble.BorderThickness = new Thickness(1);
        bubble.CornerRadius = new CornerRadius(8);
        bubble.Padding = new Thickness(6, 3);
        bubble.IsVisible = false;
        bubble.ZIndex = 1000;
        View.PointerPressed += (_, _) => { Rt.FireFor("int:pressed", this); if (Draggable) Rt.Dragging = this; };
        View.PointerReleased += (_, _) => Rt.FireFor("int:clicked", this);
        View.PointerEntered += (_, _) => Rt.FireFor("int:mouse-entered", this);
        View.PointerExited += (_, _) => Rt.FireFor("int:mouse-departed", this);
    }

    public IEnumerable<Sprite> Instances()
    {
        var all = new List<Sprite> { Original };
        all.AddRange(Original.clones);
        return all;
    }

    public Sprite MakeClone()
    {
        var c = new Sprite(this);
        Original.clones.Add(c);
        Rt.Stage.Children.Add(c.View);
        Rt.Stage.Children.Add(c.bubble);
        c.Update();
        return c;
    }

    public void RemoveClone()
    {
        if (!IsClone) return;
        Deleted = true;
        Rt.Stage.Children.Remove(View);
        Rt.Stage.Children.Remove(bubble);
        Original.clones.Remove(this);
    }

    public void AddCostume(string name, string file, double cx, double cy)
    {
        try { costumes.Add((name, new Bitmap(Rt.AssetPath(file)), cx, cy, file)); }
        catch (Exception ex) { Console.Error.WriteLine("Kostüm '" + file + "' fehlt: " + ex.Message); }
    }

    public void AddSound(string name, string file) => sounds[name] = file;
    public string? SoundFile(string name) => sounds.TryGetValue(name, out var f) ? f : null;

    public void Init(double x, double y, double heading, double scale, int rotation, int costume, bool hidden)
    {
        X = x; Y = y; Heading = heading; Scale = scale;
        RotationStyle = rotation; Costume = costume; Visible = !hidden;
        Rt.Stage.Children.Add(View);
        if (IsStage) Rt.Stage.Children.Add(Rt.PenLayer);       // Stift-Spuren liegen über dem Hintergrund, unter den Sprites
        else Rt.Stage.Children.Add(bubble);
        Update();
    }

    static double Norm(double h) => ((h % 360) + 360) % 360;

    Transform? MakeTransform()
    {
        if (RotationStyle == 1) return new RotateTransform(Heading - 90);
        if (RotationStyle == 2 && Heading > 180) return new ScaleTransform(-1, 1);
        return null;
    }

    void Update()
    {
        if (costumes.Count == 0) { View.IsVisible = false; return; }
        var c = costumes[Math.Clamp(Costume, 1, costumes.Count) - 1];
        var shown = c.Bmp;
        double fxColor = GetEffect("color"), fxBright = GetEffect("brightness"), fxFish = GetEffect("fisheye"),
               fxWhirl = GetEffect("whirl"), fxPixel = GetEffect("pixelate"), fxMosaic = GetEffect("mosaic");
        if (fxColor != 0 || fxBright != 0 || fxFish != 0 || fxWhirl != 0 || fxPixel != 0 || fxMosaic != 0)
            shown = Fx.Apply(c.FileName, fxColor, fxBright, fxFish, fxWhirl, fxPixel, fxMosaic) ?? c.Bmp;
        View.Source = shown;
        View.IsVisible = Visible;
        View.Opacity = Math.Clamp(1 - GetEffect("ghost") / 100, 0, 1);
        View.Stretch = Stretch.Fill;

        if (IsStage)
        {
            View.Width = Rt.StageW; View.Height = Rt.StageH;
            Canvas.SetLeft(View, 0); Canvas.SetTop(View, 0);
            return;
        }

        double pw = Math.Max(1, c.Bmp.PixelSize.Width), ph = Math.Max(1, c.Bmp.PixelSize.Height);
        double w = pw * Scale, h = ph * Scale;
        double left = Rt.StageW / 2 + X - c.Cx * Scale;
        double top = Rt.StageH / 2 - Y - c.Cy * Scale;
        View.Width = w; View.Height = h;
        Canvas.SetLeft(View, left); Canvas.SetTop(View, top);
        View.RenderTransformOrigin = new RelativePoint(c.Cx / pw, c.Cy / ph, RelativeUnit.Relative);
        View.RenderTransform = MakeTransform();

        if (bubble.IsVisible)
        {
            Canvas.SetLeft(bubble, left + w);
            Canvas.SetTop(bubble, Math.Max(0, top - 36));
        }
    }

    public Rect Box() => new(Canvas.GetLeft(View), Canvas.GetTop(View), View.Width, View.Height);

    // Bewegung
    public void GoTo(double x, double y)
    {
        if (PenDown && !IsStage) Rt.DrawLine(X, Y, x, y, PenColor(), PenSize);
        X = x; Y = y;
        Update();
    }

    public void SetHeading(double h) { Heading = Norm(h); Update(); }
    public void Turn(double deg) => SetHeading(Heading + deg);
    public void SetRotationStyle(int style) { RotationStyle = style; Update(); }

    public void Move(double steps)
    {
        double r = Heading * Math.PI / 180;
        GoTo(X + steps * Math.Sin(r), Y + steps * Math.Cos(r));
    }

    public async Task Glide(double secs, double x, double y)
    {
        double x0 = X, y0 = Y;
        var sw = Stopwatch.StartNew();
        while (true)
        {
            double t = secs <= 0 ? 1 : Math.Min(1, sw.Elapsed.TotalSeconds / secs);
            GoTo(x0 + (x - x0) * t, y0 + (y - y0) * t);
            if (t >= 1) break;
            await Rt.Frame();
        }
    }

    public void BounceOffEdge()
    {
        var b = Box();
        bool side = b.Left < 0 || b.Right > Rt.StageW;
        bool vert = b.Top < 0 || b.Bottom > Rt.StageH;
        if (side) Heading = -Heading;
        if (vert) Heading = 180 - Heading;
        if (side || vert)
        {
            Heading = Norm(Heading);
            X = Math.Clamp(X, -Rt.StageW / 2, Rt.StageW / 2);
            Y = Math.Clamp(Y, -Rt.StageH / 2, Rt.StageH / 2);
            Update();
        }
    }

    // Aussehen
    public void Show() { Visible = true; Update(); }
    public void Hide() { Visible = false; bubble.IsVisible = false; Update(); }
    public void SetScale(double s) { Scale = Math.Max(0.01, s); Update(); }

    public void Say(object? text)
    {
        string s = Rt.S(text);
        bubbleText.Text = s;
        bubble.IsVisible = s.Length > 0 && !IsStage;
        Update();
    }

    public async Task SayFor(object? text, double secs)
    {
        Say(text);
        await Rt.Wait(secs);
        Say("");
    }

    public string CostumeName() => costumes.Count == 0 ? "" : costumes[Math.Clamp(Costume, 1, costumes.Count) - 1].Name;

    void Switched()
    {
        Update();
        if (IsStage) Rt.Broadcast(CostumeName());          // "Wenn Hintergrund wechselt zu ..." ist bei der Bridge ein Nachrichten-Hut
    }

    public void SwitchCostume(object? x)
    {
        if (costumes.Count == 0) return;
        if (Rt.TryNum(x, out var d))
        {
            int n = (int)Math.Round(d);
            Costume = (((n - 1) % costumes.Count) + costumes.Count) % costumes.Count + 1;
        }
        else
        {
            string name = Rt.S(x);
            int i = costumes.FindIndex(c => c.Name == name);
            if (i < 0) return;
            Costume = i + 1;
        }
        Switched();
    }

    public void NextCostume()
    {
        if (costumes.Count == 0) return;
        Costume = Costume % costumes.Count + 1;
        Switched();
    }

    public object? CostumeAttr(string attr, object? which)
    {
        if (costumes.Count == 0) return 0d;
        int idx = Math.Clamp(Costume, 1, costumes.Count) - 1;
        string w = Rt.S(which);
        if (w != "" && w != "current")
            idx = Rt.TryNum(which, out var d) ? Math.Clamp((int)d, 1, costumes.Count) - 1 : Math.Max(0, costumes.FindIndex(c => c.Name == w));
        var cos = costumes[idx];
        return attr switch
        {
            "name" => cos.Name,
            "width" => (double)cos.Bmp.PixelSize.Width,
            "height" => (double)cos.Bmp.PixelSize.Height,
            _ => 0d
        };
    }

    // Effekte: ghost wirkt als Transparenz, alle anderen werden über Fx auf die Pixel gerechnet.
    public double GetEffect(string name) => effects.TryGetValue(name, out var v) ? v : 0;
    public void SetEffect(string name, double v) { effects[name] = v; Update(); }
    public void ChangeEffect(string name, double v) => SetEffect(name, GetEffect(name) + v);
    public void ClearEffects() { effects.Clear(); Update(); }

    public void ToFront()
    {
        Rt.Stage.Children.Remove(View);
        Rt.Stage.Children.Add(View);
    }

    public void Back(double layers)
    {
        int idx = Rt.Stage.Children.IndexOf(View);
        if (idx < 0) return;
        Rt.Stage.Children.RemoveAt(idx);
        Rt.Stage.Children.Insert(Math.Min(Math.Max(idx - (int)layers, 3), Rt.Stage.Children.Count), View);
    }

    // Stift
    public void Stamp()
    {
        if (costumes.Count == 0 || !Visible) return;
        var img = new Image
        {
            Source = View.Source, Width = View.Width, Height = View.Height, Stretch = Stretch.Fill,
            Opacity = View.Opacity, RenderTransformOrigin = View.RenderTransformOrigin, RenderTransform = MakeTransform()
        };
        Canvas.SetLeft(img, Canvas.GetLeft(View));
        Canvas.SetTop(img, Canvas.GetTop(View));
        Rt.PenLayer.Children.Add(img);
    }

    Color PenColor() => Hsv(penHue, penSat, penBri, penAlpha);

    static Color Hsv(double h100, double s100, double v100, double a100)
    {
        double h = (((h100 % 100) + 100) % 100) / 100 * 360, s = Math.Clamp(s100, 0, 100) / 100, v = Math.Clamp(v100, 0, 100) / 100;
        double c = v * s, x = c * (1 - Math.Abs((h / 60) % 2 - 1)), m = v - c;
        double r, g, b;
        if (h < 60) { r = c; g = x; b = 0; }
        else if (h < 120) { r = x; g = c; b = 0; }
        else if (h < 180) { r = 0; g = c; b = x; }
        else if (h < 240) { r = 0; g = x; b = c; }
        else if (h < 300) { r = x; g = 0; b = c; }
        else { r = c; g = 0; b = x; }
        return Color.FromArgb((byte)(Math.Clamp(a100, 0, 100) / 100 * 255), (byte)((r + m) * 255), (byte)((g + m) * 255), (byte)((b + m) * 255));
    }

    /// <summary>Snap!-Farbwert "r,g,b" oder "r,g,b,a" (r,g,b 0..255, a 0..1).</summary>
    public void SetPenColor(string spec)
    {
        var parts = spec.Split(',');
        if (parts.Length < 3) return;
        var v = new double[4] { 0, 0, 0, 1 };
        for (int i = 0; i < parts.Length && i < 4; i++)
            if (!double.TryParse(parts[i].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out v[i])) return;
        double r = v[0] / 255, g = v[1] / 255, b = v[2] / 255;
        double max = Math.Max(r, Math.Max(g, b)), min = Math.Min(r, Math.Min(g, b)), d = max - min, h = 0;
        if (d > 0)
        {
            if (max == r) h = ((g - b) / d) % 6;
            else if (max == g) h = (b - r) / d + 2;
            else h = (r - g) / d + 4;
            h *= 60;
            if (h < 0) h += 360;
        }
        penHue = h / 360 * 100;
        penSat = max == 0 ? 0 : d / max * 100;
        penBri = max * 100;
        penAlpha = v[3] * 100;
    }

    public void SetPenDim(string dim, double value, bool relative)
    {
        switch (dim.ToLowerInvariant())
        {
            case "hue": penHue = relative ? penHue + value : value; break;
            case "saturation": penSat = Math.Clamp(relative ? penSat + value : value, 0, 100); break;
            case "brightness": penBri = Math.Clamp(relative ? penBri + value : value, 0, 100); break;
            case "transparency": penAlpha = Math.Clamp(100 - (relative ? (100 - penAlpha) + value : value), 0, 100); break;
        }
    }

    public double GetPenAttr(string attr) => attr.ToLowerInvariant() switch
    {
        "hue" => penHue,
        "saturation" => penSat,
        "brightness" => penBri,
        "transparency" => 100 - penAlpha,
        "size" => PenSize,
        _ => 0
    };
}
""";
}
