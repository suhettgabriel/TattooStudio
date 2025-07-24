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

            TempData["SuccessMessage"] = "Obrigado! Seus dados foram recebidos e você será informado(a) em primeira mão sobre a próxima agenda.";
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
                    new TattooRequestAnswer { FormFieldId = 6, Value = Input.HasTattooedBefore ? "Sim" : "Não" }
                }
            };

            await _requestRepo.CreateRequestAsync(newRequest);
        }
    }

    public class WaitingListInputModel
    {
        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "WhatsApp")]
        [Required(ErrorMessage = "O WhatsApp é obrigatório.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Estúdio de Interesse")]
        [Required(ErrorMessage = "O estúdio é obrigatório.")]
        public int StudioId { get; set; }

        [Display(Name = "Estado")]
        public string? State { get; set; }

        [Display(Name = "Cidade")]
        public string? City { get; set; }

        [Display(Name = "Não resido no Brasil")]
        public bool IsForeigner { get; set; }

        [Display(Name = "País / Cidade (se estrangeiro)")]
        public string? ForeignLocation { get; set; }

        [Display(Name = "Já tatuou comigo?")]
        public bool HasTattooedBefore { get; set; }

        [Display(Name = "O que gostaria de tatuar?")]
        [Required(ErrorMessage = "A descrição da ideia é obrigatória.")]
        public string TattooIdea { get; set; } = string.Empty;
    }
}