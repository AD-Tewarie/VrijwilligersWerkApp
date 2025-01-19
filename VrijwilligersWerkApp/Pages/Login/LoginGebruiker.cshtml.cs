using Application.Authenticatie.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace VrijwilligersWerkApp.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticatieService authenticatieService;
        private readonly ILogger<LoginModel> logger;

        [BindProperty]
        [Required(ErrorMessage = "Email is verplicht.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Wachtwoord { get; set; }

        public string FeedbackMessage { get; set; }

        public LoginModel(
            IAuthenticatieService authenticatieService,
            ILogger<LoginModel> logger)
        {
            this.authenticatieService = authenticatieService ?? throw new ArgumentNullException(nameof(authenticatieService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (authenticatieService.Login(Email, Wachtwoord))
                    {
                        var gebruiker = authenticatieService.HaalGebruikerOpEmail(Email);
                        HttpContext.Session.SetString("Gebruiker", gebruiker.Naam);    
                        HttpContext.Session.SetInt32("UserId", gebruiker.UserId);
                        FeedbackMessage = "Succesvol ingelogd!";
                        return RedirectToPage("/Home");
                    }
                    FeedbackMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Fout tijdens inloggen voor gebruiker {Email}", Email);
                    FeedbackMessage = "Er is een fout opgetreden tijdens het inloggen.";
                }
            }

            return Page();
        }
    }
}