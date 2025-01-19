using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection VoegInfrastructuurToe(
            this IServiceCollection services,
            IConfiguration configuratie)
        {
            // Volgens Clean Architecture:
            // - Infrastructure is de buitenste laag
            // - Implementeert interfaces uit de Domain laag
            // - Bevat technische details zoals databases, externe services
            // - Afhankelijk van Domain interfaces, niet van Application

            // Registreer Infrastructure services via de installer
            // InfrastructureServiceInstaller bevat:
            // - Database configuratie en connecties
            // - Repository implementaties van Domain interfaces
            // - Externe service integraties
            var installer = new InfrastructureServiceInstaller();
            installer.Installeer(services, configuratie);

            return services;
        }
    }
}