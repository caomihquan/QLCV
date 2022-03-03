using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Common
{
    public class MailHelper
    {
        public void SendMail(string toEmailAddress, string subject, string content, List<HttpPostedFileBase> file)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            string body = content;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
           
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            if (file[0]==null)
            {
                
                string fileName = Path.GetFileName("C:/Users/caomi/source/repos/QLCV/QLCV/Data/BangDiem_CaoMinhQuan.pdf");
                byte[] bytes = File.ReadAllBytes("C:/Users/caomi/source/repos/QLCV/QLCV/Data/BangDiem_CaoMinhQuan.pdf");
                message.Attachments.Add(new Attachment(new MemoryStream(bytes), fileName));
            }
            else
            {
                foreach (HttpPostedFileBase f in file)
                {
                    string files = Path.GetFileName(f.FileName);
                    message.Attachments.Add(new Attachment(f.InputStream, files));

                }
                /*string fileName = Path.GetFileName(file.FileName);
                message.Attachments.Add(new Attachment(file.InputStream, fileName));*/
            }
            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }
    }
}
