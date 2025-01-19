using Application.Werk.ViewModels;

namespace Application.Werk.Interfaces
{
    public interface IWerkRegistratieBeheerService
    {
        RegistratieResultaatViewModel RegistreerVoorWerk(int gebruikerId, int werkId);
        RegistratieResultaatViewModel TrekRegistratieIn(int werkId, int gebruikerId);
        bool HeeftGebruikerRegistratie(int werkId, int gebruikerId);
    }
}