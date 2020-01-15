using Microsoft.AspNetCore.Rewrite;
using System.Net.Mail;
using System.Text;

namespace RaprockPlaylist.Functions
{
    class Validify
    {
        public static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }
    }
    class Mail
    {
        public static void SendMail(string subject, string body, string from)
        {
            MailMessage mailMessage = new MailMessage(from,"root@raprockplaylist.tk");
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            SmtpClient smtpClient = new SmtpClient("mail.raprockplaylist.tk");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Send(mailMessage);
        }
    }
    class NonWwwRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var req = context.HttpContext.Request;
            var currentHost = req.Host;
            if (currentHost.Host.StartsWith("www."))
            {
                var newHost = currentHost.Host.Substring(4);
                var newUrl = new StringBuilder().Append("https://").Append(newHost).Append(req.PathBase).Append(req.Path).Append(req.QueryString);
                context.HttpContext.Response.Redirect(newUrl.ToString(), true);
                context.Result = RuleResult.EndResponse;
            }
        }
    }
}