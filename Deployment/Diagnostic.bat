@echo off
title Fatima School Management System - Diagnostic Tool

echo =========================================================
echo   FATIMA SCHOOL MANAGEMENT SYSTEM - DIAGNOSTIC TOOL
echo =========================================================
echo.

echo Running diagnostics...
echo.

REM Set current directory
cd /d "%~dp0"

echo [1] Environment Check:
echo Current Directory: %cd%
echo ASPNETCORE_ENVIRONMENT: %ASPNETCORE_ENVIRONMENT%
set ASPNETCORE_ENVIRONMENT=Development
echo ASPNETCORE_ENVIRONMENT (after set): %ASPNETCORE_ENVIRONMENT%
echo.

echo [2] File Existence Check:
if exist "FatimaSchoolManagement.exe" (
    echo ✓ FatimaSchoolManagement.exe: FOUND
) else (
    echo ✗ FatimaSchoolManagement.exe: MISSING
)

if exist "FatimaSchool_Template.db" (
    echo ✓ FatimaSchool_Template.db: FOUND
) else (
    echo ✗ FatimaSchool_Template.db: MISSING
)

if exist "appsettings.json" (
    echo ✓ appsettings.json: FOUND
) else (
    echo ✗ appsettings.json: MISSING
)

if exist "appsettings.Development.json" (
    echo ✓ appsettings.Development.json: FOUND
) else (
    echo ✗ appsettings.Development.json: MISSING
)

if exist "wwwroot" (
    echo ✓ wwwroot folder: FOUND
) else (
    echo ✗ wwwroot folder: MISSING
)

echo.

echo [3] Database Setup:
if not exist "FatimaSchool.db" (
    echo Setting up database...
    copy "FatimaSchool_Template.db" "FatimaSchool.db" >nul 2>&1
    if %errorlevel% equ 0 (
        echo ✓ Database created successfully
    ) else (
        echo ✗ Failed to create database
    )
) else (
    echo ✓ Database already exists
)

echo.

echo [4] Port Check:
netstat -an | findstr :5000 >nul
if %errorlevel%==0 (
    echo ! Port 5000: IN USE (another application may be using it)
) else (
    echo ✓ Port 5000: AVAILABLE
)

netstat -an | findstr :5202 >nul
if %errorlevel%==0 (
    echo ! Port 5202: IN USE (another application may be using it)
) else (
    echo ✓ Port 5202: AVAILABLE
)

echo.

echo [5] Test Application Startup:
echo Attempting to start application for 10 seconds...
echo.

REM Try to start the application with timeout
timeout /t 3 /nobreak >nul
start /b "" cmd /c "set ASPNETCORE_ENVIRONMENT=Development && FatimaSchoolManagement.exe" >nul 2>&1

timeout /t 10 /nobreak >nul

REM Check if application is running
tasklist | findstr "FatimaSchoolManagement" >nul
if %errorlevel%==0 (
    echo ✓ Application started successfully
    echo.
    echo You can now access the system at:
    echo   http://localhost:5000
    echo   http://localhost:5202
    echo.
    echo The application is running in the background.
    echo To stop it, use Task Manager or restart your computer.
) else (
    echo ✗ Application failed to start
    echo.
    echo Possible solutions:
    echo   1. Run this diagnostic as Administrator
    echo   2. Check Windows Defender/Antivirus settings
    echo   3. Ensure .NET runtime is available
    echo   4. Try the PowerShell version: StartFatimaSchool.ps1
)

echo.
echo =========================================================
echo   DIAGNOSTIC COMPLETE
echo =========================================================
echo.
pause