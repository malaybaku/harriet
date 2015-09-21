#Harriet自体が直接参照しているファイルを"dll"以下のフォルダに整頓して押し込む。
#また、プラグインの受け入れ先になるディレクトリを生成する。

#なお、配置後のdllが読み出せるのは下記の処理を行っているから。
#  - 普通のdll: App.configのprobing要素として参照パスを追加してる
#  - AquesTalk: [DLLImport]で直接パス指定してる

param(
    [string]$SolutionDir,
    [string]$ProjectDir,
    [string]$TargetDir
)

#Livet
New-Item dll\Livet -ItemType Directory
Move-Item ($TargetDir + "Livet*") dll\Livet\ -Force

#IronPython(dll)
New-Item dll\IronPython -ItemType Directory
Move-Item ($TargetDir + "IronPython*") dll\IronPython\ -Force
Move-Item ($TargetDir + "Microsoft.Dynamic*") dll\IronPython\ -Force
Move-Item ($TargetDir + "Microsoft.Scripting*") dll\IronPython\ -Force

#MetroRadiance
New-Item dll\MetroRadiance -ItemType Directory
Move-Item ($TargetDir + "MetroRadiance*") dll\MetroRadiance\ -Force

#NAudio
New-Item dll\NAudio -ItemType Directory
Move-Item ($TargetDir + "NAudio*") dll\NAudio\ -Force

#Other System|Microsoft dlls
New-Item dll\System -ItemType Directory
Move-Item ($TargetDir + "System*") dll\System\ -Force
Move-Item ($TargetDir + "Microsoft*") dll\System\ -Force

#AquesTalk
New-Item dll\AquesTalk -ItemType Directory
Copy-Item ($ProjectDir + "AquesTalk\*") dll\AquesTalk\ -Recurse -Force

#IronPython(Script Path)
New-Item DLLs -ItemType Directory
New-Item Lib -ItemType Directory
Copy-Item ($ProjectDir + "IronPython\*.py") ($TargetDir + "Lib\")


#Plugin directories
New-Item Plugin\TextToPronounce -ItemType Directory
New-Item Plugin\Voice -ItemType Directory
