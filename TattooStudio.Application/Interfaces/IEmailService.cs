using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendMagicLinkEmailAsync(User user, TattooRequest request);
    }
}