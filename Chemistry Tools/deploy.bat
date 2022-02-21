@echo off
title Prepare deploy
echo Have you changed the innoSetUpScript.iss version?
pause

:: you can also use %~dp0
echo %CD%
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -o "Builds\Chemistry Tools (WIN-x64)"
iscc innoSetUpScript.iss

del Output\page\windows /Q
xcopy Output Output\page\windows
del Output /Q

cd Output\page\windows
netsparkle-generate-appcast -n "Chemistry Tools" -u https://chemistry-tools.netlify.app/windows/ --key-path "D:\elroh\Documents" -a .
exit