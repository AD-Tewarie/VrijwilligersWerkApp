namespace Application.Werk.ViewModels
{
    public class RegistratieResultaatViewModel
    {
        public bool IsSuccesvol { get; private set; }
        public string Melding { get; private set; }

        public RegistratieResultaatViewModel(bool isSuccesvol, string melding)
        {
            IsSuccesvol = isSuccesvol;
            Melding = melding;
        }

        public static RegistratieResultaatViewModel SuccesVol(string melding = "Registratie succesvol.")
        {
            return new RegistratieResultaatViewModel(true, melding);
        }

        public static RegistratieResultaatViewModel Mislukt(string melding)
        {
            return new RegistratieResultaatViewModel(false, melding);
        }
    }
}