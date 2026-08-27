using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LibVLC.Linux;
using Velopack;

namespace sb1_sb2_sb3_xml_to_Csharp_converter;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        VelopackApp.Build().Run();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            if (OperatingSystem.IsLinux())
            {
                var installer = new LibVLCLinux();
                installer.InstallVLC();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}