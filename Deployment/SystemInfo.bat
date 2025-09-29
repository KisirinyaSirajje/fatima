@echo off
title System Information - Fatima School Management System

echo =========================================================
echo   FATIMA SCHOOL MANAGEMENT SYSTEM - SYSTEM INFO
echo =========================================================
echo.

echo Current Date and Time: %date% %time%
echo Current Directory: %cd%
echo User: %username%
echo Computer: %computername%
echo Operating System: %OS%
echo Processor: %PROCESSOR_IDENTIFIER%
echo.

echo Environment Variables:
echo ASPNETCORE_ENVIRONMENT: %ASPNETCORE_ENVIRONMENT%
echo.

echo File Check:
if exist "FatimaSchoolManagement.exe" (
    echo ✓ FatimaSchoolManagement.exe: FOUND
) else (
    echo ✗ FatimaSchoolManagement.exe: MISSING
)

if exist "FatimaSchool.db" (
    echo ✓ FatimaSchool.db: FOUND
) else (
    echo ✗ FatimaSchool.db: MISSING
)

if exist "FatimaSchool_Template.db" (
    echo ✓ FatimaSchool_Template.db: FOUND
) else (
    echo ✗ FatimaSchool_Template.db: MISSING
)

if exist "wwwroot" (
    echo ✓ wwwroot folder: FOUND
) else (
    echo ✗ wwwroot folder: MISSING
)

echo.
echo Network Check:
netstat -an | findstr :5000 >nul
if %errorlevel%==0 (
    echo ✓ Port 5000: IN USE
) else (
    echo ○ Port 5000: AVAILABLE
)

netstat -an | findstr :5202 >nul
if %errorlevel%==0 (
    echo ✓ Port 5202: IN USE
) else (
    echo ○ Port 5202: AVAILABLE
)

echo.
echo Disk Space:
dir /-c | find "bytes free"

echo.
echo Process Check:
tasklist | findstr "FatimaSchoolManagement" >nul
if %errorlevel%==0 (
    echo ✓ Application is currently RUNNING
) else (
    echo ○ Application is NOT running
)

echo.
echo =========================================================
echo   END OF SYSTEM INFORMATION
echo =========================================================
echo.
echo If you're experiencing issues, please provide this information
echo along with your problem description.
echo.
pause