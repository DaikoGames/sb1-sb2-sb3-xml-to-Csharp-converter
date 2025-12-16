cd
cd .\bin\Release\net9.0\win-x86
vpk pack --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter.exe
cd /d "%~dp0"
cd .\bin\Release\net9.0\win-x64
vpk pack --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter.exe
cd /d "%~dp0"
cd .\bin\Release\net9.0\win-arm64
vpk pack --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter.exe
cd /d "%~dp0"
cd .\bin\Release\net9.0\linux-x64
vpk [linux] pack --runtime linux-x64 --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter
cd /d "%~dp0"
cd .\bin\Release\net9.0\linux-arm64
vpk [linux] pack --runtime linux-arm64 --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter
cd /d "%~dp0"
cd .\bin\Release\net9.0\osx-x64
vpk [osx] pack --runtime osx-x64 --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter
cd /d "%~dp0"
cd .\bin\Release\net9.0\osx-arm64
vpk [osx] pack --runtime osx-arm64 pack --packId ScratchSnapToCsharpConverter --packVersion 1.0.0 --packDir .\publish --mainExe Converter