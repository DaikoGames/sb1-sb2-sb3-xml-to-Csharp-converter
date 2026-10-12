using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

/// <summary>
/// Erzeugt das komplette Avalonia-Projekt aus dem Modell. Jede Datei wird genau einmal komplett
/// geschrieben (statt tausender File.AppendAllText-Aufrufe). Kein "dotnet new" nötig.
/// Versionen stehen nur hier, an einer Stelle.
/// </summary>
public static class ProjectGenerator
{
    const string AvaloniaVersion = "12.1.1";
    const string LibVlcSharpVersion = "3.10.1";
    const string LibVlcWindowsVersion = "3.0.23.1";
    const string LibVlcMacVersion = "3.1.3.1";

    static string Inv(double d) => d.ToString("R", CultureInfo.InvariantCulture);

    public static void Generate(SnapProject p, string projectDir, string appName, string targetFramework,
                                bool hasIcon, AssetStore assets, ConversionReport report)
    {
        Directory.CreateDirectory(projectDir);

        var names = new NameRegistry();
        var fields = new StringBuilder();
        var build = new StringBuilder();
        var methods = new StringBuilder();
        var starts = new StringBuilder();

        // Globale Variablen einmal initialisieren, lokale pro Objekt (Schlüssel "Objekt::Name").
        foreach (var v in p.Globals) build.AppendLine("        " + VarInit(v.Name, v));

        foreach (var o in p.AllObjects)
        {
            string field = names.Get("spr", o.Name);
            fields.AppendLine($"    private Sprite {field} = null!;");

            build.AppendLine($"        {field} = Rt.AddSprite({BlockTranslator.Esc(o.Name)}, {(o.IsStage ? "true" : "false")});");
            foreach (var c in o.Costumes)
            {
                string? file = assets.Save(c.Data, o.Name + "_" + c.Name);
                if (file == null) { report.Add("Kostüm ungültig: " + o.Name + "/" + c.Name); continue; }
                build.AppendLine($"        {field}.AddCostume({BlockTranslator.Esc(c.Name)}, {BlockTranslator.Esc(file)}, {Inv(c.Cx)}, {Inv(c.Cy)});");
            }
            foreach (var s in o.Sounds)
            {
                string? file = assets.Save(s.Data, o.Name + "_" + s.Name);
                if (file == null) { report.Add("Sound ungültig: " + o.Name + "/" + s.Name); continue; }
                build.AppendLine($"        {field}.AddSound({BlockTranslator.Esc(s.Name)}, {BlockTranslator.Esc(file)});");
            }
            foreach (var v in o.Locals) build.AppendLine("        " + VarInit(o.Name + "::" + v.Name, v));
            build.AppendLine($"        {field}.Init({Inv(o.X)}, {Inv(o.Y)}, {Inv(o.Heading)}, {Inv(o.Scale)}, {o.Rotation}, {o.Costume}, {(o.Hidden ? "true" : "false")});");
            build.AppendLine();

            var tr = new BlockTranslator(o, field, report);
            int n = 0;
            foreach (var script in o.Scripts)
            {
                string method = names.Get("Script", o.Name + "_" + (n + 1));
                var r = tr.TranslateScript(script, method);
                if (r == null) continue;
                n++;
                methods.AppendLine(r.Value.Method);
                starts.AppendLine("        " + r.Value.Registration);
            }
        }

        string w = p.Width.ToString(CultureInfo.InvariantCulture), h = p.Height.ToString(CultureInfo.InvariantCulture);
        string title = BlockTranslator.Esc(p.Name);

        var cs = new StringBuilder();
        cs.AppendLine("using System;");
        cs.AppendLine("using System.Collections.Generic;");
        cs.AppendLine("using System.IO;");
        cs.AppendLine("using System.Threading.Tasks;");
        cs.AppendLine("using Avalonia.Controls;");
        cs.AppendLine("using Avalonia.Media;");
        cs.AppendLine();
        cs.AppendLine($"namespace {appName};");
        cs.AppendLine();
        cs.AppendLine("public partial class MainWindow : Window");
        cs.AppendLine("{");
        cs.Append(fields);
        cs.AppendLine();
        cs.AppendLine("    public MainWindow()");
        cs.AppendLine("    {");
        cs.AppendLine("        InitializeComponent();");
        cs.AppendLine($"        Title = {title};");
        if (hasIcon)
        {
            cs.AppendLine("        string iconFile = Rt.AssetPath(\"GameIcon.ico\");");
            cs.AppendLine("        if (File.Exists(iconFile)) Icon = new WindowIcon(iconFile);");
        }
        cs.AppendLine($"        var stage = new Canvas {{ Width = {w}, Height = {h}, Background = Brushes.White, ClipToBounds = true }};");
        cs.AppendLine("        Content = stage;");
        cs.AppendLine($"        Rt.Attach(this, stage, {w}, {h});");
        cs.AppendLine("        BuildObjects();");
        cs.AppendLine("        Opened += (_, _) => StartScripts();");
        cs.AppendLine("    }");
        cs.AppendLine();
        cs.AppendLine("    private void BuildObjects()");
        cs.AppendLine("    {");
        cs.Append(build);
        cs.AppendLine("    }");
        cs.AppendLine();
        cs.AppendLine("    private void StartScripts()");
        cs.AppendLine("    {");
        cs.Append(starts);
        cs.AppendLine("    }");
        cs.AppendLine();
        cs.Append(methods);
        cs.AppendLine("}");

        Write(projectDir, "MainWindow.axaml.cs", cs.ToString());
        Write(projectDir, "MainWindow.axaml", $$"""
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="{{appName}}.MainWindow"
        Width="{{w}}" Height="{{h}}" CanResize="False"
        RequestedThemeVariant="Light">
</Window>
""");
        Write(projectDir, "App.axaml", $$"""
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="{{appName}}.App"
             RequestedThemeVariant="Light">
  <Application.Styles>
    <FluentTheme />
  </Application.Styles>
</Application>
""");
        Write(projectDir, "App.axaml.cs", $$"""
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace {{appName}};

public partial class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();
        base.OnFrameworkInitializationCompleted();
    }
}
""");
        Write(projectDir, "Program.cs", $$"""
using System;
using Avalonia;

namespace {{appName}};

internal static class Program
{
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace();
}
""");
        Write(projectDir, "Runtime.cs", RuntimeTemplate.Source.Replace("__NS__", appName));
        string iconLine = hasIcon ? "\n    <ApplicationIcon>Assets\\GameIcon.ico</ApplicationIcon>" : "";
        Write(projectDir, appName + ".csproj", $$"""
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>{{targetFramework}}</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>disable</ImplicitUsings>
    <AssemblyName>{{appName}}</AssemblyName>
    <RootNamespace>{{appName}}</RootNamespace>
    <NoWarn>$(NoWarn);CS1998;CS0162;CS0219;CS8618</NoWarn>{{iconLine}}
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Avalonia" Version="{{AvaloniaVersion}}" />
    <PackageReference Include="Avalonia.Desktop" Version="{{AvaloniaVersion}}" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="{{AvaloniaVersion}}" />
    <PackageReference Include="LibVLCSharp" Version="{{LibVlcSharpVersion}}" />
    <PackageReference Include="VideoLAN.LibVLC.Windows" Version="{{LibVlcWindowsVersion}}" />
    <PackageReference Include="VideoLAN.LibVLC.Mac" Version="{{LibVlcMacVersion}}" />
  </ItemGroup>
  <ItemGroup>
    <None Include="Assets\**\*" CopyToOutputDirectory="PreserveNewest" />
  </ItemGroup>
</Project>
""");
    }

    static string VarInit(string key, Variable v) =>
        $"Rt.Vars[{BlockTranslator.Esc(key)}] = " +
        (v.IsList ? "new List<object?>();" : BlockTranslator.LiteralCode(v.Initial) + ";");

    static void Write(string dir, string file, string text) =>
        File.WriteAllText(Path.Combine(dir, file), text, new UTF8Encoding(false));
}
