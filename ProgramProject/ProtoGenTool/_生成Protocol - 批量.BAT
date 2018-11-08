@echo off

set Path=ProtoGen\protogen.exe

for /f "delims=" %%i in ('dir /b proto "proto/*.proto"') do %Path% -i:proto/%%i -o:cs/%%~ni.cs

pause
