using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class LoginModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IEmailService _emailService;

        public LoginModel(ITattooRequestRepository requestRepo, IEmailService emailService)
        {
            _requestRepo = requestRepo;
            _emailService = emailService;
        }

        [BindProperty, Required(ErrorMessage = "O campo E-mail é obrigatório."), EmailAddress]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var requests = await _requestRepo.GetAllRequestsAsync(Email, null, null, null);
            var latestRequest = requests.OrderByDescending(r => r.SubmissionDate).FirstOrDefault();

            if (latestRequest?.User != null)
            {
                latestRequest.MagicLinkToken = Guid.NewGuid().ToString("N");
                latestRequest.MagicLinkTokenExpiration = DateTime.UtcNow.AddMinutes(15);
                await _requestRepo.UpdateAsync(latestRequest);

                await _emailService.SendMagicLinkEmailAsync(latestRequest.User, latestRequest);
            }

            TempData["SuccessMessage"] = "Se um conta com este e-mail for encontrada, um link de acesso será enviado para sua caixa de entrada.";
            return RedirectToPage("/Index");
        }
    }
}