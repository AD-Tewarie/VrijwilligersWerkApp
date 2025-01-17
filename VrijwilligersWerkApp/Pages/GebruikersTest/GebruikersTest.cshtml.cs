using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using VrijwilligersWerkApp.Services;
using Application.Interfaces;
using Application.ViewModels;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestModel : PageModel
    {
        private readonly IGebruikersTestService testService;
        private readonly ILogger<GebruikersTestModel> logger;

        public GebruikersTestViewModel VraagModel { get; set; }
        public string FeedbackMessage { get; set; }

        public GebruikersTestModel(
            IGebruikersTestService testService,
            ILogger<GebruikersTestModel> logger)
        {
            this.testService = testService;
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                if (testService.HeeftBestaandeResultaten(userId.Value))
                {
                    ViewData["HasPreviousResults"] = true;
                    return Page();
                }

                VraagModel = testService.HaalVolgendeVraag(userId.Value);
                if (VraagModel == null)
                {
                    return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
                }

                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij ophalen test vraag");
                FeedbackMessage = "Er is een fout opgetreden bij het laden van de test.";
                return Page();
            }
        }

        public IActionResult OnPostAntwoord(int antwoord)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                bool isKlaar = testService.VerwerkAntwoord(userId.Value, antwoord);
                if (isKlaar)
                {
                    return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij verwerken antwoord");
                FeedbackMessage = "Er is een fout opgetreden bij het opslaan van je antwoord.";
                return Page();
            }
        }

        public IActionResult OnPostReset()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                testService.ResetTest(userId.Value);
            }
            return RedirectToPage();
        }
    }
}