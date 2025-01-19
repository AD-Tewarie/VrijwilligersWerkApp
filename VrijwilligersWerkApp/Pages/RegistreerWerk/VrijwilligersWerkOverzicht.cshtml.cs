using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VrijwilligersWerkApp.Pages.RegistreerWerk
{
    public class VrijwilligersWerkOverzichtModel : PageModel
    {
        private readonly IWerkBeheerService werkBeheerService;
        private readonly IWerkRegistratieBeheerService werkRegistratieBeheerService;
        private readonly ILogger<VrijwilligersWerkOverzichtModel> logger;

        public List<WerkAanbiedingOverzichtViewModel> WerkAanbiedingen { get; set; }
        public string FeedbackMessage { get; set; }
        public string SuccesMessage { get; set; }

        public VrijwilligersWerkOverzichtModel(
            IWerkBeheerService werkBeheerService,
            IWerkRegistratieBeheerService werkRegistratieBeheerService,
            ILogger<VrijwilligersWerkOverzichtModel> logger)
        {
            this.werkBeheerService = werkBeheerService ?? throw new ArgumentNullException(nameof(werkBeheerService));
            this.werkRegistratieBeheerService = werkRegistratieBeheerService ?? throw new ArgumentNullException(nameof(werkRegistratieBeheerService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult OnGet()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    TempData["ErrorMessage"] = "Je moet eerst inloggen.";
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                WerkAanbiedingen = werkBeheerService.HaalBeschikbareWerkAanbiedingenOp();

                if (TempData["SuccesMessage"] != null)
                {
                    SuccesMessage = TempData["SuccesMessage"].ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    FeedbackMessage = TempData["ErrorMessage"].ToString();
                }

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij ophalen vrijwilligerswerk");
                FeedbackMessage = "Er is een fout opgetreden bij het laden van het overzicht.";
                return Page();
            }
        }

        public IActionResult OnPostApply(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    TempData["ErrorMessage"] = "Je moet eerst inloggen.";
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "Ongeldig werk ID.";
                    return RedirectToPage();
                }

                var result = werkRegistratieBeheerService.RegistreerVoorWerk(userId.Value, id);
                if (result.IsSuccesvol)
                {
                    TempData["SuccesMessage"] = result.Melding ?? "Succesvol geregistreerd voor het werk.";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Melding ?? "Er is een fout opgetreden bij de registratie.";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij registreren voor werk {WerkId}", id);
                TempData["ErrorMessage"] = "Er is een fout opgetreden bij de registratie.";
                return RedirectToPage();
            }
        }
    }
}
