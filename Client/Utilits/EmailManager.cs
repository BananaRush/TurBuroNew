using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilits
{
    public class EmailManager
    {
        public EmailManager()
        {

        }

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

        public async static Task<bool> SendFile(string email, string filePath)
        {
            return await SendMail("smtp.yandex.ru", 587, "v4ne4ka1van0v@yandex.ru", "718160", "a.chirin@bm-technology.ru", "Фотография", "", filePath);
        }

        public async static Task<bool> SendMail(string smtpServer, int Port, string from, string password, string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));

                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = Port;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                await client.SendMailAsync(mail);
                mail.Dispose();
                return true;
            }
            catch(Exception el)
            {
                return false;
            }
        }
    }
}
