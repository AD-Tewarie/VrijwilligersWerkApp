namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class TestResultaat
    {
        public Dictionary<Categorie, int> Scores { get; }
        public DateTime AfgenomenOp { get; }

        private TestResultaat(Dictionary<Categorie, int> scores)
        {
            Scores = new Dictionary<Categorie, int>(scores);
            AfgenomenOp = DateTime.Now;
        }

        public static TestResultaat MaakVanScores(Dictionary<Categorie, int> scores)
        {
            if (scores == null || !scores.Any())
                throw new ArgumentException("Scores mogen niet leeg zijn.");

            return new TestResultaat(scores);
        }

        public List<Categorie> HaalTopCategorieën(int aantal = 3)
        {
            return Scores
                .OrderByDescending(s => s.Value)
                .Take(aantal)
                .Select(s => s.Key)
                .ToList();
        }
    }
}
