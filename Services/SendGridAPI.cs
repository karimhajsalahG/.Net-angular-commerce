using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Services
{
    public static class SendGridAPI
    {
        public static async Task<bool> Execute(string userEmail, string userName, string plainTextContent,
            string htmlContent, string subject)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("testangular123456789@gmail.com", "karimhajsalah");
            var to = new EmailAddress(userEmail, userName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return await Task.FromResult(true);
        }
    }
}