using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestModel : PageModel
    {
        private readonly ITestVoortgangService testVoortgangService;
        private readonly ILogger<GebruikersTestModel> _logger;

        [BindProperty]
        public GebruikersTestViewModel VraagModel { get; set; } = null!;
        public string? FeedbackMessage { get; private set; }
        public bool HeeftBestaandeResultaten { get; private set; }

        public GebruikersTestModel(
            ITestVoortgangService testVoortgangService,
            ILogger<GebruikersTestModel> logger)
        {
            this.testVoortgangService = testVoortgangService ?? throw new ArgumentNullException(nameof(testVoortgangService));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult OnGet(bool reset = false)
        {
            try
            {
                var gebruikerId = HttpContext.Session.GetInt32("UserId");
                if (!gebruikerId.HasValue)
                {
                    TempData["ErrorMessage"] = "Je moet eerst inloggen.";
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                var sessie = testVoortgangService.HaalTestOp(gebruikerId.Value);
                if (!reset && sessie.IsVoltooid)
                {
                    HeeftBestaandeResultaten = true;
                    return Page();
                }
                
                if (reset)
                {
                    testVoortgangService.ResetTest(gebruikerId.Value);
                    return RedirectToPage();
                }

                if (!testVoortgangService.HeeftGebruikerActieveTest(gebruikerId.Value))
                {
                    VraagModel = testVoortgangService.MaakNieuweTest(gebruikerId.Value);
                }
                else
                {
                    VraagModel = testVoortgangService.HaalTestOp(gebruikerId.Value);
                    if (VraagModel.IsTestNetVoltooid)
                    {
                        return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij laden van de test");
                FeedbackMessage = "Er is een fout opgetreden bij het laden van de test.";
                return Page();
            }
        }

        public IActionResult OnPost(int antwoord)
        {
            try
            {
                var gebruikerId = HttpContext.Session.GetInt32("UserId");
                if (!gebruikerId.HasValue)
                {
                    TempData["ErrorMessage"] = "Je moet eerst inloggen.";
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                if (antwoord < 1 || antwoord > 5)
                {
                    FeedbackMessage = "Selecteer een antwoord tussen 1 en 5.";
                    VraagModel = testVoortgangService.HaalTestOp(gebruikerId.Value);
                    return Page();
                }

                // Verwerk het antwoord en controleer of de test klaar is
                _logger.LogInformation(
                    "Verwerking antwoord - GebruikerId: {GebruikerId}, Antwoord: {Antwoord}",
                    gebruikerId.Value, antwoord);

                bool isTestKlaar = testVoortgangService.BeantwoordVraag(gebruikerId.Value, VraagModel.HuidigeStap, antwoord);

                if (isTestKlaar)
                {
                    _logger.LogInformation("Test voltooid voor gebruiker {GebruikerId}", gebruikerId.Value);
                    return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
                }

                // Haal de bijgewerkte status op voor de volgende vraag
                VraagModel = testVoortgangService.HaalTestOp(gebruikerId.Value);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij verwerken van antwoord");
                FeedbackMessage = "Er is een fout opgetreden bij het opslaan van uw antwoord.";
                return Page();
            }
        }

        public IActionResult OnPostBekijkResultaten()
        {
            return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
        }

        public IActionResult OnPostNieuweTest()
        {
            return RedirectToPage(new { reset = true });
        }
    }
}