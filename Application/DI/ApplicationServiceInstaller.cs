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
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Services;
using Domain.GebruikersTest.WerkScore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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

            // Test Services
            services.AddScoped<ITestVoortgangService, TestVoortgangService>();
            services.AddScoped<ITestResultaatService, GebruikersTestResultaatService>();
            services.AddScoped<ITestCategorieService, TestCategorieService>();
            services.AddScoped<ICategorieViewService, CategorieViewService>();

            // Score & Matching Services
            services.AddScoped<IWerkMatchingService, WerkMatchingService>();
            services.AddScoped<IWerkPresentatieFilterService, WerkPresentatieFilterService>();
            services.AddScoped<IWerkAanbevelingService, WerkAanbevelingService>();
            services.AddScoped<IMatchBerekeningService, MatchBerekeningService>();
            services.AddScoped<IWerkVerzamelService, WerkVerzamelService>();

            // Score Strategy
            services.AddScoped<StandaardScoreStrategy>();
            services.AddScoped<IScoreStrategy>(sp => sp.GetRequiredService<StandaardScoreStrategy>());
            services.AddScoped<IWerkScoreService, StandaardWerkScoreService>();

            // Work Services
            services.AddScoped<IWerkBeheerService, WerkBeheerService>();
            services.AddScoped<IWerkRegistratieBeheerService, WerkRegistratieBeheerService>();
            services.AddScoped<IWerkRegistratieOverzichtService, WerkRegistratieOverzichtService>();

            // User Services
            services.AddScoped<IGebruikersProfielService, GebruikersProfielService>();
            services.AddScoped<IAuthenticatieService, AuthenticatieService>();

            
        }
    }
}