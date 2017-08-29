using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlogTemplate._1.Services;

namespace BlogTemplate.Tests.Fakes
{
    class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
