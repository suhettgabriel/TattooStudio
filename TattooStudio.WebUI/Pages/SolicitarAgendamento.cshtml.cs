using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TattooStudio.Application.Interfaces;
using TattooStudio.Core.Entities;

namespace TattooStudio.WebUI.Pages
{
    public class SolicitarAgendamentoModel : PageModel
    {
        private readonly IFormFieldRepository _formFieldRepo;
        private readonly ITattooRequestRepository _requestRepo;
        private readonly IFileStorageService _fileStorage;
        private readonly IStudioRepository _studioRepo;

        public SolicitarAgendamentoModel(
            IFormFieldRepository formFieldRepo,
            ITattooRequestRepository requestRepo,
            IFileStorageService fileStorage,
            IStudioRepository studioRepo)
        {
            _formFieldRepo = formFieldRepo;
            _requestRepo = requestRepo;
            _fileStorage = fileStorage;
            _studioRepo = studioRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public IList<FormField> FormFields { get; set; } = new List<FormField>();
        public SelectList StudioOptions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadInitialDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadInitialDataAsync();
                return Page();
            }

            await SaveDynamicRequestAsync();

            TempData["SuccessMessage"] = "Sua solicitação foi enviada com sucesso! Entraremos em contato em breve.";
            return RedirectToPage("./Index");
        }

        private async Task LoadInitialDataAsync()
        {
            FormFields = await _formFieldRepo.GetAllAsync();
            if (!FormFields.Any())
            {
                ModelState.AddModelError(string.Empty, "O formulário de agendamento ainda não foi configurado pela administradora.");
            }

            var studios = await _studioRepo.GetAllAsync();
            StudioOptions = new SelectList(studios, nameof(Studio.Id), nameof(Studio.City));
        }

        private async Task SaveDynamicRequestAsync()
        {
            var newUser = new User
            {
                FullName = Input.ClientInfo.FullName,
                Email = Input.ClientInfo.Email,
                PhoneNumber = Input.ClientInfo.PhoneNumber,
                InstagramHandle = Input.ClientInfo.InstagramHandle
            };

            var newRequest = new TattooRequest
            {
                User = newUser,
                StudioId = Input.ClientInfo.StudioId
            };

            foreach (var answer in Input.Answers)
            {
                if (!string.IsNullOrEmpty(answer.Value))
                {
                    newRequest.Answers.Add(new TattooRequestAnswer
                    {
                        FormFieldId = answer.Key,
                        Value = answer.Value
                    });
                }
            }

            foreach (var fileAnswer in Input.FileAnswers)
            {
                if (fileAnswer.Value != null && fileAnswer.Value.Length > 0)
                {
                    var fileUrl = await _fileStorage.SaveFileAsync(fileAnswer.Value, "request_files");
                    newRequest.Answers.Add(new TattooRequestAnswer
                    {
                        FormFieldId = fileAnswer.Key,
                        Value = fileUrl
                    });
                }
            }

            await _requestRepo.CreateRequestAsync(newRequest);
        }
    }

    public class InputModel
    {
        public ClientInfoInputModel ClientInfo { get; set; } = new();
        public Dictionary<int, string> Answers { get; set; } = new();
        public Dictionary<int, IFormFile> FileAnswers { get; set; } = new();
    }

    public class ClientInfoInputModel
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

        [Display(Name = "Instagram (opcional)")]
        public string? InstagramHandle { get; set; }

        [Display(Name = "Local de Atendimento")]
        [Required(ErrorMessage = "Por favor, escolha um estúdio.")]
        public int StudioId { get; set; }
    }
}