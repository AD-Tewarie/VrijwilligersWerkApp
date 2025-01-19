﻿using Domain.GebruikersTest.Models;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ITestBeheer
    {
        TestSessie StartTest(int gebruikerId);
        void BeantwoordVraag(int gebruikerId, int vraagId, int antwoord);
        void ZetAffiniteit(int gebruikerId, int categorieId, int score);
        Dictionary<Categorie, int> RondTestAf(int gebruikerId);
        List<Categorie> HaalAlleCategorieënOp();
        List<TestVraag> HaalAlleTestVragenOp();
        Categorie GetCategorieOpId(int id);
    }
}