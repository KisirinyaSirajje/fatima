# Fatima School Management System - PowerShell Launcher
# Alternative to the batch file

Write-Host "=========================================================" -ForegroundColor Green
Write-Host "  OUR LADY OF FATIMA SECONDARY SCHOOL MANAGEMENT SYSTEM" -ForegroundColor Green  
Write-Host "=========================================================" -ForegroundColor Green
Write-Host ""

# Set current directory to script location
Set-Location $PSScriptRoot

# Set environment variable
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "Environment set to: $($env:ASPNETCORE_ENVIRONMENT)" -ForegroundColor Yellow

# Check if executable exists
if (-not (Test-Path "FatimaSchoolManagement.exe")) {
    Write-Host "ERROR: FatimaSchoolManagement.exe not found!" -ForegroundColor Red
    Write-Host "Please ensure all files are in the same folder." -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Initialize database if needed
if (-not (Test-Path "FatimaSchool.db")) {
    Write-Host "Setting up database for first time..." -ForegroundColor Yellow
    if (Test-Path "FatimaSchool_Template.db") {
        Copy-Item "FatimaSchool_Template.db" "FatimaSchool.db"
        Write-Host "Database initialized successfully." -ForegroundColor Green
    } else {
        Write-Host "Warning: No template database found. Application will create a new one." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Current directory: $(Get-Location)" -ForegroundColor Cyan
Write-Host "Environment: $($env:ASPNETCORE_ENVIRONMENT)" -ForegroundColor Cyan
Write-Host ""
Write-Host "The application will start in a moment..." -ForegroundColor White
Write-Host "Once started, open your web browser and go to:" -ForegroundColor White
Write-Host ""
Write-Host "    http://localhost:5000" -ForegroundColor Cyan
Write-Host "    or" -ForegroundColor White
Write-Host "    http://localhost:5202" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host ""
Write-Host "=========================================================" -ForegroundColor Green

# Start the application
try {
    & ".\FatimaSchoolManagement.exe"
} catch {
    Write-Host "Error starting application: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "Application has stopped." -ForegroundColor Yellow
Read-Host "Press Enter to exit"