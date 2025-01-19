namespace Application.GebruikersTest.ViewModels
{
    public class TestVraagViewModel
    {
        public int Id { get; set; }
        public string Tekst { get; set; } = string.Empty;
        public int CategorieId { get; set; }
        public string CategorieName { get; set; } = string.Empty;

        // Factory method 
        public static TestVraagViewModel MaakVanCategorie(
            int id,
            string naam,
            int vraagNummer,
            int totaalVragen,
            int? gekozenAntwoord = null)
        {
            return new TestVraagViewModel
            {
                Id = id,
                Tekst = $"Hoe belangrijk vind je {naam}?",
                CategorieId = id,
                CategorieName = naam,
                VraagNummer = vraagNummer,
                TotaalVragen = totaalVragen,
                GekozenAntwoord = gekozenAntwoord
            };
        }

        // Presentation properties
        public int VraagNummer { get; set; }
        public int TotaalVragen { get; set; }
        public int? GekozenAntwoord { get; set; }
    }
}