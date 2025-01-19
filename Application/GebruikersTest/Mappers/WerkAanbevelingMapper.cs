using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.ViewModels;
using Domain.GebruikersTest.WerkScore;

namespace Application.GebruikersTest.Mappers
{
    public class WerkAanbevelingMapper : IWerkAanbevelingMapper
    {
        public WerkAanbevelingViewModel MapNaarViewModel(
            WerkMetScore werkMetScore, 
            int matchPercentage, 
            string presentatieType)
        {
            return new WerkAanbevelingViewModel(
                werkMetScore.Werk.WerkId,
                werkMetScore.Werk.Titel,
                werkMetScore.Werk.Omschrijving,
                werkMetScore.Score,
                presentatieType
            );
        }
    }
}