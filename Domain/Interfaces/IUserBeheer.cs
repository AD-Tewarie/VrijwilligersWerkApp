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
        void VoegGebruikerToe(string naam, string achterNaam, string email, string wachtwoord);
        void VerwijderGebruiker(int userId);

        // Authenticatie
        bool ValideerGebruiker(string email, string wachtwoord);

        // Queries
        List<User> HaalAlleGebruikersOp();
        User HaalGebruikerOpID(int userId);
        User HaalGebruikerOpEmail(string email);
    }
}
