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

:: Your commands to run your executable go below:
"ServerRegistrationManager.exe" uninstall "IfzThumbnailHandler.dll"


setlocal enabledelayedexpansion

:: Define source folder
set "folder=C:\Program Files\IFZthumbnailHandler"

REM Check if the folder exists before attempting to delete it.
if exist !folder! (
    echo Deleting folder and all contents...
    rd /s /q !folder!
    echo Folder deleted successfully.
) else (
    echo Folder not found.
)

endlocal

echo Folder successfully deleted.




pause
