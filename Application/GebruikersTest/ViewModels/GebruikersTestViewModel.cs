using System.Collections.Generic;

namespace Application.GebruikersTest.ViewModels
{
    public class GebruikersTestViewModel
    {
        public int GebruikerId { get; set; }
        public string HuidigeVraag { get; set; } = string.Empty;
        public bool IsVoltooid { get; set; }
        public int VoortgangPercentage { get; set; }
        public bool IsLaatsteVraag { get; set; }
        public int HuidigeStap { get; set; }
        public bool HeeftBestaandeResultaten { get; set; }
        public bool IsTestNetVoltooid { get; set; }
        public List<TestVraagViewModel> Vragen { get; set; } = new();
        public Dictionary<int, int> Antwoorden { get; set; } = new();
    }
}