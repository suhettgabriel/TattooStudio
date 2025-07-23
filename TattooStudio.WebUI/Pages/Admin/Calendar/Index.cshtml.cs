using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TattooStudio.Infrastructure.Data;

namespace TattooStudio.WebUI.Pages.Admin.Calendar
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<JsonResult> OnGetAppointments(DateTime start, DateTime end)
        {
            var appointments = await _context.Appointments
                .Where(e => e.End > start && e.Start < end)
                .Select(e => new
                {
                    id = e.Id,
                    title = e.Title,
                    start = e.Start.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = e.End.ToString("yyyy-MM-ddTHH:mm:ss"),
                    url = $"/Admin/Requests/Details/{e.TattooRequestId}",
                    backgroundColor = "#198754",
                    borderColor = "#198754"
                })
                .ToListAsync();

            return new JsonResult(appointments);
        }
    }
}