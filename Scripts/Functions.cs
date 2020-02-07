using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using RaprockPlaylist.Context;
using RaprockPlaylist.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace RaprockPlaylist.Functions
{
    class Validify
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
    class Mail
    {
        public static void SendMail(string subject, string body)
        {
            var fromMail = new MailAddress("NetMailer@raprockplaylist.tk","Net Mailer");
            var toMail = new MailAddress("info@raprockplaylist.tk","Info");
            MailMessage mailMessage = new MailMessage(fromMail,toMail);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient
            {
                Host = "127.0.0.1",
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("NetMailer", "LxJbui8ith4dtc-W4suQYK-h"),
                Timeout = 20000
            };
            smtpClient.Send(mailMessage);
        }
    }
    class EntityFW
    {
        public static Visitor InitializeVisitor(PlaylistContext context, IHttpContextAccessor accessor)
        {
            Visitor visitor = context.Visitor.Where(v => v.IpAdress == accessor.HttpContext.Connection.RemoteIpAddress.ToString()).FirstOrDefault() ?? new Visitor();
            if (String.IsNullOrEmpty(visitor.IpAdress))
            {
                visitor.IpAdress = GetIpAdress(accessor);
                context.Visitor.Add(visitor);
            }
            else
            {
                context.Attach(visitor);
            }
            return visitor;
        }
        public static String GetIpAdress(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
    class Log
    {
        public static void LogActivity(PlaylistContext context, string source, string message, Visitor visitor)
        {
            Models.Log log = new Models.Log();
            log.IdVisitorNavigation = visitor;
            log.Message = message;
            log.Source = source;
            context.Log.Add(log);
            context.SaveChanges();
        }
        public static void LogActivity(PlaylistContext context, string source, string message, IHttpContextAccessor accessor)
        {
            Visitor visitor = EntityFW.InitializeVisitor(context, accessor);
            LogActivity(context, source, message, visitor);
        }
        public static void LogError(PlaylistContext context, string source, string message, Visitor visitor)
        {
            try
            {
                Models.ErrorLog errorLog = new Models.ErrorLog();
                errorLog.IdVisitorNavigation = visitor;
                errorLog.Message = message;
                errorLog.Source = source;
                context.ErrorLog.Add(errorLog);
                context.SaveChanges();
                Mail.SendMail("Error","Source:<br>"+source+"<br>Message:<br>"+message);
            }
            catch(Exception e)
            {
                Mail.SendMail("Critical Error",e.ToString());
            }
        }
        public static void LogError(PlaylistContext context, string source, string message, IHttpContextAccessor accessor)
        {
            Visitor visitor = EntityFW.InitializeVisitor(context, accessor);
            LogError(context, source, message, visitor);
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