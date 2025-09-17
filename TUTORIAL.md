Hey there - I am Daiko Games, and this is my Project.

What it does:

it converts Scratch 1, Scratch 2, Scratch 3, and Snap! Projects into executables



Before I dive more into it i just wanted to say on what **Platform**s it is **support**ed:

Currently it's only **Windows and Linux - no Mac OS**,

WHY: as its closed source and I would have to pay 99 Euros a year for an open source Project of which i will not get Money from (for developer License)

HOWEVER - there are multiple ways to run the Project on other platforms: i do think a virtual Machine of Windows can be run on mac with Oracle VirtualBox - find it here: .

Wine has not been tested by me, and I don´t know how to set it up so the Emulator emulates the file Explorer accordingly to the "Filer", if it can be done pls. tell me how I Need to know



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

The good thing with the C# converter its  already packaged - currently we didn´t build it for flatpak, but once we reach 1.0 we will think about that too :) if it is possible!
