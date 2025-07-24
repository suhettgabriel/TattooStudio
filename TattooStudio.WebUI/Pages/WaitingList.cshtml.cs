using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;
using TattooStudio.Core.Enums;

namespace TattooStudio.WebUI.Pages
{
    public class WaitingListModel : PageModel
    {
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IStudioRepository _studioRepo;

        [BindProperty]
        public WaitingListInputModel Input { get; set; } = new();

        public SelectList StudioOptions { get; set; }

        public WaitingListModel(ITattooRequestRepository requestRepo, IStudioRepository studioRepo)
        {
            _requestRepo = requestRepo;
            _studioRepo = studioRepo;
        }

        public async Task OnGetAsync()
        {
            await LoadInitialDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadInitialDataAsync();
                return Page();
            }

            await CreateTattooRequestFromWaitingList();

            TempData["SuccessMessage"] = "Obrigado! Seus dados foram recebidos e voc� ser� informado(a) em primeira m�o sobre a pr�xima agenda.";
            return RedirectToPage("/Index");
        }

        private async Task LoadInitialDataAsync()
        {
            var studios = await _studioRepo.GetAllAsync();
            StudioOptions = new SelectList(studios, nameof(Studio.Id), nameof(Studio.City));
        }

        private async Task CreateTattooRequestFromWaitingList()
        {
            var newUser = new User
            {
                FullName = Input.FullName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber
            };

            var location = Input.IsForeigner ? Input.ForeignLocation : $"{Input.City}, {Input.State}";

            var newRequest = new TattooRequest
            {
                User = newUser,
                StudioId = Input.StudioId,
                Status = RequestStatus.ListaDeEspera,
                Answers = new List<TattooRequestAnswer>
                {
                    new TattooRequestAnswer { FormFieldId = 1, Value = Input.FullName },
                    new TattooRequestAnswer { FormFieldId = 2, Value = Input.Email },
                    new TattooRequestAnswer { FormFieldId = 3, Value = Input.PhoneNumber },
                    new TattooRequestAnswer { FormFieldId = 4, Value = location },
                    new TattooRequestAnswer { FormFieldId = 5, Value = Input.TattooIdea },
                    new TattooRequestAnswer { FormFieldId = 6, Value = Input.HasTattooedBefore ? "Sim" : "N�o" }
                }
            };

            await _requestRepo.CreateRequestAsync(newRequest);
        }
    }

    public class WaitingListInputModel
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O nome � obrigat�rio.")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail � obrigat�rio.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "WhatsApp")]
        [Required(ErrorMessage = "O WhatsApp � obrigat�rio.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Est�dio de Interesse")]
        [Required(ErrorMessage = "O est�dio � obrigat�rio.")]
        public int StudioId { get; set; }

        [Display(Name = "Estado")]
        public string? State { get; set; }

        [Display(Name = "Cidade")]
        public string? City { get; set; }

        [Display(Name = "N�o resido no Brasil")]
        public bool IsForeigner { get; set; }

        [Display(Name = "Pa�s / Cidade (se estrangeiro)")]
        public string? ForeignLocation { get; set; }

        [Display(Name = "J� tatuou comigo?")]
        public bool HasTattooedBefore { get; set; }

        [Display(Name = "O que gostaria de tatuar?")]
        [Required(ErrorMessage = "A descri��o da ideia � obrigat�ria.")]
        public string TattooIdea { get; set; } = string.Empty;
    }
}