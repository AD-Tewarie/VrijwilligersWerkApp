using System.Collections.Generic;
using System.Linq;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.ViewModels
{
    public class GebruikersTestResultaatViewModel
    {
        public int GebruikerId { get; set; }
        public List<CategorieResultaat> CategorieScores { get; set; } = new();
        public List<WerkAanbeveling> AanbevolenWerk { get; set; } = new();
        public string HuidigePresentatieType { get; set; } = "top";
        public bool HeeftResultaten => CategorieScores.Any() && AanbevolenWerk.Any();

        public GebruikersTestResultaatViewModel()
        {
        }

        public GebruikersTestResultaatViewModel(
            int gebruikerId,
            Dictionary<Categorie, int> scores,
            List<WerkMetScore> aanbevolenWerk,
            string presentatieType)
        {
            GebruikerId = gebruikerId;
            HuidigePresentatieType = presentatieType;

            CategorieScores = scores.Select(s => new CategorieResultaat
            {
                CategorieId = s.Key.Id,
                CategorieNaam = s.Key.Naam,
                Score = s.Value
            }).ToList();

            AanbevolenWerk = aanbevolenWerk.Select(w => new WerkAanbeveling
            {
                WerkId = w.Werk.WerkId,
                Titel = w.Werk.Titel,
                Omschrijving = w.Werk.Omschrijving,
                MatchPercentage = w.Score
            }).ToList();
        }
    }

    public class CategorieResultaat
    {
        public int CategorieId { get; set; }
        public string CategorieNaam { get; set; } = string.Empty;
        public int Score { get; set; }
    }

    public class WerkAanbeveling
    {
        public int WerkId { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Omschrijving { get; set; } = string.Empty;
        public int MatchPercentage { get; set; }
    }
}