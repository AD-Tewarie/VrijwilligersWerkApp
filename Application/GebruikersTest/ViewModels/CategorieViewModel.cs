namespace Application.GebruikersTest.ViewModels
{
    public class CategorieViewModel
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }

        public CategorieViewModel(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }
    }
}