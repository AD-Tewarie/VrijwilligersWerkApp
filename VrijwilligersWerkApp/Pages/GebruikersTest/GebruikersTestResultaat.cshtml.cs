using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using VrijwilligersWerkApp.Helpers;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Domain;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestResultaatModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly ITestBeheer testBeheer;
        private readonly IRegistratieBeheer registratieBeheer;

        public Dictionary<Categorie, int> GesorteerdeScores { get; private set; }
        public List<VrijwilligersWerk> AanbevolenWerk { get; private set; }
        public string FeedbackMessage { get; private set; }
        public bool CanRetakeTest { get; private set; } = false;

        public GebruikersTestResultaatModel(IVrijwilligersWerkBeheer vrijwilligersWerkBeheer, ITestBeheer testBeheer, IRegistratieBeheer registratieBeheer)
        {
            werkBeheer = vrijwilligersWerkBeheer;
            this.testBeheer = testBeheer;
            this.registratieBeheer = registratieBeheer;
        }




        public void OnGet()
        {
            var filteredJobIds = HttpContext.Session.Get<List<int>>("FilteredJobIds");
            var gesorteerdeScoresIds = HttpContext.Session.Get<Dictionary<int, int>>("SortedScores");

            if (filteredJobIds == null || gesorteerdeScoresIds == null || !filteredJobIds.Any())
            {
                CanRetakeTest = true;
                FeedbackMessage = "Geen testresultaten gevonden. U kunt de test opnieuw afleggen.";
                return;
            }

            AanbevolenWerk = filteredJobIds.Select(id => werkBeheer.HaalwerkOpID(id)).ToList();
            GesorteerdeScores = gesorteerdeScoresIds.ToDictionary(
                kvp => testBeheer.GetCategorieOnId(kvp.Key),
                kvp => kvp.Value
            );
        }

        public IActionResult OnPostApply(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                FeedbackMessage = "U moet ingelogd zijn om te registreren.";
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                
                registratieBeheer.RegistreerGebruikerVoorWerk(userId.Value, id);
                FeedbackMessage = "Registratie succesvol!";
                return Page();
            }
            catch (Exception ex)
            {
                FeedbackMessage = $"Er is een fout opgetreden: {ex.Message}";
            }

            return Page();
        }
    }
}
