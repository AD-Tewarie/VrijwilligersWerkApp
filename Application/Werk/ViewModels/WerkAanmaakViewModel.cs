namespace Application.Werk.ViewModels
{
    public class WerkAanmaakViewModel
    {
        public required string Titel { get; set; }
        public required string Omschrijving { get; set; }
        public int MaxCapaciteit { get; set; }
        public required string Locatie { get; set; }
        public int CategorieId { get; set; }
    }
}