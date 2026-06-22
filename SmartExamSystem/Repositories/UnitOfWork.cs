using SmartExamSystem.Data;
using SmartExamSystem.Models;
using System.Threading.Tasks;

namespace SmartExamSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<ExamStudent> ExamStudents { get; private set; }
        public IGenericRepository<ExamMaster> ExamMasters { get; private set; }
        public IGenericRepository<ExamQuestion> ExamQuestions { get; private set; }
        public IGenericRepository<ExamAttempt> ExamAttempts { get; private set; }
        public IGenericRepository<ExamStudentAnswer> ExamStudentAnswers { get; private set; }
        public IGenericRepository<ExamProctoringLog> ExamProctoringLogs { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            ExamStudents = new GenericRepository<ExamStudent>(_context);
            ExamMasters = new GenericRepository<ExamMaster>(_context);
            ExamQuestions = new GenericRepository<ExamQuestion>(_context);
            ExamAttempts = new GenericRepository<ExamAttempt>(_context);
            ExamStudentAnswers = new GenericRepository<ExamStudentAnswer>(_context);
            ExamProctoringLogs = new GenericRepository<ExamProctoringLog>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
