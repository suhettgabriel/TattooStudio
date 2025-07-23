using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface ISystemSettingRepository
    {
        Task<SystemSetting> GetSettingsAsync();
        Task UpdateSettingsAsync(SystemSetting settings);
    }
}