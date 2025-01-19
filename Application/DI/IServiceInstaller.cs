using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public interface IServiceInstaller
    {
        void Installeer(IServiceCollection services, IConfiguration configuratie);
    }
}
