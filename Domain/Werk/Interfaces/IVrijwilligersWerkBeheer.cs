using Domain.Werk.Models;

namespace Domain.Werk.Interfaces
{
    public interface IVrijwilligersWerkBeheer
    {
        void VoegWerkToe(string titel, string omschrijving, int maxCapaciteit, int categorieId);
        void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving);
        void VerwijderWerk(int werkId);
        List<VrijwilligersWerk> BekijkAlleWerk();
        VrijwilligersWerk HaalWerkOpID(int id);

    }


}
