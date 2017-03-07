using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientNotification.cs.Model;

namespace MailNotificationTest
{
    [TestClass]
    public class MailTesting
    {
        [TestMethod]
        public void SendTest()
        {
            var mail = new MailBody();

            mail.From = "nabco@it.com";

            mail.To = "";


        }
    }
}
