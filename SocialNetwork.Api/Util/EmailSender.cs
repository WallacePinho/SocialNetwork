using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.Api.Util {
    public class EmailSender {
        private EmailConfig emailConfig;
        private SmtpClient smtpClient;

        public EmailSender() {
            emailConfig = new EmailConfig();
            smtpClient = new SmtpClient();
        }

        private async Task SendEmail(string to, string subject, string body) {
            smtpClient.EnableSsl = emailConfig.EnableSsl;
            smtpClient.Host = emailConfig.Host;
            smtpClient.Port = emailConfig.Port;
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(emailConfig.User, emailConfig.Password);
            smtpClient.EnableSsl = emailConfig.EnableSsl;
            emailConfig.SendTo = to;

            MailMessage mailMessage = new MailMessage(
                emailConfig.SendFrom,
                emailConfig.SendTo,
                subject,
                body);
            mailMessage.IsBodyHtml = true;

            try {
                await smtpClient.SendMailAsync(mailMessage);
            } catch(Exception ex) {
                throw ex.InnerException;
            }

        }

        public async Task SendEmailConfirmation(string token, string to, string userId) {
            StringBuilder body = new StringBuilder();
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedUserId = HttpUtility.UrlEncode(userId);
            var requestUrl = $"https://localhost:44345/Account/Confirm?token={encodedToken}&u={encodedUserId}";
            body.Append($"Click <a href=\"{requestUrl}\">here<a/> to confirm your e-mail address.");

            await SendEmail(to, "Email Confirmation", body.ToString());
        }

        public async Task SendPasswordRecovery(string token, string to, string userId) {
            StringBuilder body = new StringBuilder();
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedUserId = HttpUtility.UrlEncode(userId);
            var requestUrl = $"https://localhost:44345/Account/ChangePassword?token={encodedToken}&u={encodedUserId}";
            body.Append($"Click <a href=\"{requestUrl}\">here<a/> to change your password.");

            await SendEmail(to, "Password Recovery", body.ToString());
        }
    }

    public class EmailConfig {
        public string SendTo = "";
        public string SendFrom = "socialnetwork_donotreply@hotmail.com";
        public bool EnableSsl = true;
        public string User = "socialnetwork_donotreply@hotmail.com";
        public string Password = "P@ssw0rd123698745";
        public string Host = "smtp.live.com";
        public int Port = 25;
    }
}