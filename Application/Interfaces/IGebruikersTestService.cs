using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGebruikersTestService
    {
        GebruikersTestViewModel HaalVolgendeVraag(int userId);
        bool VerwerkAntwoord(int userId, int antwoord);
        void ResetTest(int userId);
        bool HeeftBestaandeResultaten(int userId);
    }
}
