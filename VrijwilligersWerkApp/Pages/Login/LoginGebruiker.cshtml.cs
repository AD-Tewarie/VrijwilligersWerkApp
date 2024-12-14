using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace VrijwilligersWerkApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserBeheer userBeheer;

        public LoginModel(IUserBeheer userBeheer)
        {
            this.userBeheer = userBeheer;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is verplicht.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Wachtwoord { get; set; }

        public string FeedbackMessage { get; set; }



        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (userBeheer.ValideerGebruiker(Email, Wachtwoord))
                    {
                        var user = userBeheer.HaalGebruikerOpEmail(Email);

                        HttpContext.Session.SetString("Gebruiker", user.Naam);    
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        FeedbackMessage = "Succesvol ingelogd!";
                        return RedirectToPage("/Home");
                    }
                    FeedbackMessage = "Ongeldige gebruikersnaam of wachtwoord.";
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