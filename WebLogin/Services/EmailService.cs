using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WebLogin.Models;

namespace WebLogin.Services
{
    public static class EmailService
    {
        // SMTP Host
        private static string Host = "smtp.somehost.com";
        // SMTP Port
        private static int Port = 111;
        // Sender name
        private static string FromName = "Someone";
        // Sender email
        private static string Email = "someone@somemail.com";
        // Email password
        private static string Password = "******";

        public static bool SendEmail(EmailDTO emailDTO)
        {
            try
            {
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress(FromName, Email));
                email.To.Add(MailboxAddress.Parse(emailDTO.To));
                email.Subject = emailDTO.Subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = emailDTO.Content
                };

                var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(Host, Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(Email, Password);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}