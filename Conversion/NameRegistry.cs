using System.Collections.Generic;
using System.Text;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Vergibt gültige, eindeutige C#-Bezeichner. Derselbe Name (pro Art) liefert immer denselben
/// Bezeichner zurück, er wird also wiederverwendet statt neu erzeugt.
/// </summary>
public sealed class NameRegistry
{
    readonly Dictionary<string, string> map = new();
    readonly HashSet<string> used = new();

    public string Get(string kind, string raw)
    {
        string key = kind + "\0" + raw;
        if (map.TryGetValue(key, out var existing)) return existing;

        string ident = Sanitize(raw);
        string candidate = kind + "_" + ident;
        for (int i = 2; !used.Add(candidate); i++) candidate = kind + "_" + ident + "_" + i;
        map[key] = candidate;
        return candidate;
    }

    public static string Sanitize(string raw)
    {
        var sb = new StringBuilder();
        foreach (char c in raw) sb.Append(char.IsLetterOrDigit(c) && c < 128 ? c : '_');
        return sb.Length == 0 ? "x" : sb.ToString();
    }

    /// <summary>Gültiger Namespace/Projektname (keine Leerzeichen, nicht mit Ziffer beginnend, kein Typname des Generats).</summary>
    // Namen, die im erzeugten Projekt sonst mit eigenen oder Avalonia-/System-Typen kollidieren würden.
    static readonly HashSet<string> Reserved = new()
    {
        "Rt", "Sprite", "App", "MainWindow", "Program", "Avalonia", "System", "Math", "Task",
        "Canvas", "Window", "Image", "Point", "Rect", "Key", "Border", "TextBlock", "Bitmap", "Brushes", "Control"
    };

    public static string ProjectName(string raw)
    {
        string s = Sanitize(raw.Trim());
        if (char.IsDigit(s[0])) s = "_" + s;
        if (Reserved.Contains(s)) s += "_App";
        return s;
    }
}
