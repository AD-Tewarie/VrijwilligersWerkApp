using Domain.Gebruikers.Interfaces;
using Domain.Gebruikers.Services;
using Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces;
using Domain.Gebruikers.Services.WachtwoordStrategy;
using Domain.GebruikersTest.Interfaces;
using Domain.GebruikersTest.Services;
using Domain.GebruikersTest.WerkScore;
using Domain.Werk.Interfaces;
using Domain.Werk.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DI
{
    public class DomainServiceInstaller : IServiceInstaller
    {
        public void Installeer(IServiceCollection services, IConfiguration configuratie)
        {
            // Werk Services
            services.AddScoped<IVrijwilligersWerkBeheer, VrijwilligersWerkBeheer>();
            services.AddScoped<IRegistratieBeheer, RegistratieBeheer>();

            // User Services
            services.AddScoped<IUserBeheer, UserBeheer>();
            services.AddScoped<IWachtwoordStrategy, DefaultWachtwoordStrategy>();

            // Score Services
            services.AddScoped<StandaardScoreStrategy>();
            services.AddScoped<IScoreStrategy>(sp => sp.GetRequiredService<StandaardScoreStrategy>());
            services.AddScoped<IWerkScoreService, StandaardWerkScoreService>();

            // Test Services
            services.AddScoped<ITestBeheer, TestBeheer>();
            services.AddScoped<ICategorieService, CategorieService>();
            services.AddScoped<ITestVraagService, TestVraagService>();
        }
    }
}