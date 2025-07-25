using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> GetByTypeAsync(EmailTemplateType type);
        Task<IList<EmailTemplate>> GetAllAsync();
        Task UpdateAsync(EmailTemplate template);
    }
}