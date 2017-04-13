@echo off

set nuget=C:\tools\nuget.exe
set tempdir=NugetTemp
set csproj=src\TableIO.NPOI\TableIO.NPOI.csproj

if exist "%tempdir%" (
	rmdir /s /q "%tempdir%"
	if exist %tempdir% goto error
)
REM build project and create nuget package.
%nuget% pack %csproj% -Build -Prop Configuration=Release -outputdirectory %tempdir%
if errorlevel 1 goto error

REM get package path.
for %%A in (%tempdir%\*) do set nupkg=%%A
echo %nupkg%

REM push nuget package.
%nuget% push %nupkg% -Source https://www.nuget.org/api/v2/package
if errorlevel 1 goto error

echo;
echo *** Nuget Upload Complete !! ***
exit 0

:error

echo;
echo *** Nuget Upload Failed !! ***
exit 1