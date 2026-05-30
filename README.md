[![Deploy to GitHub Releases](https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DaikoGames/sb1-sb2-sb3-xml-to-Csharp-converter/actions/workflows/dotnet.yml)
## What is this Project:

This project converts [Scratch](https://scratch.mit.edu/), and [Snap!](https://snap.berkeley.edu/) Projects to [Avalonia](https://avaloniaui.net/) Projects (C#)

## What is the Story about this Project:

Well it is pretty simple, i as a Developer saw Projects like [Turbowarp](https://packager.turbowarp.org/) and I was interested into the Topic. Originally it started as a [Windows Forms](https://github.com/dotnet/winforms) Project

## The Dependencies i used for my Project and their licenses:

| Link to Dependency| Link to License|
| :--- | :--- |
| [Avalonia](https://github.com/AvaloniaUI/Avalonia)|[MIT-License](https://github.com/AvaloniaUI/Avalonia?tab=MIT-1-ov-file)|
| [Magick.NET](https://github.com/dlemstra/Magick.NET)|[Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0)|
| [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)|[MIT-License](https://mit-license.org/)|
| [LibVLC](https://github.com/videolan/libvlcsharp)|[LGPL 2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html)|
| [CliWrap](https://github.com/Tyrrrz/CliWrap)|[MIT-License](https://mit-license.org/)|
| [Velopack](https://github.com/velopack/velopack)|[MIT-License](https://mit-license.org/)|
| [Avalonia-Custom-Message-Box](https://github.com/AvaloniaCommunity/MessageBox.Avalonia)|[MIT-License](https://github.com/AvaloniaCommunity/MessageBox.Avalonia?tab=MIT-1-ov-file)|
| [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)|[Linux](https://mit-license.org/) [Windows](https://qphelp.quest.com/Content/ThirdPartyComponents/MSDotNetLibraryLicense.htm?TocPath=Legal%2525252525252520Notices%252525252525257CLicenses%252525252525257C_____5)| 
| [Scratch Junior Sprite Pictures & Default Sounds](https://github.com/scratchfoundation/scratchjr)|[BSD-3-Clause](https://github.com/scratchfoundation/scratchjr?tab=BSD-3-Clause-1-ov-file) |
| [qwen-2.5-32k-500m-instruct](https://huggingface.co/hazemmabbas/Qwen2.5-0.5B-int4-block-32-acc-3-Instruct-onnx-cpu/tree/main)|[Apache 2.0](https://huggingface.co/Qwen/Qwen2.5-0.5B-Instruct/blob/main/LICENSE)|
| [Downloader](https://www.nuget.org/packages/Downloader/5.5.0?_src=template) | [MIT-License](https://licenses.nuget.org/MIT)
|[BergamotTranslatorSharp](https://www.nuget.org/packages/BergamotTranslatorSharp/0.3.4?_src=template) | [MPL 2.0](https://licenses.nuget.org/MPL-2.0)
I used [ImageMagick](https://imagemagick.org/#gsc.tab=0) for converting the png File to the .icns File. It is a great tool :)

## What works and what doesnt for compilation: 

| Publish | CPU Architecture | Working? |
| :--- | :--- | :--- |
| Windows | x86 | :white_check_mark: |
|         | x64 | :white_check_mark: |
|         | arm64 | :white_check_mark: |
| Linux | x86 | :x: |
|       | x64 | :white_check_mark: |
|       | arm64 | :white_check_mark: |
| MacOS | x86 | :x: |
|       | x64 | :white_check_mark: |
|       | arm64 | :white_check_mark: |
| Web | none | :x: |

Currently the Only stable languages are german and english. Everything else is translated via AI, because i can´t translate it myself
I am still working on the translation stuff
Btw. AI model is downloaded after you already downloaded the project, because it is way faster to do it that way, it reduces the download time a lot

## You don´t have to install anything. 
The programm installs it automatically for you :)

## For Source Compilation:

On Linux: use the CompileLinux.sh

On Windows: use the CompileWindows.bat

On MacOS: use the CompileMacOS.sh

For more infos you can join my [Discord](https://discord.gg/dvJwx5Mzzj) Server

