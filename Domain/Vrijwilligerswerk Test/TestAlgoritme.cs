using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Mapper;
using Domain.Vrijwilligerswerk_Test.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestAlgoritme
    {
        private IGebruikersTestRepository iRepos;
        private TestMapper testMapper;


        public TestAlgoritme(IGebruikersTestRepository repos, TestMapper mapper)
        {
            iRepos = repos;
            testMapper = mapper;
        }



        public Dictionary<Categorie, int> BerekenTestScoresUsingIds(Dictionary<int, int> affiniteiten, Dictionary<int, int> antwoorden)
        {
            var score = new Dictionary<Categorie, int>();

            foreach (var categorieId in affiniteiten.Keys)
            {
                Console.WriteLine(iRepos.GetCategorieOnId(1));
                var categorie =  iRepos.GetCategorieOnId(categorieId);
                if (categorie == null) continue;

                int totaleScore = 0;

                foreach (var vraagId in antwoorden.Keys)
                {
                    var vraag = iRepos.GetTestVraagOnId(vraagId);
                    if (vraag != null && vraag.CategorieId == categorieId)
                    {
                        totaleScore += antwoorden[vraagId] * affiniteiten[categorieId];
                    }
                }

                score[testMapper.MapToCategorie( categorie)] = totaleScore;
            }

            return SorteerOpScore(score);
        }



        private Dictionary<Categorie, int> SorteerOpScore(Dictionary<Categorie, int> score)
        {
            Console.WriteLine("Sorting Scores...");
            foreach (var entry in score)
            {
                Console.WriteLine($"Categorie: {entry.Key.Naam}, Score: {entry.Value}");
            }

            var filteredScores = new Dictionary<Categorie, int>();
            var keys = new List<Categorie>(score.Keys);

            while (keys.Count > 0)
            {
                Categorie hoogsteCategorie = null;
                int hoogsteScore = 0;

                foreach (var categorie in keys)
                {
                    if (score[categorie] > hoogsteScore)
                    {
                        hoogsteScore = score[categorie];
                        hoogsteCategorie = categorie;
                    }
                }

                filteredScores.Add(hoogsteCategorie, hoogsteScore);
                keys.Remove(hoogsteCategorie);
            }

            Console.WriteLine("Sorted Scores:");
            foreach (var entry in filteredScores)
            {
                Console.WriteLine($"Categorie: {entry.Key.Naam}, Score: {entry.Value}");
            }

            return filteredScores;
        }


        public List<VrijwilligersWerk> FilterEnSorteerWerk(List<VrijwilligersWerk> werkLijst, Dictionary<Categorie, int> gesorteerdeScores)
        {
            var werkMetScores = new List<(VrijwilligersWerk werk, int score)>();

            
            foreach (var werk in werkLijst)
            {
                int werkScore = 0;

                // ophalen van WerkCategorie
                var werkCategorieën = testMapper.MapToWerkCategorieList( iRepos.GetWerkCategorieënByWerkId(werk.WerkId));

                
                foreach (var werkCategorie in werkCategorieën)
                {
                    var categorieId = werkCategorie.CategorieId;
                    // check of de id van de key overeen komt met de categorieId
                    if (gesorteerdeScores.Keys.Any(c => c.Id == categorieId))
                    {
                        var categorie = gesorteerdeScores.Keys.First(c => c.Id == categorieId);
                        werkScore += gesorteerdeScores[categorie];
                    }
                }

                if (werkScore > 0)
                {
                    werkMetScores.Add((werk, werkScore));
                }
            }

            // sorteer in aflopende volgorde
            werkMetScores.Sort((x, y) => y.score.CompareTo(x.score));

            // stop de objecten in een lijst
            return werkMetScores.Select(item => item.werk).ToList();
        }






    }
}
