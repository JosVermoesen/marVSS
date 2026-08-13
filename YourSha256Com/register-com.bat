@echo off
setlocal

set "SCRIPT_DIR=%~dp0"
set "CONFIG=%~1"
set "ACTION=%~2"

if /I "%CONFIG%"=="" set "CONFIG=Debug"
if /I "%ACTION%"=="" set "ACTION=register"

set "DLL=%SCRIPT_DIR%bin\%CONFIG%\YourSha256Com.dll"
set "TLB=%SCRIPT_DIR%bin\%CONFIG%\YourSha256Com.tlb"
set "REGASM=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"

if not exist "%REGASM%" (
	echo RegAsm not found: "%REGASM%"
	exit /b 1
)

if not exist "%DLL%" (
	echo Assembly not found: "%DLL%"
	echo Build the project first.
	exit /b 1
)

if /I "%ACTION%"=="unregister" goto unregister
if /I not "%ACTION%"=="register" (
	echo Usage: register-com.bat [Debug^|Release] [register^|unregister]
	exit /b 1
)

echo Registering "%DLL%" for COM...
"%REGASM%" "%DLL%" /tlb:"%TLB%" /codebase
exit /b %errorlevel%

:unregister
echo Unregistering "%DLL%" from COM...
"%REGASM%" "%DLL%" /u /tlb:"%TLB%"
exit /b %errorlevel%
