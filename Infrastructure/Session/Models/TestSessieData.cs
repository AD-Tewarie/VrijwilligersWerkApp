using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.GebruikersTest.Models;

namespace Infrastructure.Session.Models
{
    public class TestSessieData
    {
        [JsonInclude]
        public int GebruikerId { get; set; }

        [JsonInclude]
        public Dictionary<int, int> Affiniteiten { get; set; }

        [JsonInclude]
        public Dictionary<int, int> Antwoorden { get; set; }

        [JsonInclude]
        public int HuidigeStap { get; set; }

        [JsonInclude]
        public bool IsVoltooid { get; set; }

        public TestSessieData()
        {
            Affiniteiten = new Dictionary<int, int>();
            Antwoorden = new Dictionary<int, int>();
        }

        public TestSessieData(TestSessie sessie)
        {
            GebruikerId = sessie.GebruikerId;
            Affiniteiten = new Dictionary<int, int>(sessie.Affiniteiten);
            Antwoorden = new Dictionary<int, int>(sessie.Antwoorden);
            HuidigeStap = sessie.HuidigeStap;
            IsVoltooid = sessie.IsVoltooid;
        }

        public TestSessie NaarDomeinModel()
        {
            var sessie = TestSessie.Start(GebruikerId);

            foreach (var affiniteit in Affiniteiten)
            {
                sessie.ZetAffiniteit(affiniteit.Key, affiniteit.Value);
            }

            foreach (var antwoord in Antwoorden)
            {
                sessie.VoegAntwoordToe(antwoord.Key, antwoord.Value);
            }

            for (int i = 0; i < HuidigeStap; i++)
            {
                sessie.VerhoogStap();
            }

            if (IsVoltooid)
            {
                sessie.RondAf();
            }

            return sessie;
        }
    }
}