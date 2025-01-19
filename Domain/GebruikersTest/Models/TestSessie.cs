﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.GebruikersTest.Models
{
    public class TestSessie
    {
        [JsonInclude]
        public int GebruikerId { get; private set; }

        [JsonInclude]
        public Dictionary<int, int> Affiniteiten { get; private set; }

        [JsonInclude]
        public Dictionary<int, int> Antwoorden { get; private set; }

        [JsonInclude]
        public int HuidigeStap { get; private set; }

        [JsonInclude]
        public bool IsVoltooid { get; private set; }

        [JsonConstructor]
        private TestSessie(int gebruikerId, Dictionary<int, int> affiniteiten, Dictionary<int, int> antwoorden, int huidigeStap, bool isVoltooid)
        {
            GebruikerId = gebruikerId;
            Affiniteiten = affiniteiten;
            Antwoorden = antwoorden;
            HuidigeStap = huidigeStap;
            IsVoltooid = isVoltooid;
        }

        private TestSessie(int gebruikerId)
        {
            GebruikerId = gebruikerId;
            Affiniteiten = new Dictionary<int, int>();
            Antwoorden = new Dictionary<int, int>();
            HuidigeStap = 0;
            IsVoltooid = false;
        }

        public static TestSessie Start(int gebruikerId)
        {
            return new TestSessie(gebruikerId);
        }

        public void ZetAffiniteit(int categorieId, int score)
        {
            Affiniteiten[categorieId] = score;
        }

        public void VoegAntwoordToe(int vraagId, int antwoord)
        {
            Antwoorden[vraagId] = antwoord;
        }

        public void VerhoogStap()
        {
            HuidigeStap++;
        }

        public void RondAf()
        {
            IsVoltooid = true;
        }

        public void Reset()
        {
            Affiniteiten.Clear();
            Antwoorden.Clear();
            HuidigeStap = 0;
            IsVoltooid = false;
        }
    }
}
