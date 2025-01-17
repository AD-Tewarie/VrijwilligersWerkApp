using Application.Interfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestResultaatModel : PageModel
    {
        private readonly IGebruikersTestResultaatService resultaatService;
        private readonly IVrijwilligersWerkService werkService; 
        private readonly ILogger<GebruikersTestResultaatModel> logger;

        public GebruikersTestResultaatViewModel ResultaatModel { get; private set; }
        public string SuccesMessage { get; private set; }
        public string ErrorMessage { get; private set; }

        public GebruikersTestResultaatModel(
            IGebruikersTestResultaatService resultaatService,
            IVrijwilligersWerkService werkService, 
            ILogger<GebruikersTestResultaatModel> logger)
        {
            this.resultaatService = resultaatService;
            this.werkService = werkService;
            this.logger = logger;
        }


        public IActionResult OnGet(string presentatieType = "top")
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                ResultaatModel = resultaatService.HaalResultaatOp(userId.Value, presentatieType);

                if (ResultaatModel == null)
                {
                    TempData["ErrorMessage"] = "Je moet eerst de test voltooien.";
                    return RedirectToPage("/GebruikersTest/GebruikersTest");
                }

                // Haal eventuele feedback berichten op van TempData
                if (TempData["SuccesMessage"] != null)
                {
                    SuccesMessage = TempData["SuccesMessage"].ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van testresultaten");
                ErrorMessage = "Er is een fout opgetreden bij het laden van de resultaten.";
                return Page();
            }
        }

        public IActionResult OnPostApplyForJob(int werkId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                bool success = werkService.RegistreerVoorWerk(werkId, userId.Value); // Gebruik werkService
                if (success)
                {
                    TempData["SuccesMessage"] = "Je bent succesvol geregistreerd voor dit vrijwilligerswerk!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Registratie mislukt. Mogelijk is het werk vol of ben je al geregistreerd.";
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij registreren voor werk {WerkId}", werkId);
                TempData["ErrorMessage"] = "Er is een fout opgetreden bij de registratie.";
            }

            return RedirectToPage(new { presentatieType = ResultaatModel?.HuidigePresentatieType ?? "top" });
        }



        public IActionResult OnPostResetTest()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                return RedirectToPage("/GebruikersTest/GebruikersTest", new { reset = true });
            }
            return RedirectToPage("/Login/LoginGebruiker");
        }
    }
}