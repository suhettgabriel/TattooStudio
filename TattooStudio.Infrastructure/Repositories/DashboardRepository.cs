using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TattooStudio.Application.Interfaces;
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

        public async Task<int> GetNewRequestsCountAsync()
        {
            return await _context.TattooRequests
                .CountAsync(r => r.Status == RequestStatus.NovaSolicitacao);
        }

        public async Task<int> GetPendingQuotesCountAsync()
        {
            return await _context.TattooRequests
               .CountAsync(r => r.Status == RequestStatus.EmAnalise || r.Status == RequestStatus.OrcamentoEnviado);
        }

        public async Task<int> GetConfirmedAppointmentsCountAsync(int month, int year)
        {
            return await _context.Appointments
                .CountAsync(a => a.Start.Month == month && a.Start.Year == year);
        }

        public async Task<int> GetWaitingListCountAsync()
        {
            return await _context.TattooRequests
                .CountAsync(r => r.Status == RequestStatus.ListaDeEspera);
        }

        public async Task<Dictionary<string, int>> GetRequestsByMonthAsync(int year)
        {
            return await _context.TattooRequests
                .Where(r => r.SubmissionDate.Year == year)
                .GroupBy(r => r.SubmissionDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(x => x.Month)
                .ToDictionaryAsync(
                    x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month),
                    x => x.Count
                );
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByMonthAsync(int year)
        {
            return await _context.Quotes
               .Where(q => q.Status == Core.Entities.QuoteStatus.Aprovado && q.CreatedAt.Year == year)
               .GroupBy(q => q.CreatedAt.Month)
               .Select(g => new { Month = g.Key, Total = g.Sum(q => q.Amount) })
               .OrderBy(x => x.Month)
               .ToDictionaryAsync(
                   x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month),
                   x => x.Total
               );
        }
    }
}