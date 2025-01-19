using System.Collections.Generic;
using Application.GebruikersTest.ViewModels;

namespace Application.GebruikersTest.Interfaces
{
    public interface IGebruikersTestResultaatService
    {
        List<TestResultaatViewModel> HaalTestResultatenOp(int gebruikerId);
    }
}