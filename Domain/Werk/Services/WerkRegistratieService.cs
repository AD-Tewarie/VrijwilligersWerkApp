using Domain.Common.Interfaces.Repository;
using Domain.Werk.Interfaces;

namespace Domain.Werk.Services
{
    public class WerkRegistratieService : IWerkRegistratieService
    {
        private readonly IWerkRegistratieRepository werkRegistratieRepository;

        public WerkRegistratieService(IWerkRegistratieRepository werkRegistratieRepository)
        {
            this.werkRegistratieRepository = werkRegistratieRepository;
        }

        public int HaalAantalRegistratiesOp(int werkId)
        {
            return werkRegistratieRepository.GetRegistratieCountForWerk(werkId);
        }
    }
}