-- =====================================================================
-- SmartExamSystem - Database Update and Seed Script
-- Run this in SQL Server Management Studio (SSMS) on your database.
-- =====================================================================

-- ==========================================
-- STEP 1: Update Schema (Add new columns)
-- ==========================================

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Exam_Masters' AND COLUMN_NAME = 'StartTime'
)
BEGIN
    ALTER TABLE Exam_Masters ADD [StartTime] DATETIME2 NULL;
    PRINT 'StartTime column added successfully.';
END

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Exam_Masters' AND COLUMN_NAME = 'EndTime'
)
BEGIN
    ALTER TABLE Exam_Masters ADD [EndTime] DATETIME2 NULL;
    PRINT 'EndTime column added successfully.';
END

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Exam_Masters' AND COLUMN_NAME = 'Subject'
)
BEGIN
    ALTER TABLE Exam_Masters ADD [Subject] NVARCHAR(100) NULL;
    PRINT 'Subject column added successfully.';
END
GO

-- ==========================================
-- STEP 2: Seed Data (Insert 5 Exams)
-- ==========================================

-- Check if we already have 5 exams, to avoid duplicates
IF (SELECT COUNT(*) FROM Exam_Masters) < 5
BEGIN
    DECLARE @Now DATETIME2 = GETDATE();
    DECLARE @End DATETIME2 = DATEADD(day, 7, @Now); -- Exams active for 7 days

    -- 1. ASP.NET WebForms Exam
    IF NOT EXISTS (SELECT * FROM Exam_Masters WHERE ExamName = 'ASP.NET WebForms Final')
    BEGIN
        INSERT INTO Exam_Masters (ExamName, Subject, DurationMinutes, TotalMarks, Description, Instructions, IsActive, CreatedDate, StartTime, EndTime)
        VALUES ('ASP.NET WebForms Final', 'ASP.NET WebForms', 60, 10, 'Final assessment for ASP.NET WebForms.', 'Read each question carefully before answering.', 1, @Now, @Now, @End);
        
        DECLARE @Exam1Id INT = SCOPE_IDENTITY();
        INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
        VALUES 
        (@Exam1Id, 'Which file contains the UI logic in WebForms?', '.aspx', '.aspx.cs', '.config', '.master', 'A', 5, @Now),
        (@Exam1Id, 'What is ViewState used for?', 'Routing', 'Security', 'Preserving page state across postbacks', 'Caching', 'C', 5, @Now);
    END

    -- 2. SQL Server Exam
    IF NOT EXISTS (SELECT * FROM Exam_Masters WHERE ExamName = 'SQL Server Basics')
    BEGIN
        INSERT INTO Exam_Masters (ExamName, Subject, DurationMinutes, TotalMarks, Description, Instructions, IsActive, CreatedDate, StartTime, EndTime)
        VALUES ('SQL Server Basics', 'SQL Server', 45, 10, 'Test your knowledge on SQL Server.', 'Complete the exam within the time limit.', 1, @Now, @Now, @End);
        
        DECLARE @Exam2Id INT = SCOPE_IDENTITY();
        INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
        VALUES 
        (@Exam2Id, 'Which clause is used to filter records?', 'ORDER BY', 'GROUP BY', 'HAVING', 'WHERE', 'D', 5, @Now),
        (@Exam2Id, 'What does DML stand for?', 'Data Manipulation Language', 'Data Markup Language', 'Database Management Logic', 'Domain Modeling Layer', 'A', 5, @Now);
    END

    -- 3. MVC Core Exam
    IF NOT EXISTS (SELECT * FROM Exam_Masters WHERE ExamName = 'ASP.NET Core MVC Basics')
    BEGIN
        INSERT INTO Exam_Masters (ExamName, Subject, DurationMinutes, TotalMarks, Description, Instructions, IsActive, CreatedDate, StartTime, EndTime)
        VALUES ('ASP.NET Core MVC Basics', 'MVC Core', 60, 10, 'Fundamentals of ASP.NET Core MVC.', 'Ensure a stable connection.', 1, @Now, @Now, @End);
        
        DECLARE @Exam3Id INT = SCOPE_IDENTITY();
        INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
        VALUES 
        (@Exam3Id, 'What handles incoming requests in MVC?', 'Model', 'View', 'Controller', 'Service', 'C', 5, @Now),
        (@Exam3Id, 'What is the default routing pattern in Core MVC?', '{controller}/{action}/{id?}', '{action}/{controller}', '{id}/{controller}/{action}', 'None of the above', 'A', 5, @Now);
    END

    -- 4. Java Exam
    IF NOT EXISTS (SELECT * FROM Exam_Masters WHERE ExamName = 'Java Core Principles')
    BEGIN
        INSERT INTO Exam_Masters (ExamName, Subject, DurationMinutes, TotalMarks, Description, Instructions, IsActive, CreatedDate, StartTime, EndTime)
        VALUES ('Java Core Principles', 'Java', 60, 10, 'Test on Core Java.', 'Good luck.', 1, @Now, @Now, @End);
        
        DECLARE @Exam4Id INT = SCOPE_IDENTITY();
        INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
        VALUES 
        (@Exam4Id, 'Which of these is not a Java keyword?', 'class', 'interface', 'extends', 'implement', 'D', 5, @Now),
        (@Exam4Id, 'What is the size of int in Java?', '16 bits', '32 bits', '64 bits', 'Depends on OS', 'B', 5, @Now);
    END

    -- 5. C# OOPs Exam
    IF NOT EXISTS (SELECT * FROM Exam_Masters WHERE ExamName = 'C# OOPs Concepts')
    BEGIN
        INSERT INTO Exam_Masters (ExamName, Subject, DurationMinutes, TotalMarks, Description, Instructions, IsActive, CreatedDate, StartTime, EndTime)
        VALUES ('C# OOPs Concepts', 'C# OOPs', 60, 10, 'Object-Oriented Programming in C#.', 'No external materials allowed.', 1, @Now, @Now, @End);
        
        DECLARE @Exam5Id INT = SCOPE_IDENTITY();
        INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
        VALUES 
        (@Exam5Id, 'Which keyword is used to inherit a class in C#?', 'extends', ':', 'implements', 'inherit', 'B', 5, @Now),
        (@Exam5Id, 'Can a class implement multiple interfaces in C#?', 'Yes', 'No', 'Only abstract classes can', 'Only struct can', 'A', 5, @Now);
    END

    PRINT '5 sample exams with questions have been seeded successfully!';
END
ELSE
BEGIN
    PRINT 'Exams already exist. Skipping seed process.';
END
GO
