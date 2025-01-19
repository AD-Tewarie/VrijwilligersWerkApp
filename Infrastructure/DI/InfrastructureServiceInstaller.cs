﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.DI;
using Domain.Common.Interfaces;
using Domain.Common.Interfaces.Repository;
using Domain.GebruikersTest.Interfaces;
using Infrastructure.Repos_DB;
using Microsoft.Extensions.Options;
using Infrastructure.Repos_DB.Settings;
using Infrastructure.Session;
using Microsoft.AspNetCore.Http;
using Domain.Werk.Interfaces;

namespace Infrastructure.DI
{
    public class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Installeer(IServiceCollection services, IConfiguration configuratie)
        {
            // Database Configuratie
            services.Configure<DBSettings>(
                configuratie.GetSection("ConnectionStrings"));

            // Database Service
            services.AddScoped<IDatabaseService>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<DBSettings>>().Value;
                return new DatabaseService(settings.DefaultConnection);
            });

            // Repositories
            services.AddScoped<IUserRepository>(sp =>
                new UserRepositoryDB(
                    sp.GetRequiredService<IDatabaseService>()
                ));

            services.AddScoped<IVrijwilligersWerkRepository>(sp =>
                new VrijwilligersWerkRepositoryDB(
                    sp.GetRequiredService<IDatabaseService>()
                ));

            services.AddScoped<IWerkRegistratieRepository>(sp =>
                new WerkRegistratieRepositoryDB(
                    sp.GetRequiredService<IDatabaseService>(),
                    sp.GetRequiredService<IVrijwilligersWerkRepository>(),
                    sp.GetRequiredService<IUserRepository>()
                ));

            services.AddScoped<IGebruikersTestRepository>(sp =>
                new GebruikersTestRepositoryDB(
                    sp.GetRequiredService<IDatabaseService>()
                ));

            services.AddScoped<IWerkCategorieRepository>(sp =>
                new WerkCategorieRepositoryDB(
                    sp.GetRequiredService<IDatabaseService>()
                ));

            // Http Context
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Session Services
            services.AddScoped<ITestSessieBeheer>(sp => new HttpSessionTestSessieBeheer(
                sp.GetRequiredService<IHttpContextAccessor>(),
                sp.GetRequiredService<IGebruikersTestRepository>()
            ));
        }
    }
}
