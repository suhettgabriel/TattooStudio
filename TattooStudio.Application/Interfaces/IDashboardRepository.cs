using TattooStudio.Core.Entities;

namespace TattooStudio.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetNewRequestsCountAsync();
        Task<int> GetPendingQuotesCountAsync();
        Task<int> GetConfirmedAppointmentsCountAsync(int month, int year);
        Task<int> GetWaitingListCountAsync();
        Task<Dictionary<string, int>> GetRequestsByMonthAsync(int year);
        Task<Dictionary<string, decimal>> GetRevenueByMonthAsync(int year);
    }
}