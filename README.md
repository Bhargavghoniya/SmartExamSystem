# 📚 SmartExamSystem

A comprehensive web-based examination management system built with **ASP.NET Core** and **C#**. This system enables secure exam administration, student assessment, and proctoring capabilities.

---

## 📊 Project Overview

**SmartExamSystem** is a full-stack examination platform designed to streamline the exam creation, administration, and student testing process with built-in security features and proctoring capabilities.

### Key Statistics
- **Repository**: Bhargavghoniya/SmartExamSystem
- **Language Composition**: 
  - 🔵 **HTML**: 51.9%
  - 🟠 **C#**: 42.7%
  - 🟢 **CSS**: 5.3%
  - 🟡 **JavaScript**: 0.1%

---

## ✨ Core Features

### 👨‍🎓 Student Features
- **Secure Login** with email and password authentication
- **Exam Dashboard** with available exams and exam history
- **Exam Interface** with server-side timer (prevents cheating via page refresh)
- **Auto-Save Functionality** (AJAX) to prevent answer loss
- **Result Analysis** with score breakdown and pass/fail status
- **User Profile** management

### 👨‍💼 Admin Features
- **Admin Dashboard** with system overview and statistics
- **Student Management** (Create, Read, Update, Delete)
- **Exam Management** (Create, Read, Update, Delete)
- **Question Management** (Create, Read, Update, Delete)
- **Exam Attempt Tracking** - view all student exam submissions
- **Proctoring Logs** - monitor student activity during exams

### 🔒 Security Features
- Session-based authentication with 60-minute timeout
- Password-protected student and admin accounts
- Server-side exam timer to prevent manipulation
- Activity logging and proctoring records
- Prevention of duplicate exam attempts

---

## 🛠️ Technology Stack

| Layer | Technology |
|-------|-----------|
| **Backend** | ASP.NET Core 6.0+ |
| **Language** | C# |
| **Frontend** | HTML, CSS, JavaScript |
| **View Engine** | Razor (CSHTML) |
| **Database** | SQL Server |
| **ORM** | Entity Framework Core |
| **Authentication** | Session-based |

---

## 📁 Project Structure

```
SmartExamSystem/
├── Controllers/
│   ├── AdminController.cs          # Admin panel operations
│   ├── ExamController.cs           # Exam management & submission
│   └── StudentController.cs        # Student login & dashboard
│
├── Models/
│   ├── ExamStudent.cs              # Student account model
│   ├── ExamMaster.cs               # Exam metadata
│   ├── ExamQuestion.cs             # Question definitions
│   ├── ExamAttempt.cs              # Exam submission records
│   ├── ExamStudentAnswer.cs        # Student answers
│   └── ExamProctoringLog.cs        # Activity logs
│
├── Views/
│   ├── Admin/
│   │   ├── Login.cshtml            # Admin authentication
│   │   ├── Dashboard.cshtml        # Admin overview
│   │   ├── StudentList.cshtml
│   │   ├── AddStudent.cshtml / EditStudent.cshtml
│   │   ├── ExamList.cshtml
│   │   ├── AddExam.cshtml / EditExam.cshtml
│   │   ├── QuestionList.cshtml
│   │   ├── AddQuestion.cshtml / EditQuestion.cshtml
│   │   ├── ExamAttempts.cshtml
│   │   └── ProctoringLogs.cshtml
│   │
│   ├── Exam/
│   │   ├── ExamPage.cshtml         # Exam interface (server-side timer)
│   │   └── Result.cshtml           # Score display
│   │
│   └── Student/
│       ├── Login.cshtml            # Student authentication
│       ├── Dashboard.cshtml        # Available exams & history
│       ├── AvailableExams.cshtml
│       ├── Profile.cshtml
│       └── EditProfile.cshtml
│
├── Data/
│   └── ApplicationDbContext.cs     # EF Core database context
│
├── Program.cs                      # Application configuration
├── database_update.sql             # Database migration script
└── README.md                       # This file
```

---

## 🚀 Quick Start

