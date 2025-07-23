using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages.Admin.Settings
{
    public class IndexModel : PageModel
    {
        private readonly ISystemSettingRepository _settingsRepo;

        public IndexModel(ISystemSettingRepository settingsRepo)
        {
            _settingsRepo = settingsRepo;
        }

        [BindProperty]
        public SystemSetting Settings { get; set; }

        public async Task OnGetAsync()
        {
            Settings = await _settingsRepo.GetSettingsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _settingsRepo.UpdateSettingsAsync(Settings);
            TempData["SuccessMessage"] = "Configurações salvas com sucesso!";
            return RedirectToPage();
        }
    }
}