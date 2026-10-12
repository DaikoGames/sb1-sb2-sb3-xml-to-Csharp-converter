namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Die Laufzeitbibliothek, die in jedes erzeugte Projekt kopiert wird (als Runtime.cs).
/// Alles, was mehrere Blöcke brauchen (Bewegung, Sprechblasen, Variablen, Nachrichten, Tasten ...),
/// steht hier EINMAL, der erzeugte Code pro Block ist nur noch ein Aufruf.
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
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace __NS__;

public static class Rt
{
    public static Canvas Stage = null!;
    public static double StageW = 480, StageH = 360;
    public static readonly Dictionary<string, object?> Vars = new();
    public static readonly Dictionary<string, Sprite> Sprites = new();
    public static double MouseX, MouseY, Volume = 100;
    public static bool MouseDown;
    public static string Answer = "";

    static readonly Dictionary<string, List<Func<Task>>> messageHandlers = new();
    static readonly Dictionary<string, List<Func<Task>>> keyHandlers = new();
    static readonly Dictionary<Sprite, List<Func<Task>>> clickHandlers = new();
    static readonly HashSet<string> keysDown = new();
    static readonly AsyncLocal<int> generation = new();
    static int stopGeneration;
    static DateTime timerStart = DateTime.UtcNow;

    public static string AssetPath(string file) => Path.Combine(AppContext.BaseDirectory, "Assets", file);

    // ---------- Aufbau ----------

    public static Sprite AddSprite(string name, bool isStage)
    {
        var s = new Sprite(name, isStage);
        Sprites[name] = s;
        return s;
    }

    public static void Attach(Window window, Canvas stage, double w, double h)
    {
        Stage = stage; StageW = w; StageH = h;
        stage.PointerMoved += (_, e) =>
        {
            var p = e.GetPosition(stage);
            MouseX = p.X - StageW / 2;
            MouseY = StageH / 2 - p.Y;
        };
        stage.PointerPressed += (_, _) => MouseDown = true;
        stage.PointerReleased += (_, _) => MouseDown = false;
        window.KeyDown += (_, e) =>
        {
            string k = KeyName(e.Key);
            if (keysDown.Add(k)) { Fire(keyHandlers, k); Fire(keyHandlers, "*"); }
        };
        window.KeyUp += (_, e) => keysDown.Remove(KeyName(e.Key));
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

    // ---------- Skripte, Nachrichten ----------

    public static void Run(Func<Task> f) => _ = RunAsync(f);

    static async Task RunAsync(Func<Task> f)
    {
        generation.Value = stopGeneration;
        try { await f(); }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Console.Error.WriteLine(ex); }
    }

    static void Check()
    {
        if (generation.Value != stopGeneration) throw new OperationCanceledException();
    }

    public static void StopAll() { stopGeneration++; StopSounds(); }

    static void Add<T>(Dictionary<T, List<Func<Task>>> d, T key, Func<Task> f) where T : notnull
    {
        if (!d.TryGetValue(key, out var list)) d[key] = list = new List<Func<Task>>();
        list.Add(f);
    }

    static void Fire<T>(Dictionary<T, List<Func<Task>>> d, T key) where T : notnull
    {
        if (d.TryGetValue(key, out var list)) foreach (var f in list.ToArray()) Run(f);
    }

    public static void OnMessage(string m, Func<Task> f) => Add(messageHandlers, m, f);
    public static void OnKey(string k, Func<Task> f) => Add(keyHandlers, k.ToLowerInvariant(), f);
    public static void OnClick(Sprite s, Func<Task> f) => Add(clickHandlers, s, f);
    public static void FireClick(Sprite s) => Fire(clickHandlers, s);

    public static void Broadcast(string m) { Fire(messageHandlers, m); Fire(messageHandlers, "*"); }

    public static async Task BroadcastAndWait(string m)
    {
        var all = new List<Func<Task>>();
        if (messageHandlers.TryGetValue(m, out var a)) all.AddRange(a);
        if (messageHandlers.TryGetValue("*", out var b)) all.AddRange(b);
        await Task.WhenAll(all.Select(RunAsync));
        Check();
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
        if (TryNum(a, out var x) && TryNum(b, out var y)) return x == y;
        return string.Equals(S(a), S(b), StringComparison.OrdinalIgnoreCase);
    }

    public static double Mod(double a, double b) => b == 0 ? 0 : ((a % b) + b) % b;

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

    // ---------- Variablen und Listen ----------

    public static object? Get(string key) => Vars.TryGetValue(key, out var v) ? v : 0d;
    public static void Set(string key, object? value) => Vars[key] = value;
    public static void Change(string key, double delta) => Vars[key] = N(Get(key)) + delta;

    public static List<object?> NewList() => new();
    static List<object?>? L(object? o) => o as List<object?>;
    public static double ListLen(object? list) => L(list)?.Count ?? 0;
    public static void ListAdd(object? list, object? item) => L(list)?.Add(item);
    public static bool ListContains(object? list, object? item) => L(list)?.Any(x => Eq(x, item)) ?? false;

