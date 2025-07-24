using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
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
        private readonly ISystemSettingRepository _settingsRepo;
        private readonly IPricingRuleRepository _pricingRepo;

        public SolicitarAgendamentoModel(
            IFormFieldRepository formFieldRepo,
            ITattooRequestRepository requestRepo,
            IFileStorageService fileStorage,
            IStudioRepository studioRepo,
            ISystemSettingRepository settingsRepo,
            IPricingRuleRepository pricingRepo)
        {
            _formFieldRepo = formFieldRepo;
            _requestRepo = requestRepo;
            _fileStorage = fileStorage;
            _studioRepo = studioRepo;
            _settingsRepo = settingsRepo;
            _pricingRepo = pricingRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public bool IsAgendaOpen { get; set; }
        public IList<FormField> FormFields { get; set; } = new List<FormField>();
        public SelectList StudioOptions { get; set; }
        public string PricingRulesJson { get; set; }

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

            await SaveRequestAsync();
            TempData["SuccessMessage"] = "Sua solicitação foi enviada com sucesso! Entraremos em contato em breve.";
            return RedirectToPage("./Index");
        }

        private async Task LoadInitialDataAsync()
        {
            var settings = await _settingsRepo.GetSettingsAsync();
            IsAgendaOpen = settings.IsAgendaOpen;
            FormFields = await _formFieldRepo.GetAllAsync();
            var studios = await _studioRepo.GetAllAsync();
            StudioOptions = new SelectList(studios, nameof(Studio.Id), nameof(Studio.City));
            var pricingRules = await _pricingRepo.GetAllAsync();
            PricingRulesJson = JsonSerializer.Serialize(pricingRules);
        }

        private async Task SaveRequestAsync()
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
                StudioId = Input.ClientInfo.StudioId,
                EstimatedSize = Input.EstimatedSize,
                InitialEstimate = Input.InitialEstimate
            };

            var ideaField = await GetOrCreateUploadField("Envie fotos da sua ideia");
            var bodyField = await GetOrCreateUploadField("Envie uma foto do local do corpo");

            if (Input.ReferenceImages != null && Input.ReferenceImages.Any())
            {
                foreach (var file in Input.ReferenceImages)
                {
                    var fileUrl = await _fileStorage.SaveFileAsync(file, "request_files");
                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        newRequest.Answers.Add(new TattooRequestAnswer { FormFieldId = ideaField.Id, Value = fileUrl });
                    }
                }
            }

            if (Input.BodyPartPhoto != null)
            {
                var fileUrl = await _fileStorage.SaveFileAsync(Input.BodyPartPhoto, "request_files");
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    newRequest.Answers.Add(new TattooRequestAnswer { FormFieldId = bodyField.Id, Value = fileUrl });
                }
            }

            foreach (var answer in Input.Answers)
            {
                if (!string.IsNullOrEmpty(answer.Value))
                {
                    newRequest.Answers.Add(new TattooRequestAnswer { FormFieldId = answer.Key, Value = answer.Value });
                }
            }

            await _requestRepo.CreateRequestAsync(newRequest);
        }

        private async Task<FormField> GetOrCreateUploadField(string label)
        {
            var allFields = await _formFieldRepo.GetAllAsync();
            var field = allFields.FirstOrDefault(f => f.Label == label && f.FieldType == FormFieldType.UploadArquivo);

            if (field == null)
            {
                field = new FormField
                {
                    Label = label,
                    FieldType = FormFieldType.UploadArquivo,
                    IsRequired = false,
                    Order = 999
                };
                await _formFieldRepo.AddAsync(field);
            }
            return field;
        }
    }

    public class InputModel
    {
        public ClientInfoInputModel ClientInfo { get; set; } = new();
        public Dictionary<int, string> Answers { get; set; } = new();
        public int? EstimatedSize { get; set; }
        public decimal? InitialEstimate { get; set; }

        [Display(Name = "Envie fotos da sua ideia (até 5 imagens)")]
        public List<IFormFile>? ReferenceImages { get; set; }

        [Display(Name = "Envie uma foto do local do corpo")]
        public IFormFile? BodyPartPhoto { get; set; }
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
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, escolha um estúdio válido.")]
        public int StudioId { get; set; }
    }
}