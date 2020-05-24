using System;
using System.Collections.Generic;
using System.Net.Mail;
using EasyNetQ.Common.Messages;

namespace EmailLib
{
    public class Gmail
    {
        public static void Send(EmailMessage emailMessage)
        {
            foreach (string addr in emailMessage.Recipients)
            {
                try
                {
                    MailMessage mail = new MailMessage(emailMessage.FromEmail, addr);
                    mail.Subject = emailMessage.Subject;
                    mail.Body = emailMessage.Body;
                    mail.IsBodyHtml = emailMessage.IsBodyHtml;

                    SmtpClient smtpClient = new SmtpClient(emailMessage.SmtpHost, emailMessage.Port);

                    smtpClient.Credentials = new System.Net.NetworkCredential()
                    {
                        UserName = emailMessage.UserName,
                        Password = emailMessage.Password
                    };

                    smtpClient.EnableSsl = true;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                            System.Security.Cryptography.X509Certificates.X509Chain chain,
                            System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };

                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static void Send(string subject, string body, List<string> recipients, bool isBodyHtml = false)
        {
            foreach (string addr in recipients)
            {
                Send(subject, body, addr, isBodyHtml);
            }
        }

        public static void Send(string subject, string body, string recipient, bool isBodyHtml = false)
        {
            try
            {
                MailMessage mail = new MailMessage("tmargraff@gmail.com", recipient);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isBodyHtml;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "tmargraff@gmail.com",
                    Password = "S@pphire5211"
                };

                smtpClient.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
