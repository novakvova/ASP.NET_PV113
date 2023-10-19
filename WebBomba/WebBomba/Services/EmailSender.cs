using MailKit.Net.Smtp;
using MimeKit;
using WebBomba.Helpers;
using WebBomba.Interfaces;
using WebBomba.Models;

namespace WebBomba.Services;

public class EmailSender : IEmailSender {
    private readonly EmailConfiguration _configuration;

    public EmailSender() {
        _configuration = new EmailConfiguration();
    }

    public async Task SendAsync(EmailViewModel message) {
        TextPart body = new("html") {
            Text = message.Body
        };
        Multipart multipart = new("mixed");
        multipart.Add(body);

        MimeMessage emailMessage = new() {
            Subject = message.Subject,
            Body = multipart
        };
        emailMessage.From.Add(new MailboxAddress(_configuration.From));
        emailMessage.To.Add(new MailboxAddress(message.To));

        using (SmtpClient client = new()) {
            try {
                await client.ConnectAsync(_configuration.SmtpServer, _configuration.Port, true);
                await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);
                await client.SendAsync(emailMessage);
            }
            finally {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}