using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VrijwilligersWerkApp.Pages.MijnProfiel
{
    public class GebruikersProfielModel : PageModel
    {
        private readonly IUserBeheer userBeheer;
        private readonly IRegistratieBeheer registratieBeheer;

        public User HuidigeGebruiker { get; set; }
        public List<WerkRegistratie> Registraties { get; set; }
        public string SuccesMessage { get; set; }
        public string ErrorMessage { get; set; }

        public GebruikersProfielModel(IUserBeheer userBeheer, IRegistratieBeheer registratieBeheer)
        {
            this.userBeheer = userBeheer;
            this.registratieBeheer = registratieBeheer;
        }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                // Haal gebruiker op
                HuidigeGebruiker = userBeheer.HaalGebruikerOpID(userId.Value);
                if (HuidigeGebruiker == null)
                {
                    throw new Exception("Gebruiker niet gevonden");
                }

                // Haal registraties op
                Registraties = registratieBeheer.HaalRegistratiesOp()
                    .Where(r => r.User.UserId == userId.Value)
                    .ToList();

                // Haal eventuele feedback messages op uit TempData
                if (TempData["SuccesMessage"] != null)
                {
                    SuccesMessage = TempData["SuccesMessage"].ToString();
                }
                if (TempData["ErrorMessage"] != null)
                {
                    ErrorMessage = TempData["ErrorMessage"].ToString();
                }

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Er is een fout opgetreden: {ex.Message}";
                return Page();
            }
        }

        public IActionResult OnPostCancelRegistration(int registratieId)
        {
            try
            {
                registratieBeheer.VerwijderRegistratie(registratieId);
                TempData["SuccesMessage"] = "Registratie succesvol geannuleerd.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Fout bij het annuleren van de registratie: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
