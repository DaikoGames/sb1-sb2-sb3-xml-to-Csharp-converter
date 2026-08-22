[![Deploy to GitHub Releases](https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/actions/workflows/dotnet.yml)
## What is this Project:

This project converts [Scratch](https://scratch.mit.edu/), and [Snap!](https://snap.berkeley.edu/) Projects to [Avalonia](https://avaloniaui.net/) Projects (C#)

## The app is programmed with the Help of AI (a little), if used - mostly Qwen AI, Gemini, or Grok, maybe Chat GPT too.
## What is the Story about this Project:

Well it is pretty simple, i as a Developer saw Projects like [Turbowarp](https://packager.turbowarp.org/) and I was interested into the Topic. Originally it started as a [Windows Forms](https://github.com/dotnet/winforms) Project

## The Dependencies i used for my Project and their licenses:

| Link to Dependency | Link to License |
| :--- | :--- |
| [Avalonia](https://github.com/AvaloniaUI/Avalonia) | [MIT-License](https://github.com/AvaloniaUI/Avalonia?tab=MIT-1-ov-file) |
| [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) | [MIT-License](https://mit-license.org/) |
| [LibVLC](https://github.com/videolan/libvlcsharp) | [LGPL 2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html) |
| [CliWrap](https://github.com/Tyrrrz/CliWrap) | [MIT-License](https://mit-license.org/) |
| [Velopack](https://github.com/velopack/velopack) | [MIT-License](https://mit-license.org/) 
| [Avalonia.PopUpWindow](https://github.com/DaikoGames/Avalonia.PopUpWindow) | [MIT-License](https://github.com/DaikoGames/Avalonia.PopUpWindow?tab=MIT-1-ov-file) |
| [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) | [Linux](https://mit-license.org/) [Windows](https://qphelp.quest.com/Content/ThirdPartyComponents/MSDotNetLibraryLicense.htm?TocPath=Legal%2525252525252520Notices%252525252525257CLicenses%252525252525257C_____5) | 
| [Scratch Junior Sprite Pictures & Default Sounds](https://github.com/scratchfoundation/scratchjr) | [BSD-3-Clause](https://github.com/scratchfoundation/scratchjr?tab=BSD-3-Clause-1-ov-file) |
| [Downloader](https://www.nuget.org/packages/Downloader/5.5.0?_src=template) | [MIT-License](https://licenses.nuget.org/MIT) |
| [BergamotTranslatorSharp](https://www.nuget.org/packages/BergamotTranslatorSharp/0.3.4?_src=template) | [MPL 2.0](https://licenses.nuget.org/MPL-2.0) |
| [SharpCompress](https://github.com/adamhathcock/sharpcompress) | [MIT-License](https://github.com/adamhathcock/sharpcompress?tab=MIT-1-ov-file) |
| [scratch-snap-bridge](https://github.com/DaikoGames/scratch-snap-bridge/tree/main) | [MIT-License](https://github.com/DaikoGames/scratch-snap-bridge/tree/main?tab=MIT-1-ov-file) |
| [ScratchConverter](https://github.com/DaikoGames/ScratchConverter) | [MIT-License](https://github.com/DaikoGames/ScratchConverter?tab=MIT-1-ov-file) |

I used [ImageMagick](https://imagemagick.org/#gsc.tab=0) for converting the png File to the .icns File. It is a great tool :)

## What works and what doesnt for compilation: 

| Publish | CPU Architecture | Done | Obsolete? |
| :--- | :--- | :--- | :--- |
| Windows | x86 | :white_check_mark: | :x: |
|         | x64 | :white_check_mark: | :x: |
|         | arm32 | :x: | :white_check_mark: |
|         | arm64 | :white_check_mark: | :x: |
| Linux | x86 | :x: | :white_check_mark: |
|       | x64 | :white_check_mark: | :x: |
|       | arm32 | :white_check_mark: | :x: |
|       | arm64 | :white_check_mark: | :x: |
| MacOS | x86 | :x: | :white_check_mark: |
|       | x64 | :white_check_mark: | :x: |
|       | arm32 | :x: | :white_check_mark: |
|       | arm64 | :white_check_mark: | :x: |
| Web | none | :x: | :x: |

these things will never happen, because they physically can´t run the desktop app
IOS, ANDROID

Currently the Only stable languages are german and english. Everything else is translated via AI, because i can´t translate it myself

## You don´t have to install anything. 
The programm installs it automatically for you :)

## For Source Compilation:

On Linux: use the CompileLinux.sh

On Windows: use the CompileWindows.bat

On MacOS: use the CompileMacOS.sh

For more infos you can join my [Discord](https://discord.gg/dvJwx5Mzzj) Server