    public static double ListIndexOf(object? item, object? list)
    {
        var l = L(list);
        if (l == null) return 0;
        int i = l.FindIndex(x => Eq(x, item));
        return i + 1;
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

    // ---------- Sprites untereinander ----------

    public static void GoToObject(Sprite me, string target)
    {
        if (target == "random position")
            me.GoTo(Random(-StageW / 2, StageW / 2), Random(-StageH / 2, StageH / 2));
        else if (target == "mouse-pointer")
            me.GoTo(MouseX, MouseY);
        else if (Sprites.TryGetValue(target, out var o))
            me.GoTo(o.X, o.Y);
    }

    static bool Target(Sprite me, string target, out double x, out double y)
    {
        x = y = 0;
        if (target == "mouse-pointer") { x = MouseX; y = MouseY; return true; }
        if (Sprites.TryGetValue(target, out var o)) { x = o.X; y = o.Y; return true; }
        return false;
    }

    public static void FaceTowards(Sprite me, string target)
    {
        if (Target(me, target, out var x, out var y))
            me.SetHeading(Math.Atan2(x - me.X, y - me.Y) * 180 / Math.PI);
    }

    public static double Distance(Sprite me, string target) =>
        Target(me, target, out var x, out var y) ? Math.Sqrt((x - me.X) * (x - me.X) + (y - me.Y) * (y - me.Y)) : 0;

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
                return Sprites.TryGetValue(target, out var o) && o != me && o.Visible && box.Intersects(o.Box());
        }
    }

    // ---------- Klang (LibVLC, optional: ohne VLC bleibt das Projekt stumm) ----------

    static LibVLCSharp.Shared.LibVLC? vlc;
    static bool vlcFailed;
    static readonly List<LibVLCSharp.Shared.MediaPlayer> players = new();

    static LibVLCSharp.Shared.MediaPlayer? StartSound(Sprite me, string name)
    {
        string? file = me.SoundFile(name) ?? Sprites.Values.Select(s => s.SoundFile(name)).FirstOrDefault(f => f != null);
        if (file == null || vlcFailed) return null;
        try
        {
            if (vlc == null) { LibVLCSharp.Shared.Core.Initialize(); vlc = new LibVLCSharp.Shared.LibVLC(); }
            var media = new LibVLCSharp.Shared.Media(vlc, AssetPath(file), LibVLCSharp.Shared.FromType.FromPath);
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
}

public sealed class Sprite
{
    public readonly string Name;
    public readonly bool IsStage;
    public double X, Y, Heading = 90, Scale = 1;
    public bool Visible = true;
    public int Costume = 1;
    public readonly Image View = new();

    int rotationStyle = 1;
    double ghost;
    readonly List<(string Name, Bitmap Bmp, double Cx, double Cy)> costumes = new();
    readonly Dictionary<string, string> sounds = new();
    readonly Border bubble = new();
    readonly TextBlock bubbleText = new();

    public Sprite(string name, bool isStage)
    {
        Name = name;
        IsStage = isStage;
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
        View.PointerPressed += (_, _) => Rt.FireClick(this);
    }

    public void AddCostume(string name, string file, double cx, double cy)
    {
        try { costumes.Add((name, new Bitmap(Rt.AssetPath(file)), cx, cy)); }
        catch (Exception ex) { Console.Error.WriteLine("Kostüm '" + file + "' fehlt: " + ex.Message); }
    }

    public void AddSound(string name, string file) => sounds[name] = file;
    public string? SoundFile(string name) => sounds.TryGetValue(name, out var f) ? f : null;

    public void Init(double x, double y, double heading, double scale, int rotation, int costume, bool hidden)
    {
        X = x; Y = y; Heading = heading; Scale = scale;
        rotationStyle = rotation; Costume = costume; Visible = !hidden;
        Rt.Stage.Children.Add(View);
        if (!IsStage) Rt.Stage.Children.Add(bubble);
        Update();
    }

    static double Norm(double h) => ((h % 360) + 360) % 360;

    void Update()
    {
        if (costumes.Count == 0) { View.IsVisible = false; return; }
        var c = costumes[Math.Clamp(Costume, 1, costumes.Count) - 1];
        View.Source = c.Bmp;
        View.IsVisible = Visible;
        View.Opacity = 1 - ghost / 100;
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
        View.RenderTransform = rotationStyle == 1 ? new RotateTransform(Heading - 90) : null;

        if (bubble.IsVisible)
        {
            Canvas.SetLeft(bubble, left + w);
            Canvas.SetTop(bubble, Math.Max(0, top - 36));
        }
    }

    public Rect Box() => new(Canvas.GetLeft(View), Canvas.GetTop(View), View.Width, View.Height);

    // Bewegung
    public void GoTo(double x, double y) { X = x; Y = y; Update(); }
    public void SetHeading(double h) { Heading = Norm(h); Update(); }
    public void Turn(double deg) => SetHeading(Heading + deg);

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
            if (i >= 0) Costume = i + 1;
        }
        Update();
    }

    public void NextCostume()
    {
        if (costumes.Count == 0) return;
        Costume = Costume % costumes.Count + 1;
        Update();
    }

    public void SetEffect(string name, double v) { if (name == "ghost") { ghost = Math.Clamp(v, 0, 100); Update(); } }
    public void ChangeEffect(string name, double v) { if (name == "ghost") SetEffect(name, ghost + v); }
    public void ClearEffects() { ghost = 0; Update(); }

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
        Rt.Stage.Children.Insert(Math.Max(1, idx - (int)layers), View);
    }
}
""";
}
