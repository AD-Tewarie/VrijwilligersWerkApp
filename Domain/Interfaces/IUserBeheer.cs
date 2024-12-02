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
        void VoegGebruikerToe(string naam, string achterNaam );
        List<string> GetAllUsers();
        void VerwijderGebruiker(int userId);
    }
}
