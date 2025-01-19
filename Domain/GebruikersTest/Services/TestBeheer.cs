﻿﻿﻿﻿﻿using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Services;
using Domain.Common.Interfaces.Repository;
using System.Collections.Generic;

namespace Domain.GebruikersTest.Services
{
    public class TestBeheer : ITestBeheer
    {
        private readonly IGebruikersTestRepository repository;
        private readonly IScoreStrategy scoreStrategy;
        private readonly ITestSessieBeheer sessieBeheer;

        public TestBeheer(
            IGebruikersTestRepository repository,
            IScoreStrategy scoreStrategy,
            ITestSessieBeheer sessieBeheer)
        {
            this.repository = repository;
            this.scoreStrategy = scoreStrategy;
            this.sessieBeheer = sessieBeheer;
        }

        public TestSessie StartTest(int gebruikerId)
        {
            // Verwijder bestaande sessie als die er is
            try
            {
                var bestaandeSessie = sessieBeheer.HaalOp(gebruikerId);
                if (bestaandeSessie != null)
                {
                    sessieBeheer.Verwijder(gebruikerId);
                }
            }
            catch { /* Negeer fouten als er geen sessie bestaat */ }

            // Start een nieuwe sessie
            var nieuweSessie = TestSessie.Start(gebruikerId);
            sessieBeheer.Opslaan(gebruikerId, nieuweSessie);
            return nieuweSessie;
        }

        public void BeantwoordVraag(int gebruikerId, int vraagId, int antwoord)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            sessie.VoegAntwoordToe(vraagId, antwoord);
            sessieBeheer.Opslaan(gebruikerId, sessie);
        }

        public void ZetAffiniteit(int gebruikerId, int categorieId, int score)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            sessie.ZetAffiniteit(categorieId, score);
            sessieBeheer.Opslaan(gebruikerId, sessie);
        }

        public Dictionary<Categorie, int> RondTestAf(int gebruikerId)
        {
            var sessie = sessieBeheer.HaalOp(gebruikerId);
            sessie.RondAf();

            var categorieën = repository.HaalAlleCategorieënOp();
            var vragen = repository.HaalAlleTestVragenOp();

            var scores = scoreStrategy.BerekenScores(
                new Dictionary<int, int>(sessie.Affiniteiten),
                new Dictionary<int, int>(sessie.Antwoorden),
                vragen.ToDictionary(v => v.Id),
                categorieën.ToDictionary(c => c.Id));

            sessieBeheer.Verwijder(gebruikerId);
            return scores;
        }

        public List<Categorie> HaalAlleCategorieënOp()
        {
            return repository.HaalAlleCategorieënOp();
        }

        public List<TestVraag> HaalAlleTestVragenOp()
        {
            return repository.HaalAlleTestVragenOp();
        }

        public Categorie GetCategorieOpId(int id)
        {
            return repository.GetCategorieOnId(id);
        }
    }
}
