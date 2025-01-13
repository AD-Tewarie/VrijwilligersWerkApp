using Domain.Models;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using VrijwilligersWerkApp.Services;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestModel : PageModel
    {
        private readonly ITestBeheer testBeheer;
        private readonly ITestSessionService testSessionService;

        public string HuidigeVraag { get; private set; }
        public bool IsTestKlaar { get; private set; }
        public Dictionary<Categorie, int> GesorteerdeScores { get; private set; }
        public List<VrijwilligersWerk> AanbevolenWerk { get; private set; }
        public int VoortgangPercentage { get; private set; }
        public bool IsLaatsteVraag { get; private set; }

        public GebruikersTestModel(
            ITestBeheer testBeheer,
            ITestSessionService testSessionService)
        {
            this.testBeheer = testBeheer ?? throw new ArgumentNullException(nameof(testBeheer));
            this.testSessionService = testSessionService ?? throw new ArgumentNullException(nameof(testSessionService));
        }

        public IActionResult OnGet()
        {
            if (testSessionService.HeeftBestaandeResultaten())
            {
                ViewData["HasPreviousResults"] = true;
                return Page();
            }

            LaadVolgendeVraag();
            return Page();
        }

        public IActionResult OnPostReset()
        {
            testSessionService.ResetTest();
            return RedirectToPage();
        }

        public IActionResult OnPost(int antwoord)
        {
            try
            {
                bool isKlaar = testSessionService.VerwerkAntwoord(antwoord);
                

                if (isKlaar)
                {
                    var (scores, aanbevolenWerk) = testSessionService.BerekenResultaten();
                   
                    return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
                }


                LaadVolgendeVraag(); 
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError(string.Empty, $"Er is een fout opgetreden: {ex.Message}");
                return Page();
            }
        }

        private void LaadVolgendeVraag()
        {
            var (vraagTekst, isKlaar) = testSessionService.HaalVolgendeVraag();
            HuidigeVraag = vraagTekst;
            IsTestKlaar = isKlaar;

            // Bereken voortgang
            var totaalVragen = testBeheer.HaalAlleCategorieënOp().Count +
                              testBeheer.HaalAlleTestVragenOp().Count;
            var huidigeVraagNummer = HttpContext.Session.GetInt32("Test_HuidigeStap") ?? 0;
            VoortgangPercentage = (int)((double)huidigeVraagNummer / totaalVragen * 100);
            IsLaatsteVraag = huidigeVraagNummer == totaalVragen - 1;
        }
    }
}
