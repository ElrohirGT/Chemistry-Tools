cd Output/page/macos
echo -e "\e[41mMac Deployment\e[0m"
echo "Obtaining version..."
appVersion=$(grep -oP '\d+.\d+.\d+' -m 1 Chemistry\ Tools.app/Contents/Info.plist)
echo $appVersion

zip -r "Chemistry Tools (OSX-x64) $appVersion.zip" "Chemistry Tools.app" && rm -rd "Chemistry Tools.app"
echo -e "\e[41mDONE MAC\e[0m"