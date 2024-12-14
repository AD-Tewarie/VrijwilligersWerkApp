using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserBeheer
    {
        public void VoegGebruikerToe(string naam, string achterNaam,string email, string password);
        List<User> HaalAlleGebruikersOp();
        void VerwijderGebruiker(int userId);
        public bool ValideerGebruiker(string email, string password);
        public User HaalGebruikerOpEmail(string email);
    }
}
