#!/bin/bash
echo -e "\e[41mWebpage setup\e[0m"
rm -rd Output/page/Assets
mkdir Output/page/Assets
markdown ../README.md > Output/page/index.html
cp Assets/icon.png Output/page/Assets
sed -i '1i<head><title>Chemistry Tools</title><link rel="shortcut icon" href="Assets/icon.png" type="image/x-icon" /><link rel="stylesheet" href="styles.css" /></head><body><p><img src="Assets/icon.png" alt="App Icon" title="" /></p>' Output/page/index.html
echo '</body>' >> Output/page/index.html
echo -e "\e[41mDONE\e[0m"

echo -e "\e[41mMac Compilation\e[0m"
dotnet restore -r osx-x64 && dotnet msbuild -t:BundleApp -property:Configuration=Release -p:RuntimeIdentifier=osx-x64;PublishSingleFile=true
rm -rd Output/page/macos
mkdir Output/page/macos
mv "bin/Release/net6.0/osx-x64/publish/Chemistry Tools.app" Output/page/macos/Chemistry\ Tools\.app

echo -e "\e[41mDeploying to Mac...\e[0m"
nohup ./macdeploy.sh > log.txt &

#pwd
echo -e "\e[41mLinux Compilation\e[0m"
#appVersion="0.0.0"
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o "Builds/Chemistry Tools (Linux-x64)"

appVersion=$(grep -oP '\d+.\d+.\d+' -m 1 Chemistry\ Tools.csproj)
echo The app version is: $appVersion

echo -e "\e[41mLinux Deployment\e[0m"
rm -rd "Builds/Chemistry Tools (Linux-x64)/Assets"
tar -czvf "Chemistry Tools (Linux-x64) $appVersion.tar.gz" "Builds/Chemistry Tools (Linux-x64)"

rm -rd Output/page/linux/
mkdir Output/page/linux
mv "Chemistry Tools (Linux-x64) $appVersion.tar.gz" "Output/page/linux/"
echo -e "\e[41mDONE LINUX\e[0m"

tail -1 log.txt