using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Options;
using SendWithBrevo;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptionsSnapshot<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.BrevoKey))
            {
                throw new Exception("Null BrevoKey");
            }
            await Execute(Options.BrevoKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new BrevoClient(apiKey);

            var to = new List<Recipient>() { new Recipient() };
            to[0].Email = toEmail;

            var sender = new Sender("PipeAndPuff", "pipeandpuff.sup@gmail.com");

            var isSuccess = await client.SendAsync(sender, to, subject, message);
            _logger.LogInformation(isSuccess
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
