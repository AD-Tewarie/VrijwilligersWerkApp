using Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Gebruikers.Services.WachtwoordStrategy.Data
{
    public class WachtwoordData
    {
        public string Hash { get; }
        public string Salt { get; }

        public WachtwoordData(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;

        }


        private void ValideerWachtwoord(string wachtwoord)
        {
            var fouten = new Dictionary<string, ICollection<string>>();

            if (string.IsNullOrWhiteSpace(wachtwoord))
                fouten.Add("Wachtwoord", new[] { "Wachtwoord is verplicht." });
            else if (wachtwoord.Length < 8)
                fouten.Add("Wachtwoord", new[] { "Wachtwoord moet minimaal 8 karakters lang zijn." });

            if (fouten.Any())
                throw new DomainValidationException("Validatie fouten opgetreden", fouten);
        }

    }
}
