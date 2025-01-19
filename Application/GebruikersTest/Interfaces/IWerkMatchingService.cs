using System.Collections.Generic;
using Domain.GebruikersTest.Models;
using Domain.GebruikersTest.WerkScore;
using Domain.Werk.Models;

namespace Application.GebruikersTest.Interfaces
{
    public interface IWerkMatchingService
    {
        List<WerkMetScore> BerekenWerkMatches(List<VrijwilligersWerk> beschikbaarWerk, TestSessie sessie);
    }
}