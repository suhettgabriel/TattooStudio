using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IWebHostEnvironment _env;

        public EmailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SendMagicLinkEmailAsync(User user, TattooRequest request)
        {
            var portalLink = $"https://localhost:7145/Portal/Access?token={request.MagicLinkToken}"; 

            var subject = "Seu link de acesso ao Portal do Cliente";
            var body = $@"
                <h1>Olá {user.FullName},</h1>
                <p>Recebemos uma solicitação de acesso ao seu portal de agendamentos. Use o link abaixo para entrar. Este link é válido por 15 minutos e pode ser usado apenas uma vez.</p>
                <p><a href='{portalLink}'>Acessar meu Portal</a></p>
                <p>Se você não solicitou este acesso, pode ignorar este e-mail.</p>
                <p>Atenciosamente,<br>Tattoo Studio</p>";

            await SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var pickupPath = Path.Combine(_env.ContentRootPath, "..", "EmailPickup");
            if (!Directory.Exists(pickupPath))
            {
                Directory.CreateDirectory(pickupPath);
            }

            using (var client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = pickupPath;

                var message = new MailMessage("nao-responda@tattoostudio.com", toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
            }
        }
    }
}