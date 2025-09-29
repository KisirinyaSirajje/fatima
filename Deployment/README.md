# Our Lady of Fatima Secondary School Management System
## Deployment Package - Version 2.0 (Enhanced Port Detection)

### 📋 System Requirements
- Windows 10 or later (64-bit)
- Minimum 4GB RAM
- 500MB free disk space
- Administrative privileges for first-time setup

### 🚀 Quick Start Guide (V2.0)

#### ⭐ RECOMMENDED: Smart Launcher (New!)
1. **Double-click `SmartStart.ps1`** (PowerShell version - most reliable)
   - OR **Double-click `SmartStart.bat`** (if PowerShell doesn't work)
2. **Wait for automatic port detection and browser opening**
3. **System automatically handles port conflicts!**

#### Alternative: Manual Method
1. **Double-click `StartFatimaSchool.bat`**
2. Wait for the application to start (may take 30-60 seconds on first run)
3. Open your web browser and navigate to the displayed URL

#### Step 1: Extract Files
1. Copy this entire folder to your desired location (e.g., `C:\FatimaSchool\`)
2. Ensure all files remain in the same folder

### 🆕 What's New in V2.0
- **🎯 Smart Port Detection**: Automatically finds available ports (no more "Address already in use" errors)
- **� Enhanced Launchers**: SmartStart.ps1 and SmartStart.bat with auto-configuration
- **🛠️ Better Error Handling**: Comprehensive diagnostics and troubleshooting
- **🌐 Auto-Browser Opening**: Automatically opens the correct URL when ready
- **📊 Port Conflict Resolution**: Tests multiple ports (5000, 5001, 5002, 5202, etc.)

### �📁 Folder Contents

```
FatimaSchool/
├── SmartStart.ps1                  # Smart launcher (PowerShell) - RECOMMENDED!
├── SmartStart.bat                  # Smart launcher (Batch) - Alternative
├── StartFatimaSchool.bat          # Enhanced startup script
├── StartFatimaSchool.ps1          # Enhanced PowerShell script
├── INSTALL.bat                    # Interactive setup wizard
├── Diagnostic.bat                 # System diagnostics tool
├── FatimaSchoolManagement.exe     # Main application executable
├── FatimaSchool_Template.db       # Template database with sample data
├── FatimaSchool.db               # Working database (created on first run)
├── appsettings.json              # Application configuration
├── wwwroot/                      # Web assets (CSS, JS, images)
├── Various .dll files            # Application dependencies
└── README.md                     # This file
```

### 🎯 Key Features Available

1. **Dashboard**
   - Student statistics and overview
   - Class performance analytics
   - Recent activities

2. **Student Management**
   - Add, edit, view, and delete students
   - Photo upload functionality
   - Class assignments

3. **Marks & Grading**
   - CBC grading system (BOT 10%, MOT 20%, EOT 70%)
   - Individual and class-wide mark entry
   - Automatic grade calculations

4. **Reports & Printing**
   - Individual student report cards
   - Class mark sheets (Excel & Print)
   - PDF generation for reports
   - Print-optimized layouts

### 🔧 Configuration

#### Default Settings
- **Environment**: Development (for full error details)
- **Database**: SQLite (FatimaSchool.db)
- **Port**: 5000 or 5202
- **Auto-open browser**: Disabled (manual navigation required)

#### To Change Port (Advanced Users)
1. Stop the application (Ctrl+C in command window)
2. Edit `appsettings.json`
3. Modify the "Urls" section
4. Restart using `StartFatimaSchool.bat`

### 🗃️ Database Information

#### Initial Data Included:
- **Classes**: S1, S2, S3, S4 (O-Level), S5, S6 (A-Level)
- **Subjects**: 
  - O-Level: Mathematics, English, Physics, Chemistry, Biology, History, Geography, etc.
  - A-Level: Advanced Mathematics, Literature, Advanced Physics, etc.
- **Sample Students**: Pre-populated for testing

#### Database Backup:
- The original database is preserved as `FatimaSchool_Template.db`
- To reset: Delete `FatimaSchool.db` and restart the application

### 🖨️ Printing Setup

#### For Best Printing Results:
1. Use **Chrome** or **Edge** browsers for printing
2. Set page orientation to **Portrait** for report cards
3. Set page orientation to **Landscape** for mark sheets
4. Use **A4** paper size
5. Enable **Background Graphics** for colored headers

### 🚨 Troubleshooting

#### Common Issues:

**Application won't start:**
- Ensure Windows Defender/Antivirus isn't blocking the .exe file
- Right-click `StartFatimaSchool.bat` → "Run as Administrator"
- Check if port 5000/5202 is available

**Browser doesn't open automatically:**
- Manually navigate to `http://localhost:5000` or `http://localhost:5202`
- Try different browsers (Chrome, Edge, Firefox)

**Database errors:**
- Delete `FatimaSchool.db` file
- Restart the application (will recreate from template)

**Permission errors:**
- Run as Administrator
- Ensure the folder isn't in a restricted location (avoid Program Files)

### 📞 Support Information

#### Pre-Configured Data:
- **Academic Year**: 2024/2025
- **Terms**: Term 1, Term 2, Term 3
- **Grading System**: A (80-100), B (70-79), C (60-69), D (50-59), E (0-49)

#### For Technical Support:
- Check the command window for error messages
- Note the exact error message and steps to reproduce
- Ensure all files are in the same folder

### 🔄 Updates and Maintenance

#### Regular Maintenance:
- Backup `FatimaSchool.db` regularly
- Monitor disk space (logs and uploaded files)
- Clean browser cache if experiencing display issues

#### Data Export:
- Use the built-in Excel export for mark sheets
- PDF reports can be saved for external use
- Database can be backed up by copying the .db file

### 💡 Tips for Daily Use

1. **Student Photos**: Place student photos in `wwwroot/uploads/students/`
2. **Mark Entry**: Use class-wide entry for efficiency
3. **Reports**: Generate reports at end of each term
4. **Backup**: Copy `FatimaSchool.db` to external storage weekly

### 🔒 Security Notes

- This deployment runs in Development mode for detailed error reporting
- No external internet access required
- Data stored locally on the machine
- Consider regular backups for data protection

---

**© 2024 Our Lady of Fatima Secondary School Management System**  
*Developed with CBC Grading System Implementation*

**Version**: 1.0  
**Build Date**: September 2024  
**Framework**: .NET 8.0