$nuget = "C:\tools\nuget.exe"

$csproj = "src2\TableIO.NPOI\TableIO.NPOI.csproj"
$outputDir = "src2\TableIO.NPOI\bin\Release"
$nupkgs = "src2\TableIO.NPOI\bin\Release\*.nupkg"

& $nuget pack $csproj -Build -Prop Configuration=Release -outputdirectory $outputDir

$nupkg = (Get-ChildItem $nupkgs | Sort-Object LastWriteTime | Select-Object -Last 1).FullName
# WARN: When $nuget push error occured, I can't trap the error..
& $nuget push $nupkg -Source https://api.nuget.org/v3/index.json

Write-Host "***** Deploy Complete!! *****"
trap {
    Write-Host "***** Deploy Failed!! *****"
}
