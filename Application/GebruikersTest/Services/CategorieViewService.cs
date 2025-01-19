using Application.GebruikersTest.Interfaces;

namespace Application.GebruikersTest.Services
{
    public class CategorieViewService : ICategorieViewService
    {
        private readonly Domain.GebruikersTest.Interfaces.ICategorieService categorieService;

        public CategorieViewService(Domain.GebruikersTest.Interfaces.ICategorieService categorieService)
        {
            this.categorieService = categorieService ?? throw new ArgumentNullException(nameof(categorieService));
        }

        public string GetCategorieNaam(int categorieId)
        {
            var categorie = categorieService.GetCategorieOpId(categorieId);
            return categorie?.Naam ?? string.Empty;
        }
    }
}