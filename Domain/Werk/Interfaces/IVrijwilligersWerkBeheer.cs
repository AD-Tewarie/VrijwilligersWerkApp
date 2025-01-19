using Domain.Werk.Models;

namespace Domain.Werk.Interfaces
{
    public interface IVrijwilligersWerkBeheer
    {
        void VoegWerkToe(string titel, string omschrijving, int maxCapaciteit, string locatie, int categorieId);
        void BewerkWerk(int werkId, string nieuweTitel, int nieuweCapaciteit, string nieuweBeschrijving, string nieuweLocatie);
        void VerwijderWerk(int werkId);
        List<VrijwilligersWerk> BekijkAlleWerk();
        VrijwilligersWerk HaalWerkOpID(int id);
    }
}
