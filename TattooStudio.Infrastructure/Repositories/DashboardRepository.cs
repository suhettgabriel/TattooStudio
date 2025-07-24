using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> GetNewRequestsCountAsync()
        {
            return _context.TattooRequests.CountAsync(r => r.Status == RequestStatus.NovaSolicitacao);
        }

        public Task<int> GetPendingQuotesCountAsync()
        {
            return _context.TattooRequests.CountAsync(r => r.Status == RequestStatus.EmAnalise || r.Status == RequestStatus.OrcamentoEnviado);
        }

        public Task<int> GetConfirmedAppointmentsCountAsync(int month, int year)
        {
            return _context.Appointments.CountAsync(a => a.Start.Month == month && a.Start.Year == year);
        }

        public Task<int> GetWaitingListCountAsync()
        {
            return _context.TattooRequests.CountAsync(r => r.Status == RequestStatus.ListaDeEspera);
        }

        public async Task<Dictionary<string, int>> GetRequestsByMonthAsync(int year)
        {
            var culture = new CultureInfo("pt-BR");
            var result = await _context.TattooRequests
                .Where(r => r.SubmissionDate.Year == year)
                .GroupBy(r => r.SubmissionDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(x => x.Month)
                .ToListAsync();

            return result.ToDictionary(
                x => new DateTime(year, x.Month, 1).ToString("MMMM", culture),
                x => x.Count
            );
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByMonthAsync(int year)
        {
            var culture = new CultureInfo("pt-BR");
            var result = await _context.Quotes
                .Where(q => q.Status == QuoteStatus.Aprovado && q.CreatedAt.Year == year)
                .GroupBy(q => q.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(q => q.Amount) })
                .OrderBy(x => x.Month)
                .ToListAsync();

            return result.ToDictionary(
                x => new DateTime(year, x.Month, 1).ToString("MMMM", culture),
                x => x.Total
            );
        }

        public async Task<Dictionary<RequestStatus, int>> GetRequestsByStatusCountAsync()
        {
            return await _context.TattooRequests
                .GroupBy(r => r.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count);
        }
    }
}