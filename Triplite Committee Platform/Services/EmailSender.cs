using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace Triplite_Committee_Platform.Services
{
    public class EmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; }

        public async Task SendEmailAsync(string toEmail, string templateId, object dynamicTemplateData)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            if (dynamicTemplateData == null)
            {
                   throw new Exception("Null dynamicTemplateData");
            } 
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new Exception("Null toEmail");
            }

            await Execute(Options.SendGridKey, templateId ,dynamicTemplateData, toEmail);
        }
        public async Task Execute(string apiKey,string templateId, object dynamicTemplateData, string toEmail)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("TripliteCommitteePlatform@outlook.com", "Triplite Committee Platform"),
            };
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(dynamicTemplateData);
            msg.AddTo(new EmailAddress(toEmail));

            msg.SetClickTracking(true, true);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}

//    var client = new SendGridClient(sendGridApiKey);
//            var msg = new SendGridMessage()
//            {
//                From = new EmailAddress("),
//                Subject = subject,
//                PlainTextContent = message,
//                HtmlContent = message
//            };
//            msg.AddTo(new EmailAddress(toEmail));

//            var response = await client.SendEmailAsync(msg);
//            if (response.IsSuccessStatusCode)
//            {
//                logger.LogInformation("Email queued successfully");
//            }
//            else
//            {
//                logger.LogError("Failed to send email");
//            }
//        }
//    }
//}
