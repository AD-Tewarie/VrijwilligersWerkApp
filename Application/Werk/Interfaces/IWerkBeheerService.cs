using Application.Werk.ViewModels;

namespace Application.Werk.Interfaces
{
    public interface IWerkBeheerService
    {
        void VoegWerkToe(WerkAanmaakViewModel model);
        void BewerkWerk(int werkId, WerkAanmaakViewModel model);
        void VerwijderWerk(int werkId);
        List<WerkAanbiedingOverzichtViewModel> HaalBeschikbareWerkAanbiedingenOp();
        WerkDetailsViewModel HaalWerkDetailsOp(int werkId);
    }
}