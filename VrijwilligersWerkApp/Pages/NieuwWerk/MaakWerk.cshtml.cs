using Application.GebruikersTest.Interfaces;
using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VrijwilligersWerkApp.Pages.NieuwWerk
{
    public class MaakWerkModel : PageModel
    {
        private readonly IWerkBeheerService werkBeheerService;
        private readonly ITestCategorieService testCategorieService;
        private readonly ILogger<MaakWerkModel> logger;

        [BindProperty]
        public WerkAanmaakViewModel WerkModel { get; set; } = new()
        {
            Titel = string.Empty,
            Omschrijving = string.Empty,
            Locatie = string.Empty,
            MaxCapaciteit = 0,
            CategorieId = 0
        };

        public List<SelectListItem> Categorieën { get; private set; } = new();

        public MaakWerkModel(
            IWerkBeheerService werkBeheerService,
            ITestCategorieService testCategorieService,
            ILogger<MaakWerkModel> logger)
        {
            this.werkBeheerService = werkBeheerService ?? throw new ArgumentNullException(nameof(werkBeheerService));
            this.testCategorieService = testCategorieService ?? throw new ArgumentNullException(nameof(testCategorieService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                werkBeheerService.VoegWerkToe(WerkModel);
                TempData["SuccessMessage"] = "Vrijwilligerswerk succesvol toegevoegd!";
                return RedirectToPage("/RegistreerWerk/VrijwilligersWerkOverzicht");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij toevoegen vrijwilligerswerk");
                TempData["ErrorMessage"] = $"Er is een fout opgetreden: {ex.Message}";
                LaadCategorieën();
                return Page();
            }
        }

        private void LaadCategorieën()
        {
            try
            {
                var categories = testCategorieService.HaalAlleCategorieënOp();
                Categorieën = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Naam
                }).ToList();

                Categorieën.Insert(0, new SelectListItem
                {
                    Value = "0",
                    Text = "-- Selecteer een categorie --",
                    Selected = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij laden categorieën");
                ModelState.AddModelError(string.Empty, "Fout bij het laden van categorieën");
                Categorieën = new List<SelectListItem>();
            }
        }
    }
}
