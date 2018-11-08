echo on

set Path=ProtoGen\protogen.exe

 
%Path%  -i:demo.proto    -o:OpenAPIModel\demo.cs

pause