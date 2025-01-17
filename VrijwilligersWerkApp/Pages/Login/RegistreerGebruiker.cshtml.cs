using Domain;
using Domain.Gebruikers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace VrijwilligersWerkApp.Pages.Login
{
    public class RegistreerGebruikerModel : PageModel
    {
        private readonly IUserBeheer userBeheer;

        [BindProperty]
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string Naam { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Wachtwoord { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="Email is verplicht")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string Achternaam { get; set; }

        public string FeedbackMessage { get; set; }



        public RegistreerGebruikerModel(IUserBeheer userBeheer)
        {
            this.userBeheer = userBeheer;
        }



        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    userBeheer.VoegGebruikerToe(Naam, Achternaam, Email, Wachtwoord); 
                    FeedbackMessage = "Registratie succesvol!";
                    return RedirectToPage("/Login/LoginGebruiker");
                }
                catch (Exception ex)
                {
                    FeedbackMessage = $"Fout tijdens registratie: {ex.Message}";
                }
            }
            return Page();
        }
    }
}
