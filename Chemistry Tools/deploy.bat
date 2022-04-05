@echo off
title Alice - Deploy Bot
echo Have you changed the innoSetUpScript.iss version?
echo Have you changed the Info.plist version?
pause

::you can also use %~dp0
::echo %CD%
echo [101;93m Windows deployment [0m
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -o "Builds\Chemistry Tools (WIN-x64)"
iscc innoSetUpScript.iss

:: Clens the windows dir and moves the installer to the windows dir
del Output\page\windows /Q
xcopy Output Output\page\windows
del Output /

echo [101;93m MacOS and Linux deployments [0m
echo "Please run the ./unixdeploy.sh file"
wsl

echo [101;93m Generating App Casts [0m
cd Output\page\windows
netsparkle-generate-appcast -n "Chemistry Tools" -u https://chemistry-tools.netlify.app/windows/ --key-path "D:\elroh\Documents" -a .

cd ../macos
netsparkle-generate-appcast -n "Chemistry Tools" -f true -u https://chemistry-tools.netlify.app/macos/ --key-path "D:\elroh\Documents" -a . -o macos -e zip

cd ../linux
netsparkle-generate-appcast -n "Chemistry Tools" -f true -u https://chemistry-tools.netlify.app/linux/ --key-path "D:\elroh\Documents" -a . -o linux -e tar.gz

echo [101;93m DONE [0m
cd ../
echo "Opening the explorer..."
explorer.exe .
pause