using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Interfaces;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure;

namespace VrijwilligersWerkApp.Pages.NieuwWerk
{
    public class MaakWerkModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly ITestBeheer testbeheer;

        [BindProperty]
        [Required(ErrorMessage = "Titel is verplicht.")]
        public string Titel { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Omschrijving is verplicht.")]
        public string Omschrijving { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Maximale capaciteit is verplicht.")]
        public int MaxCapaciteit { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Categorie is verplicht.")]
        public int CategorieId { get; set; }

        public List<SelectListItem> Categorieën { get; set; }

        public string FeedbackMessage { get; set; }

        public MaakWerkModel(IVrijwilligersWerkBeheer werkBeheer, ITestBeheer testBeheer)
        {
            this.werkBeheer = werkBeheer;
            this.testbeheer = testBeheer;
        }

       

        public void OnGet()
        {
            LaadCategorieën();
        }

        


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var werk = new VrijwilligersWerk(Titel, Omschrijving, MaxCapaciteit);
                    
                    werkBeheer.VoegWerkToe(werk, CategorieId);
                    FeedbackMessage = "Vrijwilligerswerk succesvol toegevoegd!";
                    LaadCategorieën();
                    return Page();
                }
                catch (Exception ex)
                {
                    FeedbackMessage = $"Er is een fout opgetreden: {ex.Message}";
                    LaadCategorieën();
                    return Page();
                }
            }

            LaadCategorieën();
            return Page();

            
        }


        // Methode voor het laden van de categorieën voor form select

        private void LaadCategorieën()
        {
            Categorieën = testbeheer.HaalAlleCategorieënOp()
                                                 .Select(item => new SelectListItem
                                                 {
                                                     Value = item.Id.ToString(),
                                                     Text = item.Naam.ToString()
                                                 })
                                                 .ToList();
        }
    }
}
