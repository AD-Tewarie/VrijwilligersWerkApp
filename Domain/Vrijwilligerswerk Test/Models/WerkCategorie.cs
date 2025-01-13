using Domain.Models;

namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class WerkCategorie
    {
        public int WerkId { get; private set; }
        public int CategorieId { get; private set; }

        private WerkCategorie(int werkId, int categorieId)
        {
            ValideerWerkCategorie(werkId, categorieId);
            WerkId = werkId;
            CategorieId = categorieId;
        }

        public static WerkCategorie Maak(int werkId, int categorieId)
        {
            return new WerkCategorie(werkId, categorieId);
        }

        private static void ValideerWerkCategorie(int werkId, int categorieId)
        {
            var fouten = new List<string>();

            if (werkId <= 0)
                fouten.Add("WerkId moet groter zijn dan 0.");

            if (categorieId <= 0)
                fouten.Add("CategorieId moet groter zijn dan 0.");

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }


    }
}
