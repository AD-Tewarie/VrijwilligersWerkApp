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
            
            werkAanbiedingen = werkBeheer.BekijkAlleWerk().ToList();
        }

        public IActionResult OnPostApply(int id)
        {
            
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                FeedbackMessage = "U moet ingelogd zijn om u te registreren.";
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                
                registratieBeheer.RegistreerGebruikerVoorWerk(userId.Value, id);
                FeedbackMessage = "Registratie succesvol!";
                return Page();
            }
            catch (Exception ex)
            {
                FeedbackMessage = $"Er is een fout opgetreden: {ex.Message}";
            }

            return Page();
        }

      
    }
}
