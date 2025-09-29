# Our Lady of Fatima Secondary School Management System
## Complete User Manual

### Table of Contents
1. [System Overview](#system-overview)
2. [Getting Started](#getting-started)
3. [Dashboard Overview](#dashboard-overview)
4. [Student Management](#student-management)
5. [Class Management](#class-management)
6. [Subject Management](#subject-management)
7. [Marks & Assessment](#marks--assessment)
8. [CBC Grading System](#cbc-grading-system)
9. [Reports & Printing](#reports--printing)
10. [Data Import/Export](#data-importexport)
11. [Troubleshooting](#troubleshooting)

---

## System Overview

### What is the Fatima School Management System?
The Fatima School Management System is a comprehensive web-based application designed specifically for **Our Lady of Fatima Secondary School**. It implements the **Ugandan Competency-Based Curriculum (CBC)** grading system and provides complete student, class, and academic record management.

### Key Features
- ✅ **Student Information Management** - Complete student profiles with photos
- ✅ **CBC-Compliant Grading** - BOT (10%), MOT (20%), EOT (70%) assessment structure
- ✅ **Academic Performance Tracking** - Individual and class-wide performance analytics
- ✅ **Report Generation** - Student report cards, class mark sheets, performance summaries
- ✅ **Print & Export** - PDF reports, Excel spreadsheets, printable documents
- ✅ **Photo Management** - Student photo upload and display
- ✅ **Data Analytics** - Performance trends, class statistics, grade distributions

### Technical Specifications
- **Platform**: Web-based (runs in any modern browser)
- **Technology**: .NET 8 ASP.NET Core MVC
- **Database**: SQLite (file-based, portable)
- **Supported Browsers**: Chrome, Edge, Firefox, Safari
- **Operating System**: Windows 10/11 (64-bit)

---

## Getting Started

### System Requirements
- Windows 10 or later (64-bit)
- Minimum 4GB RAM
- 500MB free disk space
- Modern web browser (Chrome, Edge, Firefox recommended)

### First Time Setup
1. **Copy the deployment folder** to your desired location
2. **Double-click `SmartStart.ps1`** (recommended) or `SmartStart.bat`
3. **Wait for the browser to open automatically**
4. **The system is ready to use!**

### Accessing the System
- **Default URL**: http://localhost:5001 (or as displayed by the launcher)
- **No login required** - Direct access to all features
- **Bookmark the URL** for quick access in future sessions

---

## Dashboard Overview

### Main Dashboard Features
The dashboard provides an at-a-glance view of your school's performance:

#### Student Statistics
- **Total Students**: Current enrollment count
- **Active Classes**: Number of classes with enrolled students
- **Performance Overview**: Grade distribution across all students

#### Recent Activities
- **Latest Assessments**: Recently entered marks and grades
- **New Student Registrations**: Recently added students
- **Updated Records**: Recently modified student information

#### Quick Actions
- **Add New Student**: Quick access to student registration
- **Enter Marks**: Direct link to mark entry forms
- **Generate Reports**: Access to report generation tools
- **View Classes**: Navigate to class management

### Navigation Menu
- **🏠 Home**: Return to main dashboard
- **👥 Students**: Student management and profiles
- **📊 Marks**: Assessment and grade management
- **📋 Classes**: Class organization and management
- **📄 Reports**: Report generation and export tools

---

## Student Management

### Adding New Students

#### Step-by-Step Process
1. **Navigate to Students** → Click "Add New Student"
2. **Fill Required Information**:
   - **Full Name**: Student's complete name
   - **Date of Birth**: Use date picker or format DD/MM/YYYY
   - **Gender**: Select Male or Female
   - **Registration Number**: Unique student identifier
   - **Class**: Select from available classes
   - **Parent/Guardian Contact**: Phone number or email
   - **Address**: Student's home address

3. **Upload Student Photo** (Optional):
   - Click "Choose File" next to Photo
   - Select JPG, PNG, or GIF image
   - Maximum file size: 5MB
   - Recommended size: 300x400 pixels

4. **Save Student Record**: Click "Create Student"

#### Important Notes
- **Registration Number** must be unique for each student
- **Class assignment** can be changed later if needed
- **Photo upload** is optional but recommended for reports
- All fields marked with * are required

### Editing Student Information

#### How to Edit Students
1. **Go to Students** → Click "View All Students"
2. **Find the Student**: Use search or browse the list
3. **Click "Edit"** next to the student's name
4. **Modify Information**: Update any fields as needed
5. **Save Changes**: Click "Update Student"

#### What Can Be Updated
- ✅ Personal information (name, date of birth, gender)
- ✅ Contact details (phone, address)
- ✅ Class assignment
- ✅ Student photo (upload new image)
- ✅ Registration number (ensure uniqueness)

### Student Profiles

#### Individual Student View
Each student has a detailed profile showing:
- **Personal Information**: Name, age, gender, registration number
- **Academic Details**: Current class, subjects enrolled
- **Contact Information**: Parent/guardian details, address
- **Academic Performance**: Current grades and assessment history
- **Photo Display**: Student's uploaded photograph

#### Academic History
- **Subject-wise Performance**: Grades in each subject
- **Assessment Breakdown**: BOT, MOT, and EOT marks
- **Grade Progression**: Performance trends over time
- **Attendance Records**: Class attendance information

---

## Class Management

### Understanding Classes

#### Class Structure
- **Class Name**: Identifier (e.g., "S1A", "S2B", "S3 Science")
- **Class Level**: Secondary level (S1, S2, S3, S4, S5, S6)
- **Stream/Section**: Additional classification (A, B, Science, Arts)
- **Class Teacher**: Assigned teacher (if applicable)

#### Default Classes
The system comes with pre-configured classes:
- **S1A, S1B**: Senior 1 streams
- **S2A, S2B**: Senior 2 streams
- **S3A, S3B**: Senior 3 streams
- **S4A, S4B**: Senior 4 streams
- **S5 Science, S5 Arts**: Senior 5 specialized streams
- **S6 Science, S6 Arts**: Senior 6 specialized streams

### Managing Classes

#### Adding New Classes
1. **Navigate to Classes** → Click "Add New Class"
2. **Enter Class Details**:
   - **Class Name**: Unique identifier
   - **Description**: Additional details about the class
   - **Capacity**: Maximum number of students (optional)
3. **Save Class**: Click "Create Class"

#### Editing Existing Classes
1. **Go to Classes** → Find the class to edit
2. **Click "Edit"** next to the class name
3. **Update Information**: Modify name or description
4. **Save Changes**: Click "Update Class"

#### Class Statistics
For each class, view:
- **Total Students**: Number of enrolled students
- **Average Performance**: Class-wide grade average
- **Subject Performance**: Performance in each subject
- **Grade Distribution**: Number of students in each grade category

---

## Subject Management

### Available Subjects

#### Core Subjects
The system includes standard Ugandan secondary school subjects:
- **Mathematics**: Core mathematical concepts
- **English Language**: Language arts and literature
- **Physics**: Physical sciences
- **Chemistry**: Chemical sciences
- **Biology**: Life sciences
- **History**: Historical studies
- **Geography**: Earth and environmental sciences
- **Religious Education**: Spiritual and moral education
- **Computer Studies**: ICT and computer literacy

#### Subject Configuration
- Each subject is pre-configured with appropriate CBC assessment criteria
- Subjects are available across all class levels
- Assessment weightings are automatically applied per CBC standards

### Adding Custom Subjects

#### How to Add New Subjects
1. **Contact System Administrator**: Subject addition requires backend configuration
2. **Specify Subject Details**: Name, assessment criteria, applicable classes
3. **CBC Compliance**: Ensure new subjects follow BOT/MOT/EOT structure

---

## Marks & Assessment

### CBC Assessment Structure

#### Understanding CBC Grading
The system implements the **Ugandan Competency-Based Curriculum** assessment structure:

- **BOT (Beginning of Term)**: 10% of total grade
- **MOT (Middle of Term)**: 20% of total grade  
- **EOT (End of Term)**: 70% of total grade

#### Grade Scale
- **1 (Highly Competent)**: 80-100%
- **2 (Competent)**: 65-79%
- **3 (Developing Competency)**: 50-64%
- **4 (Beginning)**: Below 50%

### Entering Marks

#### Step-by-Step Mark Entry
1. **Navigate to Marks** → Click "Add New Mark"
2. **Select Assessment Details**:
   - **Student**: Choose from dropdown list
   - **Subject**: Select the subject being assessed
   - **Assessment Type**: Choose BOT, MOT, or EOT
   - **Term**: Select current term
   - **Year**: Enter academic year

3. **Enter Assessment Scores**:
   - **Score**: Enter the raw score (0-100)
   - **Maximum Marks**: Total possible marks (usually 100)
   - **Comments**: Optional teacher comments

4. **Save Assessment**: Click "Create Mark"

#### Bulk Mark Entry
For efficiency when entering multiple student marks:
1. **Use Excel Import**: Prepare marks in Excel format
2. **Class-based Entry**: Enter marks for entire class at once
3. **Subject-based Entry**: Enter all students' marks for one subject

### Grade Calculation

#### Automatic Calculations
The system automatically:
- **Calculates Weighted Scores**: Applies CBC weightings (10%, 20%, 70%)
- **Determines Final Grades**: Converts to 1-4 grade scale
- **Updates Student Records**: Reflects new grades in profiles
- **Generates Statistics**: Updates class and school-wide performance data

#### Manual Grade Override
If needed, authorized users can:
- **Override Calculated Grades**: Manual grade assignment
- **Add Grade Comments**: Explanatory notes for unusual grades
- **Adjust Weightings**: Modify assessment percentages if required

---

## CBC Grading System

### Understanding CBC Implementation

#### Assessment Philosophy
The CBC system focuses on:
- **Competency Development**: Skills-based learning outcomes
- **Continuous Assessment**: Regular evaluation throughout the term
- **Holistic Evaluation**: Multiple assessment methods and times

#### Term Structure
- **Beginning of Term (BOT)**: Diagnostic assessment (10%)
- **Middle of Term (MOT)**: Formative assessment (20%)
- **End of Term (EOT)**: Summative assessment (70%)

### Grade Interpretation

#### Grade Meanings
- **Grade 1 - Highly Competent (80-100%)**:
  - Student demonstrates exceptional understanding
  - Exceeds curriculum expectations
  - Shows advanced application of concepts

- **Grade 2 - Competent (65-79%)**:
  - Student meets curriculum expectations
  - Shows good understanding of concepts
  - Applies knowledge effectively

- **Grade 3 - Developing Competency (50-64%)**:
  - Student shows basic understanding
  - Requires additional support
  - Developing required competencies

- **Grade 4 - Beginning (Below 50%)**:
  - Student needs significant support
  - Basic concepts not yet mastered
  - Requires intensive intervention

### Performance Tracking

#### Individual Progress
- **Term-by-Term Improvement**: Track student growth over time
- **Subject-Specific Performance**: Identify strengths and weaknesses
- **Competency Development**: Monitor skill acquisition

#### Class Performance
- **Grade Distribution**: Visual representation of class achievement
- **Subject Comparison**: Compare performance across subjects
- **Trend Analysis**: Identify improvement or decline patterns

---

## Reports & Printing

### Available Reports

#### 1. Student Report Cards

**Individual Academic Reports**
- **Complete Grade Summary**: All subjects with BOT, MOT, EOT, and final grades
- **CBC-Compliant Format**: Follows official Ugandan report card standards
- **Term Performance**: Current term's academic achievement
- **Comments Section**: Teacher and headteacher remarks
- **Student Photo**: Professional presentation with student photograph

**How to Generate**:
1. Navigate to **Reports** → **Student Report Cards**
2. **Select Student**: Choose individual or multiple students
3. **Select Term/Year**: Choose reporting period
4. **Generate PDF**: Click "Generate Report Card"
5. **Print or Save**: Use browser print function or save PDF

#### 2. Class Mark Sheets

**Comprehensive Class Performance**
- **Subject-wise Analysis**: All students' performance in each subject
- **Grade Distribution**: Visual summary of class achievement levels
- **Statistical Summary**: Class averages, highest/lowest scores
- **CBC Compliance**: BOT, MOT, EOT breakdown for each student
- **Excel Export**: Spreadsheet format for further analysis

**How to Generate**:
1. Navigate to **Reports** → **Class Mark Sheets**
2. **Select Class**: Choose specific class
3. **Select Subjects**: Choose all or specific subjects
4. **Select Term**: Choose reporting period
5. **Generate Report**: Click "Generate Mark Sheet"
6. **Export Options**: Choose PDF or Excel format

#### 3. Performance Analytics

**School-wide Statistics**
- **Grade Distribution**: Percentage of students in each grade category
- **Subject Performance**: Average performance across all subjects
- **Class Comparison**: Comparative analysis between different classes
- **Trend Analysis**: Performance changes over multiple terms
- **Top Performers**: Highest achieving students by class and subject

#### 4. Custom Reports

**Flexible Reporting Options**
- **Date Range Selection**: Custom time periods
- **Subject-Specific Reports**: Focus on individual subjects
- **Student Selection**: Custom student groups
- **Format Options**: PDF, Excel, or printable formats

### Printing Guidelines

#### Print Settings
- **Page Size**: A4 (recommended)
- **Orientation**: Portrait for report cards, Landscape for mark sheets
- **Margins**: Normal (2.54cm all sides)
- **Quality**: High quality for professional appearance

#### Print Preparation
1. **Preview Reports**: Always preview before printing
2. **Check Data**: Verify all information is accurate and current
3. **Paper Quality**: Use quality paper for official documents
4. **Color vs. Black & White**: Color recommended for charts and photos

#### Bulk Printing
- **Multiple Students**: Generate and print multiple report cards simultaneously
- **Class Reports**: Print entire class performance summaries
- **Batch Processing**: Efficient printing for large numbers of reports

---

## Data Import/Export

### Excel Integration

#### Importing Student Data

**Supported Import Formats**
- **Student Information**: Bulk import of student registration data
- **Class Assignments**: Mass assignment of students to classes
- **Mark Entry**: Bulk import of assessment scores

**Import Process**:
1. **Download Template**: Get Excel template with correct format
2. **Prepare Data**: Fill template with student/mark information
3. **Upload File**: Use import function in respective module
4. **Validate Data**: System checks for errors and duplicates
5. **Confirm Import**: Review and approve data import

**Template Columns for Student Import**:
- Registration Number, Full Name, Date of Birth, Gender
- Class, Parent Contact, Address, Email (optional)

**Template Columns for Mark Import**:
- Registration Number, Subject, Assessment Type (BOT/MOT/EOT)
- Score, Maximum Marks, Term, Year, Comments (optional)

#### Exporting Data

**Available Export Formats**
- **Excel (.xlsx)**: Spreadsheet format for analysis
- **CSV (.csv)**: Universal format for data exchange
- **PDF (.pdf)**: Formatted reports for printing and sharing

**Export Options**:
- **Complete Student Database**: All student information
- **Class Lists**: Students in specific classes
- **Performance Data**: All grades and assessments
- **Custom Selections**: User-defined data sets

### Data Backup

#### Automatic Backups
- **Database File**: SQLite database can be easily copied
- **Regular Backups**: Copy `FatimaSchool.db` to safe location
- **Version Control**: Keep multiple backup copies with dates

#### Manual Backup Process
1. **Close Application**: Ensure system is not running
2. **Copy Database**: Copy `FatimaSchool.db` to backup location
3. **Include Photos**: Copy `wwwroot/uploads` folder for student photos
4. **Label Backup**: Include date and description
5. **Test Restore**: Verify backup integrity periodically

---

## Troubleshooting

### Common Issues and Solutions

#### 1. Application Won't Start

**"Address already in use" Error**
- **Solution**: Use `SmartStart.ps1` or `SmartStart.bat`
- **Alternative**: Manually specify different port (5001, 5002, etc.)
- **Check**: Ensure no other applications using same port

**"Database not found" Error**
- **Solution**: Ensure `FatimaSchool.db` exists in application folder
- **Fix**: Copy `FatimaSchool_Template.db` to `FatimaSchool.db`
- **Verify**: Check file permissions and location

#### 2. Database Issues

**"No such table" Error**
- **Cause**: Database file is corrupted or empty
- **Solution**: Replace `FatimaSchool.db` with fresh copy of template
- **Prevention**: Regular database backups

**Slow Performance**
- **Cause**: Large database size or insufficient system resources
- **Solution**: Archive old data, increase system memory
- **Optimization**: Regular database maintenance

#### 3. Browser Issues

**Page Won't Load**
- **Check**: Correct URL (http://localhost:5001)
- **Verify**: Application is running (check console window)
- **Browser**: Try different browser or clear cache

**Features Not Working**
- **JavaScript**: Ensure JavaScript is enabled
- **Browser**: Update to latest version
- **Cache**: Clear browser cache and cookies

#### 4. Printing Problems

**Reports Not Printing Correctly**
- **Settings**: Check printer settings and page orientation
- **Browser**: Use Chrome or Edge for best printing results
- **Format**: Ensure correct paper size (A4)

**Images Missing in Reports**
- **Photos**: Verify student photos are properly uploaded
- **Path**: Check image file paths and permissions
- **Format**: Ensure images are in supported formats (JPG, PNG)

### System Maintenance

#### Regular Maintenance Tasks

**Weekly**
- **Backup Database**: Copy database file to safe location
- **Check Disk Space**: Ensure adequate storage available
- **Update Records**: Verify and update student information

**Monthly**
- **Performance Review**: Check system performance and speed
- **Data Validation**: Verify accuracy of academic records
- **User Feedback**: Gather feedback from teachers and staff

**Termly**
- **Major Backup**: Complete system backup including all files
- **Data Archive**: Archive old academic term data
- **System Update**: Check for any system updates or patches

#### Performance Optimization

**Database Optimization**
- **Remove Old Data**: Archive previous academic years
- **Index Maintenance**: Ensure database indexes are optimized
- **Regular Cleanup**: Remove temporary files and logs

**System Resources**
- **Memory Management**: Monitor system memory usage
- **Disk Cleanup**: Remove unnecessary files regularly
- **Process Monitoring**: Check for resource-intensive processes

### Getting Help

#### Support Resources
1. **User Manual**: This comprehensive guide
2. **Quick Reference**: `QUICK_START.txt` in deployment folder
3. **Troubleshooting Guide**: `TROUBLESHOOTING.txt` for specific issues
4. **System Diagnostics**: Run `Diagnostic.bat` for system analysis

#### Contact Information
- **Technical Support**: Contact system administrator
- **Training**: Arrange user training sessions for staff
- **Feature Requests**: Submit requests for new functionality
- **Bug Reports**: Report any system issues or errors

---

## Appendices

### Appendix A: Keyboard Shortcuts
- **Ctrl + N**: New student entry
- **Ctrl + S**: Save current form
- **Ctrl + P**: Print current page
- **Ctrl + F**: Search/Find function
- **F5**: Refresh page/data

### Appendix B: File Locations
- **Application**: `FatimaSchoolManagement.exe`
- **Database**: `FatimaSchool.db`
- **Photos**: `wwwroot/uploads/`
- **Backups**: `backups/` folder (if configured)
- **Logs**: `logs/` folder (if configured)

### Appendix C: System Specifications
- **.NET Version**: 8.0
- **Database**: SQLite 3.x
- **Web Framework**: ASP.NET Core MVC
- **Supported Browsers**: Chrome 90+, Edge 90+, Firefox 85+
- **Minimum RAM**: 4GB
- **Recommended RAM**: 8GB or higher

---

*© 2024 Our Lady of Fatima Secondary School. This system is designed specifically for educational institution management in compliance with Ugandan CBC standards.*