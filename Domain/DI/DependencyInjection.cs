using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection VoegDomainToe(
            this IServiceCollection services,
            IConfiguration configuratie)
        {
            // Registreer de Domain layer services via de installer
            var installer = new DomainServiceInstaller();
            installer.Installeer(services, configuratie);

            return services;
        }
    }
}