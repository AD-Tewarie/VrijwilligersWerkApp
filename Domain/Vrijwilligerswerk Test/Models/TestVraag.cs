namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class TestVraag
    {
        public int Id { get; private set; }
        public string Tekst { get; private set; }
        public int CategorieId { get; private set; }

        private TestVraag(int id, string tekst, int categorieId)
        {
            ValideerTestVraag(tekst);
            Id = id;
            Tekst = tekst;
            CategorieId = categorieId;
        }

        public static TestVraag Maak(int id, string tekst, int categorieId)
        {
            return new TestVraag(id, tekst, categorieId);
        }

        private static void ValideerTestVraag(string tekst)
        {
            if (string.IsNullOrWhiteSpace(tekst))
                throw new ArgumentException("Tekst mag niet leeg zijn.");
        }
    }
}
