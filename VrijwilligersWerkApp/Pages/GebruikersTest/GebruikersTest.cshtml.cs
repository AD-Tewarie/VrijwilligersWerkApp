using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Models;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VrijwilligersWerkApp.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Domain.Interfaces;

namespace VrijwilligersWerkApp.Pages.GebruikersTest
{
    public class GebruikersTestModel : PageModel
    {
        public int HuidigeStap { get; private set; }
        public string HuidigeVraag { get; private set; }
        public bool IsTestKlaar { get; private set; }
        public Dictionary<Categorie, int> GesorteerdeScores { get; private set; } = new Dictionary<Categorie, int>();
        public List<VrijwilligersWerk> AanbevolenWerk { get; private set; } = new List<VrijwilligersWerk>();

        private readonly ITestBeheer testBeheer;

        public Dictionary<int, int> Affiniteiten { get; set; } = new Dictionary<int, int>();


        public Dictionary<int, int> Antwoorden { get; set; } = new Dictionary<int, int>();



        public GebruikersTestModel(ITestBeheer testBeheer)
        {
            this.testBeheer = testBeheer;

        }




        public void OnGet()
        {
            // controleer of de sessie al resultaten bevat 
            var previousScores = HttpContext.Session.Get<Dictionary<int, int>>("SortedScores");
            var previousJobs = HttpContext.Session.Get<List<int>>("FilteredJobIds");

            if (previousScores != null && previousJobs != null && previousJobs.Any())
            {
                // oude results of nieuwe test 
                ViewData["HasPreviousResults"] = true;
                return;
            }

            // initialise een nieuwe test
            if (HttpContext.Session.GetInt32("HuidigeStap") == null)
            {
                HuidigeStap = 1;
                HttpContext.Session.SetInt32("HuidigeStap", HuidigeStap);
            }
            else
            {
                HuidigeStap = HttpContext.Session.GetInt32("HuidigeStap").Value;
            }

            LaadVragen();
        }



        public IActionResult OnPostReset()
        {
            // Clear all session data related to the test
            HttpContext.Session.Remove("SortedScores");
            HttpContext.Session.Remove("FilteredJobIds");
            HttpContext.Session.Remove("HuidigeStap");

            // Redirect to start the test
            return RedirectToPage("/GebruikersTest/GebruikersTest");
        }





        public IActionResult OnPost(
        int antwoord,
        [FromForm] List<int> AffiniteitenKeys,
        [FromForm] List<int> AffiniteitenValues,
        [FromForm] List<int> AntwoordenKeys,
        [FromForm] List<int> AntwoordenValues)
        {
            // zet form data om in dictionaries
            Affiniteiten = AffiniteitenKeys
                .Zip(AffiniteitenValues, (key, value) => new KeyValuePair<int, int>(key, value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Antwoorden = AntwoordenKeys
                .Zip(AntwoordenValues, (key, value) => new KeyValuePair<int, int>(key, value))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // ophalen van de huidige stap 
            HuidigeStap = HttpContext.Session.GetInt32("HuidigeStap").Value;

            var alleCategorieën = testBeheer.HaalAlleVraagCategorieënOp();

            if (HuidigeStap <= GetTotaalAffiniteitenStappen())
            {
                // affiniteit vragen input
                var huidigeCategorie = ZoekCategorieOpStap(HuidigeStap, alleCategorieën);
                if (huidigeCategorie != null)
                {
                    Affiniteiten[huidigeCategorie.Id] = antwoord;
                }
            }
            else
            {
                // normale vragen input
                var huidigeVraag = ZoekVraagOpStap(HuidigeStap - GetTotaalAffiniteitenStappen() - 1, alleCategorieën);
                if (huidigeVraag != null)
                {
                    Antwoorden[huidigeVraag.Id] = antwoord;
                }
            }

            // verhoog HuidigeStap en sla het op in de session
            HuidigeStap++;
            HttpContext.Session.SetInt32("HuidigeStap", HuidigeStap);

            // Controleer of de test klaar is
            if (HuidigeStap > (GetTotaalAffiniteitenStappen() + GetTotaalVragen()))
            {
                IsTestKlaar = true;

                // score calculatie en sorteren van werk
                GesorteerdeScores = testBeheer.BerekenTestScores(Affiniteiten, Antwoorden);
                AanbevolenWerk = testBeheer.FilterWerk(testBeheer.HaalAlleWerkOp(), GesorteerdeScores);
            }
            if (IsTestKlaar)
            {
                // omdat GesorteerdeScores een categorie als key heeft, zet ik dit om naar de categorie id (anders krijg je een error)
                HttpContext.Session.Set("SortedScores", GesorteerdeScores.ToDictionary(kvp => kvp.Key.Id, kvp => kvp.Value));
                HttpContext.Session.Set("FilteredJobIds", AanbevolenWerk.Select(w => w.WerkId).ToList());

                
                return RedirectToPage("/GebruikersTest/GebruikersTestResultaat");
            }
            else
            {
                LaadVragen();
            }

            return Page();
        }




        // methods voor onPost

        private void LaadVragen()
        {
            var alleCategorieën = testBeheer.HaalAlleVraagCategorieënOp();

            if (HuidigeStap <= GetTotaalAffiniteitenStappen())
            {
                // affiniteit vragen
                var huidigeCategorie = ZoekCategorieOpStap(HuidigeStap, alleCategorieën);
                if (huidigeCategorie != null)
                {
                    HuidigeVraag = $"Hoe belangrijk is categorie {huidigeCategorie.Naam}?";
                }
            }
            else
            {
                // normale vragen
                var vraagIndex = HuidigeStap - GetTotaalAffiniteitenStappen() - 1;
                var huidigeVraag = ZoekVraagOpStap(vraagIndex, alleCategorieën);
                if (huidigeVraag != null)
                {
                    HuidigeVraag = huidigeVraag.Tekst;
                }
            }
        }

        private VraagCategorie ZoekCategorieOpStap(int stap, List<VraagCategorie> alleCategorieën)
        {
            for (int i = 0; i < alleCategorieën.Count; i++)
            {
                if (i == stap - 1)
                {
                    return alleCategorieën[i];
                }
            }
            return null;
        }


        private TestVraag ZoekVraagOpStap(int index, List<VraagCategorie> alleCategorieën)
        {
            foreach (var categorie in alleCategorieën)
            {
                foreach (var vraag in categorie.GetVragen)
                {
                    if (index == 0)
                    {
                        return vraag;
                    }
                    index--;
                }
            }
            return null;
        }



        private int GetTotaalAffiniteitenStappen()
        {
            return testBeheer.HaalAlleVraagCategorieënOp().Count;
        }

        private int GetTotaalVragen()
        {
            return testBeheer.HaalAlleVraagCategorieënOp().Sum(categorie => categorie.GetVragen.Count);
        }
    }
}
