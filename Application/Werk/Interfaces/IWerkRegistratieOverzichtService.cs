using Application.Werk.ViewModels;

namespace Application.Werk.Interfaces
{
    public interface IWerkRegistratieOverzichtService
    {
        List<WerkRegistratieViewModel> HaalRegistratiesOp(int gebruikerId);
        List<WerkRegistratieViewModel> HaalRegistratiesOpVoorWerk(int werkId);
        int HaalAantalRegistratiesOp(int werkId);
    }
}