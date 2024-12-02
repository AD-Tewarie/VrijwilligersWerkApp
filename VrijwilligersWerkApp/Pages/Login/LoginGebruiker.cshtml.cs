using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;

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
        public string Naam { get; set; }

        [BindProperty]
        public string Achternaam { get; set; }

        public string FeedbackMessage { get; set; }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    var users = userBeheer.GetAllUsers();

                   // trying linq
                    var matchingUser = users.FirstOrDefault(
                        u => u.Naam == Naam && u.AchterNaam == Achternaam
                    );

                    if (matchingUser != null)
                    {
                        
                        HttpContext.Session.SetInt32("UserId", matchingUser.UserId);
                        FeedbackMessage = "Succesvol ingelogd!";
                        return RedirectToPage("/Home"); 
                    }

                    FeedbackMessage = "Ongeldige gebruikersnaam of achternaam.";
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