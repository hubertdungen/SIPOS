@echo off
setlocal

cd /d "%~dp0"
set "SIPOS_DEFAULT_LABEL=Beta-1.3.0"

echo.
echo SIPOS Portable Builder
echo ======================
echo.

where dotnet >nul 2>nul
if errorlevel 1 (
    echo ERROR: .NET SDK was not found.
    echo Install the .NET SDK before building the portable package.
    echo.
    pause
    exit /b 1
)

set "SIPOS_PACKAGE_LABEL=%SIPOS_DEFAULT_LABEL%"
set /p SIPOS_LABEL_INPUT=Version label [%SIPOS_DEFAULT_LABEL%]: 
if not "%SIPOS_LABEL_INPUT%"=="" set "SIPOS_PACKAGE_LABEL=%SIPOS_LABEL_INPUT%"

echo.
echo Building SIPOS portable package: %SIPOS_PACKAGE_LABEL%
echo.

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\Publish-Portable.ps1" -Version "%SIPOS_PACKAGE_LABEL%"
set "BUILD_EXIT=%ERRORLEVEL%"

echo.
if not "%BUILD_EXIT%"=="0" (
    echo Build failed with exit code %BUILD_EXIT%.
    echo.
    pause
    exit /b %BUILD_EXIT%
)

echo Portable package created in:
echo %~dp0artifacts
echo.
pause
exit /b 0
