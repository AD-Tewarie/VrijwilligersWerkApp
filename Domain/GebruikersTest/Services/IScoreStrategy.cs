using System.Collections.Generic;
using Domain.GebruikersTest.Models;
using Domain.Werk.Models;

namespace Domain.GebruikersTest.Services
{
    public interface IScoreStrategy
    {
        Dictionary<Categorie, int> BerekenScores(
            Dictionary<int, int> affiniteiten,
            Dictionary<int, int> antwoorden,
            Dictionary<int, TestVraag> vragen,
            Dictionary<int, Categorie> categorieën);

        (int score, int maximaleScore) BerekenWerkScore(
            VrijwilligersWerk werk,
            Dictionary<Categorie, int> scores,
            List<WerkCategorie> werkCategorieën,
            Dictionary<int, Categorie> categorieën);
    }
}