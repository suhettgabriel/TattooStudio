using Microsoft.EntityFrameworkCore;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.AsNoTracking().ToListAsync();
        }

        public async Task<bool> ExistsByTattooRequestIdAsync(int tattooRequestId)
        {
            return await _context.Appointments.AnyAsync(a => a.TattooRequestId == tattooRequestId);
        }
    }
}