using Application.GebruikersTest.Interfaces;
using Domain.Werk.Interfaces;
using Domain.Werk.Models;

namespace Application.GebruikersTest.Services
{
    public class WerkVerzamelService : IWerkVerzamelService
    {
        private readonly IVrijwilligersWerkBeheer vrijwilligersWerkBeheer;

        public WerkVerzamelService(IVrijwilligersWerkBeheer vrijwilligersWerkBeheer)
        {
            this.vrijwilligersWerkBeheer = vrijwilligersWerkBeheer ?? throw new ArgumentNullException(nameof(vrijwilligersWerkBeheer));
        }

        public List<VrijwilligersWerk> VerzamelBeschikbaarWerk()
        {
            return vrijwilligersWerkBeheer.BekijkAlleWerk();
        }
    }
}