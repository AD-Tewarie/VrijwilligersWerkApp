using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;

namespace VrijwilligersWerkApp.Pages.NieuwWerk
{
    public class MaakWerkModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;

        [BindProperty]
        public string Titel { get; set; }

        [BindProperty]
        public string Omschrijving { get; set; }

        [BindProperty]
        public int MaxCapaciteit { get; set; }

        public string FeedbackMessage { get; set; }

        public MaakWerkModel(IVrijwilligersWerkBeheer werkBeheer)
        {
            this.werkBeheer = werkBeheer;
        }

       

        public void OnGet()
        {
            // Default page load logic
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var werk = new VrijwilligersWerk(werkBeheer.GetNieweWerkId(), Titel, Omschrijving, MaxCapaciteit);
                    
                    werkBeheer.VoegWerkToe(werk);
                    FeedbackMessage = "Vrijwilligerswerk succesvol toegevoegd!";
                    return Page();
                }
                catch (Exception ex)
                {
                    FeedbackMessage = $"Er is een fout opgetreden: {ex.Message}";
                }
            }

            return Page();
        }
    }
}
