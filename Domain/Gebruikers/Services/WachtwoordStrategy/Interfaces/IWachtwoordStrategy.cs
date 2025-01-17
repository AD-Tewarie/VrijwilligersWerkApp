using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Gebruikers.Services.WachtwoordStrategy.Data;

namespace Domain.Gebruikers.Services.WachtwoordStrategy.Interfaces
{
    public interface IWachtwoordStrategy
    {
        WachtwoordData Hash(string wachtwoord);
        bool Valideer(string ingevoerdWachtwoord, WachtwoordData opgeslagenData);
    }
}
