using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using TattooStudio.Application.Interfaces;

namespace TattooStudio.WebUI.Pages.Admin.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardRepository _dashboardRepo;

        public IndexModel(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        public int NewRequestsCount { get; set; }
        public int PendingQuotesCount { get; set; }
        public int ConfirmedAppointmentsCount { get; set; }
        public int WaitingListCount { get; set; }
        public string CurrentMonthName { get; set; }

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;
            CurrentMonthName = today.ToString("MMMM");

            NewRequestsCount = await _dashboardRepo.GetNewRequestsCountAsync();
            PendingQuotesCount = await _dashboardRepo.GetPendingQuotesCountAsync();
            ConfirmedAppointmentsCount = await _dashboardRepo.GetConfirmedAppointmentsCountAsync(today.Month, today.Year);
            WaitingListCount = await _dashboardRepo.GetWaitingListCountAsync();
        }

        public async Task<JsonResult> OnGetChartData()
        {
            var year = DateTime.Today.Year;
            var requestsByMonth = await _dashboardRepo.GetRequestsByMonthAsync(year);
            var revenueByMonth = await _dashboardRepo.GetRevenueByMonthAsync(year);

            var labels = Enumerable.Range(1, 12).Select(i => new DateTime(year, i, 1).ToString("MMM", new CultureInfo("pt-BR")));

            var requestData = labels.Select(l => requestsByMonth.ContainsKey(l) ? requestsByMonth[l] : 0).ToList();
            var revenueData = labels.Select(l => revenueByMonth.ContainsKey(l) ? revenueByMonth[l] : 0).ToList();

            return new JsonResult(new { labels, requestData, revenueData });
        }
    }
}