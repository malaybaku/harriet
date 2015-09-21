#プラグインのうちソリューションに入ってるやつを該当する配置場所に移す

param(
    [string]$SolutionDir,
    [string]$ProjectDir,
    [string]$TargetDir
)


$TextToConverterDir = $TargetDir + "Plugin\TextToPronounce\"
$VoiceSynthesizeDir = $TargetDir + "Plugin\Voice\"

#MecabTextConverter
$MeCabDir = $SolutionDir + "MecabTextConverter\"
$MeCabTargetDir = $SolutionDir + "MecabTextConverter\bin\Debug\"

Copy-Item ($MeCabTargetDir + "MecabTextConverter*") $TextToConverterDir -Force
Copy-Item ($MeCabTargetDir + "LibNMeCab*") $TextToConverterDir -Force
Copy-Item ($MeCabDir + "dic") $TextToConverterDir -Recurse

#SapiVoiceSynthesize
$SapiTarget = $SolutionDir + "SapiVoiceSynthesize\bin\Debug\" 

Copy-Item ($SapiTarget + "SapiVoiceSynthesize*") $VoiceSynthesizeDir -Force

