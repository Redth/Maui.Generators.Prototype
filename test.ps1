New-Item -ItemType Directory -Path .\logs\ -Force
New-Item -ItemType Directory -Path .\output\ -Force

& msbuild /r /t:"Rebuild;Pack" ./Maui.Generators/Maui.Generators.csproj /bl:logs/gen.binlog

Remove-Item ./packages/maui.generators -Force -Recurse

& msbuild /r /t:Rebuild ./Sample/Sample.csproj /bl:logs/sample.binlog
