using Application.GebruikersTest.Interfaces;
using Application.GebruikersTest.Mappers;
using Application.GebruikersTest.Services;
using Application.GebruikersProfiel.Interfaces;
using Application.GebruikersProfiel.Services;
using Application.Werk.Interfaces;
using Application.Werk.Services;
using Application.Authenticatie.Interfaces;
using Application.Authenticatie.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Installeer(IServiceCollection services, IConfiguration configuratie)
        {
            // Mappers
            services.AddScoped<IGebruikersTestMapper, GebruikersTestMapper>();
            services.AddScoped<ITestVraagMapper, TestVraagMapper>();
            services.AddScoped<IWerkAanbevelingMapper, WerkAanbevelingMapper>();

            // Application Services
            services.AddScoped<ITestVoortgangService, TestVoortgangService>();
            services.AddScoped<IGebruikersTestResultaatService, GebruikersTestResultaatService>();
            services.AddScoped<ITestCategorieService, TestCategorieService>();
            services.AddScoped<ICategorieViewService, CategorieViewService>();
            services.AddScoped<IWerkMatchingService, WerkMatchingService>();
            services.AddScoped<IWerkPresentatieFilterService, WerkPresentatieFilterService>();
            services.AddScoped<IWerkAanbevelingService, WerkAanbevelingService>();
            services.AddScoped<IMatchBerekeningService, MatchBerekeningService>();
            services.AddScoped<IWerkVerzamelService, WerkVerzamelService>();
            services.AddScoped<IWerkBeheerService, WerkBeheerService>();
            services.AddScoped<IWerkRegistratieBeheerService, WerkRegistratieBeheerService>();
            services.AddScoped<IWerkRegistratieOverzichtService, WerkRegistratieOverzichtService>();
            services.AddScoped<IGebruikersProfielService, GebruikersProfielService>();
            services.AddScoped<IAuthenticatieService, AuthenticatieService>();
            services.AddScoped<ITestResultaatService, GebruikersTestResultaatService>();
        }
    }
}