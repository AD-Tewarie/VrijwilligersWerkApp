using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.GebruikersTest.ViewModels;
using Application.GebruikersTest.Interfaces;
using Domain.Werk.Interfaces;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestResultaatModel : PageModel
    {
        private readonly ITestVoortgangService testVoortgangService;
        private readonly ITestResultaatService resultaatService;
        private readonly IRegistratieBeheer registratieBeheer;

        [BindProperty]
        public GebruikersTestResultaatViewModel ResultaatModel { get; private set; } = null!;
        public string? FeedbackMessage { get; private set; }
        public string? SuccesMessage { get; private set; }
        public string? ErrorMessage { get; private set; }

        public GebruikersTestResultaatModel(
            ITestVoortgangService testVoortgangService,
            ITestResultaatService resultaatService,
            IRegistratieBeheer registratieBeheer)
        {
            this.testVoortgangService = testVoortgangService;
            this.resultaatService = resultaatService;
            this.registratieBeheer = registratieBeheer;
        }

        public IActionResult OnGet(string presentatieType = "top", bool reset = false)
        {
            try
            {
                var gebruikerId = HttpContext.Session.GetInt32("UserId");
                if (!gebruikerId.HasValue)
                {
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                if (TempData["SuccesMessage"] != null)
                {
                    SuccesMessage = TempData["SuccesMessage"]?.ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    ErrorMessage = TempData["ErrorMessage"]?.ToString();
                }

                
                int? parsedMinimumScore = null;
                if (Request.Query.ContainsKey("minimumScore") &&
                    int.TryParse(Request.Query["minimumScore"], out int score))
                {
                    parsedMinimumScore = score;
                }

                var sessie = testVoortgangService.HaalTestOp(gebruikerId.Value);
                if (sessie == null || !sessie.IsVoltooid)
                {
                    FeedbackMessage = "Geen testresultaten beschikbaar. Start de test opnieuw.";
                    return RedirectToPage("/GebruikersTest/GebruikersTest");
                }

                // Probeer eerst met de opgegeven filters
                ResultaatModel = resultaatService.HaalResultatenOp(gebruikerId.Value, presentatieType, parsedMinimumScore);

                // Valideer minimumScore voor "minimum" presentatieType
                if (presentatieType.ToLower() == "minimum" && (!parsedMinimumScore.HasValue || parsedMinimumScore.Value < 20))
                {
                    parsedMinimumScore = 20;
                }

                // Als er geen resultaten zijn, probeer met minder strikte filters
                if (!ResultaatModel.AanbevolenWerk.Any())
                {
                    // Probeer eerst met minimale relevante score
                    ResultaatModel = resultaatService.HaalResultatenOp(gebruikerId.Value, presentatieType, 20);

                    // Als er nog steeds geen resultaten zijn, probeer alle werk te tonen
                    if (!ResultaatModel.AanbevolenWerk.Any())
                    {
                        ResultaatModel = resultaatService.HaalResultatenOp(gebruikerId.Value, "alle", 20);
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Er is een fout opgetreden. Start de test opnieuw.";
                return RedirectToPage("/GebruikersTest/GebruikersTest");
            }
        }

        public IActionResult OnPostRegistreerVoorWerk(int werkId)
        {
            try
            {
                var gebruikerId = HttpContext.Session.GetInt32("UserId");
                if (!gebruikerId.HasValue)
                {
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                registratieBeheer.RegistreerGebruikerVoorWerk(gebruikerId.Value, werkId);
                TempData["SuccesMessage"] = "Je bent succesvol geregistreerd voor dit vrijwilligerswerk!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Registratie mislukt: {ex.Message}";
            }

            int minimumScore = 20;
            if (Request.Query.ContainsKey("minimumScore") &&
                int.TryParse(Request.Query["minimumScore"], out int score))
            {
                minimumScore = Math.Max(score, 20); 
            }

            return RedirectToPage(new {
                presentatieType = ResultaatModel?.HuidigePresentatieType ?? "top",
                minimumScore = minimumScore
            });
        }

        public IActionResult OnPostNieuweTest()
        {
            var gebruikerId = HttpContext.Session.GetInt32("UserId");
            if (!gebruikerId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            testVoortgangService.ResetTest(gebruikerId.Value);
            return RedirectToPage("/GebruikersTest/GebruikersTest", new { reset = true });
        }
    }
}