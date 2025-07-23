using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public SolicitarAgendamentoModel(
            IFormFieldRepository formFieldRepo,
            ITattooRequestRepository requestRepo,
            IFileStorageService fileStorage)
        {
            _formFieldRepo = formFieldRepo;
            _requestRepo = requestRepo;
            _fileStorage = fileStorage;
        }

        [BindProperty]
        public ClientInfoInputModel ClientInfo { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> Answers { get; set; } = new();

        [BindProperty]
        public Dictionary<int, IFormFile> FileAnswers { get; set; } = new();

        public IList<FormField> FormFields { get; set; } = new List<FormField>();

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadFormFieldsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadFormFieldsAsync();
                return Page();
            }

            await SaveDynamicRequestAsync();

            TempData["SuccessMessage"] = "Sua solicitação foi enviada com sucesso! Entraremos em contato em breve.";
            return RedirectToPage("./Index");
        }

        private async Task LoadFormFieldsAsync()
        {
            FormFields = await _formFieldRepo.GetAllAsync();
            if (!FormFields.Any())
            {
                ModelState.AddModelError(string.Empty, "O formulário de agendamento ainda não foi configurado.");
            }
        }

        private async Task SaveDynamicRequestAsync()
        {
            var newUser = new User
            {
                FullName = ClientInfo.FullName,
                Email = ClientInfo.Email,
                PhoneNumber = ClientInfo.PhoneNumber,
                InstagramHandle = ClientInfo.InstagramHandle
            };

            var newRequest = new TattooRequest { User = newUser };

            foreach (var answer in Answers)
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

            foreach (var fileAnswer in FileAnswers)
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

    public class ClientInfoInputModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O WhatsApp é obrigatório.")]
        public string PhoneNumber { get; set; } = string.Empty;
        public string? InstagramHandle { get; set; }
    }
}