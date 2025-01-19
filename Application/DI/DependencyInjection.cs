using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection VoegApplicationToe(
            this IServiceCollection services, 
            IConfiguration configuratie)
        {
            // Zoek en voer alle IServiceInstaller implementaties uit in de Application laag
            // Volgens Clean Architecture:
            // - Application laag is afhankelijk van Domain laag
            // - Coördineert tussen UI en domeinlogica
            // - Bevat use cases en applicatie-specifieke logica
            var installeerders = typeof(DependencyInjection).Assembly
                .ExportedTypes
                .Where(x => 
                    // Zoek alle types die IServiceInstaller implementeren
                    typeof(IServiceInstaller).IsAssignableFrom(x) && 
                    // Alleen concrete klassen, geen interfaces of abstracte klassen
                    !x.IsInterface && 
                    !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceInstaller>();

            // Voer elke installer uit om services te registreren
            // ApplicationServiceInstaller registreert:
            // - Services die use cases implementeren
            // - Mappers tussen ViewModels en Domain models
            // - Coördinerende services die Domain services gebruiken
            foreach (var installeerder in installeerders)
            {
                installeerder.Installeer(services, configuratie);
            }

            return services;
        }
    }
}
