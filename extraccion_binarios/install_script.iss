; Created by "innounp" version 2.71.2
; Setup file: OpenGD77CPSInstaller_E2026.07.13.01.exe
; Inno Setup Version: 6.6.1 (Unicode)

[Setup]
AppName=OpenGD77 CPS
AppId={{68542C1D-33BC-4EE4-80E0-D40A1A1486D3}
AppVersion=E2026.07.13.01
AppPublisherURL=https://www.opengd77.com
AppSupportURL=https://www.opengd77.com
AppUpdatesURL=https://www.opengd77.com
DefaultDirName={commonpf}\OpenGD77CPS
OutputBaseFilename=OpenGD77CPSInstaller_E2026.07.13.01
Compression=zip
DisableDirPage=auto
DisableProgramGroupPage=yes
ChangesAssociations=no
ShowLanguageDialog=yes
WizardStyle=classic light
WizardImageFile=embedded\WizardImage0.png
WizardSmallImageFile=embedded\WizardSmallImage0.png

[Files]
Source: "{app}\OpenGD77CPS.exe"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\DefaultOpenGD77.g77"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\DefaultTheme.gtm"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\DockPanel.config"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\help.xml"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\Setup.ini"; DestDir: "{app}"; Components: portable; Flags: ignoreversion 
Source: "{app}\satellites.txt"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\STM_License_SLA0044.txt"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\STDFU.dll"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\STTubeDevice30.dll"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Chinese.chm"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Chinese.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Czech.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\English.chm"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\English.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\French.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\German.chm"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\German.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Italian.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Japanese.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Polski.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Portuguese.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Slovenian.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Spanish.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\SpanishCatalan.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Swedish.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Turkish.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Ukrainian.chm"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{app}\Language\CPS\Ukrainian.xml"; DestDir: "{app}\Language\CPS"; Flags: ignoreversion 
Source: "{%TEMP}\OpenGD77CommDriver\wdi-simple.exe"; DestDir: "{%TEMP}\OpenGD77CommDriver"; Flags: ignoreversion 

[Run]
Filename: "{%TEMP}\OpenGD77CommDriver\wdi-simple.exe"; Parameters: "--vid 0x01FC9 --pid 0x0094 --type 3 --name OpenGD77"; Description: "Install OpenGD77 Comm port driver"; Flags: postinstall skipifsilent 
Filename: "{app}\OpenGD77CPS.exe"; Description: "{cm:LaunchProgram,OpenGD77 CPS}"; Flags: postinstall skipifsilent nowait

[Icons]
Name: "{commonprograms}\OpenGD77 CPS"; Filename: "{app}\OpenGD77CPS.exe"; 
Name: "{commondesktop}\OpenGD77 CPS"; Filename: "{app}\OpenGD77CPS.exe"; Tasks: desktopicon; 

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; 

[Components]
Name: "normal"; Description: "Normal installation."; Types: "normal"; 
Name: "portable"; Description: "Portable installation. This will run from a memory stick and uses an ini file to store defaults etc"; Types: "portable"; 

[Types]
Name: "normal"; Description: "Normal installation"; 
Name: "portable"; Description: "Portable installation"; 

[CustomMessages]
english.NameAndVersion=%1 version %2
english.AdditionalIcons=Additional shortcuts:
english.CreateDesktopIcon=Create a &desktop shortcut
english.CreateQuickLaunchIcon=Create a &Quick Launch shortcut
english.ProgramOnTheWeb=%1 on the Web
english.UninstallProgram=Uninstall %1
english.LaunchProgram=Launch %1
english.AssocFileExtension=&Associate %1 with the %2 file extension
english.AssocingFileExtension=Associating %1 with the %2 file extension...
english.AutoStartProgramGroupDescription=Startup:
english.AutoStartProgram=Automatically start %1
english.AddonHostProgramNotFound=%1 could not be located in the folder you selected.%n%nDo you want to continue anyway?

[Languages]
; These files are stubs
; To achieve better results after recompilation, use the real language files
Name: "english"; MessagesFile: "embedded\english.isl"; 
