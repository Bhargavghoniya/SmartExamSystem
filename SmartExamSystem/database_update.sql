-- =====================================================================
-- SmartExamSystem - Database Update Script
-- Run this in SQL Server Management Studio (SSMS) on your database: db47570
-- =====================================================================

-- Step 1: Add Password column to Exam_Students table
-- (This column is needed for student login with password)

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Exam_Students' AND COLUMN_NAME = 'Password'
)
BEGIN
    ALTER TABLE Exam_Students ADD [Password] NVARCHAR(100) NOT NULL DEFAULT '123';
    PRINT 'Password column added successfully.';
END
ELSE
BEGIN
    PRINT 'Password column already exists.';
END

-- Step 2: Update existing students with default password '123'
UPDATE Exam_Students 
SET [Password] = '123' 
WHERE [Password] IS NULL OR [Password] = '';

-- Step 3: Update the sample student email/password if needed
-- Email: bhargav@gmail.com | Password: 123
UPDATE Exam_Students SET [Password] = '123' WHERE Email = 'bhargav@gmail.com';

PRINT 'Database update complete!';

-- =====================================================================
-- After running this script, run your EF Core migration:
--   Add-Migration AddPasswordToStudent
--   Update-Database
-- =====================================================================
