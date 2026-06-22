-- =====================================================================
-- Script to automatically add 10 DIFFERENT sample questions 
-- to EVERY empty exam (exams that currently have 0 questions).
-- =====================================================================

DECLARE @Now DATETIME2 = GETDATE();

-- 1. Create a temporary table with a pool of various questions
CREATE TABLE #QuestionPool (
    Id INT IDENTITY(1,1),
    QuestionText NVARCHAR(2000),
    OptionA NVARCHAR(500),
    OptionB NVARCHAR(500),
    OptionC NVARCHAR(500),
    OptionD NVARCHAR(500),
    CorrectOption NVARCHAR(1)
);

INSERT INTO #QuestionPool (QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption)
VALUES 
('What is the main purpose of the OSI model?', 'To define networking protocols', 'To standardize communication functions', 'To design hardware components', 'To implement security', 'B'),
('Which layer of the OSI model is responsible for routing?', 'Data Link Layer', 'Network Layer', 'Transport Layer', 'Session Layer', 'B'),
('What does HTTP stand for?', 'HyperText Transfer Protocol', 'HyperText Transmission Protocol', 'HyperLink Transfer Protocol', 'HyperLink Transmission Protocol', 'A'),
('Which of the following is a NoSQL database?', 'MySQL', 'PostgreSQL', 'MongoDB', 'Oracle', 'C'),
('What is the default port for HTTPS?', '80', '443', '21', '22', 'B'),
('In OOP, what is polymorphism?', 'Hiding data', 'Multiple inheritance', 'Taking many forms', 'Creating objects', 'C'),
('Which data structure uses LIFO (Last In First Out)?', 'Queue', 'Array', 'Tree', 'Stack', 'D'),
('What is a primary key in a database?', 'A unique identifier for a record', 'A foreign key', 'A null value', 'An index', 'A'),
('Which of the following is a frontend framework?', 'Django', 'Spring Boot', 'React', 'Laravel', 'C'),
('What does CSS stand for?', 'Cascading Style Sheets', 'Creative Style Sheets', 'Computer Style Sheets', 'Colorful Style Sheets', 'A'),
('What is the time complexity of binary search?', 'O(1)', 'O(n)', 'O(log n)', 'O(n^2)', 'C'),
('Which protocol is used to send emails?', 'FTP', 'SMTP', 'POP3', 'HTTP', 'B'),
('In SQL, what command is used to remove a table?', 'DELETE', 'REMOVE', 'DROP', 'TRUNCATE', 'C'),
('What does API stand for?', 'Application Programming Interface', 'Automated Programming Interface', 'Application Process Integration', 'Active Programming Interface', 'A'),
('Which of these is a statically typed language?', 'Python', 'JavaScript', 'Java', 'Ruby', 'C'),
('What is the purpose of Git?', 'Database management', 'Version control', 'Web hosting', 'Compiling code', 'B'),
('What is a deadlock in OS?', 'A process waiting indefinitely', 'A process crashing', 'A memory leak', 'A fast CPU', 'A'),
('Which symbol is used for comments in Python?', '//', '/*', '<!--', '#', 'D'),
('What does JSON stand for?', 'JavaScript Object Notation', 'Java Syntax Object Network', 'JavaScript Oriented Node', 'Java Standard Output Network', 'A'),
('Which is NOT a valid HTTP method?', 'GET', 'POST', 'SEND', 'PUT', 'C');

-- 2. Find all empty exams and insert 10 random questions from the pool
DECLARE @EmptyExamId INT;
DECLARE @QuestionsAdded INT = 0;

DECLARE ExamCursor CURSOR FOR
SELECT ExamId
FROM Exam_Masters e
WHERE NOT EXISTS (SELECT 1 FROM Exam_Questions q WHERE q.ExamId = e.ExamId);

OPEN ExamCursor;
FETCH NEXT FROM ExamCursor INTO @EmptyExamId;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Insert 10 random questions from the pool into this empty exam
    INSERT INTO Exam_Questions (ExamId, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, Marks, CreatedDate)
    SELECT TOP 10 
        @EmptyExamId, 
        QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectOption, 
        5, -- 5 marks per question
        @Now
    FROM #QuestionPool
    ORDER BY NEWID(); -- NEWID() randomizes the selection

    SET @QuestionsAdded = @QuestionsAdded + 10;
    
    FETCH NEXT FROM ExamCursor INTO @EmptyExamId;
END

CLOSE ExamCursor;
DEALLOCATE ExamCursor;

DROP TABLE #QuestionPool;

PRINT 'Successfully added 10 DIFFERENT random questions to EVERY empty exam!';
