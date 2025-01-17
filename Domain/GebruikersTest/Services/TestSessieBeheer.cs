using Domain.GebruikersTest.Interfaces;
using Domain.Vrijwilligerswerk_Test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Services
{
    public class TestSessieBeheer : ITestSessieBeheer 
    {
        private Dictionary<int, TestSessie> actieveSessies = new();

        public TestSessie HaalOp(int userId)
        {
            if (!actieveSessies.ContainsKey(userId))
            {
                actieveSessies[userId] = TestSessie.Start();
            }
            return actieveSessies[userId];
        }

        public void Opslaan(int userId, TestSessie sessie)
        {
            actieveSessies[userId] = sessie;
        }

        public void Verwijder(int userId)
        {
            actieveSessies.Remove(userId);
        }
    }
}
