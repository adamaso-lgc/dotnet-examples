using FieldUp.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FieldUp.Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
}

public class EmailService(IOptions<EmailOptions> options, ISecretProvider secrets) : IEmailService
{
    private readonly EmailOptions _options = options.Value;
    private readonly ISecretProvider _secrets = secrets ?? throw  new ArgumentNullException(nameof(secrets));

    public async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
    {
        var password = await _secrets.GetSecretAsync(_options.Smtp.SmtpPasswordSecretKey, ct);

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync(_options.Smtp.Host, _options.Smtp.Port, MailKit.Security.SecureSocketOptions.StartTls, ct);
        if (!string.IsNullOrEmpty(_options.Smtp.Username))
        {
            await client.AuthenticateAsync(_options.Smtp.Username, password ?? string.Empty, ct);
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("FieldUp",_options.From));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);
    }
}
