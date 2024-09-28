; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "WinBooster"
#define MyAppVersion "2.0.9.5"
#define MyAppPublisher "Monolith Develpment"
#define MyAppURL "https://github.com/WinBooster"
#define MyAppExeName "WinBooster WPF.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{16277C01-31CE-4D2F-991B-300D842BE8CB}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\WinBooster
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=WinBooster
SetupIconFile=WinBooster WPF\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\BouncyCastle.Crypto.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\CSScriptLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\DiscordRPC.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\H.Formatters.BinaryFormatter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\H.Formatters.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\H.Formatters.Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\H.Pipes.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\HandyControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\MaterialDesignColors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\MaterialDesignThemes.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.CodeAnalysis.CSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.CodeAnalysis.CSharp.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.CodeAnalysis.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.CodeAnalysis.Scripting.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.Extensions.DependencyModel.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Microsoft.Xaml.Behaviors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Process.NET.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\QRCoder.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\ShowMeTheXAML.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Spire.Barcode.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\System.Management.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\Telegram.Bot.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\WinBooster WPF.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\WinBooster WPF.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\WinBooster WPF.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\WinBoosterNative.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\runtimes\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "WinBooster WPF\bin\Release\net6.0-windows7.0\ru\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

