using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Models;

namespace Domain.GebruikersTest.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly IGebruikersTestRepository repository;

        public CategorieService(IGebruikersTestRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<Categorie> HaalCategorieënVoorWerkOp(int werkId)
        {
            ValideerWerkId(werkId);

            var werkCategorieën = repository.GetWerkCategorieënByWerkId(werkId);

            return werkCategorieën
                .Select(wc => GetCategorieOpId(wc.CategorieId))
                .Where(c => c != null)
                .ToList();
        }

        public List<Categorie> HaalAlleCategorieënOp()
        {
            return repository.HaalAlleCategorieënOp();
        }

        public Categorie GetCategorieOpId(int id)
        {
            ValideerCategorieId(id);
            var categorie = repository.GetCategorieOnId(id);

            if (categorie == null)
                throw new KeyNotFoundException($"Categorie met ID {id} niet gevonden.");

            return categorie;
        }

        private void ValideerWerkId(int werkId)
        {
            if (werkId <= 0)
                throw new ArgumentException("Werk ID moet groter zijn dan 0.");
        }

        private void ValideerCategorieId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Categorie ID moet groter zijn dan 0.");
        }
    }
}
