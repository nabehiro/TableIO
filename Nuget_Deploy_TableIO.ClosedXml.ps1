$nuget = "C:\tools\nuget.exe"
$msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
if (!(Test-Path $msbuild)) {
    $msbuild = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
}

$csproj = "src2\TableIO.ClosedXml\TableIO.ClosedXml.csproj"
$nupkgs = "src2\TableIO.ClosedXml\bin\Release\*.nupkg"

& $msbuild $csproj /t:build /p:Configuration=Release
& $msbuild $csproj /t:pack /p:Configuration=Release

$nupkg = (Get-ChildItem $nupkgs | Sort-Object LastWriteTime | Select-Object -Last 1).FullName
# WARN: When $nuget push error occured, I can't trap the error..
& $nuget push $nupkg -Source https://api.nuget.org/v3/index.json

Write-Host "***** Deploy Complete!! *****"
trap {
    Write-Host "***** Deploy Failed!! *****"
}
