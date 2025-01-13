using Domain.Interfaces;
using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VrijwilligersWerkApp.Extensions;
using Microsoft.AspNetCore.Mvc;
using VrijwilligersWerkApp.Services;
using System.Text.Json;
using VrijwilligersWerkApp.Helpers;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Domain.Vrijwilligerswerk_Test.WerkScore;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Infrastructure.Interfaces;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestResultaatModel : PageModel
    {
        private readonly ITestSessionService testSessionService;
        private readonly IRegistratieBeheer registratieBeheer;

        public List<WerkMetScore> AanbevolenWerk { get; private set; }
        public Dictionary<Categorie, int> GesorteerdeScores { get; private set; }
        public string FeedbackMessage { get; private set; }
        public string SuccesMessage { get; private set; }
        public string ErrorMessage { get; private set; }
        public string HuidigePresentatieType { get; private set; } = "top";

        public GebruikersTestResultaatModel(
            ITestSessionService testSessionService,
            IRegistratieBeheer registratieBeheer)
        {
            this.testSessionService = testSessionService;
            this.registratieBeheer = registratieBeheer;
        }

        public IActionResult OnGet(string presentatieType = "top")
        {
            try
            {
                // Store the current presentation type
                HuidigePresentatieType = presentatieType?.ToLower() ?? "top";

                if (!HttpContext.Session.GetInt32("UserId").HasValue)
                {
                    return RedirectToPage("/Account/Login");
                }

                if (TempData["SuccesMessage"] != null)
                {
                    SuccesMessage = TempData["SuccesMessage"].ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                var resultaat = testSessionService.BerekenResultaten(HuidigePresentatieType);

                if (!resultaat.scores.Any())
                {
                    FeedbackMessage = "Geen testresultaten beschikbaar. Start de test opnieuw.";
                    return RedirectToPage("/GebruikersTest/GebruikersTest");
                }

                GesorteerdeScores = resultaat.scores;
                AanbevolenWerk = resultaat.werkMetScores;

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Er is een fout opgetreden bij het laden van de resultaten. Start de test opnieuw.";
                return RedirectToPage("/GebruikersTest/GebruikersTest");
            }
        }

        public IActionResult OnPostApplyForJob(int werkId)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    return RedirectToPage("/Account/Login");
                }

                registratieBeheer.RegistreerGebruikerVoorWerk(userId.Value, werkId);
                TempData["SuccesMessage"] = "Je bent succesvol geregistreerd voor dit vrijwilligerswerk!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Registratie mislukt: {ex.Message}";
            }

            // Redirect back to the same page with the same presentation type
            return RedirectToPage(new { presentatieType = HuidigePresentatieType });
        }
    }
}