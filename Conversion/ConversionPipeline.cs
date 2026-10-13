using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using CliWrap;
using Newtonsoft.Json;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

public sealed class ConversionOptions
{
    public string XmlPath = "";
    public string OutputFolder = "";
    public string? IconFile;
    public string TargetFramework = "net10.0";
    public string? PreferredName;                // z.B. Dateiname, wenn das Projekt nur "Project" heisst
    public Action<string>? Log;
    public Action<int>? Progress;
}

public sealed class ConversionResult
{
    public string ProjectDir = "";
    public string AppName = "";
    public ConversionReport Report = new();
    public int AssetsWritten, AssetsReused;
}

/// <summary>
/// Der ganze Weg an einer Stelle:
///   Snap!-XML  ->  JSON (Reihenfolge erhalten)  ->  Modell  ->  C#-/Avalonia-Projekt
/// Scratch (.sb/.sb2/.sb3) wird vorher per ScratchImport zu Snap!-XML.
/// </summary>
public static class ConversionPipeline
{
    public static ConversionResult Run(ConversionOptions o)
    {
        void Log(string m) => o.Log?.Invoke(m);
        void Step(int pct) => o.Progress?.Invoke(pct);

        Directory.CreateDirectory(o.OutputFolder);

        Log("Lese Snap!-XML");
        var root = XDocument.Load(o.XmlPath).Root ?? throw new InvalidOperationException("Leeres XML-Dokument.");
        Step(10);

        Log("Schreibe Project.json");
        var json = SnapJson.FromXml(root);
        File.WriteAllText(Path.Combine(o.OutputFolder, "Project.json"), json.ToString(Formatting.Indented));
        Step(25);

        var model = SnapJsonReader.Read(json);
        string rawName = string.IsNullOrWhiteSpace(model.Name) || model.Name == "Project"
            ? (o.PreferredName ?? model.Name) : model.Name;
        string appName = NameRegistry.ProjectName(rawName);
        model.Name = rawName;

        string projectDir = Path.Combine(o.OutputFolder, appName);
        string assetsDir = Path.Combine(projectDir, "Assets");
        if (Directory.Exists(assetsDir)) Directory.Delete(assetsDir, true);   // keine Reste alter Läufe
        Directory.CreateDirectory(assetsDir);

        bool hasIcon = false;
        if (!string.IsNullOrEmpty(o.IconFile) && File.Exists(o.IconFile))
        {
            string target = Path.Combine(assetsDir, "GameIcon.ico");
            if (Path.GetExtension(o.IconFile).Equals(".ico", StringComparison.OrdinalIgnoreCase))
                File.Copy(o.IconFile, target, true);
            else
                MainWindow.ConvertToIco(o.IconFile, target);
            hasIcon = true;
        }
        Step(40);

        Log("Erzeuge C#-Projekt " + appName);
        var assets = new AssetStore(assetsDir);
        var report = new ConversionReport();
        ProjectGenerator.Generate(model, projectDir, appName, o.TargetFramework, hasIcon, assets, report);
        Step(70);

        foreach (var kv in report.Unsupported) Log($"Nicht umgesetzt: {kv.Key} (x{kv.Value})");
        Log($"{report.Scripts} Skripte und {report.CustomBlocks} eigene Blöcke übersetzt, {report.IgnoredScripts} ohne Hut-Block übersprungen, " +
            $"{assets.Written} Dateien geschrieben, {assets.Reused} wiederverwendet");

        return new ConversionResult
        {
            ProjectDir = projectDir, AppName = appName, Report = report,
            AssetsWritten = assets.Written, AssetsReused = assets.Reused
        };
    }
}

/// <summary>Scratch (.sb, .sb2, .sb3) -> Snap!-XML mit den beiden Node-Werkzeugen aus den Submodulen.</summary>
public static class ScratchImport
{
    public static async Task<string> ToSnapXml(string input, string outputFolder, Action<string>? log)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string formatConverter = Path.Combine(baseDir, "Scratch-Format-Converter");   // .sb/.sb2 -> .sb3
        string bridge = Path.Combine(baseDir, "Scratch-To-Snap");                      // .sb3 -> Snap!-XML
        string ext = Path.GetExtension(input).ToLowerInvariant();
        string sb3 = input;

        if (ext == ".sb" || ext == ".sb2")
        {
            log?.Invoke("Konvertiere " + ext + " nach .sb3");
            string copy = Path.Combine(formatConverter, "project" + ext);
            string result = Path.Combine(formatConverter, "project.sb3");
            if (File.Exists(result)) File.Delete(result);
            File.Copy(input, copy, true);
            try { await Node(formatConverter, "convert.js"); }
            finally { File.Delete(copy); }
            if (!File.Exists(result)) throw new FileNotFoundException("convert.js hat keine project.sb3 erzeugt.", result);
            sb3 = result;
        }

        log?.Invoke("Konvertiere .sb3 nach Snap!-XML");
        string bridgeInput = Path.Combine(bridge, "project.sb3");
        string bridgeOutput = Path.Combine(bridge, "project.xml");
        if (File.Exists(bridgeOutput)) File.Delete(bridgeOutput);                      // sonst würde ein altes Ergebnis gelesen
        File.Copy(sb3, bridgeInput, true);
        await Node(bridge, "converter.js", "project.sb3", "project.xml");
        if (!File.Exists(bridgeOutput)) throw new FileNotFoundException("converter.js hat keine project.xml erzeugt.", bridgeOutput);

        Directory.CreateDirectory(outputFolder);
        string xml = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(input) + ".xml");
        File.Copy(bridgeOutput, xml, true);
        return xml;
    }

    static Task Node(string workingDir, params string[] args) =>
        Cli.Wrap("node").WithArguments(a => { foreach (var s in args) a.Add(s); })
           .WithWorkingDirectory(workingDir).ExecuteAsync().Task;
}
