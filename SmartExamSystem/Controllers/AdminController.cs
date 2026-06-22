using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartExamSystem.Data;
using SmartExamSystem.Models;
using SmartExamSystem.Services;

namespace SmartExamSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // STEP 16: Admin Dashboard
        public IActionResult Dashboard()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var totalStudents = _context.ExamStudents.Count();
            var totalExams = _context.ExamMasters.Count();
            var totalAttempts = _context.ExamAttempts.Count();
            var terminatedExams = _context.ExamAttempts.Where(a => a.Status == "Terminated").Count();

            ViewBag.TotalStudents = totalStudents;
            ViewBag.TotalExams = totalExams;
            ViewBag.TotalAttempts = totalAttempts;
            ViewBag.TerminatedExams = terminatedExams;

            // Recent attempts
            var recentAttempts = _context.ExamAttempts
                .Include(a => a.Student)
                .Include(a => a.Exam)
                .OrderByDescending(a => a.StartTime)
                .Take(10)
                .ToList();

            ViewBag.RecentAttempts = recentAttempts;

            return View();
        }

        // STEP 17: Admin View Exam Attempts
        public IActionResult ExamAttempts(int? examId = null, string? status = null)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var query = _context.ExamAttempts
                .Include(a => a.Student)
                .Include(a => a.Exam)
                .AsQueryable();

            if (examId.HasValue)
            {
                query = query.Where(a => a.ExamId == examId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            var attempts = query.OrderByDescending(a => a.StartTime).ToList();
            ViewBag.FilterExamId = examId;
            ViewBag.FilterStatus = status;
            ViewBag.Exams = _context.ExamMasters.ToList();

            return View(attempts);
        }

        // STEP 18: Admin View Proctoring Logs
        public IActionResult ProctoringLogs(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var attempt = _context.ExamAttempts
                .Include(a => a.Student)
                .Include(a => a.Exam)
                .FirstOrDefault(a => a.AttemptId == id);

            if (attempt == null)
            {
                return NotFound();
            }

            var logs = _context.ExamProctoringLogs
                .Where(l => l.AttemptId == id)
                .OrderByDescending(l => l.LogTime)
                .ToList();

            ViewBag.Attempt = attempt;
            return View(logs);
        }

        // STEP 19: Admin CRUD for Students
        public IActionResult StudentList()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var students = _context.ExamStudents.ToList();
            return View(students);
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(ExamStudent student)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                _context.ExamStudents.Add(student);
                _context.SaveChanges();

                // Send welcome email with login credentials
                try
                {
                    var loginUrl = $"{Request.Scheme}://{Request.Host}/Student/Login";
                    await _emailService.SendStudentWelcomeEmailAsync(
                        student.Email,
                        student.StudentName,
                        student.Password,
                        loginUrl
                    );
                    TempData["Success"] = "Student added successfully. Welcome email sent!";
                }
                catch (Exception ex)
                {
                    // Log the error but don't fail the student creation
                    TempData["Success"] = "Student added successfully.";
                    TempData["Warning"] = $"Welcome email could not be sent: {ex.Message}";
                }

                return RedirectToAction("StudentList");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents.FirstOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(int id, ExamStudent student)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != student.StudentId)
            {
                return NotFound();
            }

            // Retrieve existing student and update fields to avoid overwriting navigation props
            var existing = _context.ExamStudents.FirstOrDefault(s => s.StudentId == id);
            if (existing == null) return NotFound();

            existing.StudentName = student.StudentName;
            existing.Email = student.Email;
            existing.Password = student.Password;
            existing.PhoneNumber = student.PhoneNumber;
            existing.Address = student.Address;
            existing.IsActive = student.IsActive;

            _context.ExamStudents.Update(existing);
            _context.SaveChanges();
            TempData["Success"] = "Student updated successfully";
            return RedirectToAction("StudentList");
        }

        [HttpPost]
        public IActionResult DeleteStudent(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents.FirstOrDefault(s => s.StudentId == id);
            if (student != null)
            {
                _context.ExamStudents.Remove(student);
                _context.SaveChanges();
                TempData["Success"] = "Student deleted successfully";
            }

            return RedirectToAction("StudentList");
        }

        [HttpPost]
        public IActionResult ToggleStudentStatus(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents.FirstOrDefault(s => s.StudentId == id);
            if (student != null)
            {
                student.IsActive = !student.IsActive;
                _context.ExamStudents.Update(student);
                _context.SaveChanges();
                TempData["Success"] = (student.IsActive ? "Student unblocked" : "Student blocked");
            }

            return RedirectToAction("StudentList");
        }

        // STEP 20: Admin CRUD for Exams
        public IActionResult ExamList()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var exams = _context.ExamMasters.ToList();
            ViewBag.QuestionCounts = _context.ExamQuestions
                .GroupBy(q => q.ExamId)
                .ToDictionary(g => g.Key, g => g.Count());
            return View(exams);
        }

        [HttpGet]
        public IActionResult AddExam()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddExam(ExamMaster exam)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                // Check max 5 exams limit
                var totalExams = _context.ExamMasters.Count();
                if (totalExams >= 5)
                {
                    ModelState.AddModelError("", "Maximum limit of 5 exams reached. You cannot add more exams.");
                    return View(exam);
                }

                if (exam.StartTime.HasValue && exam.EndTime.HasValue && exam.StartTime >= exam.EndTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be after start time.");
                    return View(exam);
                }

                _context.ExamMasters.Add(exam);
                _context.SaveChanges();
                TempData["Success"] = "Exam created successfully";
                return RedirectToAction("ExamList");
            }

            return View(exam);
        }

        [HttpGet]
        public IActionResult EditExam(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var exam = _context.ExamMasters.FirstOrDefault(e => e.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        [HttpPost]
        public IActionResult EditExam(int id, ExamMaster exam)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != exam.ExamId)
            {
                return NotFound();
            }

            var existing = _context.ExamMasters.FirstOrDefault(e => e.ExamId == id);
            if (existing == null) return NotFound();

            if (exam.StartTime.HasValue && exam.EndTime.HasValue && exam.StartTime >= exam.EndTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time.");
                return View(exam);
            }

            existing.ExamName = exam.ExamName;
            existing.Subject = exam.Subject;
            existing.StartTime = exam.StartTime;
            existing.EndTime = exam.EndTime;
            existing.DurationMinutes = exam.DurationMinutes;
            existing.TotalMarks = exam.TotalMarks;
            existing.Description = exam.Description;
            existing.Instructions = exam.Instructions;
            existing.IsActive = exam.IsActive;
            existing.UpdatedDate = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();

            _context.ExamMasters.Update(existing);
            _context.SaveChanges();
            TempData["Success"] = "Exam updated successfully";
            return RedirectToAction("ExamList");
        }

        [HttpPost]
        public IActionResult DeleteExam(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var exam = _context.ExamMasters.FirstOrDefault(e => e.ExamId == id);
            if (exam != null)
            {
                _context.ExamMasters.Remove(exam);
                _context.SaveChanges();
                TempData["Success"] = "Exam deleted successfully";
            }

            return RedirectToAction("ExamList");
        }

        [HttpPost]
        public IActionResult ToggleExamStatus(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var exam = _context.ExamMasters.FirstOrDefault(e => e.ExamId == id);
            if (exam != null)
            {
                exam.IsActive = !exam.IsActive;
                _context.ExamMasters.Update(exam);
                _context.SaveChanges();
                TempData["Success"] = (exam.IsActive ? "Exam activated" : "Exam deactivated");
            }

            return RedirectToAction("ExamList");
        }

        // STEP 21: Admin CRUD for Questions
        public IActionResult QuestionList(int? examId = null)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            IQueryable<ExamQuestion> query = _context.ExamQuestions.Include(q => q.Exam);

            if (examId.HasValue)
            {
                query = query.Where(q => q.ExamId == examId.Value);
            }

            var questions = query.ToList();
            ViewBag.FilterExamId = examId;

            return View(questions);
        }

        [HttpGet]
        public IActionResult AddQuestion(int? examId = null)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Exams = _context.ExamMasters.ToList();
            ViewBag.SelectedExamId = examId;

            return View();
        }

        [HttpPost]
        public IActionResult AddQuestion(ExamQuestion question)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                _context.ExamQuestions.Add(question);
                _context.SaveChanges();
                TempData["Success"] = "Question added successfully";
                return RedirectToAction("QuestionList", new { examId = question.ExamId });
            }

            ViewBag.Exams = _context.ExamMasters.ToList();
            return View(question);
        }

        [HttpGet]
        public IActionResult EditQuestion(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var question = _context.ExamQuestions.FirstOrDefault(q => q.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            ViewBag.Exams = _context.ExamMasters.ToList();
            return View(question);
        }

        [HttpPost]
        public IActionResult EditQuestion(int id, ExamQuestion question)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            if (id != question.QuestionId)
            {
                return NotFound();
            }

            var existing = _context.ExamQuestions.FirstOrDefault(q => q.QuestionId == id);
            if (existing == null) return NotFound();

            existing.ExamId = question.ExamId;
            existing.QuestionText = question.QuestionText;
            existing.OptionA = question.OptionA;
            existing.OptionB = question.OptionB;
            existing.OptionC = question.OptionC;
            existing.OptionD = question.OptionD;
            existing.CorrectOption = question.CorrectOption;
            existing.Marks = question.Marks;
            existing.UpdatedDate = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();

            _context.ExamQuestions.Update(existing);
            _context.SaveChanges();
            TempData["Success"] = "Question updated successfully";
            return RedirectToAction("QuestionList", new { examId = existing.ExamId });
        }

        [HttpPost]
        public IActionResult DeleteQuestion(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }

            var question = _context.ExamQuestions.FirstOrDefault(q => q.QuestionId == id);
            if (question != null)
            {
                int examId = question.ExamId;
                _context.ExamQuestions.Remove(question);
                _context.SaveChanges();
                TempData["Success"] = "Question deleted successfully";
                return RedirectToAction("QuestionList", new { examId = examId });
            }

            return RedirectToAction("QuestionList");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Simple admin login - in production use proper authentication
            if (username == "admin" && password == "admin123")
            {
                HttpContext.Session.SetInt32("AdminId", 1);
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
