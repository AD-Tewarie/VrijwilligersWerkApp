using Domain.Models;
using Domain.Vrijwilligerswerk_Test.Interfaces;
using Domain.Vrijwilligerswerk_Test.PresentatieStrategy;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.WerkScore
{
    public class WerkScoreService : IWerkScoreService
    {
        private readonly IScoreStrategy scoreStrategy;
        private readonly IGebruikersTestRepository repository;

        public WerkScoreService(IScoreStrategy scoreStrategy, IGebruikersTestRepository repository)
        {
            this.scoreStrategy = scoreStrategy;
            this.repository = repository;
        }

        public int BerekenWerkScore(VrijwilligersWerk werk, Dictionary<Categorie, int> scores)
        {
            return scoreStrategy.BerekenWerkScore(werk, scores, repository);
        }

        public List<WerkMetScore> BerekenScoresVoorWerkLijst(
            List<VrijwilligersWerk> werkLijst,
            Dictionary<Categorie, int> scores)
        {
            return werkLijst
                .Select(werk => new WerkMetScore(
                    werk,
                    BerekenWerkScore(werk, scores)))
                .ToList();
        }
    }
}
