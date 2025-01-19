using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Domain.Common.Exceptions;
using Domain.Werk.Interfaces;

namespace Application.Werk.Services
{
    public class WerkRegistratieBeheerService : IWerkRegistratieBeheerService
    {
        private readonly IRegistratieBeheer registratieBeheer;

        public WerkRegistratieBeheerService(IRegistratieBeheer registratieBeheer)
        {
            this.registratieBeheer = registratieBeheer ?? throw new ArgumentNullException(nameof(registratieBeheer));
        }

        public RegistratieResultaatViewModel RegistreerVoorWerk(int gebruikerId, int werkId)
        {
            try
            {
                registratieBeheer.RegistreerGebruikerVoorWerk(gebruikerId, werkId);
                return RegistratieResultaatViewModel.SuccesVol();
            }
            catch (DomainValidationException ex)
            {
                
                var errorMessages = ex.ValidatieFouten
                    .SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}"))
                    .ToList();

                return RegistratieResultaatViewModel.Mislukt(string.Join("\n", errorMessages));
            }
            catch (Exception ex)
            {
                return RegistratieResultaatViewModel.Mislukt(ex.Message);
            }
        }

        public RegistratieResultaatViewModel TrekRegistratieIn(int werkId, int gebruikerId)
        {
            try
            {
                if (!registratieBeheer.HeeftGebruikerRegistratie(werkId, gebruikerId))
                {
                    return RegistratieResultaatViewModel.Mislukt("Geen registratie gevonden voor dit werk.");
                }

                var registraties = registratieBeheer.HaalRegistratiesOp()
                    .Where(r => r.VrijwilligersWerk.WerkId == werkId && r.User.UserId == gebruikerId)
                    .Select(r => r.RegistratieId)
                    .FirstOrDefault();

                registratieBeheer.VerwijderRegistratie(registraties);
                return RegistratieResultaatViewModel.SuccesVol("Registratie succesvol ingetrokken.");
            }
            catch (DomainValidationException ex)
            {
                var errorMessages = ex.ValidatieFouten
                    .SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}"))
                    .ToList();

                return RegistratieResultaatViewModel.Mislukt(string.Join("\n", errorMessages));
            }
            catch (Exception ex)
            {
                return RegistratieResultaatViewModel.Mislukt(ex.Message);
            }
        }

        public bool HeeftGebruikerRegistratie(int werkId, int gebruikerId)
        {
            return registratieBeheer.HeeftGebruikerRegistratie(werkId, gebruikerId);
        }
    }
}