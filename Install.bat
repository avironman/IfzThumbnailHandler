@echo off
:: BatchGotAdmin - Check for and get admin rights
:-------------------------------------
REM  --> Check for permissions
>nul 2>&1 "%SystemRoot%\system32\cacls.exe" "%SystemRoot%\system32\config\system"

REM --> If error flag set, we do not have admin.
if '%errorlevel%' NEQ '0' (
    echo Requesting administrative privileges...
    goto UACPrompt
) else ( goto gotAdmin )

:UACPrompt
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs"
    echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs"
    "%temp%\getadmin.vbs"
    del "%temp%\getadmin.vbs"
    exit /B

:gotAdmin
    pushd "%CD%"
    cd /d "%~dp0"
:--------------------------------------


:: Define source and destination directories
set "SourceDir=%~dp0"
set "DestDir=C:\Program Files\IFZthumbnailHandler"

:: Create the destination directory if it does not exist
if not exist "%DestDir%" mkdir "%DestDir%"

:: Copy all files from the source to the destination
xcopy "%SourceDir%*" "%DestDir%" /e /i /y

    pushd "%CD%"
    cd /d "%~dp0"
	
echo Files copied successfully to %DestDir%.


:: Your commands to run your executable go below:
"ServerRegistrationManager.exe" install "IfzThumbnailHandler.dll" -codebase

pause
