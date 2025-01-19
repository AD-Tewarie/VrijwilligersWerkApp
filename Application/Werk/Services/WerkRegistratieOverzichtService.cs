using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Domain.Werk.Interfaces;

namespace Application.Werk.Services
{
    public class WerkRegistratieOverzichtService : IWerkRegistratieOverzichtService
    {
        private readonly IRegistratieBeheer registratieBeheer;

        public WerkRegistratieOverzichtService(IRegistratieBeheer registratieBeheer)
        {
            this.registratieBeheer = registratieBeheer ?? throw new ArgumentNullException(nameof(registratieBeheer));
        }

        public List<WerkRegistratieViewModel> HaalRegistratiesOp(int gebruikerId)
        {
            var registraties = registratieBeheer.HaalRegistratiesOp()
                .Where(r => r.User.UserId == gebruikerId)
                .Select(r => new WerkRegistratieViewModel(
                    r.RegistratieId,
                    r.VrijwilligersWerk.WerkId,
                    r.User.UserId,
                    r.VrijwilligersWerk.Titel,
                    r.VrijwilligersWerk.Locatie))
                .ToList();

            return registraties;
        }

        public List<WerkRegistratieViewModel> HaalRegistratiesOpVoorWerk(int werkId)
        {
            var registraties = registratieBeheer.HaalRegistratiesOp()
                .Where(r => r.VrijwilligersWerk.WerkId == werkId)
                .Select(r => new WerkRegistratieViewModel(
                    r.RegistratieId,
                    r.VrijwilligersWerk.WerkId,
                    r.User.UserId,
                    r.VrijwilligersWerk.Titel,
                    r.VrijwilligersWerk.Locatie))
                .ToList();

            return registraties;
        }

        public int HaalAantalRegistratiesOp(int werkId)
        {
            return registratieBeheer.HaalAantalRegistratiesOp(werkId);
        }
    }
}