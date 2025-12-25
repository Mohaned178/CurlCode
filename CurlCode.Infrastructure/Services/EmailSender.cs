using System.Net;
using System.Net.Mail;
using CurlCode.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace CurlCode.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var emailSection = _configuration.GetSection("Email");
        var host = emailSection["Host"];
        var port = emailSection.GetValue<int>("Port");
        var username = emailSection["Username"];
        var password = emailSection["Password"];
        var from = emailSection["From"];
        var useSsl = emailSection.GetValue<bool>("UseSsl");

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from))
        {
            throw new InvalidOperationException("Email settings are not configured properly.");
        }

        using var message = new MailMessage
        {
            From = new MailAddress(from),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = useSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };

        if (!string.IsNullOrWhiteSpace(username))
        {
            client.Credentials = new NetworkCredential(username, password);
        }

        await client.SendMailAsync(message);
    }
}


