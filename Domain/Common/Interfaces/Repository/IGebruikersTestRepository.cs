﻿using System.Collections.Generic;
using Domain.GebruikersTest.Models;

namespace Domain.Common.Interfaces.Repository
{
    public interface IGebruikersTestRepository
    {
        List<Categorie> HaalAlleCategorieënOp();
        Categorie? GetCategorieOnId(int id);
        List<TestVraag> HaalAlleTestVragenOp();
        TestVraag? GetTestVraagOnId(int id);
        List<WerkCategorie> GetWerkCategorieënByWerkId(int werkId);
    }
}