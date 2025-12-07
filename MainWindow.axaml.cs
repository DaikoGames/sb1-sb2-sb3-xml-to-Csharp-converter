//This Code belongs to Daiko Games - it is copyrighted - don´t use it without permission
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using ExCSS;
//Gotta check if I still need SVG when I have ImageMagick
using ImageMagick;
using LibVLCSharp.Shared;
using LibVLCSharp.Avalonia;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

namespace sb1_sb2_sb3_xml_to_Csharp_converter;

public partial class MainWindow : Window
{
    public bool windows = false;
    public bool linux = false;
    public bool MacOs = false;
    public string Filename;
    public string fileName;
    public string ScratchExeFile;
    public string Foldername;
    public string ApplicationName;
    public bool deletingduplicates;
    public bool Folder;
    public string GameFolder;
    public int LineNumber;
    public byte[] svgDocumentPngString;
    public string OSToBuild;
    public string specificIOS;
    public string formatOfApplication;
    public string extensionS;
    public string ICON;
    public string LastObject;
    public string LastAXAMLname;
    public int SoundNumber = 0;


    bool LastLight = false;

    public MainWindow()
    {
        InitializeComponent();
        Core.Initialize();
        this.Icon = new WindowIcon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "ConverterIcon", "Converter.ico"));
        Theme();
        Task.Run(() => ThemeChange());
    }

    public async Task ThemeChange()
    {
        while (true)
        {
            this.ActualThemeVariantChanged += (_, args) =>
            {
                Theme();
            };
            await Task.Delay(60000);
        }
    }

    public async void Theme()
    {
        if (ActualThemeVariant == ThemeVariant.Dark)
        {
            LastLight = false;
            // i need to check the default background colour and put that in XD
            CanvasBackground.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(38, 38, 38));
            Text1.Background = Avalonia.Media.Brushes.Transparent;
            Text1.Foreground = Avalonia.Media.Brushes.White;
            FileFolderNameTextBox.Background = Avalonia.Media.Brushes.Transparent;
            FileFolderNameTextBox.Foreground = Avalonia.Media.Brushes.White;
            fileSearcher.Background = Avalonia.Media.Brushes.Transparent;
            fileSearcher.Foreground = Avalonia.Media.Brushes.White;
            fileSearcher.BorderBrush = Avalonia.Media.Brushes.White;
            Text2.Background = Avalonia.Media.Brushes.Transparent;
            Text2.Foreground = Avalonia.Media.Brushes.White;
            FolderNameTextBox.Background = Avalonia.Media.Brushes.Transparent;
            FolderNameTextBox.Foreground = Avalonia.Media.Brushes.White;
            folderSearcher.Background = Avalonia.Media.Brushes.Transparent;
            folderSearcher.Foreground = Avalonia.Media.Brushes.White;
            folderSearcher.BorderBrush = Avalonia.Media.Brushes.White;
            IconTextBox.Background = Avalonia.Media.Brushes.Transparent;
            IconTextBox.Foreground = Avalonia.Media.Brushes.White;
            IconButton.Background = Avalonia.Media.Brushes.Transparent;
            IconButton.Foreground = Avalonia.Media.Brushes.White;
            IconButton.BorderBrush = Avalonia.Media.Brushes.White;
            WindowsCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            WindowsCheckBox.Foreground = Avalonia.Media.Brushes.White;
            LinuxCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            LinuxCheckBox.Foreground = Avalonia.Media.Brushes.White;
            MacOSCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            MacOSCheckBox.Foreground = Avalonia.Media.Brushes.White;
            X64CheckBox.Background = Avalonia.Media.Brushes.Transparent;
            X64CheckBox.Foreground = Avalonia.Media.Brushes.White;
            X32CheckBox.Background = Avalonia.Media.Brushes.Transparent;
            X32CheckBox.Foreground = Avalonia.Media.Brushes.White;
            ProgressBarConverter.Background = Avalonia.Media.Brushes.Transparent;
            ProgressBarConverter.Foreground = Avalonia.Media.Brushes.White;
            csonvertButton.Background = Avalonia.Media.Brushes.Transparent;
            csonvertButton.Foreground = Avalonia.Media.Brushes.White;
            csonvertButton.BorderBrush = Avalonia.Media.Brushes.White;
            GithubRepo.Background = Avalonia.Media.Brushes.Transparent;
            GithubRepo.Foreground = Avalonia.Media.Brushes.White;
        }

        if (ActualThemeVariant == ThemeVariant.Light)
        {
            LastLight = true;
            CanvasBackground.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(255, 255, 255));
            Text1.Background = Avalonia.Media.Brushes.Transparent;
            Text1.Foreground = Avalonia.Media.Brushes.Black;
            FileFolderNameTextBox.Background = Avalonia.Media.Brushes.Transparent;
            FileFolderNameTextBox.Foreground = Avalonia.Media.Brushes.Black;
            fileSearcher.Background = Avalonia.Media.Brushes.Transparent;
            fileSearcher.Foreground = Avalonia.Media.Brushes.Black;
            fileSearcher.BorderBrush = Avalonia.Media.Brushes.Black;
            Text2.Background = Avalonia.Media.Brushes.Transparent;
            Text2.Foreground = Avalonia.Media.Brushes.Black;
            FolderNameTextBox.Background = Avalonia.Media.Brushes.Transparent;
            FolderNameTextBox.Foreground = Avalonia.Media.Brushes.Black;
            folderSearcher.Background = Avalonia.Media.Brushes.Transparent;
            folderSearcher.Foreground = Avalonia.Media.Brushes.Black;
            folderSearcher.BorderBrush = Avalonia.Media.Brushes.Black;
            IconTextBox.Background = Avalonia.Media.Brushes.Transparent;
            IconTextBox.Foreground = Avalonia.Media.Brushes.Black;
            IconButton.Background = Avalonia.Media.Brushes.Transparent;
            IconButton.Foreground = Avalonia.Media.Brushes.Black;
            IconButton.BorderBrush = Avalonia.Media.Brushes.Black;
            WindowsCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            WindowsCheckBox.Foreground = Avalonia.Media.Brushes.Black;
            LinuxCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            LinuxCheckBox.Foreground = Avalonia.Media.Brushes.Black;
            MacOSCheckBox.Background = Avalonia.Media.Brushes.Transparent;
            MacOSCheckBox.Foreground = Avalonia.Media.Brushes.Black;
            X64CheckBox.Background = Avalonia.Media.Brushes.Transparent;
            X64CheckBox.Foreground = Avalonia.Media.Brushes.Black;
            X32CheckBox.Background = Avalonia.Media.Brushes.Transparent;
            X32CheckBox.Foreground = Avalonia.Media.Brushes.Black;
            ProgressBarConverter.Background = Avalonia.Media.Brushes.Transparent;
            ProgressBarConverter.Foreground = Avalonia.Media.Brushes.Black;
            csonvertButton.Background = Avalonia.Media.Brushes.Transparent;
            csonvertButton.Foreground = Avalonia.Media.Brushes.Black;
            csonvertButton.BorderBrush = Avalonia.Media.Brushes.Black;
            GithubRepo.Background = Avalonia.Media.Brushes.Transparent;
            GithubRepo.Foreground = Avalonia.Media.Brushes.Black;
        }
        //on Windows there are filters for Colorblind people, but on linux its not on every OS, i gotta think if i add that feature or not 
        //Ok so i checked the colours, the grayscale and inverted grayscale or generally inverted colours are looking bad for me I need help from colourblind ppl :) 
    }

    public async void FileSearcherVoid(object sender, RoutedEventArgs args)
    {
        /*var openFileDialog = new OpenFileDialog
        {
            // You can specify filters here, such as file extensions
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter() { Name = "All supported Filetypes", Extensions = new List<string> { "sb", "sb2", "sb3", "xml"} },
                new FileDialogFilter() { Name = ".sb Files", Extensions = new List<string> { "sb" } },
                new FileDialogFilter() { Name = ".sb2 Files", Extensions = new List<string> { "sb2" } },
                new FileDialogFilter() { Name = ".sb3 Files", Extensions = new List<string> { "sb3" } },
                new FileDialogFilter() { Name = ".xml Files", Extensions = new List<string> { "xml" } },
            }
        };

        var result = await Task.Run(() => openFileDialog.ShowAsync(this));  // 'this' is the current window

        if (result != null && result.Length > 0)
        {
            string selectedFile = result[0];  // Get the first file selected - this is null - I don´t know why?
            fileName = Path.GetFileName(selectedFile);
            Filename = selectedFile;
            FileFolderNameTextBox.Text = selectedFile;
            extensionS = Path.GetExtension(selectedFile);
        }*/

        var TopLevelg = TopLevel.GetTopLevel(this);
        var files = await TopLevelg.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "File Chooser",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("All Filetypes")
                {
                    Patterns = new[] { "*.sb", "*.sb2", "*.sb3", "*.xml"}
                },
                new FilePickerFileType("sb Files")
                {
                    Patterns = new[] { "*.sb" }
                },
                new FilePickerFileType("sb2 Files")
                {
                    Patterns = new[] { "*.sb2" }
                },
                new FilePickerFileType("sb3 Files")
                {
                    Patterns = new[] { "*.sb3" }
                },
                new FilePickerFileType("xml Files")
                {
                    Patterns = new[] { "*.xml" }
                }
            }
        });

        
        if (files.Count >= 1)
        {
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            var fileConent = await streamReader.ReadToEndAsync();
            var selectedFileg = files[0];  // Get the first file selected - this is null - I don´t know why?
            if (selectedFileg.TryGetLocalPath() is string selectedFile)
            {
                fileName = Path.GetFileName(selectedFile);
                Filename = selectedFile;
                FileFolderNameTextBox.Text = selectedFile;
                extensionS = Path.GetExtension(selectedFile);
            }
        }
    }

    public async void FolderSearcherVoid(object sender, RoutedEventArgs args)
    {
        /*var OpenFolderDialog = new OpenFolderDialog
        {
            Title = "Select a Folder",
            Directory = Directory.GetCurrentDirectory()
        };

        var result = await OpenFolderDialog.ShowAsync(this);
        Foldername = result;
        FolderNameTextBox.Text = result;*/
        var FolderPickerDialog = new FolderPickerOpenOptions
        {
            Title = "Folder Chooser"
        };
        var folderg = await StorageProvider.OpenFolderPickerAsync(FolderPickerDialog);

        if (folderg.Count >= 1)
        {
            var selectedF = folderg[0];
            if (selectedF.TryGetLocalPath() is string result)
            {
                Foldername = result;
                FolderNameTextBox.Text = result;
            }
        }
    }

    public async void IconSearcherVoid(object sender, RoutedEventArgs args)
    {
        /*var openFileDialog = new OpenFileDialog
        {
            // You can specify filters here, such as file extensions
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter() { Name = "All supported Filetypes", Extensions = new List<string> { "ico", "png", "jpg" , "svg"} },
                new FileDialogFilter() { Name = ".ico Files", Extensions = new List<string> { "ico" } },
                new FileDialogFilter() { Name = ".png Files", Extensions = new List<string> { "png" } },
                new FileDialogFilter() { Name = ".jpg Files", Extensions = new List<string> { "jpg" } },
                new FileDialogFilter() { Name = ".svg Files", Extensions = new List<string> { "svg" } },
            }
        };

        var result = await Task.Run(() => openFileDialog.ShowAsync(this));  // 'this' is the current window

        if (result != null && result.Length > 0)
        {
            string selectedFile = result[0];  // Get the first file selected - this is null - I don´t know why?
            IconTextBox.Text = selectedFile;
            ICON = selectedFile;
        }*/
        var TopLevelg = TopLevel.GetTopLevel(this);
        var files = await TopLevelg.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "File Chooser",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("All Filetypes")
                {
                    Patterns = new[] { "*.ico", "*.png", "*.jpg", "*.svg"}
                },
                new FilePickerFileType("ico Files")
                {
                    Patterns = new[] { "*.ico" }
                },
                new FilePickerFileType("png Files")
                {
                    Patterns = new[] { "*.png" }
                },
                new FilePickerFileType("jpg Files")
                {
                    Patterns = new[] { "*.jpg" }
                },
                new FilePickerFileType("svg Files")
                {
                    Patterns = new[] { "*.svg" }
                }
            }
        });

        if (files.Count >= 1)
        {
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            var fileConent = await streamReader.ReadToEndAsync();
            var selectedFileg = files[0];  // Get the first file selected - this is null - I don´t know why?
            if (selectedFileg.TryGetLocalPath() is string selectedFile)
            {
                IconTextBox.Text = selectedFile;
                ICON = selectedFile;
            }
        }
    }
    public async void GithubLinkPressed(object sender, RoutedEventArgs args)
    {
        string Link = "https://github.com/DaikoGames";
        Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
    }

    public async void ConvertButton(object sender, RoutedEventArgs args)
    {
        if (WindowsCheckBox.IsChecked == true)
        {
            windows = true;
        }
        if (LinuxCheckBox.IsChecked == true)
        {
            linux = true;
        }
        if (MacOSCheckBox.IsChecked == true)
        {
            MacOs = true;
        }

        if(X64CheckBox.IsChecked == true)
        {
            formatOfApplication = "64-Bit";
        }
        if (X32CheckBox.IsChecked == true)
        {
            formatOfApplication = "32-Bit";
        }
        if(Arm64CheckBox.IsChecked == true)
        {
            formatOfApplication = "Arm-64";
        }
        if (WindowsCheckBox.IsChecked == true | LinuxCheckBox.IsChecked == true | MacOSCheckBox.IsChecked == true)
        {
            if (extensionS.Contains(".sb") | extensionS.Contains(".sb2") | extensionS.Contains(".sb3"))
            {
                await Task.Run(() => sbfiles(extensionS));
            }
            if (extensionS.Contains(".xml"))
            {
                await Task.Run(() => xmlfiles(extensionS));
            }
        }

        else
        {
            Console.WriteLine("Either you clicked nothign or all - just choose one at a time :-(");
        }
    }

    //The automation of converting .sb3 Files to .xml FIles isn´t working properly - so the conversion from .sb to .sb3 or .sb2 to .sb3 with Auto Hot Key - its probably because of the path - I will take a closer look to that.
    //Ok so i found out a thing, the thing is that the Neutralino app needs to be full screen and a click needs to be simulated. Gotta tell the user to relax lol XD
    public async Task sbfiles(string Extension) //This doesn´t work bc of Scratch i need to find me failure :/ lol XD
    {
        //ok i have to figure out something else, good i have an idea - the user chooses the path of where it should be converted, the program gets opened via cmd, and the user has to do it himself, when the program is over the user just closes the application and the conversion starts - waay better than automation tools
        if (OperatingSystem.IsWindows())
        {
            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.X86))
            {
                string SnapinatorX86 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-Electronified\\Electron-Forge\\SnapinatorMulti\\out\\make\\squirrel.windows\\ia32\\snapinatormulti-1.0.0-Setup.exe");
                var SnapinatorX86START = Process.Start(SnapinatorX86);
                await SnapinatorX86START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }

            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.X64))
            {
                string SnapinatorX86 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-win_x64.exe");
                var SnapinatorX64START = Process.Start(SnapinatorX86);
                await SnapinatorX64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }

            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.Arm64))
            {
                string SnapinatorArm64 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-Electronified\\Electron-Packager\\snapinatormulti-win32-arm64\\snapinatormulti.exe");
                var SnapinatorArm64START = Process.Start(SnapinatorArm64);
                await SnapinatorArm64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }
        }

        if (OperatingSystem.IsLinux())
        {
            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.X64))
            {
                string SnapinatorX86 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-linux_x64");
                var SnapinatorX64START = Process.Start(SnapinatorX86);
                await SnapinatorX64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }

            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.Arm64))
            {
                string SnapinatorArm64 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-linux_arm64");
                var SnapinatorArm64START = Process.Start(SnapinatorArm64);
                await SnapinatorArm64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }
        }

        if (OperatingSystem.IsMacOS())
        {
            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.X64))
            {
                string SnapinatorX64 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-mac_x64");
                var SnapinatorArm64START = Process.Start(SnapinatorX64);
                await SnapinatorArm64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }

            if (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.Equals(Architecture.Arm64))
            {
                string SnapinatorX64 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Snapinator-mac_arm64");
                var SnapinatorArm64START = Process.Start(SnapinatorX64);
                await SnapinatorArm64START.WaitForExitAsync();

                await Task.Run(() => ScratchTOSnapFileAndConversion(".xml"));
            }
        }

        /*byte[] FileBytes = File.ReadAllBytes(Filename);
        String FileJson = Convert.ToBase64String(FileBytes);
        string NewFile = Path.Combine(Foldername, Path.GetFileNameWithoutExtension(Filename) + ".json");
        File.Copy(Filename, NewFile);
        File.AppendAllText(NewFile, FileJson);*/
        //Ok the middle thing doesn´t work, i will just do what my gut tells me: i will just try to make the same thing that deborah did. I will use the .ts File, and try to make a .js File out of it, i mean the Archive.ts File - cuz this file tells me how to convert it with unzipit - which is actually reliable for converting those old files, in case someone is wondering why the iother things don´t work

    }

    private async void ScratchTOSnapFileAndConversion(string extension)
    {
        string EnvironmentProcessPath;
        try
        {
            string jsonPath = Path.Combine(Foldername, "Project.json");
            string XMLFile = Path.Combine(Foldername, Path.GetFileNameWithoutExtension(Filename) + ".xml");
            XmlDocument document = new XmlDocument();
            document.Load(XMLFile);
            string json = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonPath, json);

            //CodeExtractor(jsonPath);
            await Task.Run(() => NameOfProjectChanger(jsonPath, extension));
            await Task.Run(() => CodeSmasher(jsonPath, extension));
        }

        catch (Exception ex)
        {

            /*var MessageOfFailure = MessageBoxManager.GetMessageBoxStandard("IMPORTANT", "Hey, you closed the App before you moved the .xml File into the Folder, next time do that before you close the App, thanks :)", ButtonEnum.Ok);
            var result = await MessageOfFailure.ShowWindowAsync();
            if (result == ButtonResult.Ok)
            {

            }*/

            if (OperatingSystem.IsWindows())
            {
                EnvironmentProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sb1 sb2 sb3 xml to Csharp converter.exe");
                Process.GetCurrentProcess().CloseMainWindow();
                Process.Start(EnvironmentProcessPath);
            }

            if (OperatingSystem.IsLinux())
            {
                EnvironmentProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sb1 sb2 sb3 xml to Csharp converter");
                Process.GetCurrentProcess().CloseMainWindow();
                Process.Start(EnvironmentProcessPath);
            }
        }
    }

    private async void xmlfiles(string extension)
    {
        string jsonPath = Path.Combine(Foldername, "Project.json");
        string newxml = Path.Combine(Foldername, fileName);
        File.Copy(Filename, newxml, true);
        XmlDocument document = new XmlDocument();
        document.Load(newxml);
        string json = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(jsonPath, json);


        //CodeExtractor(jsonPath);
        await Task.Run(() => NameOfProjectChanger(jsonPath, extension));
        await Task.Run(() => CodeSmasher(jsonPath, extension));
    }

    private async void PictureExtractor(string Extension)
    {
        if (OperatingSystem.IsWindows())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net9.0");
        }
        if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net9.0");
        }
        //This is wrong - use same mechanics from Designer Winforms - 
        int Line = 0;
        string jsonPath = Path.Combine(Foldername, "Project.json");
        string[] Lines = File.ReadAllLines(jsonPath);
        bool NextisImageName = false;
        string ThisImageName;
        bool PNG = false;
        bool JPG = false;
        bool SVG = false;
        foreach (string line in Lines)
        {
            try
            {
                Line = Line + 1;

                if (line.Contains(" \"@image\":") && NextisImageName == true)
                {
                    string pngORjpgORsvg = line;

                    if (pngORjpgORsvg.Contains("/png") && NextisImageName == true)
                    {
                        PNG = true;
                        JPG = false;
                        SVG = false;
                    }

                    if (pngORjpgORsvg.Contains("/jpg") && NextisImageName == true)
                    {
                        PNG = false;
                        JPG = true;
                        SVG = false;
                    }

                    if (pngORjpgORsvg.Contains("/svg") && NextisImageName == true)
                    {
                        PNG = false;
                        JPG = false;
                        SVG = true;
                    }

                    if (PNG == true && NextisImageName == true)
                    {
                        string ImageLine = line;
                        string ImageName = File.ReadAllLines(jsonPath).Skip(Line - 4).Take(1).First().Replace("\"@name\"", "").Replace(":", "").Replace("\"", "").Replace(",", "").Trim();
                        string beginning = ImageLine.Replace("\"@image\": \"data:image/png;base64,", "");
                        string middle = beginning.Replace("\"", "");
                        string end = middle.Trim();
                        byte[] PNGBytes = Convert.FromBase64String(end);
                        //Now write the whole thing into the File
                        File.WriteAllBytes(Path.Combine(GameFolder, ImageName + ".png"), PNGBytes);
                    }

                    if (JPG == true && NextisImageName == true)
                    {
                        string ImageLine = line;
                        string ImageName = File.ReadAllLines(jsonPath).Skip(Line - 4).Take(1).First().Replace("\"@name\"", "").Replace(":", "").Replace("\"", "").Replace(",", "").Trim();
                        string beginning = ImageLine.Replace("\"@image\": \"data:image/jpg;base64,", "");
                        string middle = beginning.Replace("\"", "");
                        string end = middle.Trim();
                        byte[] PNGBytes = Convert.FromBase64String(end);
                        //Now write the whole thing into the File
                        File.WriteAllBytes(Path.Combine(GameFolder, ImageName + ".png"), PNGBytes);
                    }

                    if (SVG == true && NextisImageName == true)
                    {
                        string ImageLine = line;
                        string ImageName = Path.Combine(GameFolder, File.ReadAllLines(jsonPath).Skip(Line - 4).Take(1).First().Replace("\"@name\"", "").Replace(":", "").Replace("\"", "").Replace(",", "").Trim());
                        //This is the svg to png converter XD
                        try
                        {
                            string pngfolder = ImageName.Replace(".svg", ".png");
                            using (var PNGImage = new MagickImage(ImageName))
                            {
                                PNGImage.Format = MagickFormat.Png;
                                PNGImage.Write(pngfolder);
                            }
                            File.Delete(ImageName);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                    }

                    NextisImageName = false;
                }
                if (line.Contains("\"@name\":") && NextisImageName == false)
                {
                    ThisImageName = line.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                    //most probably it will check the first name
                    string XCoordinateRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                    string YCoordinateRough = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First();
                    if (XCoordinateRough.Contains("\"@x\":") && YCoordinateRough.Contains("\"@y\":"))
                    {
                        string XCoordinate = XCoordinateRough.Replace("\"@x\":", "").Replace("\"", "").Replace(",", "").Trim();
                        string YCoordinate = YCoordinateRough.Replace("\"@y\":", "").Replace("\"", "").Replace(",", "").Trim();

                        string ScaleRough = File.ReadAllLines(jsonPath).Skip(Line + 4).Take(1).First();
                        if (ScaleRough.Contains("\"@scale\":"))
                        {
                            NextisImageName = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                continue;
            }
        }
    }

    private async void SoundExtractor()
    {
        if (OperatingSystem.IsWindows())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net9.0");
        }
        if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net9.0");
        }
        int Line = 0;
        string jsonPath = Path.Combine(Foldername, "Project.json");
        string[] Lines = File.ReadAllLines(jsonPath);
        foreach (string line in Lines)
        {
            Line = Line + 1;
            try
            {
                if (line.Contains("\"sound\":"))
                {
                    string soundnameLine = File.ReadLines(jsonPath).Skip(Line).Take(1).First();
                    string name = soundnameLine.Replace("\"@name\":", "").Trim().Replace("\"", "").Trim().Replace(",", "").Trim();
                    string soundName = Path.Combine(GameFolder, name + ".wav");

                    string LineRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                    string BytesInBase64 = LineRough.Replace("\"@sound\": \"data:audio/x-wav;base64,", "").Replace("\"", "").Trim();
                    byte[] SoundBytes = Convert.FromBase64String(BytesInBase64);
                    File.WriteAllBytes(soundName, SoundBytes);
                }
            }

            catch (FormatException)
            {
                continue;
            }
        }
    }

    string GameObjectName;

    private async void NameOfProjectChanger(string JSON, string Extension)
    {
        int currentLine = 0;
        string CsFile = Path.Combine(Foldername, GameObjectName + ".cs");
        string[] json = File.ReadAllLines(JSON);

        foreach (string line in json)
        {
            try
            {
                if (Extension == ".xml")
                {
                    if (line.Contains("\"@name\":"))
                    {
                        string nearlyGameObjectName = line.Replace("\"@name\":", "").Trim();
                        string AlmostGameObjectName = nearlyGameObjectName.Replace("\"", "").Trim();
                        GameObjectName = AlmostGameObjectName.Replace(",", "").Trim();
                        currentLine = currentLine + 1;
                        if (currentLine == 3)
                        {
                            ApplicationName = GameObjectName;
                            ApplicationName = ApplicationName.Replace(" ", "_");
                        }
                    }

                    else
                    {
                        currentLine = currentLine + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                break;
            }
        }
    } //not finished

    private async void CopyAllPicturesAndSoundsToBin()
    {
        //Actually I will make the base64 strings into the project :) Not the Filepaths, bc that would be dumb :-/
    }

    string LastName;

    private async void Designer(string JSON, string extension) //This works properly :)
    {
        string DefaultGameFolder = Path.Combine(Foldername, ApplicationName);
        // i need to rename the pictures

        if (OperatingSystem.IsWindows())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net9.0");
        }
        if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net9.0");
        }

        //Setting up the basics of the Form1.Designer.cs
        string WindowEditorFile = Path.Combine(DefaultGameFolder, "MainWindow.axaml"); //IT FINALLY WORKS :) - porting to Avalonia --> not completely working tho
        string WindowCsFile = Path.Combine(DefaultGameFolder, "MainWindow.axaml.cs");
        File.WriteAllText(WindowEditorFile, "<Window          xmlns=\"https://github.com/avaloniaui\"");
        File.AppendAllText(WindowEditorFile, "\n                  xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
        File.AppendAllText(WindowEditorFile, "\n                  xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"");
        File.AppendAllText(WindowEditorFile, "\n                  xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"");
        File.AppendAllText(WindowEditorFile, "\n                  mc:Ignorable=\"d\"");
        File.AppendAllText(WindowEditorFile, "\n                  d:DesignWidth=\"" + 240 + "\"");
        File.AppendAllText(WindowEditorFile, "\n                  d:DesignHeight=\"" + 180 + "\"");
        File.AppendAllText(WindowEditorFile, "\n                  Width=\"" + 480 + "\"");
        File.AppendAllText(WindowEditorFile, "\n                  Height=\"" + 360 + "\"");
        File.AppendAllText(WindowEditorFile, "\n                  x:Class=\"" + ApplicationName + ".MainWindow\"");
        File.AppendAllText(WindowEditorFile, "\n                  Title=\"" + ApplicationName + "\"");
        File.AppendAllText(WindowEditorFile, "\n                  RequestedThemeVariant =\"Light\"");
        File.AppendAllText(WindowEditorFile, "\n                  CanResize =\"False\"");
        File.AppendAllText(WindowEditorFile, "\n>");
        File.AppendAllText(WindowEditorFile, "\n<Canvas>");
        //File.AppendAllText(WindowEditorFile, "\n   <Canvas>");
        //Background and Icon Feature at last - its hard :-/

        //need to make a void that check everything, based on the things that will be used inside of the project
        File.WriteAllText(WindowCsFile, "\n using System;");
        File.AppendAllText(WindowCsFile, "\n using System.IO;");
        File.AppendAllText(WindowCsFile, "\n using Avalonia.Controls;");
        File.AppendAllText(WindowCsFile, "\n using LibVLCSharp.Shared;");
        File.AppendAllText(WindowCsFile, "\n using Avalonia.Interactivity;");
        File.AppendAllText(WindowCsFile, "\n using Avalonia.Media.Imaging;");
        File.AppendAllText(WindowCsFile, "\n namespace " + ApplicationName + ";");
        File.AppendAllText(WindowCsFile, "\n public partial class MainWindow : Window {");

        int Line = 0;
        List<string> MessageNames = new List<string>();
        int pngname = 0;

        Line = 0;
        pngname = pngname + 1;
        //byte[] pngbytes = File.ReadAllBytes(pngs);
        string[] JsonLines = File.ReadAllLines(JSON);

        string ImageNameN;
        string NewImageName;

        //Here make the icon of the Project if needed in the future

        if (ICON != null)
        {
            if (ICON.Contains("ico"))
            {
                ImageNameN = Path.Combine(GameFolder, "Game.ico");
                File.Copy(ICON, ImageNameN, true);
            }
            if (ICON.Contains("png"))
            {
                ImageNameN = Path.Combine(GameFolder, "Game.png");
                NewImageName = Path.Combine(GameFolder, "GameIcon.ico");
                File.Copy(ICON, ImageNameN, true);
                using (var PNGImage = new MagickImage(ImageNameN))
                {
                    PNGImage.Format = MagickFormat.Icon;
                    PNGImage.Write(NewImageName);
                }
            }

            if (ICON.Contains("jpg"))
            {
                ImageNameN = Path.Combine(GameFolder, "Game.jpg");
                NewImageName = Path.Combine(GameFolder, "GameIcon.ico");
                File.Copy(ICON, ImageNameN, true);
                using (var JPGImage = new MagickImage(ImageNameN))
                {
                    JPGImage.Format = MagickFormat.Icon;
                    JPGImage.Write(NewImageName);
                }
            }

            //This should work - i am testing it right away :)
            if (ICON.Contains("svg"))
            {
                ImageNameN = Path.Combine(GameFolder, "Game.svg");
                NewImageName = Path.Combine(GameFolder, "GameIcon.ico");
                File.Copy(ICON, ImageNameN, true);
                using (var SVGImage = new MagickImage(ImageNameN))
                {
                    SVGImage.Format = MagickFormat.Icon;
                    SVGImage.Write(NewImageName);
                }
            }
        }

        foreach (string jsonline in JsonLines)
        {
            if (jsonline.Contains("\"receiveInteraction\""))
            {

            }
        }
        string ValueFile = Path.Combine(DefaultGameFolder ,"Value.axaml.cs");
        foreach (string jsonline in JsonLines)
        {
            Line = Line + 1;
            if (jsonline.Contains("\"receiveMessage\""))
            {
                string messageNameRough = File.ReadLines(JSON).Skip(Line).First();
                string messageName = messageNameRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (!MessageNames.Contains(messageName))
                {
                    MessageNames.Add(messageName);
                    File.AppendAllText(ValueFile, " \n public bool " + messageName + ";");
                }
            }
        }

        bool OnlyVars = false;
        List<string> Variables = new List<string>();
        foreach (string jsonline in JsonLines)
        {
            if (jsonline.Contains("\"variables\":"))
            {
                OnlyVars = true;
            }

            if (jsonline.Contains("\"@name\":") && OnlyVars == true)
            {
                string NameOfVar = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (!Variables.Contains(NameOfVar))
                {
                    Variables.Add(NameOfVar);
                    File.AppendAllText(ValueFile, "\n public int " + NameOfVar + " = 0;");
                }
            }
        }
        int TextNumber = 0;
        bool DaysSince2000 = false;

        Line = 0;
        foreach (string jsonline in JsonLines)
        {
            if (jsonline.Contains("\"UNSUPPORTED: sensing_dayssince2000\""))
            {
                DaysSince2000 = true;
            }
        }

        File.AppendAllText(WindowCsFile, "\n public MainWindow(){");
        File.AppendAllText(WindowCsFile, "\n InitializeComponent();");
        File.AppendAllText(WindowCsFile, "\n this.Icon = new WindowIcon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + \"GameIcon\", \"GameIcon.ico\"));");

        if (DaysSince2000 == true)
        {
            File.AppendAllText(WindowCsFile, "\n int Year = 0;");
            File.AppendAllText(WindowCsFile, "\n int Mont = 0;");
            File.AppendAllText(WindowCsFile, "\n int Day = 0;");
            File.AppendAllText(WindowCsFile, "\n int DaysAfter = 0;");

            File.AppendAllText(WindowCsFile, "\nYear = DateTime.Now.Year;");
            File.AppendAllText(WindowCsFile, "\nMonth = DateTime.Now.Month;");
            File.AppendAllText(WindowCsFile, "\nDay = DateTime.Now.Day;");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = 0 + Day;");
            File.AppendAllText(WindowCsFile, "\nwhile (Year != 2000){ //noKlammerPlease");
            File.AppendAllText(WindowCsFile, "\nYear = Year - 1");
            File.AppendAllText(WindowCsFile, "\nif(DateTime.IsLeapYear(Year)){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 366;");
            File.AppendAllText(WindowCsFile, "\nelse{");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 365;");
            File.AppendAllText(WindowCsFile, "\nif(Month == 1){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 2){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 28");
            File.AppendAllText(WindowCsFile, "\nif(Month == 3){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 4){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 30");
            File.AppendAllText(WindowCsFile, "\nif(Month == 5){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 6){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 30");
            File.AppendAllText(WindowCsFile, "\nif(Month == 7){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 8){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 9){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 30");
            File.AppendAllText(WindowCsFile, "\nif(Month == 10){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31");
            File.AppendAllText(WindowCsFile, "\nif(Month == 11){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 30");
            File.AppendAllText(WindowCsFile, "\nif(Month == 12){");
            File.AppendAllText(WindowCsFile, "\nDaysAfter = DaysAfter + 31}}");
        }

        foreach (string jsonline in JsonLines)
        {
            Line = Line + 1;
            if (jsonline.Contains("\"receiveMessage\""))
            {
                //Check if there is an if condition

                string messageNameRough = File.ReadLines(JSON).Skip(Line).First();
                string messageName = messageNameRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (!MessageNames.Contains(messageName))
                {
                    MessageNames.Add(messageName);
                    File.AppendAllText(ValueFile, " \n public bool " + messageName + " == false;");
                }
            }
        }

        Line = 0;

        List<string> CostumeNames = new List<string>();
        int DocumentLine = 0;
        //Here assign all the different Images form AXAML to the Actual Images 
        //Need to find a way to make the bools of the Messages at the beginning and then let them be interactable as bool
        foreach (string jsonline in JsonLines)
        {
            DocumentLine = DocumentLine + 1;
        }

        bool NextisImageName = false;
        string ThisImageName;
        double Scale;
        int ScaleNumber = 0;
        int XCoordinateLine = 0;
        int YCoordinateLine = 0;

        //BIG PROBLEM - when an image has a negative y coordinate the thing somehow doesn´t always work correctly
        foreach (string jsonline in JsonLines)
        {
            try
            {
                Line = Line + 1;
                if (Line + 15 < DocumentLine)
                {
                    if (jsonline.Contains("\"@name\":") && NextisImageName == true)
                    {
                        string ImageName = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                        using (System.Drawing.Image PNGimage = System.Drawing.Image.FromFile(Path.Combine(GameFolder, File.ReadAllLines(JSON).Skip(Line - 1).Take(1).First().Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim() + ".png")))
                        {
                            // need half of the width and height and gotta - that to the converted coordinates : IT WORKS :)
                            string RoughPNGwidth = Convert.ToString(PNGimage.Width);
                            string RoughPNGheight = Convert.ToString(PNGimage.Height);
                            double PNGwidth = Convert.ToDouble(RoughPNGwidth);
                            double PNGheight = Convert.ToDouble(RoughPNGheight);
                            double SCALEROUGH = double.Parse(File.ReadAllLines(JSON).Skip(ScaleNumber).Take(1).First().Replace("\"@scale\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                            double ScALE = SCALEROUGH;
                            double XCOORDINATEROUGH = double.Parse(File.ReadAllLines(JSON).Skip(XCoordinateLine).Take(1).First().Replace("\"@x\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                            double XCOORDINATE = XCOORDINATEROUGH;
                            double YCOORDINATEROUGH = double.Parse(File.ReadAllLines(JSON).Skip(YCoordinateLine).Take(1).First().Replace("\"@y\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                            double YCOORDINATE = YCOORDINATEROUGH;
                            File.AppendAllText(WindowEditorFile, "\n                  Width=\"" + ((PNGwidth) * ScALE) + "\"");
                            File.AppendAllText(WindowEditorFile, "\n                  Height=\"" + ((PNGheight) * ScALE) + "\"");
                            //THESE COORDINATE SYSTEMS ARE DIFFERENT -need to understand canvas coordinate system correctly
                            File.AppendAllText(WindowEditorFile, "\n                  Canvas.Left=\"" + (240 + XCOORDINATE - ((PNGwidth * ScALE) / 2)) + "\"");
                            File.AppendAllText(WindowEditorFile, "\n                  Canvas.Top=\"" + (180 + (-YCOORDINATE) - ((PNGheight * ScALE) / 2)) + "\"");
                            File.AppendAllText(WindowEditorFile, ">");
                            //Now here the clarifications - this will be harder than expected :( but its doable :)
                            File.AppendAllText(WindowEditorFile, "\n           </Image>");
                        }
                        File.AppendAllText(WindowCsFile, ImageName + ".png\"));");
                        //Now write the whole thing into the File
                        NextisImageName = false;
                    }

                    if (jsonline.Contains("\"@name\":") && NextisImageName == false)
                    {
                        ThisImageName = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                        //most probably it will check the first name
                        string XCoordinateRough = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                        string YCoordinateRough = File.ReadAllLines(JSON).Skip(Line + 2).Take(1).First();
                        if (XCoordinateRough.Contains("\"@x\":") && YCoordinateRough.Contains("\"@y\":"))
                        {
                            XCoordinateLine = Line + 1;
                            YCoordinateLine = Line + 2;

                            string ScaleRough = File.ReadAllLines(JSON).Skip(Line + 4).Take(1).First();
                            ScaleNumber = Line + 4;
                            if (ScaleRough.Contains("\"@scale\":"))
                            {
                                Scale = Convert.ToDouble(ScaleRough.Replace("\"@scale\":", "").Replace("\"", "").Replace(",", "").Replace(".", ",").Trim());
                                File.AppendAllText(WindowEditorFile, "\n           <Image Name=\"Image" + ThisImageName + "\"");

                                //Somehow this is wrong - i will find out what it is :)
                                File.AppendAllText(WindowCsFile, "\n Image" + ThisImageName + ".Source = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, \"");
                                NextisImageName = true;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                continue;
            }
        }
        Line = 0;
        //ok i got an idea - take the x and y coordinates for each Objects and just make xValues and yValues and write them
        File.AppendAllText(WindowCsFile, "\n await Task.Start(() => Game());}");

        string[] AXAMLfile = File.ReadAllLines(WindowEditorFile);

        string LastXCoordinate;
        string LastYCoordinate;
        foreach (string AXAMLline in AXAMLfile)
        {
            Line = Line + 1;
            if (AXAMLline.Contains("<Image Name=\""))
            {
                LastAXAMLname = AXAMLline.Replace("<Image Name=\"", "").Replace("\"", "").Replace(">", "").Trim();
            }

            if (AXAMLline.Contains("Canvas.Left=\""))
            {
                LastXCoordinate = AXAMLline.Replace("Canvas.Left=\"", "").Replace("\"", "").Trim();
                File.AppendAllText(ValueFile, "\n public double xCoordinate" + LastAXAMLname + " = " + LastXCoordinate + ";");
            }
            if (AXAMLline.Contains("Canvas.Top=\""))
            {
                LastYCoordinate = AXAMLline.Replace("Canvas.Top=\"", "").Replace("\"", "").Replace(">", "").Trim();
                File.AppendAllText(ValueFile, "\n public double yCoordinate" + LastAXAMLname + " = " + LastYCoordinate + ";");
            }
        }

        Line = 0;
        foreach (string jsonline in JsonLines)
        {
            Line = Line + 1;

            //THe Last Object name is wrong, i gotta look through that some other day, but now it is way better than before :) atleast something
            if (jsonline.Contains("\"@name\":"))
            {
                //Check if LastObject was a sound
                string PossibleLastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (File.ReadAllLines(JSON).Skip(Line).Take(1).Contains("\"@center-x\":"))
                {
                    LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                }
            }
            if (jsonline.Contains("\"@hidden\":"))
            {
                if (jsonline.Contains("true"))
                {
                    File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == false;");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }
                if (jsonline.Contains("false"))
                {
                    File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == true;");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }
            }
        }

        Line = 0;

        bool BounceOff = false;
        foreach (string jsonlines in JsonLines)
        {
            Line = Line + 1;
            if (jsonlines.Contains("\"reportTouchingObject\""))
            {
                string EdgeOrNot = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                if (EdgeOrNot.Contains("\"edge\""))
                {
                    BounceOff = true;
                    break;
                }

                if (EdgeOrNot.Contains("\"l\":"))
                {
                    string MousePointerProlly = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First().Replace("\"option\": \"", "").Replace("\"", "").Trim();
                    if(MousePointerProlly == "mouse-pointer")
                    {
                        //ich glaube ich mache daraus eigene Voids, das ist viel Schlauer ngl
                    }
                }
            }
        }

        File.AppendAllText(WindowCsFile, "\n Paralell.Invoke(");

        if (BounceOff == true)
        {
            File.AppendAllText(WindowCsFile, "\n () => BounceOff,");
        }


        Line = 0;

        foreach (string jsonline in JsonLines)
        {
            Line = Line + 1;
            if (jsonline.Contains("\"@name\":"))
            {
                //Check if LastObject was a sound
                string PossibleLastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (File.ReadAllLines(JSON).Skip(Line).Take(1).First().Contains("\"@center-x\":"))
                {
                    LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Replace("-", "").Replace("_", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n() => " + LastObject + ",");
                }
            }
        }

        File.AppendAllText(WindowCsFile, ");");
        Line = 0;

        double XCoordinate = 0;
        double YCoordinate = 0;

        int Repeatingnumber = 0;
        int layernumber = 0;
        TextNumber = 0;
        //int SoundNumber = 0;
        int CostumeNumber = 0;
        int RandomNumberInt = 0;

        bool IF = false;


        bool NextIsEquals = false;
        bool TheOneAfterTheNextIsEquals = false;
        bool NextIsBigger = false;
        bool TheOneAfterTheNextIsBigger = false;
        bool NextIsSmaller = false;
        bool TheOneAfterTheNextIsSmaller = false;

        bool NextIsPlus = false;
        bool TheOneAfterTheNextIsPlus = false;
        bool NextIsMinus = false;
        bool TheOneAfterTheNextIsMinus = false;
        bool NextIsMultiply = false;
        bool TheOneAfterTheNextIsMultiply = false;
        bool NextIsDivide = false;
        bool TheOneAfterTheNextIsDivide = false;

        int GlobablClickNumber = 0; //i need to change it so there is only one void for the inputs especially global ones -- a problem for later
        double NotExactWidth = 0;
        double NotExactHeight = 0;

        double SCALE = 0;
        bool PressedOrReleased = false;
        bool VOID = false;

        int LineRound = 0;
        int NoLineRound = 0;
        bool ComplicatedMathVariable = false;
        bool NextIsRound = false;
        bool DotThing = false;
        bool IfElse = false;
        //ok so i need some things around the edges to detect it, it is possible but hard :3
        //check if the coordinates of the objects are bigger as the woidth or the height XD
        var SoundVLC = new LibVLC();

        if (BounceOff == true)
        {
            File.AppendAllText(WindowCsFile, "\n public void BounceOff(){");
            File.AppendAllText(WindowCsFile, "\n while(true){");

            foreach (string jsonlines in JsonLines)
            {
                Line = Line + 1;
                if (jsonlines.Contains("\"reportTouchingObject\""))
                {
                    string EdgeOrNot = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                    if (EdgeOrNot.Contains("\"edge\""))
                    {

                        File.AppendAllText(WindowCsFile, "\n if(Convert.ToInt32(Canvas.GetLeft(Image" + LastObject + ")) >= 480 || Convert.ToInt32(Canvas.GetTop(Image" + LastObject + ")) >= 360){");
                        File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", 480);");
                        File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", 360);");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        File.AppendAllText(WindowCsFile, "\n }");

                    }
                }
            }

            File.AppendAllText(WindowCsFile, "\n }");
            File.AppendAllText(WindowCsFile, "\n }");
        }

        Line = 0;
        //ok make now a function that reads out all volume things and based on the first volume of an object its called here. If there is no first volume its always 100%
        List<string> LastObjects = new List<string>();
        List<string> VolumeAlreadySet = new List<string>();
        foreach(string jsonline in JsonLines)
        {
            Line = Line + 1;
            //Here the name of the object
            if (jsonline.Contains("\"@name\":"))
            {
                string IDX = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string X = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();

                if(IDX.Contains("\"@idx\":") && X.Contains("\"@x\":"))
                {
                    LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                    if (!LastObjects.Contains(LastObject))
                    {
                        LastObjects.Add(LastObject);
                    }
                }
            }

            if(jsonline.Contains("\"setVolume\"") && LastObjects != null)
            {
                if (!VolumeAlreadySet.Contains(LastObject))
                {
                    string VolumeText = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                    if (VolumeText.Contains("\"l\":"))
                    {
                        if (LastObjects.Contains(LastObject))
                        {
                            VolumeAlreadySet.Add(LastObject);
                            File.AppendAllText(ValueFile, "\n string " + LastObject + "VOLUME = " + VolumeText.Replace("\"l\":", "").Replace("\"", "").Trim() + ";"); 
                        }
                    }
                }
            }

            //Here the sound volume based on LastObject
        }

        Line = 0;
        File.AppendAllText(WindowCsFile, "\n public Void Game(){");
        List<string> SoundPlayerList = new List<string>();
        foreach (string jsonline in JsonLines)
        {
            Line = Line + 1;

            //This is wrong
            if (jsonline.Contains("{"))
            {
                if (LineRound > 0 && NoLineRound == 0)
                {
                    //Check if there is something of the jsonlines after  -> down below O_O if that is the case then do nothing
                    NoLineRound = NoLineRound + 1;
                }
            }

            if (jsonline.Contains("\"@scale\":"))
            {
                SCALE = double.Parse(jsonline.Replace("\"@scale\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
            }

            //SOmehow there is a problem with x Coordinates
            if (jsonline.Contains("\"@x\":"))
            {
                XCoordinate = double.Parse(jsonline.Replace("\"@x\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
            }

            if (jsonline.Contains("\"@y\":"))
            {
                YCoordinate = double.Parse(jsonline.Replace("\"@y\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
            }

            if (jsonline.Contains("\"@center-x\":"))
            {
                NotExactWidth = double.Parse(jsonline.Replace("\"@center-x\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
            }

            if (jsonline.Contains("\"@center-y\":"))
            {
                NotExactHeight = double.Parse(jsonline.Replace("\"@center-y\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
            }


            //pngwidt must me centerx and centery 
            //Location///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Location///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Location///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Gotta change that to Canvas.Left and Canvas.Top
            //THe width is hard
            //Gotta change that to Canvas.Left and Canvas.Top

            //not correct now but it will be :)
            if (jsonline.Contains("\"receiveGo\""))
            {
                string ForwardProbably = File.ReadAllLines(JSON).Skip(Line + 3).Take(1).First();
                if (ForwardProbably.Contains("forward"))
                {
                    int ValueForward = Convert.ToInt32(File.ReadAllLines(JSON).Skip(Line + 4).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim());
                    File.AppendAllText(WindowCsFile, "\n XValueImage" + LastObject + "= XValueImage" + LastObject + " + " + ValueForward + ";");
                    File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image " + LastObject + ", XValueImage" + LastObject + ");");
                }
            }

            //Here it is made a bit unclear
            if (jsonline.Contains("\"@s\": \"forward\""))
            {
                string XMovement = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                if(XMovement.Contains("block") == false)
                {
                    File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , " + XMovement + ");");
                }
                if (XMovement.Contains("block"))
                {
                    File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , ");
                    DotThing = true;
                }
            }

            if (jsonline.Contains("\"turn\"")) //Wrong
            {
                string AngleRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle);
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"turnLeft\"")) //Wrong
            {
                string AngleRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle);
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doGotoObject\""))
            {
                string Option = File.ReadAllLines(JSON).Skip(Line + 2).Take(1).First(); // --> this thin might be wrong, everything else seems to be valid
                if (Option.Contains("\"random position\""))
                {
                    using (System.Drawing.Image PNGimage = System.Drawing.Image.FromFile(Path.Combine(GameFolder, LastObject + ".png")))
                    {
                        // need half of the width and height and gotta - that to the converted coordinates : IT WORKS :)
                        string RoughPNGwidth = Convert.ToString(PNGimage.Width);
                        string RoughPNGheight = Convert.ToString(PNGimage.Height);
                        double PNGwidth = Convert.ToDouble(RoughPNGwidth);
                        double PNGheight = Convert.ToDouble(RoughPNGheight);
                        RandomNumberInt = RandomNumberInt + 1;
                        File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + " = this.FinControl<Image>(\"" + LastObject + "\");");
                        File.AppendAllText(WindowCsFile, "\n Random RandomX" + RandomNumberInt + " = new Random();");
                        File.AppendAllText(WindowCsFile, "\n int randomX" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 480 + ");");
                        File.AppendAllText(WindowCsFile, "\n Random RandomY" + RandomNumberInt + " = new Random();");
                        File.AppendAllText(WindowCsFile, "\n int randomY" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 360 + ");");
                        File.AppendAllText(WindowCsFile, "\n TranslateTransform Coordinates" + LastObject + " = new TranslateTransform();");
                        File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", RandomX" + RandomNumberInt + ");");
                        File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", RandomY" + RandomNumberInt + ");");
                    }
                }
                //Get the Coordinates
                //else if there is the Option to random position generate a random position 
            }

            if (jsonline.Contains("\"gotoXY\""))
            {
                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + " = " + (240 + XCoordinate - (((NotExactWidth * 2) * SCALE) / 2)));
                File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + " = " + (180 + (-YCoordinate) - (((NotExactHeight * 2) * SCALE) / 2)));
                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , xCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + " , yCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                //Now I need to make a code that litreally changes the Coordinates of the Object
            }

            if (jsonline.Contains("\"doGlide\""))
            {
                //This will be the hardest one ngl XD
                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + " = " + (240 + XCoordinate - (((NotExactWidth * 2) * SCALE) / 2)));
                File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + " = " + (180 + (-YCoordinate) - (((NotExactHeight * 2) * SCALE) / 2)));
                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , xCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + " , yCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"setHeading\"")) //fixed Rotation like turn to 90°
            {
                string AngleRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle);
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doFaceTowards\""))
            {
                //Get the Object to face towards, get the position of the mouse if it is the mouse
                //that is actually pretty hard but doable if i have enough brain cells XD
                string ObjectToFaceTowards = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                if(ObjectToFaceTowards == "Mouse")
                {
                    //Will probably only be supported when v 1.0 is done
                }
                else
                {
                    ObjectToFaceTowards = LastObject;
                    File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                    File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                    File.AppendAllText(WindowCsFile, "\n double LeftObject" + LastObject + " = Canvas.GetLeft(" + LastObject + ");");
                    File.AppendAllText(WindowCsFile, "\n double TopObject" + LastObject + " = Canvas.GetTop(" + LastObject + ");");
                    File.AppendAllText(WindowCsFile, "\n Rotation" + LastObject + " = Math.Atan(LeftObject" + LastObject + " , TopObject" + LastObject + ");");
                    File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = Rotation" + LastObject);
                    //File.AppendAllText(WindowCsFile);
                }
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"changeXPosition\""))
            {
                //ok, so basically i have to make the xPosition and yPosition a bool but i am not 100% sure if I am able to at the earliest version (1.0)
                //Check if its a number or a random number --> Mathematic things are not implemented yet
                string PossibleNumber = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                if (PossibleNumber.Contains("\"l\":"))
                {
                    string Number = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();

                    File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + " = xCoordinate" + LastObject + " + " + Number);
                    File.AppendAllText(WindowCsFile, "\n Canvas.Left(Image" + LastObject + ", xCoordinate" + LastObject + ");");
                }
            }

            if (jsonline.Contains("\"setXPosition\""))
            {
                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + "=" + (240 + XCoordinate - (((NotExactWidth * 2) * SCALE) / 2)) + ";");
                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , xCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"changeYPosition\""))
            {
                //Check if its a number or a random number --> Mathematic things are not implemented yet
                string PossibleNumber = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                if (PossibleNumber.Contains("\"l\":"))
                {
                    string Number = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();

                    File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + " = yCoordinate" + LastObject + " + " + Number);
                    File.AppendAllText(WindowCsFile, "\n Canvas.Top(Image" + LastObject + ", yCoordinate" + LastObject + ");");
                }
            }

            if (jsonline.Contains("\"setYPosition\""))
            {
                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + "=" + (240 + YCoordinate - (((NotExactWidth * 2) * SCALE) / 2)) + ";");
                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + " , yCoordinate" + LastObject + ");");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //ok so basically the ground technology of these bubbles works, but i need to make ´em first
            if (jsonline.Contains("\"doSayFor\""))
            {
                TextNumber = TextNumber + 1;
                string Text = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                //ok you gotta make a new LastObject which is the SayFor Image, and even make one here:
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true");
                //Get the seconds and delay the task 
                double TimeOfSaying = Convert.ToInt32(File.ReadAllLines(JSON).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim());
                File.AppendAllText(WindowCsFile, "\n Task.Delay(" + TimeOfSaying + ");");
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == false");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"bubble\"")) //Finally works a little bit better
            {
                TextNumber = TextNumber + 1;
                //Same here
                string Text = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                //Need to make a script that lets the bubble follow the player and then show up with the visibility thing
                //Create the bubble as an Image now - hardest part i will ever code lol 
            }

            if (jsonline.Contains("\"doThinkFor\""))
            {
                TextNumber = TextNumber + 1;
                string Text = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                //Same here
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true;");
                //Get the seconds and delay the task 
                double TimeOfThinking = Convert.ToInt32(File.ReadAllLines(JSON).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim());
                File.AppendAllText(WindowCsFile, "\n Task.Delay(" + TimeOfThinking + ");");
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == false;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doThink\""))
            {
                TextNumber = TextNumber + 1;
                string Text = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                //Same here
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doSwitchToCostume\""))
            {
                // i need to find which costume the next one is
            }

            if (jsonline.Contains("\"doWearNextCostume\""))
            {
                CostumeNumber = CostumeNumber + 1;
                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Source == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Image" + LastObject + CostumeNumber + ");");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                //Find the next costume - hard part :(
            }

            if (jsonline.Contains("\"doTellTo\""))
            {
                File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"reportTouchingObject\""))
            {
                if (IF == true)
                {
                    string ObjectToTouchRough = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                    string ObjectToTouch = ObjectToTouchRough.Replace("\"option\":", "").Replace("\"", "").Replace(",", "").Trim();
                    //check if there was an if statement before or an && Statement or else it wouldn´t make sense
                    string PossibleIfStatement = File.ReadAllLines(JSON).Skip(Line - 3).Take(1).First();
                    if (PossibleIfStatement.Contains("\"doIf\""))
                    {
                        //The problem is I need to understand Rects :( - found a solution :)
                        File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n if(" + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                    }

                    if (PossibleIfStatement.Contains("\"reportAnd\""))
                    {
                        File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n" + " && " + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                    }

                    if (PossibleIfStatement.Contains("\"doWaitUntil\""))
                    {
                        File.AppendAllText(WindowCsFile, "public async Task WaitUntil" + LastObject + "(){");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                    }

                    if (PossibleIfStatement.Contains("\"reportNot\""))
                    {
                        File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                        File.AppendAllText(WindowCsFile, "\n" + "!" + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                    }

                    //Check if there was an Object that has already been Touched in a List, and then Write those Objects (Rects in the beginning), like the ifs in the usual things
                }
            }

            if (jsonline.Contains("\"goToLayer\""))
            {
                string LayerRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Layer = LayerRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                //File.AppendAllText(WindowCsFile, ); -- I need to find a way to change the layer - litreally change the layer of the object
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"show\"") | jsonline.Contains("\"reportShown\""))////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                if (jsonline.Contains("\"reportShown\""))
                {
                    File.AppendAllText(WindowCsFile, "\n if(Image" + LastObject + ".Visibility == true){");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }
                if (jsonline.Contains("\"show\""))
                {
                    File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == true;");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }

            }

            if (jsonline.Contains("\"hide\""))////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == false;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"setScale\"")) //This is wrong - not completely wrong tho
            {
                if (File.Exists(LastObject + ".png"))
                {
                    string NewScaleRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                    int NewScale = Convert.ToInt32(NewScaleRough.Replace("\"l\":", "").Replace("\"", "").Trim());

                    using (var image = System.Drawing.Image.FromFile(Path.Combine(GameFolder, LastObject + ".png"))) // it somehow uses sounds too - error !!!IMPORTANT
                    {
                        double SizeX = image.Width;
                        double SizeY = image.Height;
                        File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                        File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Width = " + (SizeX * NewScale));
                        File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Height = " + (SizeY * NewScale));
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                    }
                }
            }

            if (jsonline.Contains("\"goToLayer\""))
            {
                string layer = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First().Replace("\"option\":", "").Replace("\"", "").Trim();

                if (layer.Contains("\"back\""))
                {
                    File.AppendAllText(WindowCsFile, "\n Canvas.SetZIndex(" + LastObject + " , 0);");
                }

                if (layer.Contains("\"front\""))
                {
                    File.AppendAllText(WindowCsFile, "\n Canvas.SetZIndex(" + LastObject + " , 1);");
                }
                //gotta make something that adds up so the max layer can be read
                //max layer or least layer, gotta think about that
            }

            if (jsonline.Contains("\"goBack\""))
            {
                string Layer = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();

                //layernumber = layernumber + Layer;
            }

            //SOUND//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (jsonline.Contains("\"doPlaySoundUntilDone\""))
            {
                try
                {
                    SoundNumber = SoundNumber + 1;
                    string NameOfSound = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                    string SoundFolder = Path.Combine(GameFolder, NameOfSound + ".wav");
                    if (File.Exists(SoundFolder))
                    {
                        //DONT USE A USING DIRECTIVE HERE. IT MAKES THE SOUND WRITING WRONG. 
                       
                        var Sound = new Media(SoundVLC, SoundFolder); //, FromType.FromLocation
                        await Sound.Parse(MediaParseOptions.ParseLocal);
                        var SoundLength = Sound.Duration;
                        File.AppendAllText(WindowCsFile, "\n var SOUND" + SoundNumber + " = new LibVLC();");
                        File.AppendAllText(WindowCsFile, "\n MediaPlayer MediaPlayer" + SoundNumber + " = new MediaPlayer(SOUND" + SoundNumber + ");");
                        File.AppendAllText(WindowCsFile, "\n var MediaOfMediaPlayer" + SoundNumber + " = new Media(SOUND" + SoundNumber + " , Path.Combine(AppDomain.CurrentDomain.BaseDirectory, \"" + NameOfSound + ".wav\")" + ", FromType.FromPath);");
                        File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Play(MediaOfMediaPlayer" + SoundNumber + ");");
                        File.AppendAllText(WindowCsFile, "\n Await Task.Delay(" + SoundLength + ");");
                        /*Sound.ParseStop();
                        Sound.Dispose()*/;
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        if (SoundPlayerList.Contains("MediaPlayer" + SoundNumber) == false)
                        {
                            SoundPlayerList.Add("MediaPlayer" + SoundNumber);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
                //Somehow always after doplaySoundUntil done it never stops at the net playSound - i wonder why :-O
                //ok i need to find the lenght of the sound and then go on with the task somehow, i need to find a tutorial or some kind of help

            }
           

            if (jsonline.Contains("\"playSound\""))
            {
                try
                {
                    SoundNumber = SoundNumber + 1;
                    string NameOfSound = File.ReadAllLines(JSON).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                    string SoundFile = Path.Combine(GameFolder, NameOfSound + ".wav");
                    if (File.Exists(SoundFile))
                    {
                        File.AppendAllText(WindowCsFile, "\n var SOUND" + SoundNumber + " = new LibVLC();");
                        File.AppendAllText(WindowCsFile, "\n MediaPlayer MediaPlayer" + SoundNumber + " = new MediaPlayer(SOUND" + SoundNumber + ");");
                        File.AppendAllText(WindowCsFile, "\n var MediaOfMediaPlayer" + SoundNumber + " = new Media(SOUND" + SoundNumber + " , Path.Combine(AppDomain.CurrentDomain.BaseDirectory,\"" + NameOfSound + ".wav\")" + ", FromType.FromPath);");
                        File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Play(MediaOfMediaPlayer" + SoundNumber + ");");
                        File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        if (SoundPlayerList.Contains("MediaPlayer" + SoundNumber) == false)
                        {
                            SoundPlayerList.Add("MediaPlayer" + SoundNumber);
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
               
            }

            if (jsonline.Contains("\"doStopAllSounds\""))
            {
                foreach (string SoundPlayer in SoundPlayerList)
                {
                    File.AppendAllText(WindowCsFile, "\n" + SoundPlayer + ".Mute = true;");
                }

                foreach (string SoundPlayer in SoundPlayerList)
                {
                    File.AppendAllText(WindowCsFile, "\n" + SoundPlayer + ".Stop = true;");
                }
            }

            if (jsonline.Contains("\"changeVolume\""))
            {
                string NumberOfVolumeRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First(); //They are all in percent
                string NumberOfVolume = NumberOfVolumeRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Volume = " + LastObject + "VOLUME + " + "(" + NumberOfVolume  + ");");
            }

            if (jsonline.Contains("\"setVolume\""))
            {
                string NumberOfVolumeRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First(); //They are all in percent
                string NumberOfVolume = NumberOfVolumeRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Volume = " + NumberOfVolume + ";");
            }

            //Events//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (jsonline.Contains("\"receiveMessage\""))
            {
                LineRound = LineRound + 1;
                string MessageRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                File.AppendAllText(WindowCsFile, "\n if(" + Message + " == true){");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"sendMessage\""))
            {
                string MessageRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                File.AppendAllText(WindowCsFile, "\n" + Message + " == true;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            //OK SO I THINK I GOT A SOLUTION -> well those things are kinda dumb, i think i need to rewrite these .Contains things completely. It has some bad quirks O_O ngl i think its cuz it mixes up these words -> I FIXED IT :) it was just a single line XD. I litreally clicked like 100 debugger lines to find that out XD. dumbest mistake ever ngl frfr :skull:
            if (jsonline.Contains("\"doBroadcast\""))
            {
                string MessageRough = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                File.AppendAllText(WindowCsFile, "\n" + Message + " == true;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }
            //REPEATING///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //This is a big thing - i can feel it ;-)
            if (jsonline.Contains("\"doWait\""))
            {
                string HowLong = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                if (HowLong.Contains("\"reportRandom\""))
                {
                    //This will be hard but doable :3
                    double One = double.Parse(File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First().Replace("\"", "").Trim());
                    double Two = double.Parse(File.ReadAllLines(JSON).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim());
                    File.AppendAllText(WindowCsFile, "\n Random RandomNumber = new Random()");
                    File.AppendAllText(WindowCsFile, "\n int RandomNumberInt" + LastObject + " = RandomNumber.Next(" + One + " , " + Two + ");");
                    File.AppendAllText(WindowCsFile, "\n await Task.Delay(RandomNumberInt" + LastObject + ");");
                }
                else if (HowLong.Contains("\"l\":"))
                {
                    double ActualLength = double.Parse(HowLong.Replace("\"l\":", "").Replace("\"", "").Trim(), CultureInfo.InvariantCulture) * 1000;
                    File.AppendAllText(WindowCsFile, "\n await Task.Delay(" + ActualLength + ");");
                }
            }

            if (jsonline.Contains("\"doRepeat\"")) // i make a bool that gets true if it looks for random integers and it finds one /////////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                string RepeatingNumberRough = File.ReadAllLines(JSON).Skip(Line).First();
                string RepeatingNumber = RepeatingNumberRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                //Check if RepeatingNumber is a Random Number or an addition and do the things need to make a void that checks things in between 
                File.AppendAllText(WindowCsFile, "\n int Repeating" + Repeatingnumber + " = " + RepeatingNumber);
                File.AppendAllText(WindowCsFile, "\n while (Repeating" + Repeatingnumber + "!=" + RepeatingNumber + "){");
                File.AppendAllText(WindowCsFile, "\n Repeating" + Repeatingnumber + "=" + "Repeating" + Repeatingnumber + " +1;");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doForever\"")) // i make a bool that gets true if it looks for random integers and it finds one ////////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                File.AppendAllText(WindowCsFile, "\n while (true){");
                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }
            if (jsonline.Contains("\"doIf\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                IF = true;
                //File.AppendAllText(WindowCsFile, "\n if(");
                //File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
            }

            if (jsonline.Contains("\"doIfElse\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                string PossibleTouchStatement = File.ReadAllLines(JSON).Skip(Line + 3).Take(1).First();
                if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false)
                {
                    File.AppendAllText(WindowCsFile, "\n if{");

                    //There is surely a way to check when there is a else statement: for example {, and make a bool that tells the thing that its an else or an if now lol

                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");

                    IfElse = true;
                }
            }

            if (jsonline.Contains("\"doWaitUntil\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                string PossibleTouchStatement = File.ReadAllLines(JSON).Skip(Line + 3).Take(1).First();
                if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false && PossibleTouchStatement.Contains("\"reportKeyPressed\""))
                {

                }
                //First check if there is any Block like reportTouchingObject - is a Block that can´t be used inside an if statement as its an if statement itself
                //File.AppendAllText();
            }

            if (jsonline.Contains("\"doUntil\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////
            {
                LineRound = LineRound + 1;
                string PossibleTouchStatement = File.ReadAllLines(JSON).Skip(Line + 3).Take(1).First();
                if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false && PossibleTouchStatement.Contains("\"reportKeyPressed\""))
                {
                    File.AppendAllText(WindowCsFile, "\n if(");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }
            }

            if (jsonline.Contains("\"createClone\""))
            {
                string MyselfOrSomeoneElse = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                if (MyselfOrSomeoneElse.Contains("\"myself\""))
                {
                    //Now create a clone lol 8-)
                }
            }

            if (jsonline.Contains("\"doStopThis\""))
            {
                string OptionRough = File.ReadAllLines(JSON).Skip(Line).Take(2).First();
                if (OptionRough.Contains("\"option\""))
                {
                    if (OptionRough.Contains("\"all\""))
                    {
                        File.AppendAllText(WindowCsFile, "\nApplication.Current?.Shutdown();");
                    }

                    if (OptionRough == "\"other scripts in sprite\"")
                    {
                        // ok no the idea of turning every object into a void is a bad idea - cuz it will complicate stuff even more, the thing is when I make a void of a object i gotta call it at the beginning, and I cat call multiple voids at once O_O so i just have to kind of make some other solution, Maybe this block will never be implemented O_O sorry =_= i can´t do everything but i can do much
                        //File.AppendAllText(WindowCsFile, "//This is not doable rn :(");
                        //Ok i just had a wild idea, what if I just make bools that represent those voids, and whenever a void is litreally called they get turned on or off, and doing voids for sprites is a good idea, i just have to call them all at the beginning if I am correct. THose bools can be turned of and on as they are world bools, when those bools have turned the voids simply don´t work, I think i just beat the worst type of block O_O lets just hope i am right XD. 

                    }
                }
            }

            //Get the last Object Name
            /////////////////////////////THIS IS FOR }///////////////////////////////////////////////////////////////////////////////////////////

            if (jsonline.Contains("\"@name\":"))
            {

                string PossibleLastObject = jsonline.Replace("\"@idx\":", "").Replace("\"", "").Replace(",", "").Trim();
                if (File.ReadAllLines(JSON).Skip(Line).Take(1).First().Contains("\"@idx\":"))
                {
                    LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Replace("-", "").Replace("_", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n } \n public Task " + LastObject + "(){"); //Ok i am not sure of giving down a } before the public Task
                }
            }

            //A on click event via 2 voids if there is a press and a release event 
            //Foreach Button Click a Void - if there are double voids with
            //no voids at ALL when clicking, that is wrong (it complicetes things way more :( I know that it is :-/!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            if (jsonline.Contains("\"receiveInteraction\""))
            {
                string EitherPressedOrReleasedRough = File.ReadAllLines(JSON).Skip(Line + 1).First();
                string EitherPressedOrReleased = EitherPressedOrReleasedRough.Replace("\"option\":", "").Replace("\"", "").Trim();
                PressedOrReleased = true;

                if (EitherPressedOrReleased == "pressed")
                {
                    File.AppendAllText(WindowCsFile, "\n private async Task " + LastObject + "OnPressed(object sender, Avalonia.PointerPressedEventArgs e){");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }

                if (EitherPressedOrReleased == "released")
                {
                    File.AppendAllText(WindowCsFile, "\n private async Task " + LastObject + "OnReleased(object sender, Avalonia.PointerPressedEventArgs e){");
                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                }
            }

            if (jsonline.Contains("\"reportMouseDown\"")) //I need to find a way to make clicking in without a void
            {
                GlobablClickNumber = GlobablClickNumber + 1;
                File.AppendAllText(WindowCsFile, "\n private Task MouseDown" + GlobablClickNumber + "(object sender, PointerReleasedEventArgws e){");
            }

            if (jsonline.Contains("\"reportKeyPressed\""))
            {
                //Check if it is an if or wait until or something else
                string PossibleIfStatement = File.ReadAllLines(JSON).Skip(Line - 3).Take(1).First();
                string KeyRough = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                string Key = KeyRough.Replace("\"option\":", "").Replace("\"", "").Trim(); //Key needs to be big - this is halfly correct, also Numbers and Arrows should be included - need to find a way for that too

                if (PossibleIfStatement.Contains("\"doIf\"")) ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                {
                    LineRound = LineRound + 1;
                    File.AppendAllText(WindowCsFile, "\n if(e.KeyCode == Keys." + Key + "){");
                }

                if (PossibleIfStatement.Contains("\"doWaitUntil\""))
                {
                    LineRound = LineRound + 1;
                    File.AppendAllText(WindowCsFile, "\n while(e.KeyCode != Keys." + Key + "){");
                }

                if (PossibleIfStatement.Contains("\"doUntil\""))
                {
                    LineRound = LineRound + 1;
                    File.AppendAllText(WindowCsFile, "\n while(e.KeyCode != Keys." + Key + "){");
                }
            }

            if (jsonline.Contains("\"reportTouchingColor\""))
            {
                //ok i need a system where it detects the objects - my 
            }

            if (jsonline.Contains(""))
            {

            }

            if (jsonline.Contains("\"UNSUPPORTED: sensing_dayssince2000\""))
            {
                if (NextIsEquals == true)
                {
                    File.AppendAllText(WindowCsFile, " == DaysAfter)");
                    NextIsEquals = false;
                }

                if (TheOneAfterTheNextIsEquals == true)
                {
                    File.AppendAllText(WindowCsFile, "DaysAfter");
                    TheOneAfterTheNextIsEquals = false;
                    NextIsEquals = true;
                }
            }

            ///MATH//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (jsonline.Contains("\"reportEquals\""))
            {
                TheOneAfterTheNextIsEquals = true;
            }

            if (jsonline.Contains("\"reportGreaterThan\"")) //I need to add all the other things, like bigger, less, addition subtraction, division and multiplication
            {
                TheOneAfterTheNextIsBigger = true;
            }

            if (jsonline.Contains("\"reportLessThan\""))
            {
                TheOneAfterTheNextIsSmaller = true;
            }

            // I am not quite sure how I will manage to let it work but I somehow will :)
            if (jsonline.Contains("\"reportSum\""))
            {
                TheOneAfterTheNextIsPlus = true;
            }

            if (jsonline.Contains("\"reportDifference\""))
            {
                TheOneAfterTheNextIsMinus = true;
            }

            if (jsonline.Contains("\"reportProduct\""))
            {
                TheOneAfterTheNextIsMultiply = true;
            }

            if (jsonline.Contains("\"reportQuotient\""))
            {
                TheOneAfterTheNextIsDivide = true;
            }

            if (jsonline.Contains("\"l\":"))
            {
                //this one handles 4 out of 5 things, when there are two numbers it doesn´t work out correctly - i have to fix that in the future - but i will take it now as its not that important

                string ImportantNumber = jsonline.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();

                if (NextIsEquals == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " == " + ImportantNumber + ")");
                    NextIsEquals = false;
                }

                if (NextIsBigger == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " <= " + ImportantNumber + ")");
                    NextIsBigger = false;
                }

                if (NextIsSmaller == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " >= " + ImportantNumber + ")");
                    NextIsSmaller = false;
                }

                if (NextIsPlus == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " + " + ImportantNumber + ")");
                    NextIsPlus = false;
                }

                if (NextIsMinus == true)
                {
                    File.AppendAllText(WindowCsFile, " - " + ImportantNumber + ")");
                    NextIsMinus = false;
                }

                if (NextIsMultiply == true)
                {
                    File.AppendAllText(WindowCsFile, " * " + ImportantNumber + ")");
                    NextIsMultiply = false;
                }

                if (NextIsDivide == true)
                {
                    File.AppendAllText(WindowCsFile, " / " + ImportantNumber + ")");
                    NextIsDivide = false;
                }

                if (TheOneAfterTheNextIsEquals == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsEquals = false;
                    NextIsEquals = true;
                }

                if (TheOneAfterTheNextIsBigger == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsBigger = false;
                    NextIsBigger = true;
                }

                if (TheOneAfterTheNextIsSmaller == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsSmaller = false;
                    NextIsSmaller = true;
                }

                if (TheOneAfterTheNextIsPlus == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsPlus = false;
                    NextIsPlus = true;
                }

                if (TheOneAfterTheNextIsMinus == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsMinus = false;
                    NextIsMinus = true;
                }

                if (TheOneAfterTheNextIsMultiply == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsMultiply = false;
                    NextIsMultiply = true;
                }

                if (TheOneAfterTheNextIsDivide == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantNumber);
                    TheOneAfterTheNextIsDivide = false;
                    NextIsDivide = true;
                }
            }

            //Somehow on additions and other things but not equasions it is the case that the variables Name is written twice next to each other, gotta change that :-( I was really close
            if (jsonline.Contains("\"@var\":"))
            {
                string ImportantVar = jsonline.Replace("\"@var\":", "").Replace("\"", "").Replace(",", "").Trim();

                if (NextIsEquals == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " == " + ImportantVar + ")");
                    NextIsEquals = false;
                    
                }

                if (NextIsBigger == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " <= " + ImportantVar + ")");
                    NextIsBigger = false;
                }

                if (NextIsSmaller == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " >= " + ImportantVar + ")");
                    NextIsSmaller = false;
                }

                if (NextIsPlus == true && NextIsPlus == false && NextIsMinus == false && NextIsMultiply == false && NextIsDivide == false)
                {
                    File.AppendAllText(WindowCsFile, " + " + ImportantVar + ")");
                    NextIsPlus = false;
                }

                if (NextIsMinus == true)
                {
                    File.AppendAllText(WindowCsFile, " - " + ImportantVar + ")");
                    NextIsMinus = false;
                }

                if (NextIsMultiply == true)
                {
                    File.AppendAllText(WindowCsFile, " * " + ImportantVar + ")");
                    NextIsMultiply = false;
                }

                if (NextIsDivide == true)
                {
                    File.AppendAllText(WindowCsFile, " / " + ImportantVar + ")");
                    NextIsDivide = false;
                }

                if (TheOneAfterTheNextIsEquals == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsEquals = false;
                    NextIsEquals = true;
                }

                if (TheOneAfterTheNextIsBigger == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsBigger = false;
                    NextIsBigger = true;
                }

                if (TheOneAfterTheNextIsSmaller == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsSmaller = false;
                    NextIsSmaller = true;
                }

                if (TheOneAfterTheNextIsPlus == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsPlus = false;
                    NextIsPlus = true;
                }

                if (TheOneAfterTheNextIsMinus == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsMinus = false;
                    NextIsMinus = true;
                }

                if (TheOneAfterTheNextIsMultiply == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsMultiply = false;
                    NextIsMultiply = true;
                }

                if (TheOneAfterTheNextIsDivide == true)
                {
                    File.AppendAllText(WindowCsFile, ImportantVar);
                    TheOneAfterTheNextIsDivide = false;
                    NextIsDivide = true;
                }
            }

            if (jsonline.Contains("\"reportJoinWords\""))
            {
                string FirstPart = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                string SecondPart = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                File.AppendAllText(WindowCsFile, FirstPart + " + " + SecondPart);
            }

            if (jsonline.Contains("\"reportRound\""))
            {
                File.AppendAllText(WindowCsFile, "Math.Round(");
                ComplicatedMathVariable = true;
                NextIsRound = true;
            }

            //this is not complete yet 
            if (jsonline.Contains("\"reportMonadic\""))
            {
                //This will be even harder - XD 
                string Option = File.ReadAllLines(JSON).Skip(Line + 1).Take(1).First();
                if (Option.Contains("\"abs\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Abs(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"floor\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Floor(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"ceiling\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Ceiling(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"sqrt\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Sqrt(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"sin\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Sin(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"cos\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Cos(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"tan\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Tan("); 
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"asin\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Asin(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"acos\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Acos(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"atan\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Atan(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"ln\""))
                {
                    //I don´t think that this is logarithm natural? -> yeah it is the default one: gotta find a way XD
                    File.AppendAllText(WindowCsFile, "Math.Log(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"log\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Log(");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"e^\""))
                {
                    //I do hope its not the euler number O_O that would be horrific ngl it is :D, and i managed it :3
                    File.AppendAllText(WindowCsFile, "Math.Pow(Math.E, ");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }

                if (Option.Contains("\"10^\""))
                {
                    File.AppendAllText(WindowCsFile, "Math.Pow(10, ");
                    ComplicatedMathVariable = true;
                    NextIsRound = true;
                }
            }
            //Speaking Bubble is missing - this will be achieved by drawing one based on the length of the text here: 
            if (jsonline.Contains("block"))
            {
                string ProbablyVar = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
                if (ProbablyVar.Contains("var"))
                {
                    File.AppendAllText(WindowCsFile, ProbablyVar.Replace("\"@var\":", "").Replace("\"", "").Replace(":", "").Trim());
                }
            }

            if (jsonline.Contains("}"))
            {
                if (LineRound >= 0 && NoLineRound > 0)
                {
                    NoLineRound = NoLineRound - 1;
                    //Don´t append stuff lol
                }

                if (LineRound > 0 && NoLineRound == 0)
                {
                    LineRound = LineRound - 1;
                    File.AppendAllText(WindowCsFile, "\n}");
                }

                if (jsonline.Contains("},") | jsonline.Contains("}"))
                {
                    if (ComplicatedMathVariable == true && NextIsRound == false && IfElse == false)
                    {
                        //Ok so basically i need to check if other blocks were used before that need a ) or a ); at the end
                        File.AppendAllText(WindowCsFile, ")");
                        ComplicatedMathVariable = false;
                        if (DotThing == true)
                        {
                            File.AppendAllText(WindowCsFile, ");");
                            DotThing = false;
                        }
                    }

                    if (ComplicatedMathVariable == true && NextIsRound == true && IfElse == false)
                    {
                        NextIsRound = false;
                    }

                    //This doesn´t work LOL
                    if (IfElse == true)
                    {
                        File.AppendAllText(WindowCsFile, "\n else{");
                        IfElse = false;
                    }
                }
            }
            //I need to find a way to get the } and find out if there was something at the back of it - if yes the } is unneccesary to read - only if there is nothing serious

            //I HAVE AN IDEA - first I let the things be written down without the } and after that i let them get implemented - is way dumber but it should work!!!
        }
        
        string[] XAMLlines = File.ReadAllLines(WindowEditorFile);
        string newXAML = Path.Combine(DefaultGameFolder, "NMainWindow1.axaml");
        foreach (string xamlines in XAMLlines)
        {
            if (!xamlines.Contains("Margin"))
            {
                string newxamlLINE = xamlines.Replace(",", ".");
                File.AppendAllText(newXAML, "\n" + newxamlLINE);
            }
            if (xamlines.Contains("Margin"))
            {
                File.AppendAllText(newXAML, "\n" + xamlines);
            }
        }
        File.WriteAllText(WindowEditorFile, File.ReadAllText(newXAML));
        File.Delete(newXAML);

        int FileEnd = Line;
        Line = 0;
        string[] CsLines = File.ReadAllLines(WindowCsFile);
        //get to find all{ and before the { set a } into the upper line of the {
        string NewCsFile = Path.Combine(DefaultGameFolder, "MainWindow1.axaml.cs"); //HOOOLDUP WAIT A MINUTE - this works but on some lines it is just unnecesary to put a } there but it does i will check the lines where there must not be any and I will patch it :)
        string NewerCsFile = Path.Combine(DefaultGameFolder, "MainWindow2.axaml.cs");

        string ExtraVoidsFile = Path.Combine(DefaultGameFolder, "Voids.axaml.cs");
        string NewLine = File.ReadAllLines(JSON).Skip(Line).Take(1).First();
        foreach (string CsLine in CsLines)
        {
            try
            {
                if (CsLine.Contains("void"))
                {
                    File.AppendAllText(ExtraVoidsFile, "\n" + CsLine);
                    VOID = true;
                }

                if (!CsLine.Contains("void") && !CsLine.Contains("//VoidOver") && VOID == false)
                {
                    File.AppendAllText(NewCsFile, "\n" + CsLine);
                }

                if (!CsLine.Contains("void") && !CsLine.Contains("//VoidOver") && VOID == true)
                {
                    File.AppendAllText(ExtraVoidsFile, "\n" + CsLine);
                }

                if (CsLine.Contains("//VoidOver"))
                {
                    if (VOID == true)
                    {
                        File.AppendAllText(ExtraVoidsFile, "\n" + CsLine);
                        VOID = false;
                        //Else it must be a bug lol
                    }
                }
            }

            catch (Exception ex)
            {
                break;
            }
        }
        //currently removed } cuz of good reasons - gonna implement that again

        File.WriteAllText(WindowCsFile, File.ReadAllText(NewCsFile));
        File.Delete(NewCsFile);
        File.AppendAllText(WindowCsFile, File.ReadAllText(ExtraVoidsFile));
        string NewMAINFile = Path.Combine(Foldername, "NEWMainWindow.axaml.cs");
        string[] ValueLines = File.ReadAllLines(ValueFile);
        foreach(string CsLine in CsLines)
        {
            if(CsLine.Contains("public MainWindow()"))
            {
                foreach(string line in ValueLines)
                {
                    File.AppendAllText(NewMAINFile, "\n" + line);
                }
                File.AppendAllText(NewMAINFile, "\n" + CsLine);
            }
            else
            {
                File.AppendAllText(NewMAINFile, "\n" + CsLine);
            }
        }
        File.WriteAllText(WindowCsFile, File.ReadAllText(NewMAINFile));
        File.Delete(NewMAINFile);
        File.Delete(ValueFile);
        File.Delete(ExtraVoidsFile);
        File.AppendAllText(WindowCsFile, "\n}\n}");
        //read all the stuff of the voids
        //Get the click voids down
        //Copy all pictures to the folder of the finsihed application (Subfolders should be made in the end)
        //Now it should check teh additions multiplications, divisions, and subtrac
        //Ok so currently the code is full of scattered voids - what I mean is that I should change that :(
        File.AppendAllText(WindowEditorFile, "\n   </Canvas>");
        File.AppendAllText(WindowEditorFile, "\n</Window>");
        //Check the MainWíndow for not finished things like the random integer if it has a range that is a variable or something else
        //if there is something it gets edited, and after that the { and } should be set, also this will be a challange

    }



    private async Task CodeSmasher(string JSON, string Extension) //mostly written in CMD - bc it would be too hard elsely :( - works :) - only writing in C# code :/
    {
        if (OperatingSystem.IsWindows())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName); //For sb3 the application name is wrong encoded - fix that ! Nearly finished with the big problem
            string slnFile = Path.Combine(GameFolder, ApplicationName + ".sln");
            string CMDFile = Path.Combine(Foldername, "Game.cmd");//use bat for both linux and windows
                                                                  //Windows x64 test
            File.AppendAllText(CMDFile, " cd " + Foldername);
            File.AppendAllText(CMDFile, "\n dotnet new avalonia.mvvm -o " + ApplicationName);
            File.AppendAllText(CMDFile, "\n cd " + GameFolder);
            File.AppendAllText(CMDFile, "\n dotnet new sln");
            File.AppendAllText(CMDFile, "\n dotnet sln add " + ApplicationName + ".csproj");
            File.AppendAllText(CMDFile, "\n dotnet add package LibVLCSharp");
            //The resolution x File is missing :( need to find a way to get it - maybe C# too :(

            //runs the App
            File.AppendAllText(CMDFile, "\n dotnet build " + ApplicationName + ".sln"); //runs the code - need to make the layout based on the json now!
            var DotnetRun = Process.Start(new ProcessStartInfo(CMDFile) { UseShellExecute = false, CreateNoWindow = true });
            await DotnetRun.WaitForExitAsync();
            //Killer kills Application and then himself, cuz he was seen and fears his future :-( what a poor guy/gal XD

            string KillApplicationFile = Path.Combine(Foldername, "Kill.cmd");
            File.AppendAllText(KillApplicationFile, "\n Taskkill /IM " + ApplicationName + ".exe");
            var KillApp = Process.Start(new ProcessStartInfo(KillApplicationFile) { UseShellExecute = false, CreateNoWindow = true });
            await KillApp.WaitForExitAsync();
            await Task.Run(() => PictureExtractor(".xml"));
            await Task.Run(() => SoundExtractor());
            await Task.Run(() => Designer(JSON, Extension));
            //CopyAllPicturesAndSoundsToBin();
            await Task.Run(() => ExeBuilder());
        }

        if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName); //For sb3 the application name is wrong encoded - fix that ! Nearly finished with the big problem
            string slnFile = Path.Combine(GameFolder, ApplicationName + ".sln");
            var Process1 = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                Arguments = "\n dotnet new avalonia.mvvm -o " + ApplicationName + "\n cd " + GameFolder + "\n dotnet new sln" + "\n dotnet sln add " + ApplicationName + ".csproj" + "\n dotnet add package LibVLCSharp" + "\n dotnet build " + ApplicationName + ".sln",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var ProcessOne = Process.Start(Process1);
            await ProcessOne.WaitForExitAsync();
            //The resolution x File is missing :( need to find a way to get it - maybe C# too :(

            //runs the App
            await Task.Run(() => PictureExtractor(".xml"));
            await Task.Run(() => SoundExtractor());
            await Task.Run(() => Designer(JSON, Extension));
            //CopyAllPicturesAndSoundsToBin();
            //await Task.Run(() => ExeBuilder());
        }
    }

    private async void ExeBuilder() //This has to be edited :(
    {
        if (OperatingSystem.IsWindows())
        {
            string GameBuilder = Path.Combine(Foldername, "GameBuilder.cmd");
            File.AppendAllText(GameBuilder, "\n cd " + GameFolder);
            File.AppendAllText(GameBuilder, "\n dotnet run " + ApplicationName + ".sln");
            var process1 = Process.Start(new ProcessStartInfo(GameBuilder) { UseShellExecute = false, CreateNoWindow = true });
            await process1.WaitForExitAsync();
            
            string GameFinisher = Path.Combine(Foldername, "GameFinisher.cmd");
            File.AppendAllText(GameFinisher, "\n Taskkill /IM " + ApplicationName + ".exe");
            var process2 = Process.Start(new ProcessStartInfo(GameFinisher) { UseShellExecute = false, CreateNoWindow = true });
            await process2.WaitForExitAsync();

            //Here a cmd script for building the winforms project, need to find CPU architecture 
            //Build the application

            string GameDeployer = Path.Combine(Foldername, "GameFinisher.cmd");
            File.AppendAllText(GameDeployer, "\n cd " + Foldername);

            if (windows == true)
            {
                if (formatOfApplication == "32-Bit")
                {
                    Console.WriteLine("32 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }

            if (linux == true)
            {
                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }

            if (MacOs == true)
            {
                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }

            //ProgressBarConverter.Value = 100; //Need to make the whole script in order so it works way better -rn the code is pure spaghetti code, but later it will be a flat Pizza i promise XD
        }

        
        if (OperatingSystem.IsLinux())
        {
            string GameBuilder = Path.Combine(Foldername, "GameBuilder.sh");
            File.AppendAllText(GameBuilder, "\n cd " + GameFolder);
            //Process.Start(ProcessStartInfo()"\n dotnet run " + ApplicationName + ".sln");
            var process1 = Process.Start(new ProcessStartInfo(GameBuilder) { UseShellExecute = false, CreateNoWindow = true });
            process1.WaitForExitAsync();

           
            string GameDeployer = Path.Combine(Foldername, "GameFinisher.sh");
            File.AppendAllText(GameDeployer, "\n cd " + Foldername);

            if (windows == true)
            {
                if (formatOfApplication == "32-Bit")
                {
                    Console.WriteLine("32 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }

            if (linux == true)
            {
                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }

            if (MacOs == true)
            {
                if (formatOfApplication == "64-Bit")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained true");
                }

                if (formatOfApplication == "Arm-64")
                {
                    Console.WriteLine("64 bit Machine");
                    File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-arm64 -p:PublishSingleFile=true --self-contained true");
                }
            }
            //ProgressBarConverter.Value = 100; //Need to make the whole script in order so it works way better -rn the code is pure spaghetti code, but later it will be a flat Pizza i promise XD
        }

    }
}