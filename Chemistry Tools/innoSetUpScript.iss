; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Chemistry Tools"
#define MyAppVersion "0.0.1"
#define MyAppPublisher "ElrohirGT"
#define MyAppURL "https://github.com/ElrohirGT/Chemistry-Tools"
#define MyAppExeName "Chemistry Tools.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{002774B1-13F1-4A47-A061-1EA8011C9066}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\LICENSE
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=Chemistry Tools Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\Chemistry Tools.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\libHarfBuzzSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\libSkiaSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\elroh\Documents\Developer\Desktop Development\Chemistry Tools\Chemistry Tools\Builds\Chemistry Tools (WIN-x64)\libsodium.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

