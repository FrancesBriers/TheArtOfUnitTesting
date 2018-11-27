using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace LogAn.UnitTests
{
    public class EmailInfo
    {
        public string Body;
        public string To;
        public string Subject;
    }
    public class FakeWebService : IWebService
    {
        public string LastError;
        public Exception ToThrow;
        public void LogError(string message)
        {
            LastError = message;
        }
    }
    public class FakeEmailService : IEmailService
    {
        public EmailInfo email = null;
        public void SendEmail (EmailInfo emailInfo)
        {
            email = emailInfo;
        }

        public void SendEmail(string to, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    class MockingUnitTests
    {
        [Test]
        public void Analyze_TooShortFileName_CallsWebService()
        {
            FakeWebService mockService = new FakeWebService();
            LogAnalyzer log = new LogAnalyzer(mockService);
            string tooShortFileName = "abc.ext";

            log.IsValidLogFileName(tooShortFileName);

            StringAssert.Contains("Filename too short: abc.ext", mockService.LastError);

        }

    }

    [TestFixture]
    public class LogAnalyzer2Tests
    {
        //Tests the Stub web service and Mock email sender
        [Test]
        public void Analyze_WebServiceThrows_SendsEmail()
        {
            FakeWebService stubService = new FakeWebService();
            stubService.ToThrow = new Exception("fake exception");

            FakeEmailService mockEmail = new FakeEmailService();
            LogAnalyzer2 log = new LogAnalyzer2(stubService, mockEmail);

            string tooShortFileName = "abc.ext";
            log.Analyze(tooShortFileName);

            EmailInfo expectedEmail = new EmailInfo
            {
                Body = "fake exception",
                To = "someone@somewhere.com",
                Subject = "can't log"
            };
            Assert.AreEqual(expectedEmail, mockEmail.email);
        }
    }

}

