using Application.GebruikersProfiel.Interfaces;
using Application.GebruikersProfiel.ViewModels;
using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Domain.Gebruikers.Interfaces;

namespace Application.GebruikersProfiel.Services
{
    public class GebruikersProfielService : IGebruikersProfielService
    {
        private readonly IUserBeheer userBeheer;
        private readonly IWerkRegistratieOverzichtService registratieOverzichtService;
        private readonly IWerkRegistratieBeheerService registratieBeheerService;

        public GebruikersProfielService(
            IUserBeheer userBeheer,
            IWerkRegistratieOverzichtService registratieOverzichtService,
            IWerkRegistratieBeheerService registratieBeheerService)
        {
            this.userBeheer = userBeheer ?? throw new ArgumentNullException(nameof(userBeheer));
            this.registratieOverzichtService = registratieOverzichtService ?? throw new ArgumentNullException(nameof(registratieOverzichtService));
            this.registratieBeheerService = registratieBeheerService ?? throw new ArgumentNullException(nameof(registratieBeheerService));
        }

        public GebruikersProfielViewModel HaalProfielOp(int gebruikerId)
        {
            var gebruiker = userBeheer.HaalGebruikerOpID(gebruikerId);
            var registraties = registratieOverzichtService.HaalRegistratiesOp(gebruikerId);

            return new GebruikersProfielViewModel(
                gebruiker.UserId,
                gebruiker.Naam,
                gebruiker.AchterNaam,
                gebruiker.Email,
                registraties
            );
        }

        public void AnnuleerRegistratie(int registratieId)
        {
            var registratie = registratieOverzichtService.HaalRegistratiesOpVoorWerk(registratieId).FirstOrDefault();
            if (registratie != null)
            {
                registratieBeheerService.TrekRegistratieIn(registratie.WerkId, registratie.GebruikerId);
            }
        }

        public bool BestaatGebruiker(int gebruikerId)
        {
            try
            {
                var gebruiker = userBeheer.HaalGebruikerOpID(gebruikerId);
                return gebruiker != null;
            }
            catch
            {
                return false;
            }
        }
    }
}