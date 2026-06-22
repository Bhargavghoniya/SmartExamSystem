namespace SmartExamSystem.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a welcome email to a newly created student with their login credentials.
        /// </summary>
        Task SendStudentWelcomeEmailAsync(string toEmail, string studentName, string password, string loginUrl);
    }
}
