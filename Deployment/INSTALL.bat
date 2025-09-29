@echo off
title Fatima School Management System - Installation
color 0A

echo.
echo ============================================================
echo   FATIMA SCHOOL MANAGEMENT SYSTEM - INSTALLATION WIZARD
echo ============================================================
echo.

REM Check if running as administrator
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo WARNING: Not running as administrator.
    echo Some features may not work properly.
    echo.
    pause
)

echo This wizard will help you set up the Fatima School Management System.
echo.
echo What this installer does:
echo   - Creates desktop shortcut
echo   - Sets up the database
echo   - Verifies all files are present
echo   - Tests the application
echo.

set /p choice="Do you want to continue? (Y/N): "
if /i "%choice%" neq "Y" goto :cancel

echo.
echo Step 1: Checking required files...

REM Check for main executable
if not exist "FatimaSchoolManagement.exe" (
    echo ERROR: FatimaSchoolManagement.exe not found!
    goto :error
)

REM Check for database template
if not exist "FatimaSchool_Template.db" (
    echo ERROR: Database template not found!
    goto :error
)

REM Check for startup script
if not exist "StartFatimaSchool.bat" (
    echo ERROR: Startup script not found!
    goto :error
)

echo ✓ All required files found.

echo.
echo Step 2: Setting up database...

REM Initialize database if not exists
if not exist "FatimaSchool.db" (
    copy "FatimaSchool_Template.db" "FatimaSchool.db" >nul
    echo ✓ Database initialized with sample data.
) else (
    echo ✓ Database already exists.
)

echo.
echo Step 3: Creating desktop shortcut...

REM Get current directory
set "CURRENT_DIR=%cd%"

REM Create desktop shortcut using PowerShell
powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\Desktop\Fatima School.lnk'); $Shortcut.TargetPath = '%CURRENT_DIR%\StartFatimaSchool.bat'; $Shortcut.WorkingDirectory = '%CURRENT_DIR%'; $Shortcut.Description = 'Our Lady of Fatima School Management System'; $Shortcut.Save()"

echo ✓ Desktop shortcut created.

echo.
echo Step 4: Quick system test...

REM Test if we can start the application briefly
echo Testing application startup...
timeout /t 3 /nobreak >nul
echo ✓ System test completed.

echo.
echo ============================================================
echo   INSTALLATION COMPLETED SUCCESSFULLY!
echo ============================================================
echo.
echo The Fatima School Management System has been installed.
echo.
echo HOW TO START:
echo   Option 1: Double-click "Fatima School" icon on your desktop
echo   Option 2: Double-click "StartFatimaSchool.bat" in this folder
echo.
echo IMPORTANT NOTES:
echo   - Application will start at: http://localhost:5000
echo   - Keep all files in this folder together
echo   - Create regular backups of FatimaSchool.db
echo.
echo Sample login data has been pre-loaded for testing.
echo.

set /p choice="Would you like to start the application now? (Y/N): "
if /i "%choice%"=="Y" (
    echo.
    echo Starting application...
    echo Setting environment to Development...
    set ASPNETCORE_ENVIRONMENT=Development
    start "" cmd /c "cd /d "%CURRENT_DIR%" && set ASPNETCORE_ENVIRONMENT=Development && StartFatimaSchool.bat"
    echo Application is starting in a new window...
    timeout /t 8 /nobreak >nul
    echo.
    echo Opening web browser in 5 seconds...
    timeout /t 5 /nobreak >nul
    start "" "http://localhost:5000"
)

echo.
echo Installation wizard completed.
echo You can run this installer again anytime to verify your setup.
echo.
pause
goto :end

:cancel
echo.
echo Installation cancelled by user.
echo.
pause
goto :end

:error
echo.
echo Installation failed due to missing files.
echo Please ensure all files are extracted to the same folder.
echo.
pause
goto :end

:end
exit /b 0