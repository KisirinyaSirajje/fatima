@echo off
title Our Lady of Fatima Secondary School Management System

echo =========================================================
echo   OUR LADY OF FATIMA SECONDARY SCHOOL MANAGEMENT SYSTEM
echo =========================================================
echo.
echo Starting the application...
echo.

REM Set the current directory to the script location
cd /d "%~dp0"

REM Set the environment to Development with explicit export
set ASPNETCORE_ENVIRONMENT=Development
echo Environment set to: %ASPNETCORE_ENVIRONMENT%

REM Check if the exe file exists
if not exist "FatimaSchoolManagement.exe" (
    echo ERROR: FatimaSchoolManagement.exe not found!
    echo Please ensure all files are in the same folder.
    pause
    exit /b 1
)

REM Check if database exists, if not copy the template
if not exist "FatimaSchool.db" (
    echo Setting up database for first time...
    if exist "FatimaSchool_Template.db" (
        copy "FatimaSchool_Template.db" "FatimaSchool.db" >nul 2>&1
        if %errorlevel% equ 0 (
            echo Database initialized successfully.
        ) else (
            echo Warning: Could not copy template database.
        )
    ) else (
        echo Warning: No template database found. Application will create a new one.
    )
)

echo Current directory: %cd%
echo Environment: %ASPNETCORE_ENVIRONMENT%
echo.

REM Check if default ports are available
netstat -an | findstr :5000 >nul
if %errorlevel%==0 (
    echo Warning: Port 5000 is in use by another application.
    netstat -an | findstr :5202 >nul
    if %errorlevel%==0 (
        echo Warning: Port 5202 is also in use.
        echo Using automatic port detection...
        set ASPNETCORE_URLS=http://localhost:0
    ) else (
        echo Using alternative port 5202...
        set ASPNETCORE_URLS=http://localhost:5202
    )
) else (
    echo Using default port 5000...
    set ASPNETCORE_URLS=http://localhost:5000
)

echo The application will start in a moment...
echo.
if defined ASPNETCORE_URLS (
    if "%ASPNETCORE_URLS%"=="http://localhost:0" (
        echo The application will automatically find an available port.
        echo Look for the line: "Now listening on: http://localhost:[PORT]"
        echo Copy that URL to your web browser.
    ) else (
        echo Once started, open your web browser and go to:
        echo     %ASPNETCORE_URLS%
    )
) else (
    echo Once started, open your web browser and go to:
    echo     http://localhost:5000
    echo     or
    echo     http://localhost:5202
)
echo.
echo Press Ctrl+C to stop the application
echo.
echo =========================================================

REM Start the application with explicit environment and URL
cmd /c "set ASPNETCORE_ENVIRONMENT=Development && set ASPNETCORE_URLS=%ASPNETCORE_URLS% && FatimaSchoolManagement.exe"

echo.
echo Application has stopped.
pause