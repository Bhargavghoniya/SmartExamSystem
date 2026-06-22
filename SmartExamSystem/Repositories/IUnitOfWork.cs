using SmartExamSystem.Models;
using System;
using System.Threading.Tasks;

namespace SmartExamSystem.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<ExamStudent> ExamStudents { get; }
        IGenericRepository<ExamMaster> ExamMasters { get; }
        IGenericRepository<ExamQuestion> ExamQuestions { get; }
        IGenericRepository<ExamAttempt> ExamAttempts { get; }
        IGenericRepository<ExamStudentAnswer> ExamStudentAnswers { get; }
        IGenericRepository<ExamProctoringLog> ExamProctoringLogs { get; }

        Task<int> CompleteAsync();
    }
}
