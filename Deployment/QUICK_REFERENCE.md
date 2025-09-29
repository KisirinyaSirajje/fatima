# Fatima School Management System - Quick Reference Guide
## Essential Operations & Common Tasks

---

## 🚀 **STARTING THE SYSTEM**

### ⭐ **Recommended Method**
1. **Double-click `SmartStart.ps1`**
2. **Wait for browser to open automatically**
3. **System ready to use!**

### 🔄 **Alternative Methods**
- **SmartStart.bat** (if PowerShell doesn't work)
- **StartFatimaSchool.bat** (manual method)
- **INSTALL.bat** (interactive setup)

### 🌐 **Access URLs**
- **Primary**: http://localhost:5001
- **Backup**: http://localhost:5000 or http://localhost:5002
- **Status**: Check launcher output for actual URL

---

## 👥 **STUDENT MANAGEMENT**

### ➕ **Add New Student**
1. **Students** → **Add New Student**
2. **Fill required fields**: Name, DOB, Gender, Registration #, Class
3. **Upload photo** (optional): JPG/PNG, max 5MB
4. **Click "Create Student"**

### ✏️ **Edit Student Information**
1. **Students** → **View All Students**
2. **Find student** → **Click "Edit"**
3. **Update information** → **Save Changes**

### 🔍 **Find Students**
- **Search by name**: Use search box at top
- **Filter by class**: Select class from dropdown
- **Browse all**: View complete student list

### 📸 **Student Photos**
- **Supported formats**: JPG, JPEG, PNG, GIF
- **Maximum size**: 5MB
- **Recommended size**: 300x400 pixels
- **Location**: wwwroot/uploads folder

---

## 📊 **GRADES & ASSESSMENT**

### 📝 **Enter Marks**
1. **Marks** → **Add New Mark**
2. **Select**: Student, Subject, Assessment Type
3. **Choose**: BOT (10%), MOT (20%), or EOT (70%)
4. **Enter**: Score and Maximum Marks
5. **Add term/year** → **Save**

### 🎯 **CBC Grade Scale**
- **Grade 1 (Highly Competent)**: 80-100%
- **Grade 2 (Competent)**: 65-79%
- **Grade 3 (Developing)**: 50-64%
- **Grade 4 (Beginning)**: Below 50%

### ⚖️ **Assessment Weights**
- **BOT** (Beginning of Term): 10%
- **MOT** (Middle of Term): 20%
- **EOT** (End of Term): 70%

### 🏆 **View Student Performance**
1. **Students** → **Click student name**
2. **View academic history and current grades**
3. **See subject-wise performance breakdown**

---

## 📄 **REPORTS & PRINTING**

### 🎓 **Student Report Cards**
1. **Reports** → **Student Report Cards**
2. **Select student(s)**
3. **Choose term/year**
4. **Generate PDF** → **Print or Save**

### 📋 **Class Mark Sheets**
1. **Reports** → **Class Mark Sheets**
2. **Select class and subjects**
3. **Choose term/year**
4. **Generate** → **PDF or Excel**

### 🖨️ **Printing Tips**
- **Page size**: A4
- **Orientation**: Portrait (report cards), Landscape (mark sheets)
- **Quality**: High quality for official documents
- **Preview first**: Always preview before printing

### 💾 **Export Options**
- **PDF**: For printing and official documents
- **Excel**: For data analysis and further editing
- **CSV**: For data exchange with other systems

---

## 🏫 **CLASS MANAGEMENT**

### 📚 **Default Classes**
- **S1A, S1B**: Senior 1 streams
- **S2A, S2B**: Senior 2 streams
- **S3A, S3B**: Senior 3 streams
- **S4A, S4B**: Senior 4 streams
- **S5 Science, S5 Arts**: Senior 5 specialized
- **S6 Science, S6 Arts**: Senior 6 specialized

### ➕ **Add New Class**
1. **Classes** → **Add New Class**
2. **Enter class name** (e.g., "S1C", "S3 Commerce")
3. **Add description** (optional)
4. **Save class**

### 👥 **View Class Information**
- **Student count**: Number of enrolled students
- **Performance statistics**: Class-wide grade averages
- **Subject performance**: Performance in each subject

---

## 📖 **SUBJECTS**

### 📚 **Available Subjects**
- Mathematics
- English Language
- Physics
- Chemistry
- Biology
- History
- Geography
- Religious Education
- Computer Studies

### ➕ **Adding Custom Subjects**
- Contact system administrator
- Specify CBC assessment criteria
- Ensure compliance with curriculum standards

---

## 💾 **DATA MANAGEMENT**

### 🔄 **Backup Database**
1. **Close application** completely
2. **Copy `FatimaSchool.db`** to safe location
3. **Copy `wwwroot/uploads` folder** (student photos)
4. **Label with date** for easy identification

### 📥 **Import Data**
1. **Prepare Excel file** with correct format
2. **Use import function** in respective module
3. **Validate data** before confirming
4. **Review imported records** for accuracy

### 📤 **Export Data**
- **Student lists**: Excel format for external use
- **Grade reports**: PDF for official documents
- **Performance data**: CSV for analysis

---

## 🔧 **TROUBLESHOOTING**

### ❗ **Common Issues & Quick Fixes**

#### **Application Won't Start**
- **Use SmartStart.ps1** (handles port conflicts automatically)
- **Check error messages** in launcher output
- **Try different launcher** (SmartStart.bat, INSTALL.bat)

#### **"No such table" Error**
1. **Close application**
2. **Delete `FatimaSchool.db`**
3. **Copy `FatimaSchool_Template.db` to `FatimaSchool.db`**
4. **Restart application**

#### **Page Won't Load**
- **Check URL**: http://localhost:5001
- **Verify application is running** (check console window)
- **Try different browser** or clear cache

#### **Photos Not Displaying**
- **Check file format**: Use JPG, PNG, GIF
- **Check file size**: Maximum 5MB
- **Verify upload path**: wwwroot/uploads folder exists

#### **Printing Issues**
- **Use Chrome or Edge** for best results
- **Check page orientation**: Portrait vs Landscape
- **Verify printer settings**: A4 paper, normal margins

#### **Slow Performance**
- **Close unnecessary programs**
- **Check available disk space**
- **Restart application periodically**

### 🆘 **Emergency Recovery**

#### **Restore from Backup**
1. **Close application**
2. **Replace `FatimaSchool.db` with backup copy**
3. **Replace `wwwroot/uploads` with backup photos**
4. **Restart application**

#### **Reset to Factory Defaults**
1. **Close application**
2. **Delete `FatimaSchool.db`**
3. **Copy `FatimaSchool_Template.db` to `FatimaSchool.db`**
4. **Delete contents of `wwwroot/uploads`**
5. **Restart application**

---

## ⚡ **KEYBOARD SHORTCUTS**

- **Ctrl + N**: New record (context-dependent)
- **Ctrl + S**: Save current form
- **Ctrl + P**: Print current page
- **Ctrl + F**: Find/Search
- **F5**: Refresh page
- **Alt + Left**: Browser back
- **Alt + Right**: Browser forward

---

## 📞 **GETTING HELP**

### 📚 **Documentation**
- **USER_MANUAL.md**: Complete user guide
- **TECHNICAL_DOCUMENTATION.md**: Developer reference
- **TROUBLESHOOTING.txt**: Specific error solutions

### 🔧 **Diagnostic Tools**
- **Diagnostic.bat**: System analysis tool
- **SystemInfo.bat**: Basic system information
- **Error logs**: Check console output for errors

### 📧 **Support Contacts**
- **Technical Support**: Contact system administrator
- **Training**: Request user training sessions
- **Feature Requests**: Submit enhancement requests

---

## 🎯 **BEST PRACTICES**

### 📅 **Daily Operations**
- **Backup database weekly** to prevent data loss
- **Enter marks regularly** rather than bulk entry
- **Verify student information** when adding new records
- **Check reports before printing** to ensure accuracy

### 🔒 **Data Security**
- **Keep backups** in multiple locations
- **Close application** when not in use
- **Monitor disk space** to prevent issues
- **Update student photos** annually

### 📊 **Academic Management**
- **Enter BOT marks early** in term
- **Record MOT marks** at mid-term
- **Complete EOT marks** before report generation
- **Review calculated grades** for accuracy

### 🖨️ **Report Preparation**
- **Verify all marks entered** before generating reports
- **Check student photos** are current and appropriate
- **Preview reports** before printing
- **Use high-quality paper** for official documents

---

## 📱 **QUICK CONTACT CARD**

### 🚀 **Start System**: Double-click `SmartStart.ps1`
### 🌐 **Access URL**: http://localhost:5001
### 💾 **Backup Files**: `FatimaSchool.db` + `wwwroot/uploads`
### 🆘 **Emergency Reset**: Copy template to working database
### 📞 **Support**: System administrator

---

*© 2024 Our Lady of Fatima Secondary School*  
*Quick Reference Guide v2.0 - Keep this handy for daily operations!*