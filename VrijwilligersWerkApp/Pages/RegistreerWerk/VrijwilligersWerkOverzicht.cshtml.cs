using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;


namespace VrijwilligersWerkApp.Pages.RegistreerWerk
{
    public class VrijwilligersWerkOverzichtModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly IRegistratieBeheer registratieBeheer;

        public List<VrijwilligersWerk> werkAanbiedingen { get; set; }
        public string FeedbackMessage { get; set; }

        public VrijwilligersWerkOverzichtModel(IVrijwilligersWerkBeheer werkBeheer, IRegistratieBeheer registratieBeheer)
        {
            this.werkBeheer = werkBeheer;
            this.registratieBeheer = registratieBeheer;
        }

        public void OnGet()
        {
            // Fetch all job offers
            werkAanbiedingen = werkBeheer.BekijkAlleWerk().ToList();
        }

        public IActionResult OnPostApply(int id)
        {
            var selectedJob = werkBeheer.HaalwerkOpID(id);
            if (selectedJob != null)
            {
                try
                {
                    
                    registratieBeheer.RegistreerGebruikerVoorWerk(GetCurrentUserId(), id);

                    FeedbackMessage = "Je aanmelding is succesvol geregistreerd!";
                    return Page();
                }
                catch (Exception ex)
                {
                    FeedbackMessage = $"Er is een fout opgetreden bij de aanmelding: {ex.Message}";
                    return Page();
                }
            }

            FeedbackMessage = "Registratie Mislukt.";
            return Page();
        }

        private int GetCurrentUserId()
        {
            //test zonder inlogsysteem
            return 1;
        }
    }
}
