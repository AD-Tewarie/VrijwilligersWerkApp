using Domain.Gebruikers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gebruikers.Interfaces
{
    public interface IUserBeheer
    {
        void VoegGebruikerToe(string naam, string achterNaam, string email, string wachtwoord);
        void VerwijderGebruiker(int userId);
        bool ValideerGebruiker(string email, string wachtwoord);
        List<User> HaalAlleGebruikersOp();
        User HaalGebruikerOpID(int userId);
        User HaalGebruikerOpEmail(string email);
    }
}
