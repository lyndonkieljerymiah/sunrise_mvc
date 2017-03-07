using ClientNotification.cs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClientNotification.cs.Service
{
    public static class SendMail
    {

        public static void Send(MailBody mailBody)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(mailBody.To));
            message.Subject = mailBody.Subject;
            message.Body = mailBody.Body;

            using(var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
    }
}
