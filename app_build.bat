@cd /d %~sdp0

set "binpath_com=Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\"
set "binpath_pro=Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\"
set "binpath_ent=Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\"
set "path=%path%;C:\%binpath_com%"
set "path=%path%;D:\%binpath_com%"
set "path=%path%;E:\%binpath_com%"
set "path=%path%;F:\%binpath_com%"
set "path=%path%;C:\%binpath_pro%"
set "path=%path%;D:\%binpath_pro%"
set "path=%path%;E:\%binpath_pro%"
set "path=%path%;F:\%binpath_pro%"
set "path=%path%;C:\%binpath_ent%"
set "path=%path%;D:\%binpath_ent%"
set "path=%path%;E:\%binpath_ent%"
set "path=%path%;F:\%binpath_ent%"
msbuild src/genshin-switch.sln /t:Rebuild /p:Configuration=Release

set "binpath_7z=:\Program Files\7-Zip\"
set "path=%path%;C:\%binpath_7z%"
set "path=%path%;D:\%binpath_7z%"
set "path=%path%;E:\%binpath_7z%"
set "path=%path%;F:\%binpath_7z%"
del .\src\bin\Release\net48\*.config
del .\src\bin\Release\net48\*.pdb
del .\src\bin\Release\net48\*.xml
cd .\src\bin\Release
ren net48 genshin-switch
7z a ..\..\..\genshin-switch_build%date:~,4%%date:~5,2%%date:~8,2%.7z .\genshin-switch\ -t7z -mx=5 -r -y
ren genshin-switch net48

set "binpath_rea=:\Program Files (x86)\Eziriz\.NET Reactor\"
set "path=%path%;C:\%binpath_rea%"
set "path=%path%;D:\%binpath_rea%"
set "path=%path%;E:\%binpath_rea%"
set "path=%path%;F:\%binpath_rea%"
cd net48
dotNET_Reactor.Console.exe -file "genshin-switch.exe" -satellite_assemblies "Hardcodet.NotifyIcon.Wpf.dll -embed;MaterialDesignColors.dll -embed;MaterialDesignThemes.Wpf.dll -embed;YamlDotNet.dll -embed" -embed 1 -stringencryption 0 -suppressildasm 0 -obfuscation 0
cd genshin-switch_Secure
7z a ..\..\..\..\..\genshin-switch_single_build%date:~,4%%date:~5,2%%date:~8,2%.7z .\* -t7z -mx=5 -r -y
@pause
