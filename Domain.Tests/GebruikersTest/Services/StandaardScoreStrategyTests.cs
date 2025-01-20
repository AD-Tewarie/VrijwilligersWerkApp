using System;
using System.Collections.Generic;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.Services;
using Domain.Werk.Models;
using Domain.Common.Data;
using Xunit;

namespace Domain.Tests.GebruikersTest.Services
{
    public class StandaardScoreStrategyTests
    {
        private readonly StandaardScoreStrategy _scoreStrategy;

        public StandaardScoreStrategyTests()
        {
            _scoreStrategy = new StandaardScoreStrategy();
        }

        [Fact]
        public void Moet_CorrecteTotaalscoreBerekenen_MetNormaleInput()
        {
            // Arrange
            var categorie = Categorie.Maak(1, "Test Categorie");
            var affiniteiten = new Dictionary<int, int> { { 1, 3 } }; // Gemiddelde affiniteit
            var antwoorden = new Dictionary<int, int> { { 1, 4 }, { 2, 4 } }; // Hoge scores
            var vragen = new Dictionary<int, TestVraag>
            {
                { 1, TestVraag.Maak(1, "Vraag 1", 1) },
                { 2, TestVraag.Maak(2, "Vraag 2", 1) }
            };
            var categorieën = new Dictionary<int, Categorie> { { 1, categorie } };

            // Act
            var scores = _scoreStrategy.BerekenScores(affiniteiten, antwoorden, vragen, categorieën);

            // Assert
            Assert.True(scores.ContainsKey(categorie));
            Assert.True(scores[categorie] > 0 && scores[categorie] <= 100);
        }

        [Fact]
        public void Moet_NulScore_GevenBijOntbrekendeCategorie()
        {
            // Arrange
            var categorie = Categorie.Maak(1, "Test Categorie");
            var nietBestaandeCategorie = Categorie.Maak(2, "Bestaat Niet");
            var affiniteiten = new Dictionary<int, int> { { 1, 3 } };
            var antwoorden = new Dictionary<int, int> { { 1, 4 } };
            var vragen = new Dictionary<int, TestVraag>
            {
                { 1, TestVraag.Maak(1, "Vraag 1", 1) }
            };
            var categorieën = new Dictionary<int, Categorie> { { 1, categorie } };

            // Act
            var scores = _scoreStrategy.BerekenScores(affiniteiten, antwoorden, vragen, categorieën);

            // Assert
            Assert.True(scores.ContainsKey(categorie));
            Assert.False(scores.ContainsKey(nietBestaandeCategorie));
        }

        [Fact]
        public void Moet_ConsistentieBonus_ToekennenBijHogeConsistenteScores()
        {
            // Arrange
            var categorie = Categorie.Maak(1, "Test Categorie");
            var affiniteiten = new Dictionary<int, int> { { 1, 5 } }; // Hoge affiniteit
            var antwoorden = new Dictionary<int, int> { { 1, 5 }, { 2, 5 }, { 3, 5 } }; // Consistent hoge scores
            var vragen = new Dictionary<int, TestVraag>
            {
                { 1, TestVraag.Maak(1, "Vraag 1", 1) },
                { 2, TestVraag.Maak(2, "Vraag 2", 1) },
                { 3, TestVraag.Maak(3, "Vraag 3", 1) }
            };
            var categorieën = new Dictionary<int, Categorie> { { 1, categorie } };

            // Act
            var scores = _scoreStrategy.BerekenScores(affiniteiten, antwoorden, vragen, categorieën);

            // Assert
            Assert.True(scores[categorie] > 90); // Score moet hoog zijn vanwege bonus
        }

        [Fact]
        public void Moet_ConsistentieMalus_ToekennenBijLageConsistenteScores()
        {
            // Arrange
            var categorie = Categorie.Maak(1, "Test Categorie");
            var affiniteiten = new Dictionary<int, int> { { 1, 1 } }; // Lage affiniteit
            var antwoorden = new Dictionary<int, int> { { 1, 1 }, { 2, 1 }, { 3, 1 } }; // Consistent lage scores
            var vragen = new Dictionary<int, TestVraag>
            {
                { 1, TestVraag.Maak(1, "Vraag 1", 1) },
                { 2, TestVraag.Maak(2, "Vraag 2", 1) },
                { 3, TestVraag.Maak(3, "Vraag 3", 1) }
            };
            var categorieën = new Dictionary<int, Categorie> { { 1, categorie } };

            // Act
            var scores = _scoreStrategy.BerekenScores(affiniteiten, antwoorden, vragen, categorieën);

            // Assert
            Assert.True(scores[categorie] < 30); // Score moet laag zijn vanwege malus
        }

        [Fact]
        public void Moet_CorrecteTotaleWerkScore_BerekenenMetMatchendeCategorieen()
        {
            // Arrange
            var werkData = new WerkData(1, "Test Werk", "Test omschrijving", 10, "Test locatie");
            var werk = VrijwilligersWerk.LaadVanuitData(werkData);
            var categorie1 = Categorie.Maak(1, "Categorie 1");
            var categorie2 = Categorie.Maak(2, "Categorie 2");
            
            var scores = new Dictionary<Categorie, int>
            {
                { categorie1, 80 },
                { categorie2, 60 }
            };

            var werkCategorieën = new List<WerkCategorie>
            {
                WerkCategorie.Maak(1, 1),
                WerkCategorie.Maak(1, 2)
            };

            var categorieën = new Dictionary<int, Categorie>
            {
                { 1, categorie1 },
                { 2, categorie2 }
            };

            // Act
            var (totaleScore, maximaleScore) = _scoreStrategy.BerekenWerkScore(werk, scores, werkCategorieën, categorieën);

            // Assert
            Assert.Equal(140, totaleScore); // 80 + 60
            Assert.Equal(200, maximaleScore); // 2 categorieën * 100
        }
    }
}