namespace Application.Werk.ViewModels
{
    public class WerkAanbiedingOverzichtViewModel
    {
        public int WerkId { get; set; }
        public required string Titel { get; set; }
        public required string Omschrijving { get; set; }
        public int MaxCapaciteit { get; set; }
        public int AantalRegistraties { get; set; }
        public required string Locatie { get; set; }
        public bool IsVol => AantalRegistraties >= MaxCapaciteit;
    }
}