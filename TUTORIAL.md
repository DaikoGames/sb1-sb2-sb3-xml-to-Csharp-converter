Hey there - I am Daiko Games, and this is my Project.

What it does:

it converts Scratch 1, Scratch 2, Scratch 3, and Snap! Projects into executables



Before I dive more into it i just wanted to say on what **Platform**s it is **support**ed:

Currently it's only **Windows and Linux - Mac OS too!**,
My Programs are not signed, so you have to accept that there is a warning by your computer. It is not a virus, don´t worry XD!

What I used for the Conversion:

I used Snapinator for the conversion process - as it can make Scratch 1 binary files easily readable (by converting it into the Snap! format) - you can find Snapinator here:

https://github.com/snapinator/snapinator.github.io

I used Electron and Nutralino for the .exe Files, which you can Find here: 

Electron: https://www.electronjs.org
Neutralino: https://neutralino.js.org

Ok, so for the conversion from .sb/.sb2/.sb3 to .xml you just have to let the Snapinator Window open, put in the filename of the FIle you wanna convert to .xml and then download it, and put the same File into the Foldername where you wanna convert it. The File should have the exact same Name of the Original File - without any numbers or any other stuff that hasn´t been the original name of the File. The rest is done by the converter :).


**What you Need** to run the whole Converter:

Windows: dotnet: https://dotnet.microsoft.com/en-us/download

**Linux: dotnet: there is a tutorial on the .NET Website: https://learn.microsoft.com/en-us/dotnet/core/install/linux**
       **LibVLC: there is a tutorial there: https://github.com/videolan/libvlcsharp/blob/3.x/docs/linux-setup.md**

The good thing with the C# converter its  already packaged - currently we didn´t build it for flatpak, but once we reach 1.0 we will think about that too :) if it is possible!
