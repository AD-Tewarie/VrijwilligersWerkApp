using Application.GebruikersProfiel.Interfaces;
using Application.GebruikersProfiel.ViewModels;
using Application.Werk.Interfaces;
using Application.Werk.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VrijwilligersWerkApp.Pages.MijnProfiel
{
    public class GebruikersProfielModel : PageModel
    {
        private readonly IGebruikersProfielService gebruikersProfielService;
        private readonly IWerkRegistratieBeheerService werkRegistratieBeheerService;
        private readonly IWerkRegistratieOverzichtService werkRegistratieOverzichtService;
        private readonly ILogger<GebruikersProfielModel> logger;

        public GebruikersProfielViewModel ProfielData { get; private set; }
        public List<WerkRegistratieViewModel> Registraties { get; private set; }

        public GebruikersProfielModel(
            IGebruikersProfielService gebruikersProfielService,
            IWerkRegistratieBeheerService werkRegistratieBeheerService,
            IWerkRegistratieOverzichtService werkRegistratieOverzichtService,
            ILogger<GebruikersProfielModel> logger)
        {
            this.gebruikersProfielService = gebruikersProfielService ?? throw new ArgumentNullException(nameof(gebruikersProfielService));
            this.werkRegistratieBeheerService = werkRegistratieBeheerService ?? throw new ArgumentNullException(nameof(werkRegistratieBeheerService));
            this.werkRegistratieOverzichtService = werkRegistratieOverzichtService ?? throw new ArgumentNullException(nameof(werkRegistratieOverzichtService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult OnGet()
        {
            var gebruikerId = HttpContext.Session.GetInt32("UserId");
            if (!gebruikerId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                if (!gebruikersProfielService.BestaatGebruiker(gebruikerId.Value))
                {
                    return RedirectToPage("/Login/LoginGebruiker");
                }

                ProfielData = gebruikersProfielService.HaalProfielOp(gebruikerId.Value);
                Registraties = werkRegistratieOverzichtService.HaalRegistratiesOp(gebruikerId.Value);
                return Page();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij ophalen gebruikersprofiel voor gebruiker {GebruikerId}", gebruikerId);
                TempData["ErrorMessage"] = "Er is een fout opgetreden bij het ophalen van uw profiel.";
                return RedirectToPage("/Home");
            }
        }

        public IActionResult OnPostAnnuleerRegistratie(int werkId)
        {
            var gebruikerId = HttpContext.Session.GetInt32("UserId");
            if (!gebruikerId.HasValue)
            {
                return RedirectToPage("/Login/LoginGebruiker");
            }

            try
            {
                var result = werkRegistratieBeheerService.TrekRegistratieIn(werkId, gebruikerId.Value);
                if (result.IsSuccesvol)
                {
                    TempData["SuccessMessage"] = result.Melding;
                }
                else
                {
                    TempData["ErrorMessage"] = result.Melding;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij annuleren registratie voor werk {WerkId} door gebruiker {GebruikerId}", werkId, gebruikerId);
                TempData["ErrorMessage"] = "Er is een fout opgetreden bij het annuleren van de registratie.";
            }

            return RedirectToPage();
        }
    }
}
