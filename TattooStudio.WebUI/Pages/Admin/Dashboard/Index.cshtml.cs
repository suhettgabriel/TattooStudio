using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using TattooStudio.Application.Interfaces;
using TattooStudio.WebUI.Helpers;
using TattooStudio.Core.Enums;

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
            CurrentMonthName = today.ToString("MMMM", new CultureInfo("pt-BR"));

            NewRequestsCount = await _dashboardRepo.GetNewRequestsCountAsync();
            PendingQuotesCount = await _dashboardRepo.GetPendingQuotesCountAsync();
            ConfirmedAppointmentsCount = await _dashboardRepo.GetConfirmedAppointmentsCountAsync(today.Month, today.Year);
            WaitingListCount = await _dashboardRepo.GetWaitingListCountAsync();
        }

        public async Task<JsonResult> OnGetChartData()
        {
            var year = DateTime.Today.Year;
            var culture = new CultureInfo("pt-BR");
            var requestsByMonth = await _dashboardRepo.GetRequestsByMonthAsync(year);
            var revenueByMonth = await _dashboardRepo.GetRevenueByMonthAsync(year);

            var labels = Enumerable.Range(1, 12).Select(i => new DateTime(year, i, 1).ToString("MMMM", culture)).ToList();

            var requestData = labels.Select(label => requestsByMonth.ContainsKey(label) ? requestsByMonth[label] : 0).ToList();
            var revenueData = labels.Select(label => revenueByMonth.ContainsKey(label) ? revenueByMonth[label] : 0).ToList();

            return new JsonResult(new { labels, requestData, revenueData });
        }

        public async Task<JsonResult> OnGetStatusChartData()
        {
            var statusCounts = await _dashboardRepo.GetRequestsByStatusCountAsync();

            var labels = statusCounts.Keys.Select(status => EnumHelpers.GetDisplayName(status)).ToList();
            var counts = statusCounts.Values.ToList();
            var colors = statusCounts.Keys.Select(status => EnumHelpers.GetStatusColor(status)).ToList();

            return new JsonResult(new { labels, counts, colors });
        }
    }
}