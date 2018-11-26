using System;

namespace LogAn
{
    public interface IWebService
    {
        void LogError(string message);
    }

    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }

    public class LogAnalyzer
    {
        private IWebService service;
        public bool WasLastFileNameValid { get; set; }

        public LogAnalyzer(IWebService service)
        {
            this.service = service;
        }

        public void Analyze(string fileName)
        {
            if (fileName.Length < 8)
            {
                service.LogError("Filename too short: " + fileName);
            }
        }
        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("filename has to be provided");
            }
            if (!fileName.EndsWith(".SLF",StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            WasLastFileNameValid = true;
            return true;
        }

    }

    public class LogAnalyzer2
    {
        private IWebService service;
        private IEmailService email;

        public LogAnalyzer2(IWebService service, IEmailService email)
        {
            Email = email;
            Service = service;
        }
        public IEmailService Email
        {
            get { return email; }
            set { email = value; }
        }

        public IWebService Service
        {
            get { return service; }
            set { service = value; }          
        }
        public void Analyze(string fileName)
        {
            if(fileName.Length<8)
            {
                try
                {
                    Service.LogError("Filename too short: " + fileName);
                }
                catch (Exception e)
                {
                    email.SendEmail("someone@somewhere.com", "cannot log", e.Message);
                }
            }
        }
    }
}
