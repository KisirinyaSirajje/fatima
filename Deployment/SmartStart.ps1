# Fatima School Management System - Smart Port Detection (PowerShell)
param(
    [int]$PreferredPort = 5000
)

Write-Host "=========================================================" -ForegroundColor Green
Write-Host "  FATIMA SCHOOL MANAGEMENT SYSTEM - SMART LAUNCHER" -ForegroundColor Green  
Write-Host "=========================================================" -ForegroundColor Green
Write-Host ""

# Set current directory and environment
Set-Location $PSScriptRoot
$env:ASPNETCORE_ENVIRONMENT = "Development"

Write-Host "Environment: $($env:ASPNETCORE_ENVIRONMENT)" -ForegroundColor Yellow
Write-Host "Checking for available ports..." -ForegroundColor Cyan
Write-Host ""

# Function to check if a port is available
function Test-Port {
    param([int]$Port)
    
    try {
        $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, $Port)
        $listener.Start()
        $listener.Stop()
        return $true
    }
    catch {
        return $false
    }
}

# Find an available port
$PortsToTry = @(5000, 5001, 5002, 5202, 5203, 5204, 3000, 3001, 8000, 8080, 8081, 8082)
$SelectedPort = $null

foreach ($Port in $PortsToTry) {
    if (Test-Port -Port $Port) {
        Write-Host "checkmark Port $Port is available" -ForegroundColor Green
        $SelectedPort = $Port
        break
    } else {
        Write-Host "X Port $Port is in use" -ForegroundColor Red
    }
}

if ($null -eq $SelectedPort) {
    Write-Host ""
    Write-Host "Warning: All common ports are in use." -ForegroundColor Yellow
    Write-Host "Trying with random high port..." -ForegroundColor Yellow
    
    # Generate random port between 49152-65535 (dynamic/private port range)
    do {
        $RandomPort = Get-Random -Minimum 49152 -Maximum 65535
    } while (-not (Test-Port -Port $RandomPort))
    
    $SelectedPort = $RandomPort
    Write-Host "checkmark Found available port: $SelectedPort" -ForegroundColor Green
}

Write-Host ""
Write-Host "Selected port: $SelectedPort" -ForegroundColor Green
$env:ASPNETCORE_URLS = "http://localhost:$SelectedPort"

# Check required files
Write-Host ""
Write-Host "Checking system requirements..." -ForegroundColor Cyan

if (-not (Test-Path "FatimaSchoolManagement.exe")) {
    Write-Host "X FatimaSchoolManagement.exe not found!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Initialize database if needed
if (-not (Test-Path "FatimaSchool.db")) {
    Write-Host "Setting up database..." -ForegroundColor Yellow
    if (Test-Path "FatimaSchool_Template.db") {
        Copy-Item "FatimaSchool_Template.db" "FatimaSchool.db"
        Write-Host "checkmark Database initialized" -ForegroundColor Green
    } else {
        Write-Host "Warning: No template database found" -ForegroundColor Yellow
    }
} else {
    # Verify database integrity by checking if tables exist
    try {
        $sqliteCheck = & sqlite3.exe "FatimaSchool.db" ".tables" 2>$null
        if ($sqliteCheck -notlike "*Students*") {
            Write-Host "Database appears corrupted, refreshing..." -ForegroundColor Yellow
            Remove-Item "FatimaSchool.db" -Force
            Copy-Item "FatimaSchool_Template.db" "FatimaSchool.db"
            Write-Host "checkmark Database refreshed" -ForegroundColor Green
        } else {
            Write-Host "checkmark Database verified" -ForegroundColor Green
        }
    } catch {
        Write-Host "Could not verify database, proceeding..." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "=========================================================" -ForegroundColor Green
Write-Host "Starting application..." -ForegroundColor White
Write-Host ""
Write-Host "Application will be available at:" -ForegroundColor White
Write-Host "  http://localhost:$SelectedPort" -ForegroundColor Cyan
Write-Host ""
Write-Host "Opening browser in 10 seconds..." -ForegroundColor Yellow

# Start application in background and wait for it to be ready
$AppProcess = Start-Process -FilePath ".\FatimaSchoolManagement.exe" -PassThru -WindowStyle Hidden

# Wait a bit and then try to open browser
Start-Sleep -Seconds 8

# Check if the application is responding
try {
    $response = Invoke-WebRequest -Uri "http://localhost:$SelectedPort" -TimeoutSec 5 -UseBasicParsing
    if ($response.StatusCode -eq 200) {
        Write-Host "checkmark Application is ready!" -ForegroundColor Green
        Start-Process "http://localhost:$SelectedPort"
    }
}
catch {
    Write-Host "Application is still starting up. Please wait and try manually:" -ForegroundColor Yellow
    Write-Host "  http://localhost:$SelectedPort" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host "=========================================================" -ForegroundColor Green

# Wait for the process to complete
$AppProcess.WaitForExit()

Write-Host ""
Write-Host "Application has stopped." -ForegroundColor Yellow
Read-Host "Press Enter to exit"