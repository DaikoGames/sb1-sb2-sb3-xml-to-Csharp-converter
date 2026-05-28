//This Code belongs to Daiko Games - it is copyrighted - don�t use it without permission
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using CliWrap;
using CliWrap.Buffered;
using Downloader;
using ImageMagick;
//Gotta check if I still need SVG when I have ImageMagick
using ImageMagick.Drawing;
using LibVLCSharp.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PopUpWindowNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
//using Microsoft.Win32.SafeHandles;
using System.Threading.Tasks;
using System.Xml;
using BergamotTranslatorSharp;



namespace sb1_sb2_sb3_xml_to_Csharp_converter;

//One of my biggest issues is that bools are written as , even though they need .
public partial class MainWindow : Window
{

    string DotnetVersion;
    public bool windows = false;
    public bool linux = false;
    public bool MacOs = false;
    public bool Snapinator = false;
    public bool Scratch = false;
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

    public string LastSprite;
    public string LastAXAMLname;
    public int SoundNumber = 0;

    public int Line = 0;
    public string GameObjectName;

    bool LastLight = false;

    public bool SomethingNotInstalled = false;
    public bool StillDoing = false;
    public string ModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AImodel");
    public string IconPath;
    public WindowIcon IconWindow;
    public bool REQUIREMENTSmessage = false;
    public List<string> ScratchJnrFiles = new List<string>() { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tulip2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tree4.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tree2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tree3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tree1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Tornado.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Thundercloud.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Theatre.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenGirl3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenGirl2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenGirl1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenBoy3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenBoy2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "TeenBoy1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Teen3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Teen2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Table.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Summer.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Sun.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Suburbs.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Stool.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Starfish.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Star3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Star2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Spring.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Star.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Space.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "SoccerNet.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Soccerball.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Snake.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Shop.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "ShootingStar.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Seahorse.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Scubadiver.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "School.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Savannah.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "SailBoat.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Rowboat.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Rocket.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Red.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Rancher.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Rabbit.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Purple.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "PolarBear.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Planet.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Penguin.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Pig.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Peach.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Pasture.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Park.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "NightTable.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Mushroom.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Mother.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "MoonBkg.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Monkey.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Moon.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Lizard.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Mailbox.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Library.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Lake.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Jungle.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Inuit.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Igloo.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "House4.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "House1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "House3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "House.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Horse.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Gym2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Gym.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Grandmother.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Grandfather.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Girl3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Girl2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Girl1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Girl.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Giraffe.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Frog.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fort.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fly.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fish2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Flowers.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fence.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fish1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Father.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Farmer1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Farmer.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Farm.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fall.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Fairy.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Evergreen.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "EmptyRoom.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Elephant.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Earth.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Duck.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Dragon.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Dog.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Desert.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Daisy3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Daisy2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Daisy1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Daffodil.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "CrescentMoon.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Creek.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Crab.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Cloud1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Classroom.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "City.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Child1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Chicken.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Castle.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Car2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Car1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Car.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Camel.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Cake.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Butterfly.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Cactus.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bus.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Boy3.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Boy2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Boy1.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Boy.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Boat2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Blue.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bird.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bike2.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bike.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bedroom.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bed.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "BeachSunrise.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "BeachDay.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "BeachNight.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bat.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Barn.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Basketball.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Bank.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Baby.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Ball.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Astronaut.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Arctic.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Apple.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Aeroplane.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Apartment.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Zebra.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Woods.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Wizard.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Winter.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Whale.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Weed.svg"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", "Underwater.svg") };
    public List<string> ScratchJnrSoundFiles = new List<string>() { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "copy.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "cut.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "entertap.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "exittap.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "grab.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "keydown.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "pop.mp3"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "snap.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "splash.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "tap.wav"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", "boing.wav") };
    public MainWindow()
    {
        InitializeComponent();
        Core.Initialize();
        //Task.Run(() => CheckRequirements());

        Task.Run(() => ShowOrNot());
        Trace.WriteLine("Initializing the App");
        CultureInfo LanguageOfUser = CultureInfo.CurrentUICulture;
        string Language = LanguageOfUser.TwoLetterISOLanguageName;
        Trace.Write(Language);

        if (Language.Contains("de"))
        {
            //German
            Text1.Content = "Bitte Wählen, oder schreiben Sie die Datei die Sie bearbeiten wollen";
            FileFolderNameTextBox.Watermark = "Ihre Datei sollte eine .sb, .sb2, .sb3 oder eine .xml Datei sein:";
            Text2.Content = "Bitte suchen Sie sich ihren Ordner aus wo das Projekt gespeichert werden soll";
            FolderNameTextBox.Watermark = "Ihr Ordner";
            IconText.Content = "Bitte Wählen Sie die Icon Datei aus wenn es für Sie nötig ist";
            IconTextBox.Watermark = "Die Icon Datei vom Projekt (nicht notwendig)";
            SnapinatorOrNot.Content = "Soll der Converter den Snap Weg oder den Scratch Weg nehmen?";
            SnapinatorCheckBox.Content = "Snapinator";
            ScratchCheckBox.Content = "Scratch";
            OSTextAndCPU.Content = "Bitte suchen Sie das OS, und die Architektur aus ";
            WindowsCheckBox.Content = "Windows";
            LinuxCheckBox.Content = "Linux";
            MacOSCheckBox.Content = "Mac OS";
            convertButton.Content = "konvertieren!";
            GithubRepo.Content = "Dieses Projekt wurde von Daiko Games erstellt";
        }

        if (Language == "en")
        {
            //English
            Text1.Content = "Please Select the File you want to convert or write it down here:";
            FileFolderNameTextBox.Watermark = "Your File, it should be a .sb, .sb2, .sb3, .xml File";
            Text2.Content = "Please select the Folder where your converted Project should be stored:";
            FolderNameTextBox.Watermark = "Your Folder";
            IconText.Content = "Please select the Icon File if necessary:";
            IconTextBox.Watermark = "the Icon of the Application(Not necessary)";
            SnapinatorOrNot.Content = "Should the converter use Snap! or the Scratch Way?:";
            SnapinatorCheckBox.Content = "Snap";
            ScratchCheckBox.Content = "Scratch";
            OSTextAndCPU.Content = "Please select the OS you want to build it for and what architecture:";
            WindowsCheckBox.Content = "Windows";
            LinuxCheckBox.Content = "Linux";
            MacOSCheckBox.Content = "Mac OS";
            convertButton.Content = "Convert!";
            GithubRepo.Content = "This Project was made by: Daiko Games";
        }

        if (!Language.Contains("de") && !Language.Contains("en"))
        {
            Task.Run(() => ChangeLanguage());
        }
        ProgressBarConverter.Minimum = 0;
        ProgressBarConverter.Maximum = 100;
        ProgressBarConverter.Value = 0;
        //In the future people can add their languages here: 
        //Velopack Build and run stuff is not working
        IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "ConverterIcon", "Converter.ico");
        if (File.Exists(IconPath))
        {
            IconWindow = new Avalonia.Controls.WindowIcon(IconPath);
            this.Icon = new WindowIcon(IconPath);
        }
        Theme();

        Task.Run(() => ThemeChange());
        //CheckRequirements();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (StillDoing == true && SomethingNotInstalled == true)
        {
            e.Cancel = true;
        }
        if (StillDoing == false && SomethingNotInstalled == false)
        {
            e.Cancel = false;
        }
    }

    public async Task ChangeLanguage()
    {
        Trace.Write("Changing Language now");
        //https://github.com/Freeesia/BergamotTranslatorSharp
        CultureInfo LanguageOfUser = CultureInfo.CurrentUICulture;
        string Language = LanguageOfUser.TwoLetterISOLanguageName;
        List<string> TranslateText = new List<string> { "Please Select the File you want to convert or write it down here:", "Your File, it should be a .sb, .sb2, .sb3, .xml File", "Please select the Folder where your converted Project should be stored:", "Your Folder", "Please select the Icon File if necessary:", "the Icon of the Application(Not necessary)", "Should the converter use Snap! or the Scratch Way?:", "Snap", "Scratch", "Please select the OS you want to build it for and what architecture:", "Windows", "Linux", "Mac OS", "Convert!", "This Project was made by: Daiko Games", };
        Trace.WriteLine("Initializiere den Translator");
        //https://docs.libretranslate.com

        int TextNmbr = 0;
        try
        {
            foreach(string translatedText in TranslateText)
            {
                TextNmbr = TextNmbr + 1;
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TranslateFolder", "models", "en-" + Language, "config.yml");

                using var service = new BlockingService(configPath);

                var translated = service.Translate(translatedText);

                Trace.WriteLine(translated);

                
                if(TextNmbr == 1)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => Text1.Content = translated);
                }

                if (TextNmbr == 2)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => FileFolderNameTextBox.Watermark = translated);
                }

                if (TextNmbr == 3)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => Text2.Content = translated);
                }

                if (TextNmbr == 4)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => FolderNameTextBox.Watermark = translated);
                }

                if (TextNmbr == 5)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => IconText.Content = translated);
                }

                if(TextNmbr == 6)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => IconTextBox.Watermark = translated);
                }

                if (TextNmbr == 7)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => SnapinatorOrNot.Content = translated);
                }

                if (TextNmbr == 8)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => SnapinatorCheckBox.Content = translated);
                }

                if (TextNmbr == 9)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => ScratchCheckBox.Content = translated);
                }

                if (TextNmbr == 10)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => OSTextAndCPU.Content = translated);
                }

                if (TextNmbr == 11)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => WindowsCheckBox.Content = translated);
                }

                if (TextNmbr == 12)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => LinuxCheckBox.Content = translated);
                }

                if (TextNmbr == 13)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => MacOSCheckBox.Content = translated);
                }

                if(TextNmbr == 14)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => convertButton.Content = translated);
                }

                if(TextNmbr == 15)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => GithubRepo.Content = translated);
                }
            }
        }
        catch (Exception ex)
        {
            Trace.Write("I am sorry but the language you have is currently not available for the converter :(");
        }
    }

    PopUp Requirements = new PopUp();
    public void YesButtonClick(object? sender, RoutedEventArgs e)
    {
        Requirements.ActualPopUp.Close();
    }

    public void OkButtonClick(object? sender, RoutedEventArgs e)
    {
        Requirements.ActualPopUp.Close();
    }

    public void NoButtonClick(object ?sender, RoutedEventArgs e)
    {
        Requirements.ActualPopUp.Close();
    }
    public async Task ShowOrNot()
    {

        while (true)
        {
            if (SomethingNotInstalled == true)
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => convertButton.IsEnabled = false);
                if (REQUIREMENTSmessage == false)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Requirements.PopUpWindow(false, false, Avalonia.Media.Colors.White, Avalonia.Media.Colors.Black, true, 500, 250, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.ico"), "Dependency Error", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.png"),  "#They will be installed asap. If the Progress bar is at 100% and the button should flicker close the whole app and reopen it.#", true, true, false, false);
                        Requirements.YesButton.Click += YesButtonClick;
                        Requirements.OkButton.Click += OkButtonClick;
                        Requirements.NoButton.Click += NoButtonClick;
                        REQUIREMENTSmessage = true;
                    });
                }
            }

            if (SomethingNotInstalled == false)
            {
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => convertButton.IsEnabled = true);
            }

            await Task.Delay(1000);
        }
    }

    public async Task CheckRequirements()
    {
        CultureInfo LanguageOfUser = CultureInfo.CurrentUICulture;
        string Language = LanguageOfUser.TwoLetterISOLanguageName;
        while (true)
        {
            StillDoing = true;
            var DownloadOption = new DownloadConfiguration
            {
                ParallelDownload = true,
                ChunkCount = 8
            };

            string IconFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon");
            if (!Directory.Exists(IconFolder))
            {
                Trace.WriteLine("Directory Fails");
                SomethingNotInstalled = true;
                Directory.CreateDirectory(IconFolder);
            }

            string ConverterFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scratch-Format-converter");
            Trace.WriteLine("Checking Requirements");
            if (!Directory.Exists(ConverterFolder))
            {
                Trace.WriteLine("Directory Fails");
                SomethingNotInstalled = true;
                Directory.CreateDirectory(ConverterFolder);
            }

            //Initialize Components 

            string InstallerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Install");

            if (!Directory.Exists(InstallerFolder))
            {
                Trace.WriteLine("Directory Fails");
                SomethingNotInstalled = true;
                Directory.CreateDirectory(InstallerFolder);
            }

            string SVGFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr");

            if (!Directory.Exists(SVGFolder))
            {
                SomethingNotInstalled = true;
                Trace.WriteLine("Directory Fails");
                Directory.CreateDirectory(SVGFolder);
            }
            // i somehow didn´t includdee all sounds into the github
            string SOUNDfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr");

            if (!Directory.Exists(SOUNDfolder))
            {
                SomethingNotInstalled = true;
                Trace.WriteLine("Directory Fails");
                Directory.CreateDirectory(SOUNDfolder);
            }

            string TranslationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translation");
            if (!Directory.Exists(TranslationFolder))
            {
                SomethingNotInstalled = true;
                Trace.WriteLine("Directory Fails");
                Directory.CreateDirectory(TranslationFolder);
            }

            string PNGFile = Path.Combine(IconFolder, "Convert.png");
            if (!File.Exists(PNGFile))
            {
                SomethingNotInstalled = true;
                var PNGFileDownloader = new DownloadService(DownloadOption);
                await PNGFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/raw/refs/heads/main/ConverterIcon/Converter.png", new DirectoryInfo(IconFolder));
            }

            string ICOfile = Path.Combine(IconFolder, "Convert.ico");
            if (!File.Exists(ICOfile))
            {
                SomethingNotInstalled = true;
                var ICOFileDownloader = new DownloadService(DownloadOption);
                await ICOFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/raw/refs/heads/main/ConverterIcon/Converter.ico", new DirectoryInfo(IconFolder));
            }

            string IcnsFile = Path.Combine(IconFolder, "Convert.icns");
            if (!File.Exists(IcnsFile))
            {
                SomethingNotInstalled = true;
                var ICNSFileDownloader = new DownloadService(DownloadOption);
                await ICNSFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/raw/refs/heads/main/ConverterIcon/Converter.icns", new DirectoryInfo(IconFolder));
            }

            string ConverterFile = Path.Combine(ConverterFolder, "Convert.js");
            if (!File.Exists(ConverterFile))
            {
                SomethingNotInstalled = true;
                var ConverterFileDownloader = new DownloadService(DownloadOption);
                await ConverterFileDownloader.DownloadFileTaskAsync("https://raw.githubusercontent.com/DaikoGames/Scratch-Format-converter/refs/heads/main/Convert.js", new DirectoryInfo(ConverterFolder));
            }

            string NPMpackageJSON = Path.Combine(ConverterFolder, "package.json");
            if (!File.Exists(NPMpackageJSON))
            {
                SomethingNotInstalled = true;
                var NPMpackageJSONDownloader = new DownloadService(DownloadOption);
                await NPMpackageJSONDownloader.DownloadFileTaskAsync("https://raw.githubusercontent.com/DaikoGames/Scratch-Format-converter/refs/heads/main/package.json", new DirectoryInfo(ConverterFolder));

            }

            string NPMpackageLockJSON = Path.Combine(ConverterFolder, "package-lock.json");
            if (!File.Exists(NPMpackageLockJSON))
            {
                SomethingNotInstalled = true;
                var NPMpackageLockJSONDownloader = new DownloadService(DownloadOption);
                await NPMpackageLockJSONDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/Scratch-Format-converter/blob/main/package-lock.json", new DirectoryInfo(ConverterFolder));
            }

            if(Language != "de" && Language != "en")
            {
                var CheckNodeJS = await Cli.Wrap("python").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (CheckNodeJS.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails node");
                    await Cli.Wrap("choco").WithArguments(args => args.Add("install").Add("python --version=3.14.5")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                    await Cli.Wrap("pip").WithArguments(args => args.Add("install").Add("libretranslate")).WithWorkingDirectory(TranslationFolder).ExecuteBufferedAsync();
                    await Cli.Wrap("libretranslate").WithArguments(args => args.Add("--load-only de," + Language)).WithWorkingDirectory(TranslationFolder).ExecuteBufferedAsync();
                    //libretranslate --update-models for all the languages
                    //libretranslate --load-only en,es,fr,de,it,pt,nl,pl,ru,uk,cs,sk,sl,hu,ro,bg,el,da,sv,nb,fi,et,lv,lt,ga,sq,ca,gl,eu,eo
                    //python.exe "Lib\site-packages\libretranslate\main.py"
                }
            }
            

            foreach (string ScratchJnrFile in ScratchJnrFiles)
            {
                if (!File.Exists(ScratchJnrFile))
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("File Fails - SVGLibrary");
                    var SVGDownloader = new DownloadService(DownloadOption);
                    await SVGDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/raw/refs/heads/main/svg_library_ScratchJnr/" + Path.GetFileName(ScratchJnrFile), new DirectoryInfo(SVGFolder));
                }
            }

            foreach (string ScratchJnrFile in ScratchJnrSoundFiles)
            {
                if (!File.Exists(ScratchJnrFile))
                {
                    SomethingNotInstalled = true;
                    var WAVDownloader = new DownloadService(DownloadOption);
                    await WAVDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/raw/refs/heads/main/wav_library_ScratchJnr/" + Path.GetFileName(ScratchJnrFile), new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
                }
            }

            if (OperatingSystem.IsWindows())
            {
                string DotnetInstallerFile = Path.Combine(InstallerFolder, "dotnet-install.ps1");
                if (!File.Exists(DotnetInstallerFile))
                {

                    SomethingNotInstalled = true;
                    var DotnetInstallerFileDownloader = new DownloadService(DownloadOption);
                    await DotnetInstallerFileDownloader.DownloadFileTaskAsync("https://dot.net/v1/dotnet-install.ps1", new DirectoryInfo(InstallerFolder));
                }

                //First check if winget exist
                var WingetVersion = await Cli.Wrap("winget").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (WingetVersion.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails - winget");
                    var DownloadWinget = new DownloadService(DownloadOption);
                    await DownloadWinget.DownloadFileTaskAsync("https://github.com/microsoft/winget-cli/releases/download/v1.28.240/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle", new DirectoryInfo(InstallerFolder));
                    string FileDownload = Path.Combine(InstallerFolder, "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle");
                    Process.Start(new ProcessStartInfo(FileDownload) { UseShellExecute = true });
                }

                var PowershellVersion = await Cli.Wrap("pwsh").WithArguments(args => args.Add("-Version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (PowershellVersion.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails powershell");
                    await Cli.Wrap("winget").WithArguments(args => args.Add("install").Add("--id").Add("Microsoft.PowerShell").Add("--source").Add("winget")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                }

                var ChocolateyVersion = await Cli.Wrap("choco").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (ChocolateyVersion.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails choco");
                    await Cli.Wrap("pwsh").WithArguments(args => args.Add("-c").Add("irm https://community.chocolatey.org/install.ps1|iex")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                }

                var DotnetVersion = await Cli.Wrap("dotnet").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (DotnetVersion.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails dotnet");
                    //install Dotnet, and Avalonia Template too
                    await Cli.Wrap("pwsh").WithArguments(args => args.Add("./dotnet-install.ps1").Add("-Runtime").Add("dotnet").Add("-Version").Add("9.0.0")).ExecuteBufferedAsync(); ;
                    await Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("install").Add("Avalonia.Templates")).ExecuteBufferedAsync();
                }

                var CheckNodeJS = await Cli.Wrap("node").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (CheckNodeJS.ExitCode != 0)
                {
                    SomethingNotInstalled = true;
                    Trace.WriteLine("Dependency Fails node");
                    await Cli.Wrap("choco").WithArguments(args => args.Add("install").Add("nodejs")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                }



                Trace.WriteLine("Nothing Fails anymore");
                SomethingNotInstalled = false;
                StillDoing = false;
            }

            if (OperatingSystem.IsLinux())
            {
                var OSName = await (Cli.Wrap("hostnamectl").ExecuteBufferedAsync());
                string OSNameText = OSName.ToString();
                string[] ALlLines = File.ReadAllLines(OSNameText);
                foreach (string Lines in ALlLines)
                {
                    if (Lines.Contains("Operating System:"))
                    {

                        if (Lines.Contains("Debian"))
                        {
                            //I don´t need powershell on Linux to install dotnet runtime
                            var DotnetVersion = await Cli.Wrap("dotnet").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                            if (DotnetVersion.ExitCode != 0)
                            {
                                await Cli.Wrap("sudo").WithArguments(args => args.Add("apt-get").Add("install").Add("-y").Add("dotnet-sdk" + DotnetVersion)).ExecuteBufferedAsync();
                            }

                            //I need homebrew to install npm and nodeJS
                            var CheckCurl = await Cli.Wrap("curl").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                            if (CheckCurl.ExitCode != 0)
                            {
                                await Cli.Wrap("sudo").WithArguments(args => args.Add("apt").Add("install").Add("curl")).ExecuteBufferedAsync();
                            }

                            var CheckHomebrew = await Cli.Wrap("brew").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                            if (CheckHomebrew.ExitCode != 0)
                            {
                                await Cli.Wrap("curl").WithArguments(args => args.Add("-o-").Add("https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh | bash")).WithEnvironmentVariables(env => env.Set("NONINTERACTIVE", "1")).ExecuteBufferedAsync();
                            }
                            await Cli.Wrap("brew").WithArguments(args => args.Add("install").Add("node@25")).ExecuteBufferedAsync();
                        }

                        if (Lines.Contains("Ubuntu"))
                        {
                            var DotnetVersion = await Cli.Wrap("dotnet").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                            if (DotnetVersion.ExitCode != 0)
                            {
                                await Cli.Wrap("sudo").WithArguments(args => args.Add("apt-get").Add("install").Add("-y").Add("dotnet-sdk" + DotnetVersion)).ExecuteBufferedAsync();
                            }

                            //I need homebrew to install npm and nodeJS
                            var CheckCurl = await Cli.Wrap("curl").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                            if (CheckCurl.ExitCode != 0)
                            {
                                await Cli.Wrap("sudo").WithArguments(args => args.Add("apt").Add("install").Add("curl")).ExecuteBufferedAsync();
                            }

                            var CheckHomebrew = await Cli.Wrap("brew").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                            if (CheckHomebrew.ExitCode != 0)
                            {
                                await Cli.Wrap("curl").WithArguments(args => args.Add("-o-").Add("https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh | bash")).WithEnvironmentVariables(env => env.Set("NONINTERACTIVE", "1")).ExecuteBufferedAsync();
                            }
                            await Cli.Wrap("brew").WithArguments(args => args.Add("install").Add("node@25")).ExecuteBufferedAsync();
                        }
                        if (Lines.Contains("Arch"))
                        {

                        }

                    }
                }
            }

            if (OperatingSystem.IsMacOS())
            {
                //I don´t need powershell on Linux to install dotnet runtime
                var DotnetVersion = await Cli.Wrap("dotnet").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                if (DotnetVersion.ExitCode != 0)
                {
                    await Cli.Wrap("sudo").WithArguments(args => args.Add("apt-get").Add("install").Add("-y").Add("dotnet-sdk-9.0")).ExecuteBufferedAsync();
                }

                //I need homebrew to install npm and nodeJS
                var CheckCurl = await Cli.Wrap("curl").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                if (CheckCurl.ExitCode != 0)
                {
                    await Cli.Wrap("sudo").WithArguments(args => args.Add("apt").Add("install").Add("curl")).ExecuteBufferedAsync();
                }

                var CheckHomebrew = await Cli.Wrap("brew").WithArguments(args => args.Add("--version")).ExecuteBufferedAsync();
                if (CheckHomebrew.ExitCode != 0)
                {
                    await Cli.Wrap("curl").WithArguments(args => args.Add("-o-").Add("https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh | bash")).WithEnvironmentVariables(env => env.Set("NONINTERACTIVE", "1")).ExecuteBufferedAsync();
                }
                await Cli.Wrap("brew").WithArguments(args => args.Add("install").Add("node@25")).ExecuteBufferedAsync();
            }

            //Check if npm is installed at the location of ScratchConverter
            await (Cli.Wrap("npm").WithArguments(args => args.Add("install")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync());
            await Task.Delay(6000);

        }

    }

    public async Task ThemeChange()
    {
        Trace.WriteLine("Changing the Theme");
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
            ProgressBarConverter.Foreground = Avalonia.Media.Brushes.Gray;
            convertButton.Background = Avalonia.Media.Brushes.Transparent;
            convertButton.Foreground = Avalonia.Media.Brushes.White;
            convertButton.BorderBrush = Avalonia.Media.Brushes.White;
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
            ProgressBarConverter.Background = Avalonia.Media.Brushes.White;
            ProgressBarConverter.Foreground = Avalonia.Media.Brushes.Gray;
            convertButton.Background = Avalonia.Media.Brushes.Transparent;
            convertButton.Foreground = Avalonia.Media.Brushes.Black;
            convertButton.BorderBrush = Avalonia.Media.Brushes.Black;
            GithubRepo.Background = Avalonia.Media.Brushes.Transparent;
            GithubRepo.Foreground = Avalonia.Media.Brushes.Black;
        }
        //on Windows there are filters for Colorblind people, but on linux its not on every OS, i gotta think if i add that feature or not 
        //Ok so i checked the colours, the grayscale and inverted grayscale or generally inverted colours are looking bad for me I need help from colourblind ppl :) 
    }
    public async void FileSearcherVoid(object sender, RoutedEventArgs args)
    {
        Trace.WriteLine("Searching for a File");

        var TopLevelg = TopLevel.GetTopLevel(this);
        var files = await TopLevelg.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "File Chooser",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("All Filetypes")
                {
                    Patterns = new[] { "*.sb", "*.sb2", "*.sb3", "*.sjr" , "*.xml"}
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
                new FilePickerFileType("sjr Files")
                {
                    Patterns = new[] { "*.sjr" }
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
            var selectedFileg = files[0];  // Get the first file selected - this is null - I don�t know why?
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
        Trace.WriteLine("Searching for a Folder");

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
        Trace.WriteLine("Searching for an Icon");

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
            var selectedFileg = files[0];  // Get the first file selected - this is null - I don�t know why?
            if (selectedFileg.TryGetLocalPath() is string selectedFile)
            {
                IconTextBox.Text = selectedFile;
                ICON = selectedFile;
            }
        }
    }

    public async void GithubLinkPressed(object sender, RoutedEventArgs args)
    {
        Trace.WriteLine("Visiting my Github :)");
        string Link = "https://github.com/DaikoGames";
        Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
        //System.Diagnostics.Process.Start("explorer", Link);
    }

    public async void ConvertButton(object sender, RoutedEventArgs args)
    {
        if (SomethingNotInstalled == true)
        {
            convertButton.IsEnabled = false;

        }

        if (SomethingNotInstalled == false)
        {
            Trace.WriteLine("Starting Conversion");
            convertButton.IsEnabled = false;
            var DotnetVersionOfComputer = await Cli.Wrap("dotnet").WithArguments("--version").ExecuteBufferedAsync();
            string DotnetVersionT = DotnetVersionOfComputer.StandardOutput.Trim();
            DotnetVersion = Convert.ToString(int.Parse(DotnetVersionT.Split('.')[0])) + ".0";
            Trace.WriteLine("dotnet version: " + DotnetVersion);

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

            if (X64CheckBox.IsChecked == true)
            {
                formatOfApplication = "64-Bit";
            }

            if (X32CheckBox.IsChecked == true)
            {
                formatOfApplication = "32-Bit";
            }

            if (Arm64CheckBox.IsChecked == true)
            {
                formatOfApplication = "Arm-64";
            }

            if (SnapinatorCheckBox.IsChecked == true)
            {
                Snapinator = true;
            }

            if (ScratchCheckBox.IsChecked == true)
            {
                Scratch = true;
            }

            if (WindowsCheckBox.IsChecked == true | LinuxCheckBox.IsChecked == true | MacOSCheckBox.IsChecked == true)
            {
                try
                {
                    if (extensionS.Contains(".sb") | extensionS.Contains(".sb2") | extensionS.Contains(".sb3") | extensionS.Contains(".sjr"))
                    {
                        if (Scratch == true | Snapinator == true)
                        {
                            await Task.Run(() => sbfiles(extensionS));
                        }
                    }
                    if (extensionS.Contains(".xml"))
                    {
                        await Task.Run(() => xmlfiles(extensionS));
                    }
                }
                catch (Exception ex)
                {
                    await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        PopUp Applicationlocation = new PopUp();
                        Applicationlocation.PopUpWindow(false, false, Avalonia.Media.Colors.White, Avalonia.Media.Colors.Black, true, 500, 350,Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.ico"), "Successful Build", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MessageBoxImages", "Info.png"), "Your Project was built for " + formatOfApplication + " and is located at " + Foldername, false, true, false, false);

                    }

                    );

                }
            }

            else
            {
                Console.WriteLine("Either you clicked nothign or all - just choose one at a time :-(");
            }
        }

    }

    //The automation of converting .sb3 Files to .xml FIles isn�t working properly - so the conversion from .sb to .sb3 or .sb2 to .sb3 with Auto Hot Key - its probably because of the path - I will take a closer look to that.
    //Ok so i found out a thing, the thing is that the Neutralino app needs to be full screen and a click needs to be simulated. Gotta tell the user to relax lol XD

    public async Task sbfiles(string Extension) //This doesn�t work bc of Scratch i need to find me failure :/ lol XD
    {
        Trace.WriteLine("Converting .sb or .sb2 to .sb3 now");
        //ok i have to figure out something else, good i have an idea - the user chooses the path of where it should be converted, the program gets opened via cmd, and the user has to do it himself, when the program is over the user just closes the application and the conversion starts - waay better than automation tools
        string FileExtension = Path.GetExtension(Filename);
        if (FileExtension == ".sb" | FileExtension == ".sb2" | FileExtension == ".sb3")
        {
            if (OperatingSystem.IsWindows() | OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
            {
                string ConverterFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScratchConverter");
                string ConvertDestination = Path.Combine(ConverterFolder, "project.sb");
                File.Copy(Filename, ConvertDestination, true);
                string JSFile = Path.Combine(ConverterFolder, "Convert.js");
                //Ok it works, now check if the modules are installed - if not install with npm again LOL
                //My users have to have Powershell, i know it sounds bad, but it is the most efficient way there is

                if (FileExtension == ".sb" | FileExtension == ".sb2")
                {
                    if (OperatingSystem.IsWindows())
                    {
                        await Cli.Wrap("node").WithArguments(args => args.Add("Convert.js")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
                        string OldSB3 = Path.Combine(ConverterFolder, "project.sb3");
                        string NewSB3 = Path.Combine(Foldername, "project.sb3");
                        File.Copy(OldSB3, NewSB3, true);
                        File.Delete(ConvertDestination);
                        File.Delete(OldSB3);
                        FileExtension = ".sb3";
                        Filename = NewSB3;
                    }
                }

                if (FileExtension == ".sb3")
                {
                    string ZipFILE = Path.Combine(Foldername, Path.GetFileNameWithoutExtension(Filename) + ".zip");
                    File.Copy(Filename, ZipFILE);
                    File.Delete(Filename);
                    ZipFile.ExtractToDirectory(ZipFILE, Foldername, true);
                }
                //Convert to C# now directly
            }
        }
        if (FileExtension == ".sjr")
        {

            //Now convert the Scratch Junior project to normal Scratch
            string Zipfile = Path.Combine(Foldername, Path.GetFileNameWithoutExtension(Filename) + ".zip");
            File.Copy(Filename, Zipfile, true);
            ZipFile.ExtractToDirectory(Zipfile, Foldername, true);
            File.Delete(Zipfile);
            string MainJSON = Path.Combine(Foldername, "project", "data.json");
            var parsedJSON = JToken.Parse(File.ReadAllText(MainJSON));
            string PrettyJSON = parsedJSON.ToString(Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(MainJSON, PrettyJSON);
            string[] JSONText = File.ReadAllLines(MainJSON);
            ApplicationName = "ScratchJnr";
            string DefaultFolder = Path.Combine(Foldername, ApplicationName);
            if (OperatingSystem.IsWindows())
            {
                GameFolder = Path.Combine(DefaultFolder + "\\bin\\Debug\\net" + DotnetVersion);
            }
            if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
            {
                GameFolder = Path.Combine(DefaultFolder + "/bin/Debug/net" + DotnetVersion);
            }
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("avalonia.mvvm").Add("-o").Add(ApplicationName)).WithWorkingDirectory(Foldername).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("sln")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("sln").Add("add").Add(ApplicationName + ".csproj")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("LibVLCSharp")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("VideoLAN.LibVLC.Windows")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("VideoLAN.LibVLC.Mac")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("easyAsyncCancel")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("build").Add(ApplicationName + ".slnx")).WithWorkingDirectory(DefaultFolder).ExecuteAsync());


            string WindowEditorFile = Path.Combine(DefaultFolder, "MainWindow.axaml");
            string WindowCsFile = Path.Combine(DefaultFolder, "MainWindow.axaml.cs");

            File.AppendAllText(WindowEditorFile, "<Window             Name=\"Default\""); //Es fehlt hier was
            File.AppendAllText(WindowEditorFile, "\n                  xmlns=\"https://github.com/avaloniaui\"");
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
            File.AppendAllText(WindowEditorFile, "\n<Canvas Name=\"ProjectCanvas\">");
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
            string XCoordinate;
            string YCoordinate;
            string Width;
            string Height;
            string TimeToWait;
            string MessageOfSprite;

            //It seems like Scratch Junior doesn´t have pictures embedded into the code nor to the zip, so i have to get the original Pictures somehow 

            foreach (string LINE in JSONText)
            {
                Line = Line + 1;
                if (LINE.Contains("\"lastSprite\""))
                {
                    LastSprite = LINE.Replace("\"lastSprite\":", "").Replace("\"", "").Replace(",", "").Replace(" ", "_").Trim();
                }

                if (LINE.Contains("\"xcoor\""))
                {
                    XCoordinate = LINE.Replace("\"xcoor\":", "").Replace(",", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n public double " + LastSprite + "XCOORDINATE = " + XCoordinate + ";");
                }

                if (LINE.Contains("\"ycoor\""))
                {
                    YCoordinate = LINE.Replace("\"ycoor\":", "").Replace(",", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n public double " + LastSprite + "YCOORDINATE = " + YCoordinate + ";");
                }

                if (LINE.Contains("\"message\""))
                {
                    MessageOfSprite = File.ReadAllLines(MainJSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n public bool " + MessageOfSprite + " = false;");
                }
            }

            Line = 0;

            foreach (string LINE in JSONText)
            {
                if (LINE.Contains("\"playsnd\""))
                {
                    Line = Line + 1;
                    //The sound player must be given  above lol
                    //Example:

                    /* System;
                    using System.IO;
                    using Avalonia.Controls;
                    using LibVLCSharp.Shared;
                    using Avalonia.Interactivity;
                    using Avalonia.Media.Imaging;
                    namespace ScratchJnr;

                   public partial class MainWindow : Window
                   {
                       private LibVLC _libVLC;
                       private MediaPlayer _mediaPlayer;

                       public MainWindow()
                       {
                           InitializeComponent();
                           Core.Initialize();

                           Blue.Source = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Blue.png"));

                           // 2. Initialize the class-level fields (NO 'using' or 'var' here)
                           _libVLC = new LibVLC();
                           _mediaPlayer = new MediaPlayer(_libVLC);

                           // 3. Create the media and play
                           var media = new Media(_libVLC, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pop.mp3"), FromType.FromPath);

                           _mediaPlayer.Media = media;
                           _mediaPlayer.Play();
                       }
                   }*/
                string SOUNDname = File.ReadAllLines(MainJSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n private LibVLC libVLC" + SOUNDname + ";");
                    File.AppendAllText(WindowCsFile, "\n private LibVLC MediaPlayer MediaPlayer" + SOUNDname + ";");
                }
            }
            File.AppendAllText(WindowCsFile, "\n public MainWindow(){");
            File.AppendAllText(WindowCsFile, "\n InitializeComponent();");
            /*if(IconTextBox.Text != null)
            {
                File.Copy(IconTextBox.Text.ToString(), Path.Combine(GameFolder, "GameIcon.ico"), true);
                File.AppendAllText(WindowCsFile, "\n this.Icon = new WindowIcon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + \"GameIcon.ico\"));");
            }*/

            Line = 0;

            foreach (string LINE in JSONText)
            {
                Line = Line + 1;
                if (LINE.Contains("\"md5\":"))
                {

                    string SpriteName = LINE.Replace("\"md5\":", "").Replace("\"", "").Replace(",", "").Trim();

                    string SpriteFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr", SpriteName);
                    if (File.Exists(SpriteFolder))
                    {
                        //now add the axaml and the C# Text

                        //Values (Messages) are the same names as Images currently -FIX IT ASAP lol

                        string NewImageName = Path.Combine(GameFolder, Path.GetFileNameWithoutExtension(SpriteName) + ".png");

                        using (var SVGImage = new MagickImage(SpriteFolder))
                        {
                            SVGImage.Format = MagickFormat.Png;
                            SVGImage.Write(NewImageName);
                        }

                        using (var PNGimage = new MagickImage(NewImageName))
                        {
                            string RoughPNGwidth = Convert.ToString(PNGimage.Width);
                            string RoughPNGheight = Convert.ToString(PNGimage.Height);
                            double PNGwidth = Convert.ToDouble(RoughPNGwidth);
                            double PNGheight = Convert.ToDouble(RoughPNGheight);
                            File.AppendAllText(WindowEditorFile, "\n <Image");
                            File.AppendAllText(WindowEditorFile, " Name=\"" + Path.GetFileNameWithoutExtension(SpriteName) + "\"");
                            File.AppendAllText(WindowEditorFile, "\n                  Width=\"" + (PNGwidth) + "\"");
                            File.AppendAllText(WindowEditorFile, "\n                  Height=\"" + (PNGheight) + "\"");
                            //THESE COORDINATE SYSTEMS ARE DIFFERENT -need to understand canvas coordinate system correctly
                            File.AppendAllText(WindowEditorFile, "\n                  Canvas.Left=\"" + 0 + "\"");
                            File.AppendAllText(WindowEditorFile, "\n                  Canvas.Top=\"" + 0 + "\"");
                            File.AppendAllText(WindowEditorFile, ">");
                            File.AppendAllText(WindowEditorFile, "\n           </Image>");
                            File.AppendAllText(WindowCsFile, "\n " + Path.GetFileNameWithoutExtension(SpriteName) + ".Source = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, \"" + Path.GetFileName(NewImageName) + "\"));");
                        }

                    }
                }

                if (LINE.Contains("playsnd"))
                {
                    string SoundName = File.ReadAllLines(MainJSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                    string SoundPathOrigin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr", SoundName);
                    string NewSoundPath = Path.Combine(GameFolder, SoundName);
                    File.Copy(SoundPathOrigin, NewSoundPath, true);
                }

            }
            File.AppendAllText(WindowEditorFile, "</Canvas>");
            File.AppendAllText(WindowEditorFile, "</Window>");
            Line = 0;

            foreach (string LINE in JSONText)
            {
                Line = Line + 1;
                if (LINE.Contains("\"lastSprite\""))
                {
                    //The Name of a sprite
                }
                if (LINE.Contains("\"xcoor\""))
                {
                    XCoordinate = LINE.Replace("\"xcoor\":", "").Replace(",", "").Trim();
                }
                if (LINE.Contains("\"ycoor\""))
                {
                    YCoordinate = LINE.Replace("\"ycoor\":", "").Replace(",", "").Trim();
                }
                if (LINE.Contains("\"w\""))
                {
                    Width = LINE.Replace("\"w\":", "").Replace(",", "").Trim();
                    //Width
                }
                if (LINE.Contains("\"h\""))
                {
                    Height = LINE.Replace("\"w\":", "").Replace(",", "").Trim();
                    //Height
                }
                if (LINE.Contains("\"forward\""))
                {

                }
                if (LINE.Contains("\"back\""))
                {

                }
                if (LINE.Contains("\"up\""))
                {

                }
                if (LINE.Contains("\"down\""))
                {

                }
                if (LINE.Contains("\"right\""))
                {
                    //turn right
                }

                if (LINE.Contains("\"left\""))
                {
                    //turn right
                }

                if (LINE.Contains("\"hop\""))
                {
                    //Hopping
                }

                if (LINE.Contains("\"onclick\""))
                {
                    //Clicking
                }

                if (LINE.Contains("\"say\""))
                {

                }

                if (LINE.Contains("\"grow\""))
                {

                }

                if (LINE.Contains("\"shrink\""))
                {

                }

                if (LINE.Contains("\"same\""))
                {

                }

                if (LINE.Contains("\"hide\""))
                {

                }

                if (LINE.Contains("\"show\""))
                {

                }

                if (LINE.Contains("\"ontouch\""))
                {

                }

                if (LINE.Contains("\"playsnd\""))
                {
                    //The sound player must be given  above lol

                    string SOUNDname = File.ReadAllLines(MainJSON).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                    File.AppendAllText(WindowCsFile, "\n LibVLC" + Path.GetFileNameWithoutExtension(SOUNDname) + " = new LibVLC();");
                    File.AppendAllText(WindowCsFile, "\n MediaPlayer MediaPlayer" + Path.GetFileNameWithoutExtension(SOUNDname) + " = new MediaPlayer(LibVLC" + Path.GetFileNameWithoutExtension(SOUNDname) + ");");
                    File.AppendAllText(WindowCsFile, "\n var MediaOfMediaPlayer" + Path.GetFileNameWithoutExtension(SOUNDname) + " = new Media(LibVLC" + Path.GetFileNameWithoutExtension(SOUNDname) + " , Path.Combine(AppDomain.CurrentDomain.BaseDirectory, \"" + SOUNDname + "\")" + ", FromType.FromPath);");
                    File.AppendAllText(WindowCsFile, "\n MediaPlayer" + Path.GetFileNameWithoutExtension(SOUNDname) + ".Media = MediaOfMediaPlayer" + Path.GetFileNameWithoutExtension(SOUNDname));
                    File.AppendAllText(WindowCsFile, "\n MediaPlayer" + Path.GetFileNameWithoutExtension(SOUNDname) + ".Play();");

                }

                if (LINE.Contains("\"wait\""))
                {

                }

                if (LINE.Contains("\"stopmine\""))
                {

                }

                if (LINE.Contains("\"setspeed\""))
                {

                }

                if (LINE.Contains("\"repeat\""))
                {

                }

                if (LINE.Contains("\"onmessage\""))
                {

                }


                if (LINE.Contains("\"forever\""))
                {

                }
            }

            File.AppendAllText(WindowCsFile, "}");
        }
    }

    private async void xmlfiles(string extension)
    {
        Line = 0;
        string jsonPath = Path.Combine(Foldername, "Project.json");
        string newxml = Path.Combine(Foldername, fileName);
        File.Copy(Filename, newxml, true);
        XmlDocument document = new XmlDocument();
        document.Load(newxml);
        string json = JsonConvert.SerializeXmlNode(document, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(jsonPath, json);

        //CodeExtractor(jsonPath);
        //await Task.Run(() => NameOfProjectChanger(jsonPath, extension));
        int currentLine = 0;
        string CsFile = Path.Combine(Foldername, GameObjectName + ".cs");
        string[] Json = File.ReadAllLines(jsonPath);

        foreach (string line in Json)
        {
            try
            {
                if (extension == ".sb2" | extension == ".sb3")
                {
                    ApplicationName = Path.GetFileNameWithoutExtension(Filename).Replace(" ", "-");
                }
                if (extension == ".xml")
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

        //await Task.Run(() => CodeSmasher(jsonPath, extension));
        if (OperatingSystem.IsWindows() | OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
        {
            GameFolder = Path.Combine(Foldername, ApplicationName); //For sb3 the application name is wrong encoded - fix that ! Nearly finished with the big problem
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("avalonia.mvvm").Add("-o").Add(ApplicationName)).WithWorkingDirectory(Foldername).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("sln")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("sln").Add("add").Add(ApplicationName + ".csproj")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("LibVLCSharp")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("VideoLAN.LibVLC.Windows")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("VideoLAN.LibVLC.Mac")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("add").Add("package").Add("easyAsyncCancel")).WithWorkingDirectory(GameFolder).ExecuteAsync());
            await (Cli.Wrap("dotnet").WithArguments(args => args.Add("build").Add(ApplicationName + ".slnx")).WithWorkingDirectory(GameFolder).ExecuteAsync());

            //Need multiple Cli.Wraps
            if (Snapinator == true && Scratch == false)
            {
                if (OperatingSystem.IsWindows())
                {
                    GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net" + DotnetVersion);
                }
                if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
                {
                    GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net" + DotnetVersion);
                }
                //This is wrong - use same mechanics from Designer Winforms - 

                string[] LInes = File.ReadAllLines(jsonPath);
                bool NextisImageName = false;
                string ThisImageName;
                bool PNG = false;
                bool JPG = false;
                bool SVG = false;

                if (Snapinator == true)
                {
                    foreach (string line in LInes)
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

                                //This doesn�t work LOL XD 
                                //Get the ratio - maybe its bc the �File doesn�t have a true size declared

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
                                            PNGImage.BackgroundColor = MagickColors.Transparent;
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

                if (Scratch == true)
                {
                    //Transparent pixels are converted to white ones somehow - BUG
                    string[] PICTUREfiles = Directory.GetFiles(Foldername, "*.svg", SearchOption.TopDirectoryOnly);
                    foreach (string pictureFILEsvg in PICTUREfiles)
                    {
                        using (var SVGimage = new MagickImage(pictureFILEsvg))
                        {
                            SVGimage.BackgroundColor = MagickColors.Transparent;
                            SVGimage.Format = MagickFormat.Png;
                            SVGimage.Write(Path.Combine(GameFolder, Path.GetFileNameWithoutExtension(pictureFILEsvg) + ".png"));
                        }
                    }

                    string[] PICTUREfiles2 = Directory.GetFiles(Foldername, "*.jpg", SearchOption.TopDirectoryOnly);
                    foreach (string pictureFILEsvg in PICTUREfiles)
                    {
                        using (var SVGimage = new MagickImage(pictureFILEsvg))
                        {
                            SVGimage.BackgroundColor = MagickColors.Transparent;
                            SVGimage.Format = MagickFormat.Png;
                            SVGimage.Write(Path.Combine(GameFolder, Path.GetFileNameWithoutExtension(pictureFILEsvg) + ".png"));
                        }
                    }
                }
            }

            //await Task.Run(() => SoundExtractor());

            if (OperatingSystem.IsWindows())
            {
                GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net" + DotnetVersion);
            }
            if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
            {
                GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net" + DotnetVersion);
            }
            Line = 0;

            string[] Lines = File.ReadAllLines(jsonPath);
            foreach (string line in Lines)
            {
                Line = Line + 1;
                try
                {
                    if (line.Contains("\"sound\":"))
                    {
                        string soundnameLine = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
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

            //await Task.Run(() => Designer(JSON, Extension));

            int ImportantCharacter = 0;
            int AllLines = 0;
            string DefaultGameFolder = Path.Combine(Foldername, ApplicationName);
            // i need to rename the pictures

            if (OperatingSystem.IsWindows())
            {
                GameFolder = Path.Combine(Foldername, ApplicationName + "\\bin\\Debug\\net" + DotnetVersion);
            }
            if (OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
            {
                GameFolder = Path.Combine(Foldername, ApplicationName + "/bin/Debug/net" + DotnetVersion);
            }

            //Setting up the basics of the Form1.Designer.cs
            string WindowEditorFile = Path.Combine(DefaultGameFolder, "MainWindow.axaml"); //IT FINALLY WORKS :) - porting to Avalonia --> not completely working tho
            string WindowCsFile = Path.Combine(DefaultGameFolder, "MainWindow.axaml.cs");
            File.AppendAllText(WindowEditorFile, "<Window             Name=\"Default\""); //Es fehlt hier was
            File.AppendAllText(WindowEditorFile, "\n                  xmlns=\"https://github.com/avaloniaui\"");
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
            File.AppendAllText(WindowEditorFile, "\n<Canvas Name=\"ProjectCanvas\">");
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

            Line = 0;
            List<string> MessageNames = new List<string>();
            int pngname = 0;

            Line = 0;
            pngname = pngname + 1;
            //byte[] pngbytes = File.ReadAllBytes(pngs);
            string[] JsonLines = File.ReadAllLines(jsonPath);

            string ImageNameN;
            string NewImageName;

            //Here make the icon of the Project if needed in the future

            if (ICON != null)
            {
                //Gotta add a watermark LOL
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

            if (Scratch == false && Snapinator == true)
            {
                foreach (string jsonline in JsonLines)
                {
                    if (jsonline.Contains("\"receiveInteraction\""))
                    {

                    }
                }
                string ValueFile = Path.Combine(DefaultGameFolder, "Value.axaml.cs");
                foreach (string jsonline in JsonLines)
                {
                    Line = Line + 1;
                    if (jsonline.Contains("\"receiveMessage\""))
                    {
                        string messageNameRough = File.ReadLines(jsonPath).Skip(Line).First();
                        string messageName = messageNameRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Replace("(", "_").Replace(")", "_").Trim().Replace(" ", "_");
                        if (!MessageNames.Contains(messageName))
                        {
                            MessageNames.Add(messageName);
                            File.AppendAllText(ValueFile, " \n public bool " + messageName + " = false;");
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
                        string NameOfVar = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Replace("(", "_").Replace(")", "_").Trim().Replace(" ", "_");
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
                File.AppendAllText(WindowCsFile, "\n public int WindowWidth = 480;");
                File.AppendAllText(WindowCsFile, "\n public int WindowHeight = 360;");

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

                        string messageNameRough = File.ReadLines(jsonPath).Skip(Line).First();
                        string messageName = messageNameRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Replace("(", "_").Replace(")", "_").Trim().Replace(" ", "_");
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
                List<string> ShowableVars = new List<string>();
                //BIG PROBLEM - when an image has a negative y coordinate the thing somehow doesn�t always work correctly
                //In Update 1.5 i will add ZIndex by default here :3
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
                                using (var PNGimage = new MagickImage(Path.Combine(GameFolder, File.ReadAllLines(jsonPath).Skip(Line - 1).Take(1).First().Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim() + ".png")))
                                {
                                    string RoughPNGwidth = Convert.ToString(PNGimage.Width);
                                    string RoughPNGheight = Convert.ToString(PNGimage.Height);
                                    double PNGwidth = Convert.ToDouble(RoughPNGwidth);
                                    double PNGheight = Convert.ToDouble(RoughPNGheight);
                                    double SCALEROUGH = double.Parse(File.ReadAllLines(jsonPath).Skip(ScaleNumber).Take(1).First().Replace("\"@scale\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                                    double ScALE = SCALEROUGH;
                                    double XCOORDINATEROUGH = double.Parse(File.ReadAllLines(jsonPath).Skip(XCoordinateLine).Take(1).First().Replace("\"@x\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                                    double XCOORDINATE = XCOORDINATEROUGH;
                                    double YCOORDINATEROUGH = double.Parse(File.ReadAllLines(jsonPath).Skip(YCoordinateLine).Take(1).First().Replace("\"@y\":", "").Replace("\"", "").Replace(",", "").Trim(), CultureInfo.InvariantCulture);
                                    double YCOORDINATE = YCOORDINATEROUGH;
                                    File.AppendAllText(WindowEditorFile, "\n                  Width=\"" + ((PNGwidth) * ScALE) + "\"");
                                    File.AppendAllText(WindowEditorFile, "\n                  Height=\"" + ((PNGheight) * ScALE) + "\"");
                                    //THESE COORDINATE SYSTEMS ARE DIFFERENT -need to understand canvas coordinate system correctly
                                    File.AppendAllText(WindowEditorFile, "\n                  Canvas.Left=\"" + (240 + XCOORDINATE - ((PNGwidth * ScALE) / 2)) + "\"");
                                    File.AppendAllText(WindowEditorFile, "\n                  Canvas.Top=\"" + (180 + (-YCOORDINATE) - ((PNGheight * ScALE) / 2)) + "\"");
                                    File.AppendAllText(WindowEditorFile, ">");
                                    File.AppendAllText(WindowEditorFile, "\n           </Image>");
                                }
                                File.AppendAllText(WindowCsFile, "@" + ImageName + ".png\"));");
                                //Now write the whole thing into the File
                                NextisImageName = false;
                            }

                            if (jsonline.Contains("\"@name\":") && NextisImageName == false)
                            {
                                ThisImageName = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                                //most probably it will check the first name
                                string XCoordinateRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                                string YCoordinateRough = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First();
                                if (XCoordinateRough.Contains("\"@x\":") && YCoordinateRough.Contains("\"@y\":"))
                                {
                                    XCoordinateLine = Line + 1;
                                    YCoordinateLine = Line + 2;

                                    string ScaleRough = File.ReadAllLines(jsonPath).Skip(Line + 4).Take(1).First();
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

                List<string> TextList = new List<string>();
                foreach (string jsonline in JsonLines)
                {
                    // This has serious bugs, going to do that maybe in a week :( don�t have time cuz of school LMAO
                    Line = Line + 1;

                    if (jsonline.Contains("\"bubble\"") | jsonline.Contains("\"doThink\"") | jsonline.Contains("\"doSayFor\"") | jsonline.Contains("\"doThinkFor\""))
                    {
                        string Text = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\": ", "").Replace("\"", "").Replace(",", "").Trim();
                        TextNumber = TextNumber + 1;
                        string FileNameTitle = Text.Replace(" ", "_").Replace("�", "").Replace("^", "").Replace("!", "").Replace("\"", "").Replace("�", "").Replace("$", "").Replace("%", "").Replace("&", "").Replace("/", "").Replace("{", "").Replace("(", "").Replace("[", "").Replace(")", "").Replace("]", "").Replace("=", "").Replace("}", "").Replace("?", "").Replace("\\", "").Replace("`", "").Replace("�", "").Replace("@", "").Replace("*", "").Replace("+", "").Replace("~", "").Replace("'", "").Replace("#", "").Replace(">", "").Replace("<", "").Replace("|", "").Replace(";", "").Replace(",", "").Replace(":", "").Replace(".", "").Replace("-", "").Replace("_", "").Trim();
                        string FileName = Path.Combine(GameFolder, FileNameTitle + ".png");
                        if (!File.Exists(FileName))
                        {
                            using var ImagePath = new MagickImage(MagickColors.Transparent, 480, 360);
                            new Drawables()
                                .FontPointSize(67)
                                .Font("Calibri")
                                .FillColor(MagickColors.Black)
                                .TextAlignment(ImageMagick.TextAlignment.Center)
                                .Text(240, 180, Text)
                                .Draw(ImagePath);
                            ImagePath.Write(FileName);
                        }

                        File.AppendAllText(WindowEditorFile, "\n           <Image Name=\"Image" + TextNumber + "\"");
                        File.AppendAllText(WindowEditorFile, "\n                  Width=\"" + 240 + "\"");
                        File.AppendAllText(WindowEditorFile, "\n                  Height=\"" + 180 + "\"");
                        File.AppendAllText(WindowEditorFile, "\n                  Canvas.Left=\"" + 240 + "\"");
                        File.AppendAllText(WindowEditorFile, "\n                  Canvas.Top=\"" + 180 + "\"");
                        File.AppendAllText(WindowEditorFile, ">");
                        File.AppendAllText(WindowEditorFile, "\n           </Image>");
                        File.AppendAllText(WindowCsFile, "\n img" + TextNumber + ".Source = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @\"" + FileName + "\"));");
                    }


                    if (jsonline.Contains("\"doShowVar\""))
                    {
                        //The Only thing that is wrong here is the name i think
                        File.AppendAllText(WindowCsFile, "\"doShowVar\"");
                        string VariableName = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                        //Ok i need to make a picture out of the var XD how do i do that - yeah Skia sharp probably with a textbox inside that can�t be edited
                        if (!ShowableVars.Contains(VariableName))
                        {
                            ShowableVars.Add(VariableName);
                            File.AppendAllText(WindowEditorFile, "\n <Button Name=\"Image" + VariableName + "\"");
                            File.AppendAllText(WindowEditorFile, "\n Width=\"75\"");
                            File.AppendAllText(WindowEditorFile, "\n Height=\"30\"");
                            File.AppendAllText(WindowEditorFile, "\n Canvas.Top=\"0\"");
                            File.AppendAllText(WindowEditorFile, "\n Canvas.Lef=\"0\"");
                            File.AppendAllText(WindowEditorFile, "\n />");
                        }
                    }
                }

                Line = 0;

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
                        if (File.ReadAllLines(jsonPath).Skip(Line).Take(1).Contains("\"@center-x\":"))
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
                        string EdgeOrNot = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                        if (EdgeOrNot.Contains("\"edge\""))
                        {
                            BounceOff = true;
                            break;
                        }

                        if (EdgeOrNot.Contains("\"l\":"))
                        {
                            string MousePointerProlly = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"option\": \"", "").Replace("\"", "").Trim();
                            if (MousePointerProlly == "mouse-pointer")
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
                        if (File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Contains("\"@center-x\":"))
                        {
                            LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Replace("-", "").Replace("_", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n () => " + LastObject + ",");
                        }
                    }
                }

                File.AppendAllText(WindowCsFile, ");}");
                Line = 0;

                double XCoordinate = 0;
                double YCoordinate = 0;

                int Repeatingnumber = 0;
                int layernumber = 0;
                TextNumber = 0;
                //int SoundNumber = 0;
                int CostumeNumber = 0;
                int RandomNumberInt = 0;


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

                bool ComplicatedMathVariable = false;
                bool NextIsRound = false;
                bool DotThing = false;

                bool SomethingElseThanRound = false;
                //ok so i need some things around the edges to detect it, it is possible but hard :3
                //check if the coordinates of the objects are bigger as the woidth or the height XD
                var SoundVLC = new LibVLC();

                int ReceiveMessage = 0;
                int IF = 0;
                bool AsyncTask = false;
                bool LineANDBool = false;
                bool LineNotBool = false;
                if (BounceOff == true)
                {
                    File.AppendAllText(WindowCsFile, "\n public void BounceOff(){");
                    File.AppendAllText(WindowCsFile, "\n while(true){");

                    foreach (string jsonlines in JsonLines)
                    {
                        Line = Line + 1;
                        if (jsonlines.Contains("\"reportTouchingObject\""))
                        {
                            string EdgeOrNot = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            if (EdgeOrNot.Contains("\"edge\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n //\"reportTouchingObject\"");
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
                foreach (string jsonline in JsonLines)
                {
                    Line = Line + 1;
                    //Here the name of the object
                    if (jsonline.Contains("\"@name\":"))
                    {
                        string IDX = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                        string X = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();

                        if (IDX.Contains("\"@idx\":") && X.Contains("\"@x\":"))
                        {
                            LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Trim();
                            if (!LastObjects.Contains(LastObject))
                            {
                                LastObjects.Add(LastObject);
                            }
                        }
                    }

                    if (jsonline.Contains("\"setVolume\"") && LastObjects != null)
                    {
                        if (!VolumeAlreadySet.Contains(LastObject))
                        {
                            string VolumeText = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
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
                List<string> ProjectList = new List<string>();
                List<string> SoundPlayerList = new List<string>();
                List<string> TaskLines = new List<string>();
                string LastTask = "";
                int Percentage = 0;
                bool Else = false;
                int LineOR = 0;
                bool LineORBool = false;
                List<int> OrLines = new List<int>();
                foreach (string jsonline in JsonLines)
                {
                    try
                    {
                        Line = Line + 1;

                        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => ProgressBarConverter.Value = Line / DocumentLine * 100);
                        /*if (AsyncTask == false)
                        {*/

                        //This is wrong
                        //It seems like only this if structure is wrong, gotta check it next week LOL//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                        if (jsonline.Contains("\"@scale\":"))
                        {
                            SCALE = double.Parse(jsonline.Replace("\"@scale\":", "").Replace("\"", "").Replace(",", "").Replace(";", "").Trim(), CultureInfo.InvariantCulture);
                        }

                        //SOmehow there is a problem with x Coordinates
                        if (jsonline.Contains("\"@x\":"))
                        {
                            XCoordinate = double.Parse(jsonline.Replace("\"@x\":", "").Replace("\"", "").Replace(",", "").Replace(";", "").Trim(), CultureInfo.InvariantCulture);
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
                            string ForwardProbably = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First();
                            if (ForwardProbably.Contains("forward"))
                            {
                                string ValueForward = File.ReadAllLines(jsonPath).Skip(Line + 4).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                                File.AppendAllText(WindowCsFile, "\n XValueImage" + LastObject + "= XValueImage" + LastObject + " + " + ValueForward + ";");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image " + LastObject + ", XValueImage" + LastObject + ");");
                            }
                        }

                        //Here it is made a bit unclear
                        if (jsonline.Contains("\"@s\": \"forward\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"forward\"");
                            string XMovement = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                            if (XMovement.Contains("block") == false)
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
                            File.AppendAllText(WindowCsFile, "\n //\"turn\"");
                            string AngleRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                            File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle + ";");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"turnLeft\"")) //Wrong
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"turnLeft\"");
                            string AngleRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                            File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle + ";");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }


                        if (jsonline.Contains("\"setHeading\"")) //fixed Rotation like turn to 90�
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setHeading\"");
                            string AngleRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Angle = AngleRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                            File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = " + Angle + ";");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"doFaceTowards\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doFaceTowards\"");
                            //Get the Object to face towards, get the position of the mouse if it is the mouse
                            //that is actually pretty hard but doable if i have enough brain cells XD
                            string ObjectToFaceTowards = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if(ObjectToFaceTowards.Contains("\"random position\""))
                            {

                            }

                            if (ObjectToFaceTowards.Contains("\"mouse-pointer\""))
                            {

                            }

                            if (ObjectToFaceTowards.Contains("\"center\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + "Center = new RotateTransform();");
                                File.AppendAllText(WindowCsFile, "\n var image" + LastObject + "Center = this.FindControl<Image>(\"Image" + LastObject + "\");");
                                File.AppendAllText(WindowCsFile, "\n Rotation" + LastObject + " = Math.Atan(WindowWidth/2, WindowHeight/2s);");
                                File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = Rotation" + LastObject);
                            }

                            else
                            {
                                ObjectToFaceTowards = LastObject;
                                File.AppendAllText(WindowCsFile, "\n var Rotation" + LastObject + " = new RotateTransform();");
                                File.AppendAllText(WindowCsFile, "\n var image" + LastObject + " = this.FindControl<Image>(\"Image" + LastObject + "\");");
                                File.AppendAllText(WindowCsFile, "\n int LeftObjectImage" + LastObject + " = Canvas.GetLeft(" + LastObject + ");");
                                File.AppendAllText(WindowCsFile, "\n int TopObjectImage" + LastObject + " = Canvas.GetTop(" + LastObject + ");");
                                File.AppendAllText(WindowCsFile, "\n Rotation" + LastObject + " = Math.Atan(LeftObject" + LastObject + " , TopObject" + LastObject + ");");
                                File.AppendAllText(WindowCsFile, "\n image" + LastObject + ".Angle = Rotation" + LastObject);
                                //File.AppendAllText(WindowCsFile);
                            }
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        // this is right, one thing to change tho: it doesn�t use the variable the whole time XD -> that makes the code wrong sadly, but only 10%
                        if (jsonline.Contains("\"gotoXY\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"gotoXY\"");
                            XCoordinate = Convert.ToDouble(File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", ""));
                            YCoordinate = Convert.ToDouble(File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", ""));

                            File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + " = " + Convert.ToInt32((240 + XCoordinate - (((NotExactWidth * 2) * SCALE) / 2))) + ";");
                            File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + " = " + Convert.ToInt32((180 + (-YCoordinate) - (((NotExactHeight * 2) * SCALE) / 2))) + ";");
                            File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , xCoordinate" + LastObject + ");");
                            File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + " , yCoordinate" + LastObject + ");");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            //Now I need to make a code that litreally changes the Coordinates of the Object
                        }

                        if (jsonline.Contains("\"doGlide\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doGlide\"");
                            string Define = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();

                            if (Define.Contains("\"l\": \""))
                            {
                                int SecondsToWait = Convert.ToInt32(Define.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim());
                                string RandomForSure = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First();
                                if (RandomForSure.Contains("\"reportRandom\""))
                                {
                                    File.AppendAllText(WindowCsFile, "\n Random RandomX" + RandomNumberInt + " = new Random();");
                                    File.AppendAllText(WindowCsFile, "\n int randomX" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 480 + ");");
                                    File.AppendAllText(WindowCsFile, "\n Random RandomY" + RandomNumberInt + " = new Random();");
                                    File.AppendAllText(WindowCsFile, "\n int randomY" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 360 + ");");
                                    File.AppendAllText(WindowCsFile, "\n await Task.Delay(" + (SecondsToWait * 1000) + ");");
                                    File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", randomX" + RandomNumberInt + ");");
                                    File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", randomY" + RandomNumberInt + ");");
                                }
                            }
                            //I found out that this here is wrong XD lets get it fixed :3
                            if (Define.Contains("\"l\": ["))
                            {
                                int WaitSeconds = Convert.ToInt32(File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim());
                                string XCoordinateNumber = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                                string YCoordinateNumber = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First().Replace("\"", "").Trim();
                                File.AppendAllText(WindowCsFile, "\n await Task.Delay(" + Convert.ToInt32((WaitSeconds * 1000)) + ");");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", randomX" + XCoordinateNumber + ");");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", randomY" + YCoordinateNumber + ");");
                            }
                        }

                        if (jsonline.Contains("\"doGotoObject\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doGoToObject\"");
                            string Option = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First(); // --> this thin might be wrong, everything else seems to be valid
                                                                                                        //Doing for mouse will be hard, but i can do it
                            if (Option.Contains("\"mouse-pointer\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n this.Cursor = new Cursor(Cursor.Current.Handle);");
                                File.AppendAllText(WindowCsFile, "\n int PositiomMouse = Cursor.Position.X;");
                                File.AppendAllText(WindowCsFile, "\n int PositionMouse = Cursor.Position.Y;");
                                //Now you have to turn the Object Position to the Cursor Position
                                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + " = this.FinControl<Image>(\"" + LastObject + "\");"); //Don�t know if i need this LOL
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", PositionMouse);");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", PositionMouse);");
                            }

                            if (Option.Contains("\"random position\""))
                            {
                                // need half of the width and height and gotta - that to the converted coordinates : IT WORKS :
                                RandomNumberInt = RandomNumberInt + 1;
                                File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + " = this.FinControl<Image>(\"" + LastObject + "\");"); //Don�t know if i need this LOL
                                File.AppendAllText(WindowCsFile, "\n Random RandomX" + RandomNumberInt + " = new Random();");
                                File.AppendAllText(WindowCsFile, "\n int randomX" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 480 + ");");
                                File.AppendAllText(WindowCsFile, "\n Random RandomY" + RandomNumberInt + " = new Random();");
                                File.AppendAllText(WindowCsFile, "\n int randomY" + RandomNumberInt + " = Random" + RandomNumberInt + ".Next(" + 0 + "," + 360 + ");");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", randomX" + RandomNumberInt + ");");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", randomY" + RandomNumberInt + ");");
                            }

                            else
                            {
                                string PossibleObjectRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                                if (PossibleObjectRough.Contains("\"l\":"))
                                {
                                    string PossibleObject = PossibleObjectRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                                    File.AppendAllText(WindowCsFile, "\n var Image" + PossibleObject + " = this.FindControl<Image>(\"Image" + PossibleObject + "\");");
                                    File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + " = this.FinControl<Image>(\"" + LastObject + "\");"); //Don�t know if i need this LOL
                                    File.AppendAllText(WindowCsFile, "\n int PositionX = Canvas.GetLeft(Image" + PossibleObject + ");");
                                    File.AppendAllText(WindowCsFile, "\n int PositionY = Canvas.GetTop(Image" + PossibleObject + ");");
                                    File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", PositionX);");
                                    File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", PositionY);");
                                    //Its an object
                                }
                            }
                        }

                        if (jsonline.Contains("\"changeXPosition\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"changeXPosition\"");
                            //ok, so basically i have to make the xPosition and yPosition a bool but i am not 100% sure if I am able to at the earliest version (1.0)
                            //Check if its a number or a random number --> Mathematic things are not implemented yet
                            string PossibleNumber = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (PossibleNumber.Contains("\"l\":"))
                            {
                                string Number = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();

                                File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + " = xCoordinate" + LastObject + " + " + Number + ";");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + ", xCoordinate" + LastObject + ");");
                            }
                        }

                        if (jsonline.Contains("\"setXPosition\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setXPosition\"");
                            File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n xCoordinate" + LastObject + "=" + Convert.ToInt32(((240 + XCoordinate - (((NotExactWidth * 2) * SCALE) / 2)))) + ";");
                            File.AppendAllText(WindowCsFile, "\n Canvas.SetLeft(Image" + LastObject + " , xCoordinate" + LastObject + ");");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"changeYPosition\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"changeYPosition\"");
                            //Check if its a number or a random number --> Mathematic things are not implemented yet
                            string PossibleNumber = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (PossibleNumber.Contains("\"l\":"))
                            {
                                string Number = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();

                                File.AppendAllText(WindowCsFile, "\n int yCoordinate" + LastObject + " = yCoordinate" + LastObject + " + " + Number + ";");
                                File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + ", yCoordinate" + LastObject + ");");
                            }
                        }

                        if (jsonline.Contains("\"setYPosition\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setYPosition\"");
                            File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                            File.AppendAllText(WindowCsFile, "\n yCoordinate" + LastObject + "=" + Convert.ToInt32((240 + YCoordinate - (((NotExactWidth * 2) * SCALE) / 2))) + ";");
                            File.AppendAllText(WindowCsFile, "\n Canvas.SetTop(Image" + LastObject + " , yCoordinate" + LastObject + ");");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"doSetVar\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doSetVar\"");
                            string NormalVariableOrOtherThings = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (NormalVariableOrOtherThings.Contains("\"l\": ["))
                            {
                                string Variable = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();

                                if (Variable.Contains("{"))
                                {
                                    string ActualThing = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First();
                                    string ValueOfOption = File.ReadAllLines(jsonPath).Skip(Line + 4).Take(1).First().Replace("\"", "").Trim();
                                    if (ActualThing.Contains("\"option\""))
                                    {
                                        //Here there are multiple Options (3)
                                        if (ActualThing.Contains("\"rotation style\""))
                                        {
                                            File.AppendAllText(WindowCsFile, "\n //rotation style");
                                            File.AppendAllText(WindowCsFile, "// I don�t know how to implement this yet, give me a week lol");
                                        }
                                    }
                                }

                                if (!Variable.Contains("{"))
                                {
                                    string Value = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim();
                                    File.AppendAllText(WindowCsFile, "\n" + Variable + " = " + Value + ";");
                                }
                            }
                        }

                        //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //Visibility//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        //ok so basically the ground technology of these bubbles works, but i need to make �em first
                        if (jsonline.Contains("\"doSwitchToCostume\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doSwitchToCostume\"");
                            string CostumeToChangeTo = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Image" + CostumeToChangeTo + ".png);");
                            // i need to find which costume the next one is
                        }

                        if (jsonline.Contains("\"doWearNextCostume\""))
                        {
                            CostumeNumber = CostumeNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"doWearNextCostume\"");
                            File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Source == Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Image" + LastObject + CostumeNumber + ".png);");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            //Find the next costume - hard part :(
                        }

                        if (jsonline.Contains("\"doSayFor\""))
                        {
                            TextNumber = TextNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"doSayFor\"");
                            string Text = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                            //ok you gotta make a new LastObject which is the SayFor Image, and even make one here:
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == true");
                            //Get the seconds and delay the task 
                            string TimeOfSaying = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Task.Delay(" + TimeOfSaying + ");");
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == false");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"bubble\"")) //Finally works a little bit better
                        {
                            TextNumber = TextNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"bubble\"");
                            //Same here
                            if (File.Exists(fileName))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == true");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                //Need to make a script that lets the bubble follow the player and then show up with the visibility thing
                                //Create the bubble as an Image now - hardest part i will ever code lol 
                            }
                        }

                        if (jsonline.Contains("\"doThinkFor\""))
                        {
                            TextNumber = TextNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"doThinkFor\"");
                            string Text = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                            //Same here
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == true;");
                            //Get the seconds and delay the task 
                            string TimeOfThinking = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Task.Delay(" + TimeOfThinking + ");");
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == false;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"doThink\""))
                        {
                            TextNumber = TextNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"doThink\"");
                            string Text = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                            //Same here
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == true;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"doAsk\""))
                        {
                            TextNumber = TextNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"doAsk\"");
                            string Text = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"", "").Replace(",", "").Trim();
                            //Same here
                            File.AppendAllText(WindowCsFile, "\n Image img" + TextNumber + ".Visibility == true;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"setEffect\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setEffect\"");
                        }

                        if (jsonline.Contains("\"changeEffect\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"changeEffect\"");
                        }

                        if (jsonline.Contains("\"clearEffects\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"clearEffects\"");
                        }

                        //Don�t know if this is a speaking Bubble
                        if (jsonline.Contains("\"doTellTo\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doTellTo\"");
                            File.AppendAllText(WindowCsFile, "\n Image img" + LastObject + ".Visibility == true;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        if (jsonline.Contains("\"reportTouchingObject\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"reportTouchingObject\"");
                            if (IF > 0)
                            {
                                string ObjectToTouchRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                                string ObjectToTouch = ObjectToTouchRough.Replace("\"option\":", "").Replace("\"", "").Replace(",", "").Trim();
                                //check if there was an if statement before or an && Statement or else it wouldn�t make sense
                                string PossibleIfStatement = File.ReadAllLines(jsonPath).Skip(Line - 3).Take(1).First();
                                if (PossibleIfStatement.Contains("\"doIf\""))
                                {
                                    //The problem is I need to understand Rects :( - found a solution :)
                                    File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n if(" + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                    IF = IF + 1;
                                    SomethingElseThanRound = true;
                                }

                                if (PossibleIfStatement.Contains("\"reportAnd\""))
                                {
                                    File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n" + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                    IF = IF + 1;
                                    SomethingElseThanRound = true;
                                }

                                if (PossibleIfStatement.Contains("\"doWaitUntil\""))
                                {
                                    File.AppendAllText(WindowCsFile, "public async Task WaitUntil" + LastObject + "(){");
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                    IF = IF + 1;
                                    SomethingElseThanRound = true;

                                }

                                if (PossibleIfStatement.Contains("\"reportNot\""))
                                {
                                    File.AppendAllText(WindowCsFile, "\n Rect " + LastObject + "TOUCHED = new Rect(Canvas.GetLeft(Image" + LastObject + "), Canvas.GetTop(Image" + LastObject + "), Image" + LastObject + ".Width, Image" + LastObject + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n Rect " + ObjectToTouch + "TOUCHED = new Rect(Canvas.GetLeft(Image" + ObjectToTouch + "), Canvas.GetTop(Image" + ObjectToTouch + "), Image" + ObjectToTouch + ".Width, Image" + ObjectToTouch + ".Height);");
                                    File.AppendAllText(WindowCsFile, "\n" + "!" + LastObject + "TOUCHED.Interacts(" + ObjectToTouch + "TOUCHED)){");
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                    IF = IF + 1;
                                    SomethingElseThanRound = true;
                                }
                                //Don�t use lineRound here, its automatically used in if statements
                                //Check if there was an Object that has already been Touched in a List, and then Write those Objects (Rects in the beginning), like the ifs in the usual things
                            }
                        }

                        if (jsonline.Contains("\"goToLayer\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"goToLayer\"");
                            string LayerRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            string Layer = LayerRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                            if (Layer.Contains("\"front\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".ZIndex = 1;");
                            }
                            if (Layer.Contains(""))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".ZIndex = -1;");
                            }
                        }

                        if (jsonline.Contains("\"show\"") | jsonline.Contains("\"reportShown\""))////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        {

                            File.AppendAllText(WindowCsFile, "\n //\"show\" | \"reportShown\"");
                            if (jsonline.Contains("\"reportShown\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n if(Image" + LastObject + ".Visibility == true){");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                IF = IF + 1;
                            }
                            if (jsonline.Contains("\"show\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == true;");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            }

                        }

                        if (jsonline.Contains("\"hide\""))////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"hide\"");
                            File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Visibility == false;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        //make one value above and use it here XD
                        if (jsonline.Contains("\"setScale\"")) //This is wrong - not completely wrong tho
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setScale\"");
                            if (File.Exists(Path.Combine(GameFolder, LastObject + ".png")))
                            {
                                string NewScaleRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                                int NewScale = Convert.ToInt32(NewScaleRough.Replace("\"l\":", "").Replace("\"", "").Trim());

                                using (var image = new MagickImage(Path.Combine(GameFolder, LastObject + ".png"))) // it somehow uses sounds too - error !!!IMPORTANT
                                {
                                    double SizeX = image.Width;
                                    double SizeY = image.Height;
                                    File.AppendAllText(WindowCsFile, "\n var Image" + LastObject + "= this.FindControl<Image>(\"" + LastObject + "\");");
                                    File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Width = " + (SizeX * NewScale) + ";");
                                    File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".Height = " + (SizeY * NewScale) + ";");
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                }
                            }
                        }

                        if (jsonline.Contains("\"goToLayer\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"goToLayer\"");
                            string layer = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"option\":", "").Replace("\"", "").Trim();

                            if (layer.Contains("\"back\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".ZIndex = -1;");
                            }

                            if (layer.Contains("\"front\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".ZIndex = 1;");
                            }
                            //gotta make something that adds up so the max layer can be read
                            //max layer or least layer, gotta think about that
                        }

                        if (jsonline.Contains("\"goBack\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"goBack\"");
                            string Layer = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Image" + LastObject + ".ZIndex = -1;");
                            //layernumber = layernumber + Layer;
                        }

                        //SOUND//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        if (jsonline.Contains("\"doPlaySoundUntilDone\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doPlaySoundUntilDone\"");
                            try
                            {
                                SoundNumber = SoundNumber + 1;
                                string NameOfSound = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
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
                                    File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                    if (SoundPlayerList.Contains("MediaPlayer" + SoundNumber) == false)
                                    {
                                        SoundPlayerList.Add("MediaPlayer" + SoundNumber);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                continue;
                            }
                            //Somehow always after doplaySoundUntil done it never stops at the net playSound - i wonder why :-O
                            //ok i need to find the lenght of the sound and then go on with the task somehow, i need to find a tutorial or some kind of help

                        }


                        if (jsonline.Contains("\"playSound\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"playSound\"");
                            try
                            {
                                SoundNumber = SoundNumber + 1;
                                string NameOfSound = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
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
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                continue;
                            }

                        }

                        if (jsonline.Contains("\"doStopAllSounds\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doStopAllSounds\"");
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
                            File.AppendAllText(WindowCsFile, "\n //\"changeVolume\"");
                            string NumberOfVolumeRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First(); //They are all in percent
                            string NumberOfVolume = NumberOfVolumeRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Volume = " + LastObject + "VOLUME + " + "(" + NumberOfVolume + ");");
                        }

                        if (jsonline.Contains("\"setVolume\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"setVolume\"");
                            string NumberOfVolumeRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First(); //They are all in percent
                            string NumberOfVolume = NumberOfVolumeRough.Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n MediaPlayer" + SoundNumber + ".Volume = " + NumberOfVolume + ";");
                        }


                        //Events//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (jsonline.Contains("\"receiveMessage\""))
                        {
                            ReceiveMessage = ReceiveMessage + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"receiveMessage\"");
                            string MessageRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n if(" + Message + " == true){");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            IF = IF + 1;
                        }

                        if (jsonline.Contains("\"sendMessage\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"sendMessage\"");
                            string MessageRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n" + Message + " == true;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }

                        //OK SO I THINK I GOT A SOLUTION -> well those things are kinda dumb, i think i need to rewrite these .Contains things completely. It has some bad quirks O_O ngl i think its cuz it mixes up these words -> I FIXED IT :) it was just a single line XD. I litreally clicked like 100 debugger lines to find that out XD. dumbest mistake ever ngl frfr :skull:
                        if (jsonline.Contains("\"doBroadcast\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doBroadcast\"");
                            string MessageRough = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string Message = MessageRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n" + Message + " == true;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                        }
                        //REPEATING///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        //This is a big thing - i can feel it ;-)

                        if (jsonline.Contains("\"doWait\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doWait\"");
                            string HowLong = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (HowLong.Contains("\"reportRandom\""))
                            {
                                //This will be hard but doable :3
                                double One = double.Parse(File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Trim());
                                double Two = double.Parse(File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim());
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

                        //Somehow not all whiles do get written - need to fix that, but all {and}are perfect 
                        if (jsonline.Contains("\"doRepeat\"")) // i make a bool that gets true if it looks for random integers and it finds one /////////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doRepeat\"");
                            string RepeatingNumberRough = File.ReadAllLines(jsonPath).Skip(Line).First();
                            string RepeatingNumber = RepeatingNumberRough.Replace("\"l\":", "").Replace("\"", "").Replace(",", "").Trim();
                            //Check if RepeatingNumber is a Random Number or an addition and do the things need to make a void that checks things in between 
                            File.AppendAllText(WindowCsFile, "\n int Repeating" + Repeatingnumber + " = " + RepeatingNumber);
                            File.AppendAllText(WindowCsFile, "\n while (Repeating" + Repeatingnumber + "!=" + RepeatingNumber + "){");
                            File.AppendAllText(WindowCsFile, "\n Repeating" + Repeatingnumber + "=" + "Repeating" + Repeatingnumber + " +1;");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            IF = IF + 1;
                            SomethingElseThanRound = true;
                        }
                        //Somehow not all whiles do get written - need to fix that, but all {and}are perfect 
                        if (jsonline.Contains("\"doForever\"")) // i make a bool that gets true if it looks for random integers and it finds one ////////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doForever\"");
                            File.AppendAllText(WindowCsFile, "\n while (true){");
                            File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                            IF = IF + 1;
                            SomethingElseThanRound = true;
                        }

                        if (jsonline.Contains("\"doIf\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doIf\"");
                            IF = IF + 1;
                            SomethingElseThanRound = true;
                        }

                        if (jsonline.Contains("\"doIfElse\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doIfElse\"");
                            string PossibleTouchStatement = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First();
                            if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false)
                            {
                                File.AppendAllText(WindowCsFile, "\n if{");

                                //There is surely a way to check when there is a else statement: for example {, and make a bool that tells the thing that its an else or an if now lol

                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                IF = IF + 1;
                                Else = true;
                                SomethingElseThanRound = true;
                            }
                        }
                        if (!jsonline.Contains("\"doIfElse\""))
                        {
                            Else = false;
                        }
                        if (jsonline.Contains("\"doWaitUntil\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doWaitUntil\"");
                            string PossibleTouchStatement = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First();
                            if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false && PossibleTouchStatement.Contains("\"reportKeyPressed\""))
                            {
                                File.AppendAllText(WindowCsFile, "while(");
                                //Check what is inside
                                File.AppendAllText(WindowCsFile, "await Task.Delay()");
                            }
                            //First check if there is any Block like reportTouchingObject - is a Block that can�t be used inside an if statement as its an if statement itself
                            //File.AppendAllText();
                        }

                        if (jsonline.Contains("\"doUntil\"")) // i make a bool that gets true if it looks for random integers and it finds one ///////////////////////////////////////////////////////////////////////////
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doUntil\"");
                            string PossibleTouchStatement = File.ReadAllLines(jsonPath).Skip(Line + 3).Take(1).First();
                            if (PossibleTouchStatement.Contains("\"reportTouchingObject\"") == false && PossibleTouchStatement.Contains("\"reportKeyPressed\""))
                            {
                                File.AppendAllText(WindowCsFile, "while( == true){");
                                File.AppendAllText(WindowCsFile, "//This is not finished//");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                IF = IF + 1;
                                SomethingElseThanRound = true;
                            }
                        }

                        if (jsonline.Contains("\"createClone\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"createClone\"");
                            string MyselfOrSomeoneElse = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            if (MyselfOrSomeoneElse.Contains("\"myself\""))
                            {
                                //Now create a clone lol 8-)
                                File.AppendAllText(WindowCsFile, "\n //Create a clone");
                                File.AppendAllText(WindowCsFile, "\n Image IMG" + LastObject + " = new Image{");
                                File.AppendAllText(WindowCsFile, "\n Width = Image" + LastObject + ".Width");
                                File.AppendAllText(WindowCsFile, "\n Width = Image" + LastObject + ".Height");
                                File.AppendAllText(WindowCsFile, "\n Background = Image" + LastObject + ".Background};");
                                File.AppendAllText(WindowCsFile, "\n ProjectCanvas.Children.Add(IMG" + LastObject + ");");
                            }
                        }



                        //Get the last Object Name
                        /////////////////////////////THIS IS FOR }///////////////////////////////////////////////////////////////////////////////////////////

                        if (jsonline.Contains("\"@name\":"))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"@name\"");
                            string PossibleLastObject = jsonline.Replace("\"@idx\":", "").Replace("\"", "").Replace(",", "").Trim();
                            if (File.ReadAllLines(jsonPath).Skip(Line).Take(1).First().Contains("\"@idx\":"))
                            {
                                LastObject = jsonline.Replace("\"@name\":", "").Replace("\"", "").Replace(",", "").Replace("-", "").Replace("_", "").Trim();
                                File.AppendAllText(WindowCsFile, "\n } \n public Task " + LastObject + "(){"); //Ok i am not sure of giving down a } before the public Task
                            }
                        }

                        //A on click event via 2 voids if there is a press and a release event 
                        //Foreach Button Click a Void - if there are double voids with
                        //no voids at ALL when clicking, that is wrong (it complicetes things way more :( I know that it is :-/!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        //The private async Tasks are critical, they need to be done at the very end, but idk how to do it for now
                        if (jsonline.Contains("\"receiveInteraction\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"receiveInteraction\"");
                            string EitherPressedOrReleasedRough = File.ReadAllLines(jsonPath).Skip(Line + 1).First();
                            string EitherPressedOrReleased = EitherPressedOrReleasedRough.Replace("\"option\":", "").Replace("\"", "").Trim();
                            PressedOrReleased = true;

                            if (EitherPressedOrReleased == "pressed")
                            {
                                File.AppendAllText(WindowCsFile, "\n private async Task " + LastObject + "OnPressed(object sender, Avalonia.PointerPressedEventArgs e){");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                IF = IF + 1;
                                AsyncTask = true;
                                TaskLines.Add(" private async Task " + LastObject + "OnPressed(object sender, Avalonia.PointerPressedEventArgs e){");
                                LastTask = " private async Task " + LastObject + "OnPressed(object sender, Avalonia.PointerPressedEventArgs e){";
                            }

                            if (EitherPressedOrReleased == "released")
                            {
                                File.AppendAllText(WindowCsFile, "\n private async Task " + LastObject + "OnReleased(object sender, Avalonia.PointerPressedEventArgs e){");
                                File.AppendAllText(WindowCsFile, "/*" + Line + "*/");
                                IF = IF + 1;
                                SomethingElseThanRound = true;
                                AsyncTask = true;
                                TaskLines.Add(" private async Task " + LastObject + "OnReleased(object sender, Avalonia.PointerPressedEventArgs e){");
                                LastTask = " private async Task " + LastObject + "OnRelease(object sender, Avalonia.PointerPressedEventArgs e){";
                            }
                        }

                        if (jsonline.Contains("\"reportMouseDown\"")) //I need to find a way to make clicking in without a void
                        {
                            GlobablClickNumber = GlobablClickNumber + 1;
                            File.AppendAllText(WindowCsFile, "\n //\"reportMouseDown\"");
                            File.AppendAllText(WindowCsFile, "\n private Task MouseDown" + GlobablClickNumber + "(object sender, PointerReleasedEventArgws e){");
                            IF = IF + 1;
                            SomethingElseThanRound = true;
                            AsyncTask = true;
                            LastTask = "private Task MouseDown" + GlobablClickNumber + "(object sender, PointerReleasedEventArgws e){";
                        }

                        if (jsonline.Contains("\"reportKeyPressed\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"reportKeyPressed\"");
                            //Check if it is an if or wait until or something else
                            string PossibleIfStatement = File.ReadAllLines(jsonPath).Skip(Line - 3).Take(1).First();
                            string KeyRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            string Key = KeyRough.Replace("\"option\":", "").Replace("\"", "").Trim(); //Key needs to be big - this is halfly correct, also Numbers and Arrows should be included - need to find a way for that too

                            if (PossibleIfStatement.Contains("\"doIf\"")) ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            {
                                File.AppendAllText(WindowCsFile, "\n if(e.KeyCode == Keys." + Key + "){");
                                IF = IF + 1;
                                SomethingElseThanRound = true;
                            }

                            if (PossibleIfStatement.Contains("\"doWaitUntil\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n while(e.KeyCode != Keys." + Key + "){");
                                IF = IF + 1;
                                SomethingElseThanRound = true;
                            }

                            if (PossibleIfStatement.Contains("\"doUntil\""))
                            {
                                File.AppendAllText(WindowCsFile, "\n while(e.KeyCode != Keys." + Key + "){");
                                IF = IF + 1;
                                SomethingElseThanRound = true;

                            }
                        }

                        if (jsonline.Contains("\"reportTouchingColor\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"reportTouchingColor\"");
                            //ok i need a system where it detects the objects - my 
                        }

                        if (jsonline.Contains("reportOr"))
                        {
                            LineORBool = true;
                        }
                        if (jsonline.Contains("reportAnd"))
                        {
                            LineANDBool = true;
                        }
                        if (jsonline.Contains("reportNot"))
                        {
                            LineNotBool = true;
                        }
                        if (jsonline.Contains("\"UNSUPPORTED: sensing_dayssince2000\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"UNSUPPORTED: sensing_dayssince2000\"");
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
                            File.AppendAllText(WindowCsFile, "\n //\"reportEquals\"");
                        }

                        if (jsonline.Contains("\"reportGreaterThan\"")) //I need to add all the other things, like bigger, less, addition subtraction, division and multiplication
                        {
                            TheOneAfterTheNextIsBigger = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportGreaterThan\"");
                        }

                        if (jsonline.Contains("\"reportLessThan\""))
                        {
                            TheOneAfterTheNextIsSmaller = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportLessThan\"");
                        }

                        // I am not quite sure how I will manage to let it work but I somehow will :)
                        if (jsonline.Contains("\"reportSum\""))
                        {
                            TheOneAfterTheNextIsPlus = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportSum\"");
                        }

                        if (jsonline.Contains("\"reportDifference\""))
                        {
                            TheOneAfterTheNextIsMinus = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportDifference\"");
                        }

                        if (jsonline.Contains("\"reportProduct\""))
                        {
                            TheOneAfterTheNextIsMultiply = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportProduct\"");
                        }

                        if (jsonline.Contains("\"reportQuotient\""))
                        {
                            TheOneAfterTheNextIsDivide = true;
                            File.AppendAllText(WindowCsFile, "\n //\"reportQuotient\"");
                        }

                        if (jsonline.Contains("\"l\":"))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"l\"");
                            //this one handles 4 out of 5 things, when there are two numbers it doesn�t work out correctly - i have to fix that in the future - but i will take it now as its not that important

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
                            File.AppendAllText(WindowCsFile, "\n //\"@var\"");
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
                            File.AppendAllText(WindowCsFile, "\"reportJoinWords\"");
                            string FirstPart = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            string SecondPart = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            File.AppendAllText(WindowCsFile, FirstPart + " + " + SecondPart);
                        }

                        if (jsonline.Contains("\"reportRound\""))
                        {
                            File.AppendAllText(WindowCsFile, "\"reportRound\"");
                            File.AppendAllText(WindowCsFile, "Math.Round(");
                            ComplicatedMathVariable = true;
                            NextIsRound = true;
                        }

                        //this is not complete yet 
                        if (jsonline.Contains("\"reportMonadic\""))
                        {
                            File.AppendAllText(WindowCsFile, "\"reportMonadic\"");
                            //This will be even harder - XD 
                            string Option = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
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
                                //I don�t think that this is logarithm natural? -> yeah it is the default one: gotta find a way XD
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
                            string ProbablyVar = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (ProbablyVar.Contains("var"))
                            {
                                File.AppendAllText(WindowCsFile, ProbablyVar.Replace("\"@var\":", "").Replace("\"", "").Replace(":", "").Trim());
                            }
                        }

                        if (jsonline.Contains("\"doChangeVar\""))
                        {
                            File.AppendAllText(WindowCsFile, "\"doChangeVar\"");
                            string NormalVariableOrOtherThings = File.ReadAllLines(jsonPath).Skip(Line).Take(1).First();
                            if (NormalVariableOrOtherThings.Contains("\"l\": ["))
                            {
                                string Variable = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First().Replace("\"", "").Replace(",", "").Trim();

                                if (!Variable.Contains("{"))
                                {
                                    string Value = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"", "").Trim();
                                    File.AppendAllText(WindowCsFile, "\n" + Variable + " = " + Variable + " + " + Value + ";");
                                }
                            }
                        }

                        if (jsonline.Contains("\"doShowVar\""))
                        {
                            File.AppendAllText(WindowCsFile, "\"doShowVar\"");
                            string VariableName = File.ReadAllLines(jsonPath).Skip(Line - 2).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Image" + VariableName + ".Visibility == true;");
                        }

                        if (jsonline.Contains("\"doHideVar\""))
                        {
                            string VariableName = File.ReadAllLines(jsonPath).Skip(Line - 2).Take(1).First().Replace("\"l\":", "").Replace("\"", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n Image" + VariableName + ".Visibility == false;");
                        }
                        //This needs to be edited

                        //Basic things work now, now gotta find out why not everythi�ng works, 80% works tho :)
                        //receiveMessage doesn�t work correctly, it doesn�t make a } after its done everytime
                        if (jsonline.Contains("}") | jsonline.Contains("},"))
                        {
                            string BeforeLine = File.ReadAllLines(jsonPath).Skip(Line - 2).Take(1).First();
                            string BeforeThatLine = File.ReadAllLines(jsonPath).Skip(Line - 3).Take(1).First();

                            if (IF > 0)
                            {
                                //Check these:   }]}, and these }]}
                                if (BeforeLine.Contains("]"))
                                {
                                    if (BeforeThatLine.Contains("}"))
                                    {
                                        //I know this looks wrong, but it is actually good  don�t edit!!
                                        while (IF > 0)
                                        {
                                            if (Else == false)
                                            {
                                                File.AppendAllText(WindowCsFile, "\n}\n");
                                                IF = IF - 1;
                                            }
                                            if (Else == true)
                                            {
                                                File.AppendAllText(WindowCsFile, "\n}\nelse{");
                                            }
                                        }
                                        if (AsyncTask == true)
                                        {
                                            AsyncTask = false;
                                        }
                                    }
                                }
                                if (!BeforeLine.Contains("]"))
                                {
                                    if (!BeforeThatLine.Contains("}"))
                                    {

                                        if (LineANDBool == true)
                                        {
                                            File.AppendAllText(WindowCsFile, "&&");
                                        }

                                        if (LineORBool == true)
                                        {
                                            File.AppendAllText(WindowCsFile, "|");
                                        }

                                        if (LineNotBool == true)
                                        {
                                            File.AppendAllText(WindowCsFile, "!=");
                                        }
                                    }
                                }
                            }
                        }
                        //This isn�t working. it should be way easier to do this - tomorrow i will fix it
                        if (jsonline.Contains("\"doStopThis\""))
                        {
                            File.AppendAllText(WindowCsFile, "\n //\"doStopThis\"");
                            string OptionRough = File.ReadAllLines(jsonPath).Skip(Line + 1).Take(1).First();
                            if (OptionRough.Contains("\"option\""))
                            {
                                if (OptionRough.Contains("\"all\""))
                                {
                                    foreach (string AsyncLine in TaskLines)
                                    {
                                        File.AppendAllText(WindowCsFile, "\n await " + AsyncLine + ".CancelAfter(500);");
                                    }
                                }

                                if (OptionRough.Contains("\"this block\""))
                                {
                                    // I need to cancel the task
                                    //easyAsyncCancel

                                    File.AppendAllText(WindowCsFile, "\n await " + LastTask + ".CancelAfter(500);");
                                }

                                if (OptionRough.Contains("\"other scripts in sprite\""))
                                {
                                    foreach (string AsyncLine in TaskLines)
                                    {
                                        if (AsyncLine.Contains(LastObject))
                                        {
                                            File.AppendAllText(WindowCsFile, "\n await " + AsyncLine + ".CancelAfter(500);");
                                        }
                                    }
                                    // I need to cancel all tasks of the current object 
                                }
                            }
                        }

                        if (jsonline.Contains("\"comment\":"))
                        {
                            string CommentText = File.ReadAllLines(jsonPath).Skip(Line + 2).Take(1).First().Replace("\"#text\":", "").Replace(",", "").Trim();
                            File.AppendAllText(WindowCsFile, "\n//" + CommentText);
                        }
                        /*
                        }*/

                        if (AsyncTask == true)
                        {

                        }
                    }

                    catch (Exception ex)
                    {
                        File.AppendAllText(WindowCsFile, "\n //Error " + jsonline + " " + Line);
                    }
                }
                int FileEnd = Line;
                Line = 0;

                Line = 0;
                string[] CSlines = File.ReadAllLines(WindowCsFile);
                string NewCsFile = Path.Combine(GameFolder, "MainWindowTwo.axaml.cs");
                string NewAXAMLfile = Path.Combine(GameFolder, "MainWindowTwo.axaml");
                foreach (string line in CSlines)
                {
                    if (line.Contains("public MainWindow(){"))
                    {
                        string Values = File.ReadAllText(ValueFile).Replace(",", ".");
                        File.AppendAllText(NewCsFile, "\n" + Values);
                        File.AppendAllText(NewCsFile, "\n public MainWindow(){");
                    }
                    else
                    {
                        File.AppendAllText(NewCsFile, "\n" + line);
                    }
                }

                //Well now it works
                File.WriteAllText(WindowCsFile, File.ReadAllText(NewCsFile));
                string ValuesXAML = File.ReadAllText(WindowEditorFile);
                File.AppendAllText(NewAXAMLfile, ValuesXAML.Replace(",", "."));
                File.WriteAllText(WindowEditorFile, File.ReadAllText(NewAXAMLfile));
                File.Delete(ValueFile);
                File.Delete(NewCsFile);
                File.Delete(NewAXAMLfile);

                //Need to make a , . changing and add the values 
                File.AppendAllText(WindowCsFile, "\n}\n}");
                //read all the stuff of the voids
                //Get the click voids down
                //Copy all pictures to the folder of the finsihed application (Subfolders should be made in the end)
                //Now it should check teh additions multiplications, divisions, and subtrac
                //Ok so currently the code is full of scattered voids - what I mean is that I should change that :(
                File.AppendAllText(WindowEditorFile, "\n   </Canvas>");
                File.AppendAllText(WindowEditorFile, "\n</Window>");
                //Check the MainW�ndow for not finished things like the random integer if it has a range that is a variable or something else
                //if there is something it gets edited, and after that the { and } should be set, also this will be a challange

                if (Scratch == true && Snapinator == false)
                {
                    if (extension == ".sb2")
                    {

                    }

                    if (extension == ".sb3")
                    {

                    }
                }
            }

            //CopyAllPicturesAndSoundsToBin();
            await Task.Run(() => ExeBuilder());
        }

    }


    public async Task ExeBuilder() //This has to be edited :(
    {
        //Somehow a thread is beeing opened that shouldn�t idk why lol
        try
        {
            if (OperatingSystem.IsWindows() | OperatingSystem.IsLinux() | OperatingSystem.IsMacOS())
            {
                if (windows == true)
                {
                    if (formatOfApplication == "32-Bit")
                    {
                        Console.WriteLine("32 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x86 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("win-x86").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }

                    if (formatOfApplication == "64-Bit")
                    {
                        Console.WriteLine("64 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("win-x64").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }

                    if (formatOfApplication == "Arm-64")
                    {
                        Console.WriteLine("64 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r win-arm64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("win-arm64").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }
                }

                if (linux == true)
                {
                    if (formatOfApplication == "64-Bit")
                    {
                        Console.WriteLine("64 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("linux-x86").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }

                    if (formatOfApplication == "Arm-64")
                    {
                        Console.WriteLine("64 bit Machine");
                        // File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r linux-arm64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("linux-x64").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }
                }

                if (MacOs == true)
                {
                    if (formatOfApplication == "64-Bit")
                    {
                        Console.WriteLine("64 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("osx-x86").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }

                    if (formatOfApplication == "Arm-64")
                    {
                        Console.WriteLine("64 bit Machine");
                        //File.AppendAllText(GameDeployer, "\n dotnet publish -c Release -r osx-arm64 -p:PublishSingleFile=true --self-contained true");
                        await (Cli.Wrap("dotnet").WithArguments(args => args.Add("publish").Add("-c").Add("Release").Add("-r").Add("osx-arm64").Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true")).WithWorkingDirectory(GameFolder).ExecuteAsync());
                    }
                }
                await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    PopUp SuccesfulBuild = new PopUp();
                    SuccesfulBuild.PopUpWindow(false, false, Avalonia.Media.Colors.White, Avalonia.Media.Colors.Black, true, 500, 350, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.ico"), "Succesful Build", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MessageBoxImages", "Info.png"), "Your Project was built for " + formatOfApplication + " and is located at " + Foldername, false, true, false, false);
                }
                );
                //ProgressBarConverter.Value = 100; //Need to make the whole script in order so it works way better -rn the code is pure spaghetti code, but later it will be a flat Pizza i promise XD
            }
        }

        catch (Exception ex)
        {
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                PopUp SuccesfulBuild = new PopUp();
                SuccesfulBuild.PopUpWindow(false, false, Avalonia.Media.Colors.White, Avalonia.Media.Colors.Black, true, 500, 350, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.ico"), "Unsuccesful Build", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MessageBoxImages", "Error.png"), "Your Project was sadly not built, \n but it is located at " + Foldername + ", \n either it has some bugs that need to be fixed, or it is just a internal error of this program", false, true, false, false);
            }
            );
        }
    }
}