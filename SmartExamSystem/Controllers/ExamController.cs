using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartExamSystem.Data;
using SmartExamSystem.Models;

namespace SmartExamSystem.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Start(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login", "Student");
            }

            var exam = _context.ExamMasters
                .FirstOrDefault(e => e.ExamId == id && e.IsActive == true);

            if (exam == null)
            {
                return NotFound();
            }

            if (exam.StartTime.HasValue && SmartExamSystem.Helpers.TimeHelper.GetLocalTime() < exam.StartTime.Value)
            {
                TempData["Error"] = $"This exam is not available yet. It will start at {exam.StartTime.Value:g}.";
                return RedirectToAction("AvailableExams", "Student");
            }

            if (exam.EndTime.HasValue && SmartExamSystem.Helpers.TimeHelper.GetLocalTime() > exam.EndTime.Value)
            {
                TempData["Error"] = $"This exam has already ended at {exam.EndTime.Value:g}.";
                return RedirectToAction("AvailableExams", "Student");
            }

            // STEP 22: Prevent duplicate exam attempt
            var existingAttempt = _context.ExamAttempts
                .FirstOrDefault(a => a.StudentId == studentId.Value && 
                                     a.ExamId == exam.ExamId &&
                                     (a.Status == "Started" || a.Status == "Submitted" || a.Status == "Terminated"));

            if (existingAttempt != null)
            {
                TempData["Error"] = "You have already attempted this exam. Please contact the administrator to reset it.";
                return RedirectToAction("AvailableExams", "Student");
            }

            var attempt = new ExamAttempt
            {
                StudentId = studentId.Value,
                ExamId = exam.ExamId,
                StartTime = SmartExamSystem.Helpers.TimeHelper.GetLocalTime(),
                Status = "Started",
                Score = 0,
                WarningCount = 0,
                TerminateReason = ""
            };

            _context.ExamAttempts.Add(attempt); 
            _context.SaveChanges();

            HttpContext.Session.SetInt32("AttemptId", attempt.AttemptId);
            HttpContext.Session.SetInt32("ExamId", exam.ExamId);

            return RedirectToAction("ExamPage", new { id = attempt.AttemptId });
        }

        public IActionResult ExamPage(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login", "Student");
            }

            var attempt = _context.ExamAttempts
                .Include(a => a.Exam)
                .FirstOrDefault(a => a.AttemptId == id && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return NotFound();
            }

            if (attempt.Status != "Started")
            {
                return RedirectToAction("Result", new { id = attempt.AttemptId });
            }

            // Check if exam time has expired
            var examEndTime = attempt.StartTime.AddMinutes(attempt.Exam.DurationMinutes);
            if (SmartExamSystem.Helpers.TimeHelper.GetLocalTime() > examEndTime)
            {
                // Auto-submit expired exam with score from saved answers
                var savedAnswers = _context.ExamStudentAnswers
                    .Where(sa => sa.AttemptId == attempt.AttemptId && sa.IsCorrect)
                    .Join(_context.ExamQuestions, sa => sa.QuestionId, q => q.QuestionId, (sa, q) => q.Marks)
                    .Sum();

                attempt.Score = savedAnswers;
                attempt.EndTime = examEndTime;
                attempt.Status = "Submitted";
                _context.SaveChanges();

                TempData["Error"] = "Your exam time has expired. The exam has been auto-submitted with your saved answers.";
                return RedirectToAction("AvailableExams", "Student");
            }

            var questions = _context.ExamQuestions
                .Where(q => q.ExamId == attempt.ExamId)
                .OrderBy(q => q.QuestionId)
                .ToList();

            ViewBag.AttemptId = attempt.AttemptId;
            ViewBag.ExamName = attempt.Exam.ExamName;
            ViewBag.DurationMinutes = attempt.Exam.DurationMinutes;
            ViewBag.StartTime = attempt.StartTime;
            ViewBag.RemainingSeconds = (int)Math.Max(0, (examEndTime - SmartExamSystem.Helpers.TimeHelper.GetLocalTime()).TotalSeconds);

            return View(questions);
        }

        // STEP 24: Save answer one by one
        [HttpPost]
        public IActionResult SaveAnswer(int attemptId, int questionId, string selectedOption)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            var attempt = _context.ExamAttempts
                .FirstOrDefault(a => a.AttemptId == attemptId && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return Json(new { success = false, message = "Attempt not found" });
            }

            if (attempt.Status != "Started")
            {
                return Json(new { success = false, message = "Exam already finished" });
            }

            // Check if answer already exists for this question
            var existingAnswer = _context.ExamStudentAnswers
                .FirstOrDefault(a => a.AttemptId == attemptId && a.QuestionId == questionId);

            var question = _context.ExamQuestions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question == null)
            {
                return Json(new { success = false, message = "Question not found" });
            }

            bool isCorrect = selectedOption == question.CorrectOption;

            if (existingAnswer != null)
            {
                // Update existing answer
                existingAnswer.SelectedOption = selectedOption;
                existingAnswer.IsCorrect = isCorrect;
                _context.ExamStudentAnswers.Update(existingAnswer);
            }
            else
            {
                // Create new answer
                var studentAnswer = new ExamStudentAnswer
                {
                    AttemptId = attemptId,
                    QuestionId = questionId,
                    SelectedOption = selectedOption,
                    IsCorrect = isCorrect
                };
                _context.ExamStudentAnswers.Add(studentAnswer);
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Answer saved" });
        }

        [HttpPost]
        public IActionResult SubmitExam([FromForm] int attemptId)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login", "Student");
            }

            var attempt = _context.ExamAttempts
                .FirstOrDefault(a => a.AttemptId == attemptId && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return NotFound();
            }

            if (attempt.Status != "Started")
            {
                return RedirectToAction("Result", new { id = attempt.AttemptId });
            }

            var questions = _context.ExamQuestions
                .Where(q => q.ExamId == attempt.ExamId)
                .ToList();

            int score = 0;

            // Get answers that were already saved via AJAX
            var savedAnswers = _context.ExamStudentAnswers
                .Where(a => a.AttemptId == attemptId)
                .ToList();

            // Parse any form-submitted answers as fallback
            var formAnswers = new Dictionary<int, string>();
            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("answers[") && key.EndsWith("]"))
                {
                    var idStr = key.Substring(8, key.Length - 9);
                    if (int.TryParse(idStr, out int questionId))
                    {
                        formAnswers[questionId] = Request.Form[key].ToString();
                    }
                }
            }

            foreach (var question in questions)
            {
                var savedAnswer = savedAnswers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
                
                if (savedAnswer != null)
                {
                    // Answer was already saved via AJAX
                    if (savedAnswer.IsCorrect)
                    {
                        score += question.Marks;
                    }
                }
                else if (formAnswers.ContainsKey(question.QuestionId))
                {
                    // Fallback: save answer from form submission
                    string selectedOption = formAnswers[question.QuestionId];
                    bool isCorrect = selectedOption == question.CorrectOption;

                    if (isCorrect)
                    {
                        score += question.Marks;
                    }

                    var studentAnswer = new ExamStudentAnswer
                    {
                        AttemptId = attempt.AttemptId,
                        QuestionId = question.QuestionId,
                        SelectedOption = selectedOption,
                        IsCorrect = isCorrect
                    };

                    _context.ExamStudentAnswers.Add(studentAnswer);
                }
            }

            attempt.Score = score;
            attempt.EndTime = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();
            attempt.Status = "Submitted";

            _context.SaveChanges();

            return RedirectToAction("Result", new { id = attempt.AttemptId });
        }

        public IActionResult Result(int id)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login", "Student");
            }

            var attempt = _context.ExamAttempts
                .Include(a => a.Exam)
                .Include(a => a.Student)
                .FirstOrDefault(a => a.AttemptId == id && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return NotFound();
            }

            return View(attempt);
        }

        [HttpPost]
        public IActionResult LogActivity(int attemptId, string activityType, string description)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return Json(new { success = false, message = "Session expired" });
            }

            var attempt = _context.ExamAttempts
                .FirstOrDefault(a => a.AttemptId == attemptId && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return Json(new { success = false, message = "Attempt not found" });
            }

            if (attempt.Status != "Started")
            {
                return Json(new { success = false, message = "Exam already finished" });
            }

            var log = new ExamProctoringLog
            {
                AttemptId = attempt.AttemptId,
                ActivityType = activityType,
                Description = description,
                LogTime = SmartExamSystem.Helpers.TimeHelper.GetLocalTime()
            };

            _context.ExamProctoringLogs.Add(log);

            attempt.WarningCount++;

            if (attempt.WarningCount >= 3)
            {
                attempt.Status = "Terminated";
                attempt.EndTime = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();
                attempt.TerminateReason = "Suspicious activity limit exceeded";
            }

            _context.SaveChanges();

            return Json(new
            {
                success = true,
                warningCount = attempt.WarningCount,
                status = attempt.Status
            });
        }
    }
}
