using Domain.Vrijwilligerswerk_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GebruikersTest.Interfaces
{
    public interface ITestSessieBeheer
    {
        TestSessie HaalOp(int userId);
        void Opslaan(int userId, TestSessie sessie);
        void Verwijder(int userId);
    }
}
