using Application.Authenticatie.Interfaces;
using Domain.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace VrijwilligersWerkApp.Pages.Login
{
    public class RegistreerGebruikerModel : PageModel
    {
        [BindProperty]
        public string Naam { get; set; }

        [BindProperty]
        public string Wachtwoord { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Achternaam { get; set; }

        public string FeedbackMessage { get; set; }

        private readonly IAuthenticatieService authenticatieService;
        private readonly ILogger<RegistreerGebruikerModel> logger;

        public RegistreerGebruikerModel(
            IAuthenticatieService authenticatieService,
            ILogger<RegistreerGebruikerModel> logger)
        {
            this.authenticatieService = authenticatieService;
            this.logger = logger;
        }

        public IActionResult OnPost()
        {
            try
            {
                if (authenticatieService.BestaatEmail(Email))
                {
                    ModelState.AddModelError("Email", "Dit emailadres is al in gebruik.");
                    return Page();
                }

                authenticatieService.Registreer(Naam, Achternaam, Email, Wachtwoord);
                return RedirectToPage("/Login/LoginGebruiker");
            }
            catch (DomainValidationException ex)
            {
                foreach (var error in ex.ValidatieFouten)
                {
                    foreach (var message in error.Value)
                    {
                        ModelState.AddModelError(error.Key, message);
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout tijdens registratie voor gebruiker {Email}", Email);
                ModelState.AddModelError(string.Empty, "Er is een fout opgetreden tijdens de registratie.");
                return Page();
            }
        }
    }
}
