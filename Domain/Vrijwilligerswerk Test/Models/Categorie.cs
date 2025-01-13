namespace Domain.Vrijwilligerswerk_Test
{
    public class Categorie
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }

        private Categorie(int id, string naam)
        {
            ValideerCategorie(naam);
            Id = id;
            Naam = naam;
        }

        public static Categorie Maak(int id, string naam)
        {
            return new Categorie(id, naam);
        }

        private static void ValideerCategorie(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam))
                throw new ArgumentException("Naam mag niet leeg zijn.");
        }
    }
}
