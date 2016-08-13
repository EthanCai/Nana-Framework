del /a /f Build.log

echo Start build and publish framework... >> Build.log

path   %path%;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE
rem path %path%;c:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE

devenv /build Release .\Nana.Framework.sln  >> Build.log

echo END build and publish framework... >> Build.log



