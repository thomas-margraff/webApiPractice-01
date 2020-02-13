using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace EmailLib
{
    public class Gmail
    {
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
                    Password = "Sapphire5211"
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
