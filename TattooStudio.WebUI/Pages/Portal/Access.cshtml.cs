using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TattooStudio.Application.Interfaces;

namespace TattooStudio.WebUI.Pages.Portal
{
    public class AccessModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;

        public AccessModel(ITattooRequestRepository requestRepo)
        {
            _requestRepo = requestRepo;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Token de acesso inválido ou ausente.";
                return RedirectToPage("/Portal/Login");
            }

            var request = (await _requestRepo.GetAllRequestsAsync(null, null, null, null))
                .FirstOrDefault(r => r.MagicLinkToken == token);

            if (request?.User == null || request.MagicLinkTokenExpiration < DateTime.UtcNow)
            {
                TempData["ErrorMessage"] = "Token de acesso inválido ou expirado. Por favor, solicite um novo.";
                return RedirectToPage("/Portal/Login");
            }

            request.MagicLinkToken = null;
            request.MagicLinkTokenExpiration = null;
            await _requestRepo.UpdateAsync(request);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.User.FullName),
                new(ClaimTypes.Email, request.User.Email),
                new(ClaimTypes.NameIdentifier, request.User.Id.ToString()),
                new("TattooRequestId", request.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/Portal/Dashboard");
        }
    }
}