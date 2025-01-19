using Application.Authenticatie.ViewModels;

namespace Application.Authenticatie.Interfaces
{
    public interface IAuthenticatieService
    {
        bool Login(string email, string wachtwoord);
        void Registreer(string naam, string achternaam, string email, string wachtwoord);
        bool BestaatEmail(string email);
        GebruikerViewModel HaalGebruikerOpEmail(string email);
    }
}