using Microsoft.Extensions.Logging;
using Pastel.Domain.Entities;
using Pastel.Domain.ValuesObject;
using Pastel.Handles.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Pastel.Handles.Handle
{
    public class EmailHanle : IEmailHandle
    {
        private readonly ILogger<EmailHanle> _logger;

        public EmailHanle(ILogger<EmailHanle> logger)
        {
            _logger = logger;
        }

        public bool Send(string? email, string subject, string template)
        {
            try
            {
                var _mailMessage = new MailMessage();
                _mailMessage.From = new MailAddress("evolutiondraft@gmail.com");
                _mailMessage.To.Add("rafael.biase@inovvi.com.br");
                _mailMessage.Subject = "Email de ativação";
                _mailMessage.IsBodyHtml = true;
                _mailMessage.Body = $"teste";

                SmtpClient _smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32("587"));

                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential("evolutiondraft@gmail.com", "stxhpuunmtdeijze");

                _smtpClient.EnableSsl = true;

                _smtpClient.Send(_mailMessage);

                return true;

            }
            catch (Exception error)
            {
                var message = $"{error.InnerException}\n " +
                   $"{error.Message} \n " +
                   $"{error.StackTrace}";

                _logger.LogError(message);

                return false;
            }
        }

        public string CreatedTaskEmailTemplate(TaskModel task, FullName fullName)
        {
            var email = new StringBuilder();
            email.Append($"<p>Olá, <strong>{fullName.FirstName}{fullName.LastName}</strong>,");
            email.Append($"A tarefa {task.Message} foi criada.<br>");
            email.Append($"Data de limite: {task.Deadline} </p><br>");
            email.Append("<p>Em caso de duvidas consulte seu gestor.</p>");

            return email.ToString();
        }

        public string CompletedTaskEmailTemplate(TaskModel task, FullName manager, FullName user)
        {
            var email = new StringBuilder();
            email.Append($"<p>Olá, <strong>{manager.FirstName}{manager.LastName}</strong>,");
            email.Append($"Email de conclusão de tarefa.<br>");
            email.Append($"Tarefa: {task.Message} </p><br>");
            email.Append($"Data de limite: {task.Deadline} </p><br>");
            email.Append($"Usuario: {user.FirstName}{user.LastName} </p><br>");
            email.Append($"Data de conclusão: {DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0))} </p><br>");

            return email.ToString();
        }
    }
}