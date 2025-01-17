using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Application.Interfaces;
using VrijwilligersWerkApp.Helpers;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Infrastructure.Interfaces;
using Application.ViewModels;


namespace VrijwilligersWerkApp.Pages.RegistreerWerk
{
    public class VrijwilligersWerkOverzichtModel : PageModel
    {
        private readonly IVrijwilligersWerkService werkService;
        private readonly ILogger<VrijwilligersWerkOverzichtModel> logger;

        public List<VrijwilligersWerkViewModel> WerkAanbiedingen { get; set; }
        public string FeedbackMessage { get; set; }
        public string SuccesMessage { get; set; }

        public VrijwilligersWerkOverzichtModel(
            IVrijwilligersWerkService werkService,
            ILogger<VrijwilligersWerkOverzichtModel> logger)
        {
            this.werkService = werkService;
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            try
            {
                WerkAanbiedingen = werkService.HaalAlleWerkenOp();

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

                if (werkService.RegistreerVoorWerk(id, userId.Value))
                {
                    TempData["SuccesMessage"] = "Je bent succesvol geregistreerd voor dit vrijwilligerswerk!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Registratie mislukt. Mogelijk is het werk vol of ben je al geregistreerd.";
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
