using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class TestAlgoritme
    {

        public Dictionary<Categorie, int> BerekenScores(Dictionary<Categorie, int> affiniteiten, Dictionary<TestVraag, int> antwoorden)
        {
            var score = new Dictionary<Categorie, int>();

            // affiniteiten is hier het gewicht die gebruiker toewijst aan de categorieën 
            foreach (var categorie in affiniteiten.Keys)
            {
                int totaleScore = 0;

                foreach (var antwoord in antwoorden)
                {

                    if (antwoord.Key.CategorieId == categorie.Id)
                    {
                        totaleScore += antwoord.Value * affiniteiten[categorie];
                    }

                }
                score.Add(categorie, totaleScore);

            }
            return SorteerOpScore(score);

        }



        private Dictionary<Categorie, int> SorteerOpScore(Dictionary<Categorie, int> score)
        {
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

            return filteredScores;
        }


        public List<VrijwilligersWerk> FilterEnSorteerWerk (List<VrijwilligersWerk> werkLijst, Dictionary<Categorie, int> gesorteerdeScores)
        {
            var filteredWerk = new List<VrijwilligersWerk>();

            foreach(var categorie in gesorteerdeScores.Keys)
            {
                foreach(var werk in werkLijst)
                {
                    if (werk.WerkId == categorie.Id)
                    {
                        filteredWerk.Add(werk);
                        break;
                    }
                }
            }

            return filteredWerk;

        }






    }
}
