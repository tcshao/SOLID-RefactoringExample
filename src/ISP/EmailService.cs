using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ISP
{
    public class EmailService
    {
        public string SmtpServer { get; set; }

        public EmailService(string server)
        {
            SmtpServer = server;
        }

        public void Send(string toAddress, string fromAddress, string body)
        {
            var emailMessage = new MailMessage(fromAddress, toAddress)
            {
                Body = body
            };

            new SmtpClient(SmtpServer).Send(emailMessage);
        }
    }
}
