#!/bin/bash
echo Mac Deployment 
dotnet restore -r osx-x64 && dotnet msbuild -t:BundleApp -property:Configuration=Release -p:RuntimeIdentifier=osx-x64;PublishSingleFile=true
rm -rd Output/page/macos
mkdir Output/page/macos
mv "bin/Release/net6.0/osx-x64/publish/Chemistry Tools.app" Output/page/macos/Chemistry\ Tools\.app

cd Output/page/macos
appVersion=$(grep -oP '\d+.\d+.\d+' -m 1 Chemistry\ Tools.app/Contents/Info.plist)
echo Starting compression of version: $appVersion
zip -r "Chemistry Tools (OSX-x64) $appVersion.zip" "Chemistry Tools.app"
rm -rd "Chemistry Tools.app"

cd ../../../
echo Linux Deployment
#appVersion="0.0.0"
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o "Builds/Chemistry Tools (Linux-x64)"
rm -rd "Builds/Chemistry Tools (Linux-x64)/Assets"
tar -czvf "Chemistry Tools (Linux-x64) $appVersion.tar.gz" "Builds/Chemistry Tools (Linux-x64)"

rm -rd Output/page/linux/
mkdir Output/page/linux
mv "Chemistry Tools (Linux-x64) $appVersion.tar.gz" "Output/page/linux/"