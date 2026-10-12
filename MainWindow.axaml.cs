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
using sb1_sb2_sb3_xml_to_Csharp_converter.Conversion;

//HUGE ERROR, i need to install .Net10 to run the conversion, everybody should be able to run this so i am going to make it this way, it doesn´t matter what dotnet version you have, you will be able to convert. 
//Another thing, i realized translation is having 2 Folders somehow

namespace sb1_sb2_sb3_xml_to_Csharp_converter;

//One of my biggest issues is that bools are written as , even though they need .
public partial class MainWindow : Window
{
    string DotnetVersion;
    public bool windows = false;
    public bool linux = false;
    public bool MacOs = false;
    public bool Snap = false;
    public bool Scratch = false;
    public string Filename;
    public string fileName;

    public string FileExtension; 
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
    public string[] TranslatorFileList = { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ar\\config.yml"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ar\\lex.50.50.enar.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ar\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ar\\model.enar.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ar\\vocab.enar.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-bn\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-bn\\lex.50.50.enbn.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-bn\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-bn\\model.enbn.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-bn\\vocab.enbn.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-es\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-es\\lex.50.50.enes.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-es\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-es\\model.enes.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-es\\vocab.enes.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-fr\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-fr\\lex.50.50.enfr.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-fr\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-fr\\model.enfr.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-fr\\vocab.enfr.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-hi\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-hi\\lex.50.50.enhi.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-hi\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-hi\\model.enhi.intgemm.alphas.bin"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-hi\\vocab.enhi.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-in\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-in\\lex.50.50.enid.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-in\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-in\\model.enid.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-in\\vocab.enid.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ja\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ja\\lex.50.50.enja.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ja\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ja\\srcvocab.enja.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-ja\\trgvocab.enja.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-pt\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-pt\\lex.50.50.enpt.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-pt\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-pt\\model.enpt.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-pt\\vocab.enpt.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-uk\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-uk\\lex.50.50.enuk.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-uk\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-uk\\model.enuk.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-uk\\vocab.enuk.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\lex.50.50.enzh.s2t.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\model.enzh.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\srcvocab.enzh.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh\\trgvocab.enzh.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\config.yml"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\lex.50.50.enzh_hant.s2t.bin"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\metadata.json"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\model.enzh_hant.intgemm.alphas.bin"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\srcvocab.enzh_hant.spm"),Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder\\en-zh_hant\\trgvocab.enzh_hant.spm") };

    public MainWindow()
    {
        CheckRequirements();

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
            SnapOrNot.Content = "Soll der Converter den Snap Weg oder den Scratch Weg nehmen?";
            SnapCheckBox.Content = "Snap!";
            ScratchCheckBox.Content = "Scratch";
            OSTextAndCPU.Content = "Bitte suchen Sie das OS, und die Architektur aus ";
            WindowsCheckBox.Content = "Windows";
            LinuxCheckBox.Content = "Linux";
            MacOSCheckBox.Content = "Mac OS";
            convertButton.Content = "konvertieren!";
            GithubRepo.Content = "Dieses Projekt wurde von Daiko Games erstellt";
            UpdateButton.Content = "update!";
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
            SnapOrNot.Content = "Should the converter use Snap! or the Scratch Way?:";
            SnapCheckBox.Content = "Snap!";
            ScratchCheckBox.Content = "Scratch";
            OSTextAndCPU.Content = "Please select the OS you want to build it for and what architecture:";
            WindowsCheckBox.Content = "Windows";
            LinuxCheckBox.Content = "Linux";
            MacOSCheckBox.Content = "Mac OS";
            convertButton.Content = "Convert!";
            GithubRepo.Content = "This Project was made by: Daiko Games";
            UpdateButton.Content = "update!";
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
        // one big issue is that when you delete the language you have on your PC before using the app, it fails 
        Trace.WriteLine("Changing Language now");

        var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (language == "en") return; // nothing to do

        string[] texts =
        {
            "Please Select the File you want to convert or write it down here:",
            "Your File, it should be a .sb, .sb2, .sb3, .xml File",
            "Please select the Folder where your converted Project should be stored:",
            "Your Folder",
            "Please select the Icon File if necessary:",
            "the Icon of the Application(Not necessary)",
            "Should the converter use Snap! or the Scratch Way?:",
            "Snap",
            "Scratch",
            "Please select the OS you want to build it for and what architecture:",
            "Windows",
            "Linux",
            "Mac OS",
            "Convert!",
            "This Project was made by: Daiko Games",
            "update!"
        };

        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Translate-Folder",$"en-{language}","config.yml");

        foreach (string MissingFileProbably in TranslatorFileList)
        {
            if (!File.Exists(configPath))
            {
                await CheckRequirements();
                ChangeLanguage();
            }
        }

        if (File.Exists(configPath))
        {
            string[] translated = new string[texts.Length];

            try
            {
                // Run safely off the UI thread
                await Task.Run(() =>
                {
                    // Create the service once
                    using var service = new BlockingService(configPath);

                    // Translate strings individually to avoid memory spikes and crashes
                    for (int i = 0; i < texts.Length; i++)
                    {
                        try
                        {
                            // Fallback to original text if a specific line fails
                            translated[i] = service.Translate(texts[i]) ?? texts[i];
                        }
                        catch (Exception lineEx)
                        {
                            Trace.WriteLine($"Failed to translate line {i}: {lineEx.Message}");
                            translated[i] = texts[i]; // Safe fallback
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Managed exception during translation service initialization:");
                Trace.WriteLine(ex.ToString());
                return;
            }

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Text1.Content = translated[0];
                FileFolderNameTextBox.Watermark = translated[1];
                Text2.Content = translated[2];
                FolderNameTextBox.Watermark = translated[3];
                IconText.Content = translated[4];
                IconTextBox.Watermark = translated[5];
                SnapOrNot.Content = translated[6];
                SnapCheckBox.Content = translated[7];
                ScratchCheckBox.Content = translated[8];
                OSTextAndCPU.Content = translated[9];
                WindowsCheckBox.Content = translated[10];
                LinuxCheckBox.Content = translated[11];
                MacOSCheckBox.Content = translated[12];
                convertButton.Content = translated[13];
                GithubRepo.Content = translated[14];
                UpdateButton.Content = translated[15];
            });
        }

        // Safely update the UI back on the main thread
       
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

    public void NoButtonClick(object? sender, RoutedEventArgs e)
    {
        Requirements.ActualPopUp.Close();
    }

    public async Task ShowOrNot()
    {

        while (true)
        {
            if (SomethingNotInstalled == true)
            {

                if (REQUIREMENTSmessage == false)
                {
                    /*await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                    {*/
                    /*Requirements.PopUpWindow(false, false, Avalonia.Media.Colors.White, Avalonia.Media.Colors.Black, true, 500, 250, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.ico"), "Dependency Error", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon", "Converter.png"),  "#They will be installed asap. If the Progress bar is at 100% and the button should flicker close the whole app and reopen it.#", true, true, false, false);
                    Requirements.YesButton.Click += YesButtonClick;
                    Requirements.OkButton.Click += OkButtonClick;
                    Requirements.NoButton.Click += NoButtonClick;*/
                    REQUIREMENTSmessage = true;
                    /*});*/
                }
            }
            await Task.Delay(200);
        }
    }

    public async Task CheckRequirements()
{
    if (OperatingSystem.IsLinux())
    {
        try
        {
            Debug.WriteLine("Checking for VLC...");
            await Cli.Wrap("vlc").WithArguments("--version").ExecuteAsync();
            Debug.WriteLine("VLC found!");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"VLC not found, installing... Error: {ex.Message}");
            try
            {
                var installer = new LibVLCLinux();
                installer.InstallVLC();
                Debug.WriteLine("Installation complete!");
            }
            catch (Exception installEx)
            {
                Debug.WriteLine($"Installation failed: {installEx.Message}");
            }
        }
    }

    CultureInfo LanguageOfUser = CultureInfo.CurrentUICulture;
    string Language = LanguageOfUser.TwoLetterISOLanguageName;
    
    try
    {
        StillDoing = true;
        var DownloadOption = new DownloadConfiguration
        {
            ParallelDownload = true,
            ChunkCount = 1
        };

        string IconFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConverterIcon");
        if (!Directory.Exists(IconFolder))
        {
            Trace.WriteLine("Directory Fails: ConverterIcon");
            SomethingNotInstalled = true;
            Directory.CreateDirectory(IconFolder);
        }

        string ConverterFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scratch-Format-converter");
        if (!Directory.Exists(ConverterFolder))
        {
            Trace.WriteLine("Directory Fails: Scratch-Format-converter");
            SomethingNotInstalled = true;
            Directory.CreateDirectory(ConverterFolder);
        }

        string ScratchToSnapFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scratch-To-Snap");
        if (!Directory.Exists(ScratchToSnapFolder))
        {
            Trace.WriteLine("Directory Fails: Scratch-To-Snap");
            SomethingNotInstalled = true;
            Directory.CreateDirectory(ScratchToSnapFolder);
        }

        string InstallerFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Install");
        if (!Directory.Exists(InstallerFolder))
        {
            Trace.WriteLine("Directory Fails: Install");
            SomethingNotInstalled = true;
            Directory.CreateDirectory(InstallerFolder);
        }

        string SVGFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "svg_library_ScratchJnr");
        if (!Directory.Exists(SVGFolder))
        {
            SomethingNotInstalled = true;
            Trace.WriteLine("Directory Fails: svg_library_ScratchJnr");
            Directory.CreateDirectory(SVGFolder);
        }

        string SOUNDfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wav_library_ScratchJnr");
        if (!Directory.Exists(SOUNDfolder))
        {
            SomethingNotInstalled = true;
            Trace.WriteLine("Directory Fails: wav_library_ScratchJnr");
            Directory.CreateDirectory(SOUNDfolder);
        }

        string TranslationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translate-Folder");
        if (!Directory.Exists(TranslationFolder))
        {
            SomethingNotInstalled = true;
            Trace.WriteLine("Directory Fails: Translate-Folder");
            Directory.CreateDirectory(TranslationFolder);
        }

        Trace.WriteLine("Checking Images...");
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

        Trace.WriteLine("Checking Scratch Format Converter now...");
        string ConverterFile = Path.Combine(ConverterFolder, "Convert.js");
        if (!File.Exists(ConverterFile))
        {
            SomethingNotInstalled = true;
            var ConverterFileDownloader = new DownloadService(DownloadOption);
            await ConverterFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/ScratchConverter/raw/refs/heads/main/Convert.js", new DirectoryInfo(ConverterFolder));
        }

        string NPMpackageJSON = Path.Combine(ConverterFolder, "package.json");
        if (!File.Exists(NPMpackageJSON))
        {
            SomethingNotInstalled = true;
            var NPMpackageJSONDownloader = new DownloadService(DownloadOption);
            await NPMpackageJSONDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/ScratchConverter/raw/refs/heads/main/package.json", new DirectoryInfo(ConverterFolder));
        }

        string NPMpackageLockJSON = Path.Combine(ConverterFolder, "package-lock.json");
        if (!File.Exists(NPMpackageLockJSON))
        {
            SomethingNotInstalled = true;
            var NPMpackageLockJSONDownloader = new DownloadService(DownloadOption);
            await NPMpackageLockJSONDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/ScratchConverter/raw/refs/heads/main/package-lock.json", new DirectoryInfo(ConverterFolder));
        }

        Trace.WriteLine("Checking Scratch to Snap Converter now...");
        string ScatchToSnapConverterFile = Path.Combine(ScratchToSnapFolder, "converter.js");
        if (!File.Exists(ScatchToSnapConverterFile))
        {
            SomethingNotInstalled = true;
            var ConverterFileDownloader = new DownloadService(DownloadOption);
            await ConverterFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/scratch-snap-bridge/raw/refs/heads/main/public/downloads/converter.js", new DirectoryInfo(ScratchToSnapFolder));
        }

        Trace.WriteLine("Checking SVG Library now...");
        foreach (string ScratchJnrFile in ScratchJnrFiles)
        {
            if (!File.Exists(ScratchJnrFile))
            {
                string ScratchjnrSVGFile = Path.GetFileName(ScratchJnrFile);
                var ScratchJnrFileDownloader = new DownloadService(DownloadOption);
                await ScratchJnrFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/tree/main/svg_library_ScratchJnr/" + ScratchjnrSVGFile , new DirectoryInfo(SVGFolder));
            }
        }

        Trace.WriteLine("Checking WAV Library now...");
        foreach (string ScratchJnrFile in ScratchJnrSoundFiles)
        {
            if (!File.Exists(ScratchJnrFile))
            {
                string ScratchjnrWAVFile = Path.GetFileName(ScratchJnrFile);
                var ScratchJnrFileDownloader = new DownloadService(DownloadOption);
                await ScratchJnrFileDownloader.DownloadFileTaskAsync("https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/tree/main/wav_library_ScratchJnr/" + ScratchjnrWAVFile , new DirectoryInfo(SOUNDfolder));
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
                await Cli.Wrap("pwsh").WithArguments(args => args.Add("./dotnet-install.ps1").Add("-Runtime").Add("dotnet").Add("-Version").Add("9.0.0")).ExecuteBufferedAsync();
                await Cli.Wrap("dotnet").WithArguments(args => args.Add("new").Add("install").Add("Avalonia.Templates")).ExecuteBufferedAsync();
            }

            var CheckNodeJS = await Cli.Wrap("node").WithArguments(args => args.Add("--version")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
            if (CheckNodeJS.ExitCode != 0)
            {
                SomethingNotInstalled = true;
                Trace.WriteLine("Dependency Fails node");
                await Cli.Wrap("choco").WithArguments(args => args.Add("install").Add("nodejs")).WithWorkingDirectory(ConverterFolder).ExecuteBufferedAsync();
            }
        }

        if (OperatingSystem.IsLinux() && !OperatingSystem.IsWindows())
        {
            try
            {
                var osInfo = await Cli.Wrap("hostnamectl").ExecuteBufferedAsync();
                string osOutput = osInfo.StandardOutput; // FIX: StandardOutput statt ToString()

                if (osOutput.Contains("Debian") || osOutput.Contains("Ubuntu") || osOutput.Contains("Mint"))
                {
                    Trace.WriteLine("Linux Mint/Ubuntu detected. Checking dependencies...");

                    var dotnetCheck = await Cli.Wrap("dotnet").WithArguments("--version").ExecuteBufferedAsync();
                    if (dotnetCheck.ExitCode != 0)
                    {
                        Trace.WriteLine("Installing dotnet-sdk-9.0...");
                        await Cli.Wrap("sudo").WithArguments("apt-get install -y dotnet-sdk-9.0").ExecuteBufferedAsync();
                    }

                    var curlCheck = await Cli.Wrap("curl").WithArguments("--version").ExecuteBufferedAsync();
                    var gitCheck = await Cli.Wrap("git").WithArguments("--version").ExecuteBufferedAsync();
                    
                    if (curlCheck.ExitCode != 0 || gitCheck.ExitCode != 0)
                    {
                        Trace.WriteLine("Installing curl and git (required for Homebrew)...");
                        await Cli.Wrap("sudo").WithArguments("apt-get install -y curl git").ExecuteBufferedAsync();
                    }

                    var brewCheck = await Cli.Wrap("brew").WithArguments("--version").ExecuteBufferedAsync();
                    var brewCheckAlt = await Cli.Wrap("/home/linuxbrew/.linuxbrew/bin/brew").WithArguments("--version").ExecuteBufferedAsync();

                    if (brewCheck.ExitCode != 0 && brewCheckAlt.ExitCode != 0)
                    {
                        Trace.WriteLine("Installing Homebrew (this might take a moment)...");
                        // FIX: Saubere CliWrap-Syntax ohne Escaping-Hölle für die Pipe
                        await Cli.Wrap("bash")
                            .WithArguments(args => args.Add("-c").Add("curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh | bash"))
                            .WithEnvironmentVariables(env => env.Set("NONINTERACTIVE", "1"))
                            .ExecuteBufferedAsync();
                        Trace.WriteLine("Homebrew installed. (Note: You may need to restart the app for PATH updates to take effect).");
                    }
                    else
                    {
                        Trace.WriteLine("Homebrew is already installed.");
                    }

                    // FIX: Node.js prüfen, BEVOR wir es installieren (verhindert ständiges Neu-Installieren)
                    var nodeCheck = await Cli.Wrap("node").WithArguments("--version").ExecuteBufferedAsync();
                    if (nodeCheck.ExitCode != 0)
                    {
                        Trace.WriteLine("Installing Node.js via Homebrew...");
                        await Cli.Wrap("brew").WithArguments("install node").ExecuteBufferedAsync();
                    }
                    else
                    {
                        Trace.WriteLine($"Node.js is already installed ({nodeCheck.StandardOutput.Trim()}).");
                    }
                }
                else if (osOutput.Contains("Arch"))
                {
                    Trace.WriteLine("Arch Linux detected. Please ensure dotnet-sdk, curl, git and nodejs are installed via pacman.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Linux setup failed: {ex.Message}");
                SomethingNotInstalled = true;
            }
        }

        if (OperatingSystem.IsMacOS())
        {
            try
            {
                var dotnetCheck = await Cli.Wrap("dotnet").WithArguments("--version").ExecuteBufferedAsync();
                if (dotnetCheck.ExitCode != 0)
                {
                    Trace.WriteLine("Installing dotnet-sdk via brew...");
                    await Cli.Wrap("brew").WithArguments("install --cask dotnet-sdk").ExecuteBufferedAsync();
                }

                var brewCheck = await Cli.Wrap("brew").WithArguments("--version").ExecuteBufferedAsync();
                if (brewCheck.ExitCode != 0)
                {
                    Trace.WriteLine("Installing Homebrew...");
                    await Cli.Wrap("bash")
                        .WithArguments(args => args.Add("-c").Add("curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh | bash"))
                        .WithEnvironmentVariables(env => env.Set("NONINTERACTIVE", "1"))
                        .ExecuteBufferedAsync();
                }

                var nodeCheck = await Cli.Wrap("node").WithArguments("--version").ExecuteBufferedAsync();
                if (nodeCheck.ExitCode != 0)
                {
                    Trace.WriteLine("Installing Node.js...");
                    await Cli.Wrap("brew").WithArguments("install node").ExecuteBufferedAsync();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"macOS setup failed: {ex.Message}");
                SomethingNotInstalled = true;
            }
        }

        string nodeModulesFolder = Path.Combine(ConverterFolder, "node_modules");
        if (!Directory.Exists(nodeModulesFolder))
        {
            Trace.WriteLine("Running npm install (first time setup)...");
            await Cli.Wrap("npm")
                .WithArguments(args => args.Add("install"))
                .WithWorkingDirectory(ConverterFolder)
                .ExecuteBufferedAsync();
        }
        else
        {
            Trace.WriteLine("node_modules folder exists, skipping npm install to save time.");
        }

        await Task.Delay(2000);

        if (Language != "de" && Language != "en")
        {
            foreach (string TranslatorFile in TranslatorFileList)
            {
                if (!Directory.Exists(Path.GetDirectoryName(TranslatorFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(TranslatorFile));
                }

                if (!File.Exists(TranslatorFile))
                {
                    if (TranslatorFile.Contains(".yml") || TranslatorFile.Contains(".json"))
                    {
                        var TranslateFileDownloader = new DownloadService(DownloadOption);
                        string parentFolderName = (Directory.GetParent(TranslatorFile)?.Name)?.Replace("\\", "/") ?? "";
                        // FIX: String-Interpolation statt Path.Combine für URLs (verhindert Windows-Backslashes in URLs)
                        string LinkToDownload = $"https://github.com/DaikoGames/Translate-Folder/raw/refs/heads/main/{parentFolderName}/{Path.GetFileName(TranslatorFile)}";
                        
                        Trace.WriteLine("Downloading yml or json: " + Path.GetFileName(TranslatorFile));
                        await TranslateFileDownloader.DownloadFileTaskAsync(LinkToDownload, new DirectoryInfo(Path.GetDirectoryName(TranslatorFile)));
                    }

                    if (TranslatorFile.Contains(".bin") || TranslatorFile.Contains(".spm"))
                    {
                        var TranslateFileDownloader = new DownloadService(DownloadOption);
                        string parentFolderName = (Directory.GetParent(TranslatorFile)?.Name)?.Replace("\\", "/") ?? "";
                        string LinkToDownload = $"https://github.com/DaikoGames/Translate-Folder/raw/refs/heads/main/{parentFolderName}/{Path.GetFileName(TranslatorFile)}.gz";
                        
                        Trace.WriteLine("Downloading bin or spm: " + Path.GetFileName(TranslatorFile));
                        await TranslateFileDownloader.DownloadFileTaskAsync(LinkToDownload, new DirectoryInfo(Path.GetDirectoryName(TranslatorFile)));
                        string DownloadedFile = TranslatorFile + ".gz";

                        using (var originalFileStream = new FileStream(DownloadedFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                        using (var decompressedFileStream = new FileStream(TranslatorFile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            using (var decompressionStream = new System.IO.Compression.GZipStream(originalFileStream, System.IO.Compression.CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                        File.Delete(DownloadedFile);
                    }
                }
            }
        }

        // FIX: Am Ende erfolgreich durchgelaufen -> Status zurücksetzen
        Trace.WriteLine("Nothing Fails anymore. Requirements check complete.");
        SomethingNotInstalled = false;
        StillDoing = false;
    }
    catch (Exception ex)
    {
        Trace.WriteLine($"Critical Error in CheckRequirements: {ex.Message}");
        Trace.WriteLine($"Stack: {ex.StackTrace}");
        SomethingNotInstalled = true;
        StillDoing = false;
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

    public async void UpdateButtonVoid(object sender, RoutedEventArgs args)
    {
        try
        {
            // CORRECT way for GitHub Releases
            var mgr = new UpdateManager(
                new GithubSource(
                    "https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter",
                    accessToken: null,   // public repo → null is fine
                    prerelease: false
                )
            );

            // Optional but recommended: check if the app was actually installed by Velopack
            if (!mgr.IsInstalled)
            {
                Trace.WriteLine("App is not installed via Velopack installer. Updates only work on properly installed versions.");
                return;
            }

            var updateInfo = await mgr.CheckForUpdatesAsync();
            if (updateInfo == null)
            {
                Trace.WriteLine("No new version found.");
                return;
            }

            Trace.WriteLine($"Update available: {updateInfo.TargetFullRelease.Version}");

            await mgr.DownloadUpdatesAsync(updateInfo);
            mgr.ApplyUpdatesAndRestart(updateInfo);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.ToString());
            Trace.WriteLine("Update failed. Most common reasons:");
            Trace.WriteLine("1. You are running from Visual Studio / debug / plain publish folder");
            Trace.WriteLine("2. The app was never installed with the Velopack Setup.exe");
            Trace.WriteLine("3. Network / GitHub rate limit");
        }
    }

    // ------------------------------------------------------------------------------------------
    // Konvertierung: Eingabe -> Snap!-XML -> JSON -> C#-Projekt (siehe Ordner Conversion/)
    // ------------------------------------------------------------------------------------------

    void SetProgress(int value) =>
        Avalonia.Threading.Dispatcher.UIThread.Post(() => ProgressBarConverter.Value = value);

    public async void ConvertButton(object sender, RoutedEventArgs args)
    {
        if (SomethingNotInstalled) { convertButton.IsEnabled = false; return; }

        convertButton.IsEnabled = false;
        SetProgress(0);
        try
        {
            // Zustand bei JEDEM Lauf neu setzen (vorher blieben Haken aus früheren Läufen hängen).
            windows = WindowsCheckBox.IsChecked == true;
            linux = LinuxCheckBox.IsChecked == true;
            MacOs = MacOSCheckBox.IsChecked == true;
            formatOfApplication = X64CheckBox.IsChecked == true ? "64-Bit"
                                : X32CheckBox.IsChecked == true ? "32-Bit"
                                : Arm64CheckBox.IsChecked == true ? "Arm-64" : null;

            if (string.IsNullOrWhiteSpace(Foldername))
            {
                Trace.WriteLine("Bitte einen Zielordner auswählen.");
                return;
            }

            Filename = FileFolderNameTextBox.Text?.Trim() ?? Filename;
            if (!File.Exists(Filename))
            {
                await DownloadCatrobatIfLink();
                return;
            }

            FileExtension = Path.GetExtension(Filename).ToLowerInvariant();
            fileName = Path.GetFileName(Filename);

            var dotnet = await Cli.Wrap("dotnet").WithArguments("--version").ExecuteBufferedAsync();
            DotnetVersion = int.Parse(dotnet.StandardOutput.Trim().Split('.')[0]) + ".0";

            // Die Dateiendung entscheidet den Weg, nicht (nur) die Haken Snap!/Scratch.
            if (FileExtension == ".sjr")
            {
                await ScratchJrLegacy();
                return;
            }

            string xmlPath;
            if (FileExtension == ".xml")
                xmlPath = Filename;                                                     // Snap! -> JSON -> C#
            else if (FileExtension is ".sb" or ".sb2" or ".sb3")
                xmlPath = await ScratchImport.ToSnapXml(Filename, Foldername, m => Trace.WriteLine(m));   // Scratch -> Snap! -> JSON -> C#
            else
            {
                Trace.WriteLine("Nicht unterstützte Dateiendung: " + FileExtension);
                return;
            }
            SetProgress(10);

            var result = await Task.Run(() => ConversionPipeline.Run(new ConversionOptions
            {
                XmlPath = xmlPath,
                OutputFolder = Foldername,
                IconFile = ICON,
                TargetFramework = "net" + DotnetVersion,
                PreferredName = Path.GetFileNameWithoutExtension(Filename),
                Log = m => Trace.WriteLine(m),
                Progress = p => SetProgress(10 + p * 8 / 10)
            }));

            ApplicationName = result.AppName;
            GameFolder = result.ProjectDir;
            await ExeBuilder();
            SetProgress(100);
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Konvertierung fehlgeschlagen: " + ex);
        }
        finally
        {
            convertButton.IsEnabled = true;
        }
    }

    async Task DownloadCatrobatIfLink()
    {
        const string prefix = "https://share.catrobat.org/app/project/";
        if (string.IsNullOrEmpty(Filename) || !Filename.Contains(prefix)) return;
        try
        {
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string code = Filename.Replace(prefix, "").Trim();
            var service = new DownloadService(new DownloadConfiguration { ParallelDownload = true, ChunkCount = 1 });
            await service.DownloadFileTaskAsync("https://share.catrobat.org/api/projects/" + code + "/catrobat", new DirectoryInfo(downloads));
        }
        catch (Exception ex)
        {
            Trace.WriteLine("Catrobat-Download fehlgeschlagen: " + ex.Message);
        }
    }

    public static void ConvertToIco(string inputPath, string outputPath)
    {
        SKBitmap bitmap;

        // 1. Load and scale the image (SVG or Raster)
        if (inputPath.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
        {
            using var svg = new SKSvg();
            svg.Load(inputPath);
            bitmap = new SKBitmap(256, 256);
            using var canvas = new SkiaSharp.SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            // Scale SVG to fit 256x256 while keeping aspect ratio
            var rect = svg.Picture.CullRect;
            float scale = Math.Min(256f / rect.Width, 256f / rect.Height);
            canvas.Translate((256 - rect.Width * scale) / 2, (256 - rect.Height * scale) / 2);
            canvas.Scale(scale);
            canvas.DrawPicture(svg.Picture);
        }
        else
        {
            using var original = SKBitmap.Decode(inputPath);
            bitmap = new SKBitmap(256, 256);
            using var canvas = new SkiaSharp.SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            // Scale raster image to fit 256x256 while keeping aspect ratio
            float scale = Math.Min(256f / original.Width, 256f / original.Height);
            canvas.Translate((256 - original.Width * scale) / 2, (256 - original.Height * scale) / 2);
            canvas.Scale(scale);
            canvas.DrawBitmap(original, 0, 0);
        }

        // 2. Use SkiaSharp to encode the resized image to PNG in memory
        using var image = SkiaSharp.SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        byte[] pngBytes = data.ToArray();

        // 3. Wrap the PNG bytes in a standard 22-byte ICO header
        using var stream = new FileStream(outputPath, FileMode.Create);
        using var writer = new BinaryWriter(stream);

        writer.Write((short)0);             // Reserved
        writer.Write((short)1);             // Type: 1 = ICO
        writer.Write((short)1);             // Image count
        writer.Write((byte)0);              // Width (0 means 256px)
        writer.Write((byte)0);              // Height (0 means 256px)
        writer.Write((byte)0);              // Color palette
        writer.Write((byte)0);              // Reserved
        writer.Write((short)1);             // Color planes
        writer.Write((short)32);            // Bits per pixel
        writer.Write(pngBytes.Length);      // Size of PNG data
        writer.Write(22);                   // Offset where PNG data starts (6 + 16 = 22)
        writer.Write(pngBytes);             // The actual SkiaSharp-encoded PNG data
    }

    string newFile;


    // Eine Tabelle statt neun fast gleicher Blöcke. (Vorher war die Zuordnung vertauscht: "64-Bit" unter
    // Linux/macOS hat für linux-x86/osx-x86 gebaut, und publish lief im bin/Debug-Ordner statt im Projekt.)
    static string? RuntimeId(string os, string? arch) => (os, arch) switch
    {
        ("win", "64-Bit") => "win-x64",
        ("win", "32-Bit") => "win-x86",
        ("win", "Arm-64") => "win-arm64",
        ("linux", "64-Bit") => "linux-x64",
        ("linux", "Arm-64") => "linux-arm64",
        ("osx", "64-Bit") => "osx-x64",
        ("osx", "Arm-64") => "osx-arm64",
        _ => null
    };

    public async Task ExeBuilder()
    {
        var targets = new List<string>();
        if (windows) targets.Add("win");
        if (linux) targets.Add("linux");
        if (MacOs) targets.Add("osx");

        foreach (string os in targets)
        {
            string? rid = RuntimeId(os, formatOfApplication);
            if (rid == null)
            {
                Trace.WriteLine($"{os} / {formatOfApplication}: diese Kombination wird nicht unterstützt.");
                continue;
            }

            Trace.WriteLine("Baue für " + rid);
            await Cli.Wrap("dotnet")
                .WithArguments(a => a.Add("publish").Add("-c").Add("Release").Add("-r").Add(rid)
                    .Add("-p:PublishSingleFile=true").Add("--self-contained").Add("true"))
                .WithWorkingDirectory(GameFolder)                  // Projektordner, nicht bin/Debug/netX
                .ExecuteAsync();
        }
    }
}
