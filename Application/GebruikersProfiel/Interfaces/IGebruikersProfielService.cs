using Application.GebruikersProfiel.ViewModels;

namespace Application.GebruikersProfiel.Interfaces
{
    public interface IGebruikersProfielService
    {
        GebruikersProfielViewModel HaalProfielOp(int gebruikerId);
        void AnnuleerRegistratie(int registratieId);
        bool BestaatGebruiker(int gebruikerId);
    }
}