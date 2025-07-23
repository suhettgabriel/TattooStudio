using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAsync();
        Task<bool> ExistsByTattooRequestIdAsync(int tattooRequestId);
    }
}