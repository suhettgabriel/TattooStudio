using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly AppDbContext _context;

        public EmailTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IList<EmailTemplate>> GetAllAsync()
        {
            var types = Enum.GetValues<EmailTemplateType>();
            foreach (var type in types)
            {
                await GetByTypeAsync(type);
            }
            return await _context.EmailTemplates.ToListAsync();
        }

        public async Task<EmailTemplate> GetByTypeAsync(EmailTemplateType type)
        {
            var template = await _context.EmailTemplates.FirstOrDefaultAsync(t => t.Type == type);
            if (template == null)
            {
                template = CreateDefaultTemplate(type);
                await _context.EmailTemplates.AddAsync(template);
                await _context.SaveChangesAsync();
            }
            return template;
        }

        public async Task UpdateAsync(EmailTemplate template)
        {
            _context.EmailTemplates.Update(template);
            await _context.SaveChangesAsync();
        }

        private EmailTemplate CreateDefaultTemplate(EmailTemplateType type)
        {
            switch (type)
            {
                case EmailTemplateType.RequestConfirmation:
                    return new EmailTemplate
                    {
                        Type = type,
                        Subject = "Sua solicitação de agendamento foi recebida!",
                        Body = "<p>Olá {{ClientName}},</p><p>Recebemos sua solicitação de agendamento e estamos muito felizes com o seu interesse! Em breve, sua ideia será analisada e entraremos em contato com os próximos passos.</p><p>Atenciosamente,</p><p>Tirza Setta</p>",
                        Placeholders = "{{ClientName}}"
                    };
                case EmailTemplateType.QuoteAvailable:
                    return new EmailTemplate
                    {
                        Type = type,
                        Subject = "Seu orçamento de tatuagem está pronto!",
                        Body = "<p>Olá {{ClientName}},</p><p>Boas notícias! Analisei sua ideia e o orçamento para sua nova tatuagem já está disponível no seu portal. Acesse o link abaixo para ver os detalhes e dar o próximo passo.</p><p>Link do Portal: {{PortalLink}}</p>",
                        Placeholders = "{{ClientName}}, {{PortalLink}}"
                    };
                case EmailTemplateType.BookingConfirmed:
                    return new EmailTemplate
                    {
                        Type = type,
                        Subject = "Agendamento Confirmado!",
                        Body = "<p>Olá {{ClientName}},</p><p>Seu agendamento foi confirmado com sucesso! Sua sessão está marcada para o dia {{BookingDate}} às {{BookingTime}}.</p><p>Endereço: {{StudioAddress}}</p><p>Mal posso esperar para criarmos sua arte!</p>",
                        Placeholders = "{{ClientName}}, {{BookingDate}}, {{BookingTime}}, {{StudioAddress}}"
                    };
                case EmailTemplateType.BookingReminder:
                    return new EmailTemplate
                    {
                        Type = type,
                        Subject = "Lembrete do seu agendamento de tatuagem",
                        Body = "<p>Olá {{ClientName}},</p><p>Este é um lembrete amigável de que sua sessão de tatuagem está chegando! Esperamos por você no dia {{BookingDate}} às {{BookingTime}}.</p><p>Por favor, revise nossas orientações de cuidados pré-tatuagem em seu portal.</p>",
                        Placeholders = "{{ClientName}}, {{BookingDate}}, {{BookingTime}}"
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Tipo de template desconhecido.");
            }
        }
    }
}