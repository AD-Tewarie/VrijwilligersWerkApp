using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Domain.Vrijwilligerswerk_Test;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure;
using Domain.Werk.Interfaces;
using Domain.GebruikersTest.Interfaces;
using Domain.Werk.Models;

namespace VrijwilligersWerkApp.Pages.NieuwWerk
{
    public class MaakWerkModel : PageModel
    {
        private readonly IVrijwilligersWerkBeheer werkBeheer;
        private readonly ITestBeheer testBeheer;

        [BindProperty]
        public string Titel { get; set; }

        [BindProperty]
        public string Omschrijving { get; set; }

        [BindProperty]
        public int MaxCapaciteit { get; set; }

        [BindProperty]
        public int CategorieId { get; set; }

        public List<SelectListItem> Categorieën { get; private set; }

        public MaakWerkModel(IVrijwilligersWerkBeheer werkBeheer, ITestBeheer testBeheer)
        {
            this.werkBeheer = werkBeheer;
            this.testBeheer = testBeheer;
        }

        public IActionResult OnGet()
        {
            LaadCategorieën();
            return Page();
        }

        public IActionResult OnPostCreateWork()
        {
            if (!ModelState.IsValid)
            {
                LaadCategorieën();
                return Page();
            }

            try
            {
                var werk = VrijwilligersWerk.MaakNieuw(Titel, Omschrijving, MaxCapaciteit);
                werkBeheer.VoegWerkToe(werk, CategorieId);

                TempData["SuccessMessage"] = "Vrijwilligerswerk succesvol toegevoegd!";

                // Use strong redirect to prevent form resubmission
                return new RedirectToPageResult("/RegistreerWerk/VrijwilligersWerkOverzicht");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Er is een fout opgetreden: {ex.Message}";
                LaadCategorieën();
                return Page();
            }
        }

        private void LaadCategorieën()
        {
            try
            {
                Categorieën = testBeheer.HaalAlleCategorieënOp()
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Naam
                    })
                    .ToList();

                Categorieën.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "-- Selecteer een categorie --",
                    Selected = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Fout bij het laden van categorieën: {ex.Message}");
                Categorieën = new List<SelectListItem>();
            }
        }
    }
}

