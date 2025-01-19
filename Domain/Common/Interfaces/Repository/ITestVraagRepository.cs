using Domain.GebruikersTest.Models;
using System.Collections.Generic;

namespace Domain.Common.Interfaces.Repository
{
    public interface ITestVraagRepository
    {
        List<Categorie> HaalAlleCategorieënOp();
        List<TestVraag> HaalAlleTestVragenOp();
        Categorie GetCategorieOpId(int id);
    }
}