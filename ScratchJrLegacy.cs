using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Styling;
using BergamotTranslatorSharp;
using CliWrap;
using CliWrap.Buffered;
using Downloader;
using ExCSS;
using LibVLCSharp.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PopUpWindowNamespace;
using SharpCompress;
using SharpCompress.Archives;
using SharpCompress.Common;
using ShimSkiaSharp;
using SkiaSharp;
using Svg;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
//using Microsoft.Win32.SafeHandles;

using LibVLC.Linux;
using System.Threading.Tasks;
using System.Xml;
using Velopack;
using Velopack.Sources;

//HUGE ERROR, i need to install .Net10 to run the conversion, everybody should be able to run this so i am going to make it this way, it doesn´t matter what dotnet version you have, you will be able to convert. 
//Another thing, i realized translation is having 2 Folders somehow

namespace sb1_sb2_sb3_xml_to_Csharp_converter;

//One of my biggest issues is that bools are written as , even though they need .
public partial class MainWindow : Window
{
    // Scratch Jr (.sjr): unverändert aus dem alten Code übernommen. Das ist NICHT Teil der neuen
    // Pipeline (Scratch/Snap -> JSON -> C#) und arbeitet noch mit der alten zeilenbasierten Methode.
    public async Task ScratchJrLegacy()
    {
        if (FileExtension == ".sjr")
        {

            //Now convert the Scratch Junior project to normal Scratch

            //Change this 
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

                        /*string NewImageName = Path.Combine(GameFolder, Path.GetFileNameWithoutExtension(SpriteName) + ".png");

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
                        }*/

                        // YOUR EXACT VARIABLES - nothing changed
                        string NewImageName = Path.Combine(GameFolder, Path.GetFileNameWithoutExtension(SpriteName) + ".png");

                        // This spawns a background thread instantly. The system cleans it up automatically when done.
                        _ = Task.Run(() =>
                        {
                            string extension = Path.GetExtension(SpriteName).ToLowerInvariant();
                            double PNGwidth = 0;
                            double PNGheight = 0;

                            if (extension == ".svg")
                            {
                                using var svg = new Svg.Skia.SKSvg();
                                var picture = svg.Load(SpriteName);

                                if (picture != null)
                                {
                                    // Skia and Avalonia share the EXACT same (0,0) top-left coordinate system.
                                    PNGwidth = picture.CullRect.Width;
                                    PNGheight = picture.CullRect.Height;

                                    int intWidth = (int)Math.Ceiling(PNGwidth);
                                    int intHeight = (int)Math.Ceiling(PNGheight);

                                    using var bitmap = new SkiaSharp.SKBitmap(intWidth, intHeight);
                                    using var canvas = new SkiaSharp.SKCanvas(bitmap);
                                    canvas.Clear(SkiaSharp.SKColors.Transparent);
                                    canvas.DrawPicture(picture);

                                    using var image = SkiaSharp.SKImage.FromBitmap(bitmap);
                                    using var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                                    File.WriteAllBytes(NewImageName, data.ToArray());
                                }
                            }
                            else
                            {
                                using var bitmap = SkiaSharp.SKBitmap.Decode(SpriteName);
                                PNGwidth = bitmap.Width;
                                PNGheight = bitmap.Height;
                                using var image = SkiaSharp.SKImage.FromBitmap(bitmap);
                                using var data = image.Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                                File.WriteAllBytes(NewImageName, data.ToArray());
                            }

                            // Build string ONCE (saves massive CPU/disk overhead)
                            string xamlContent = $@" <Image Name=""{Path.GetFileNameWithoutExtension(SpriteName)}""
                            Width=""{PNGwidth}""
                            Height=""{PNGheight}""
                            Canvas.Left=""0""
                            Canvas.Top=""0"">
                            </Image>";

                            string csContent = $"\n {Path.GetFileNameWithoutExtension(SpriteName)}.Source = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, \"{Path.GetFileName(NewImageName)}\"));";

                            File.AppendAllText(WindowEditorFile, xamlContent);
                            File.AppendAllText(WindowCsFile, csContent);
                        }); 

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
}
