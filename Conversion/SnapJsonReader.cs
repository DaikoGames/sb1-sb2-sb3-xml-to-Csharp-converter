using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>Liest das Projekt-JSON als Baum (statt Zeile für Zeile) in das Modell.</summary>
public static class SnapJsonReader
{
    static double D(string s, double fallback) =>
        double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : fallback;

    public static SnapProject Read(JToken json)
    {
        var root = JNode.FromJson(json);
        if (root.Tag != "project") throw new InvalidOperationException("Kein Snap!-Projekt (Wurzelelement ist nicht <project>).");

        var p = new SnapProject { Name = root.Attr("name", "Project") };
        p.Globals.AddRange(ReadVariables(root.First("variables")));

        var stage = root.First("stage") ?? throw new InvalidOperationException("Projekt hat keine <stage>.");
        p.Width = (int)D(stage.Attr("width"), 480);
        p.Height = (int)D(stage.Attr("height"), 360);

        ReadObject(stage, p.Stage);
        p.Stage.Name = "Stage";               // Stage heisst im Code immer gleich, egal wie sie in Snap heisst
        p.Stage.X = 0; p.Stage.Y = 0;

        var seen = new System.Collections.Generic.HashSet<string>();
        foreach (var sp in stage.First("sprites")?.Each("sprite") ?? Enumerable.Empty<JNode>())
        {
            var o = new SnapObject();
            ReadObject(sp, o);
            // Doppelte Sprite-Namen kann Snap! nicht erzeugen; Scratch-Konvertierungen manchmal schon.
            if (!seen.Add(o.Name)) o.Name += "_" + seen.Count;
            p.Sprites.Add(o);
        }
        return p;
    }

    static void ReadObject(JNode n, SnapObject o)
    {
        o.Name = n.Attr("name", "Object");
        o.X = D(n.Attr("x"), 0);
        o.Y = D(n.Attr("y"), 0);
        o.Heading = D(n.Attr("heading"), 90);
        o.Scale = D(n.Attr("scale"), 1);
        o.Rotation = (int)D(n.Attr("rotation"), 1);
        o.Costume = Math.Max(1, (int)D(n.Attr("costume"), 1));
        o.Hidden = n.Attr("hidden") == "true";

        foreach (var c in n.First("costumes")?.First("list")?.Each("item") ?? Enumerable.Empty<JNode>())
        {
            var cn = c.First("costume");
            if (cn == null) continue;
            o.Costumes.Add(new CostumeData(cn.Attr("name", "costume"), D(cn.Attr("center-x"), 0), D(cn.Attr("center-y"), 0), cn.Attr("image")));
        }
        foreach (var s in n.First("sounds")?.First("list")?.Each("item") ?? Enumerable.Empty<JNode>())
        {
            var sn = s.First("sound");
            if (sn != null) o.Sounds.Add(new SoundData(sn.Attr("name", "sound"), sn.Attr("sound")));
        }
        o.Locals.AddRange(ReadVariables(n.First("variables")));
        o.Scripts.AddRange(n.First("scripts")?.Each("script") ?? Enumerable.Empty<JNode>());
    }

    static System.Collections.Generic.IEnumerable<Variable> ReadVariables(JNode? vars)
    {
        if (vars == null) yield break;
        foreach (var v in vars.Each("variable"))
        {
            var first = v.Kids.FirstOrDefault();
            bool isList = first != null && first.Tag == "list";
            yield return new Variable(v.Attr("name"), first != null && first.Tag == "l" ? first.Text : "0", isList);
        }
    }
}
