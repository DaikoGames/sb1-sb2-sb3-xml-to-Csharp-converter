using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Reihenfolge-erhaltende JSON-Darstellung eines Snap!-XML-Elements.
/// Wichtig: Newtonsoft's SerializeXmlNode verliert die Reihenfolge gemischter Kinder
/// (&lt;l&gt;, &lt;block&gt;, &lt;script&gt; werden zu getrennten Arrays). Bei Blöcken ist die Reihenfolge
/// der Eingaben aber die Bedeutung (z.B. doSayFor: erst Text, dann Sekunden).
/// Deshalb: { "tag": "...", "attrs": {...}, "text": "...", "kids": [ ... ] }
/// </summary>
public static class SnapJson
{
    public static JObject FromXml(XElement e)
    {
        var o = new JObject { ["tag"] = e.Name.LocalName };
        if (e.HasAttributes)
        {
            var a = new JObject();
            foreach (var at in e.Attributes()) a[at.Name.LocalName] = at.Value;
            o["attrs"] = a;
        }
        if (e.HasElements) o["kids"] = new JArray(e.Elements().Select(FromXml));
        else if (e.Value.Length > 0) o["text"] = e.Value;
        return o;
    }
}

/// <summary>Leichtgewichtiger Knoten, der aus dem JSON gelesen wird.</summary>
public sealed class JNode
{
    public string Tag = "";
    public string Text = "";
    public Dictionary<string, string> Attrs = new();
    public List<JNode> Kids = new();

    public string Attr(string name, string fallback = "") => Attrs.TryGetValue(name, out var v) ? v : fallback;
    public IEnumerable<JNode> Each(string tag) => Kids.Where(k => k.Tag == tag);
    public JNode? First(string tag) => Kids.FirstOrDefault(k => k.Tag == tag);

    public static JNode FromJson(JToken t)
    {
        var n = new JNode { Tag = (string?)t["tag"] ?? "", Text = (string?)t["text"] ?? "" };
        if (t["attrs"] is JObject a)
            foreach (var p in a.Properties()) n.Attrs[p.Name] = (string?)p.Value ?? "";
        if (t["kids"] is JArray k)
            foreach (var c in k) n.Kids.Add(FromJson(c));
        return n;
    }
}

public sealed record Variable(string Name, string Initial, bool IsList);
public sealed record CostumeData(string Name, double Cx, double Cy, string Data);
public sealed record SoundData(string Name, string Data);

public sealed class SnapObject
{
    public string Name = "";
    public bool IsStage;
    public double X, Y, Heading = 90, Scale = 1;
    public int Rotation = 1;
    public int Costume = 1;
    public bool Hidden;
    public List<CostumeData> Costumes = new();
    public List<SoundData> Sounds = new();
    public List<Variable> Locals = new();
    public List<JNode> Scripts = new();
    public List<JNode> Blocks = new();            // eigene Blöcke (block-definition), nur für dieses Objekt
}

public sealed class SnapProject
{
    public string Name = "Project";
    public string StageName = "Stage";
    public List<JNode> Blocks = new();            // globale eigene Blöcke
    public int Width = 480, Height = 360;
    public List<Variable> Globals = new();
    public SnapObject Stage = new() { IsStage = true, Name = "Stage" };
    public List<SnapObject> Sprites = new();
    public IEnumerable<SnapObject> AllObjects => new[] { Stage }.Concat(Sprites);
}
