@echo off
setlocal EnableDelayedExpansion
title Fatima School Management System - Smart Port Detection

echo =========================================================
echo   FATIMA SCHOOL MANAGEMENT SYSTEM - SMART LAUNCHER
echo =========================================================
echo.

REM Set current directory and environment
cd /d "%~dp0"
set ASPNETCORE_ENVIRONMENT=Development

echo Checking for available ports...

REM Function to check if a port is available
set "PORT_FOUND=0"
set "SELECTED_PORT="

REM Check common ports in order of preference
for %%p in (5000 5001 5002 5202 5203 5204 3000 3001 8000 8080) do (
    if !PORT_FOUND! equ 0 (
        netstat -an | findstr :%%p >nul
        if !errorlevel! neq 0 (
            echo ✓ Port %%p is available
            set "SELECTED_PORT=%%p"
            set "PORT_FOUND=1"
        ) else (
            echo ✗ Port %%p is in use
        )
    )
)

if "%PORT_FOUND%"=="0" (
    echo.
    echo Warning: All common ports are in use.
    echo Trying to find any available port...
    
    REM Try some high ports as backup
    for %%p in (49152 49153 49154 49155 49156) do (
        if !PORT_FOUND! equ 0 (
            netstat -an | findstr :%%p >nul
            if !errorlevel! neq 0 (
                echo ✓ Found available port %%p
                set "SELECTED_PORT=%%p"
                set "PORT_FOUND=1"
            )
        )
    )
)

if "%PORT_FOUND%"=="0" (
    echo ERROR: Could not find any available ports!
    echo Please try closing some applications and run again.
    pause
    exit /b 1
)

echo.
echo Selected port: %SELECTED_PORT%
echo Environment: %ASPNETCORE_ENVIRONMENT%
echo.

REM Initialize database if needed
if not exist "FatimaSchool.db" (
    echo Setting up database...
    if exist "FatimaSchool_Template.db" (
        copy "FatimaSchool_Template.db" "FatimaSchool.db" >nul 2>&1
        echo ✓ Database initialized
    )
)

echo Starting application...
echo.

REM Start the application with the selected port
echo Using port %SELECTED_PORT%...
set "ASPNETCORE_URLS=http://localhost:%SELECTED_PORT%"

echo.
echo =========================================================
echo Application starting...
echo.
echo Application will be available at: http://localhost:%SELECTED_PORT%
echo.
echo Press Ctrl+C to stop the application
echo =========================================================
echo.

REM Start the application
FatimaSchoolManagement.exe

echo.
echo Application has stopped.
pause