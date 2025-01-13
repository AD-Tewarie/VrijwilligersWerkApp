using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;
using VrijwilligersWerkApp.Helpers;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Infrastructure.Interfaces;


namespace VrijwilligersWerkApp.Pages.RegistreerWerk
{
    public class VrijwilligersWerkOverzichtModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IRegistratieBeheer registratieBeheer;
        private readonly IGebruikersTestRepository testRepository;
        private readonly ICategorieService categorieService;
        private readonly IWerkRegistratieRepository werkRegistratieRepository;

        public List<WerkMetCategorie> WerkAanbiedingen { get; set; }
        public string FeedbackMessage { get; set; }
        public string SuccesMessage { get; set; }

        public VrijwilligersWerkOverzichtModel(
            IVrijwilligersWerkBeheer werkBeheer,
            IRegistratieBeheer registratieBeheer,
            IGebruikersTestRepository testRepository,
            ICategorieService categorieService,
            IWerkRegistratieRepository werkRegistratieRepository)
        {
            this.werkBeheer = werkBeheer;
            this.registratieBeheer = registratieBeheer;
            this.testRepository = testRepository;
            this.categorieService = categorieService;
            this.werkRegistratieRepository = werkRegistratieRepository;
        }

        public void OnGet()
        {
            var alleWerk = werkBeheer.BekijkAlleWerk();
            var werkMetCategorieen = new List<WerkMetCategorie>();

            foreach (var werk in alleWerk)
            {
                var werkCategorieën = testRepository.GetWerkCategorieënByWerkId(werk.WerkId);
                var categorieNamen = new List<string>();

                foreach (var werkCategorie in werkCategorieën)
                {
                    var categorie = categorieService.GetCategorieOpId(werkCategorie.CategorieId);
                    if (categorie != null)
                    {
                        categorieNamen.Add(categorie.Naam);
                    }
                }

                // Haal het werkelijke aantal registraties op
                int huidigeRegistraties = werkRegistratieRepository.GetRegistratieCountForWerk(werk.WerkId);

                werkMetCategorieen.Add(new WerkMetCategorie
                {
                    Werk = werk,
                    CategorieNaam = categorieNamen.Any() ? string.Join(", ", categorieNamen) : "Geen categorie",
                    HuidigeRegistraties = huidigeRegistraties
                });
            }

            WerkAanbiedingen = werkMetCategorieen;

            if (TempData["SuccesMessage"] != null)
            {
                SuccesMessage = TempData["SuccesMessage"].ToString();
            }
            if (TempData["ErrorMessage"] != null)
            {
                FeedbackMessage = TempData["ErrorMessage"].ToString();
            }
        }


        public IActionResult OnPostApply(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                FeedbackMessage = "U moet ingelogd zijn om u te registreren.";
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                registratieBeheer.RegistreerGebruikerVoorWerk(userId.Value, id);
                TempData["SuccesMessage"] = "Registratie succesvol!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Er is een fout opgetreden: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}
