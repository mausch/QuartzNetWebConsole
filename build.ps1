$ErrorActionPreference = "Stop"
if (! (test-path nuget.exe)) {
	wget https://nuget.org/nuget.exe -outfile nuget.exe
}
& ./nuget restore
$MSBUILD = [Environment]::GetEnvironmentVariable("ProgramFiles(x86)")
$MSBUILD = join-path $MSBUILD "msbuild\12.0\bin\msbuild.exe"
& $MSBUILD /m /p:Configuration=Release
& ./nuget pack QuartzNetWebConsole.nuspec
& ./nuget pack QuartzNetWebConsole.Owin.nuspec