### Prerequisites
- .NET 6.0 or higher
- SQL Server (or SQL Server Express)
- Visual Studio 2022 (or any C# IDE)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Bhargavghoniya/SmartExamSystem.git
   cd SmartExamSystem
   ```

2. **Update Database Schema**
   Run the SQL script in SQL Server Management Studio:
   ```sql
   -- Run database_update.sql to add Password column
   ALTER TABLE Exam_Students ADD [Password] NVARCHAR(100) NOT NULL DEFAULT '123';
   UPDATE Exam_Students SET [Password] = '123';
   ```

3. **Apply EF Core Migrations**
   ```bash
   Add-Migration AddPasswordToStudent
   Update-Database
   ```

4. **Configure Connection String**
   Update `appsettings.json` with your SQL Server connection string

5. **Run the Application**
   ```bash
   dotnet run
   ```
   Application will be available at `https://localhost:5001`

---

## 👤 Default Credentials

### Admin Account
- **URL**: `/Admin/Login`
- **Username**: `admin`
- **Password**: `admin123`

### Sample Student Account
- **URL**: `/Student/Login`
- **Email**: `bhargav@gmail.com`
- **Password**: `123`

---

## 🔧 Key Implementation Details

### Server-Side Timer ✅
The exam timer is calculated on the server to prevent cheating through page manipulation:
- Timer countdown calculates remaining time from server `StartTime`
- Page refresh does not reset the timer
- Automatic exam submission on timeout

### Auto-Save with AJAX ✅
Student answers are automatically saved without page refresh:
- Prevents data loss
- Real-time synchronization
- Error handling for connection issues

### Secure Session Management ✅
- Session timeout: 60 minutes of inactivity
- Automatic logout on expiration
- Secure cookie configuration

### Proctoring Logs ✅
System tracks:
- Exam start/end times
- Tab switches and window focus loss
- Copy-paste attempts
- Time spent on each question

---

## 🐛 Recent Fixes & Improvements

| Issue | Fix |
|-------|-----|
| Session not working | Moved `UseSession()` before `UseAuthorization()` in middleware pipeline |
| Timer reset on refresh | Implemented server-side timer calculation |
| No password verification | Added password check in student login |
| Navigation property corruption | Refactored CRUD operations to fetch-then-update pattern |
| Missing admin views | Created Login, EditStudent, EditExam, EditQuestion views |

---

## 📋 Features Status

| Feature | Status |
|---------|--------|
| Student Authentication | ✅ Complete |
| Exam Taking Interface | ✅ Complete |
| Admin Panel | ✅ Complete |
| Exam CRUD Operations | ✅ Complete |
| Question Management | ✅ Complete |
| Proctoring & Logging | ✅ Complete |
| Auto-Save Answers | ✅ Complete |
| Server-Side Timer | ✅ Complete |
| Result Analytics | ✅ Complete |

---

## 📚 API Endpoints

### Admin Routes
- `GET /Admin/Login` - Admin login page
- `POST /Admin/Login` - Admin authentication
- `GET /Admin/Dashboard` - Admin dashboard
- `GET/POST /Admin/AddStudent` - Create student
- `GET/POST /Admin/EditStudent/{id}` - Update student
- `GET /Admin/StudentList` - View all students

### Exam Routes
- `GET /Exam/List` - Available exams
- `POST /Exam/Start/{examId}` - Start exam
- `POST /Exam/Submit` - Submit answers
- `GET /Exam/Result/{attemptId}` - View result

### Student Routes
- `GET /Student/Login` - Student login
- `POST /Student/Login` - Student authentication
- `GET /Student/Dashboard` - Student dashboard
- `GET /Student/AvailableExams` - List exams
- `GET /Student/Profile` - User profile

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is open source and available under the MIT License.

---

## 📞 Support

For issues, questions, or suggestions:
- Open an [Issue](https://github.com/Bhargavghoniya/SmartExamSystem/issues)
- Contact the repository owner

---

## 👨‍💻 Author

**Bhargav Ghoniya**
- GitHub: [@Bhargavghoniya](https://github.com/Bhargavghoniya)

---

**Last Updated**: June 2026  
**Version**: 1.0.0
