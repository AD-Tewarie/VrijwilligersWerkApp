using Domain.PasswordStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.WachtwoordStrategy
{
    public class DefaultWachtwoordStrategy : IWachtwoordStrategy
    {
        private const int SALT_SIZE = 16;

        public WachtwoordData Hash(string wachtwoord)
        {
            if (string.IsNullOrWhiteSpace(wachtwoord))
                throw new ArgumentException("Wachtwoord mag niet leeg zijn.", nameof(wachtwoord));

            var salt = GenereerSalt();
            var hash = BerekenHash(wachtwoord, salt);

            return new WachtwoordData(hash, salt);
        }

        public bool Valideer(string ingevoerdWachtwoord, WachtwoordData opgeslagenData)
        {
            if (string.IsNullOrWhiteSpace(ingevoerdWachtwoord))
                throw new ArgumentException("Wachtwoord mag niet leeg zijn.", nameof(ingevoerdWachtwoord));

            if (opgeslagenData == null)
                throw new ArgumentNullException(nameof(opgeslagenData));

            var berekendeHash = BerekenHash(ingevoerdWachtwoord, opgeslagenData.Salt);
            return berekendeHash == opgeslagenData.Hash;
        }

        private string BerekenHash(string wachtwoord, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = Encoding.UTF8.GetBytes(salt + wachtwoord);
                var hash = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hash);
            }
        }

        private string GenereerSalt()
        {
            var salt = new byte[SALT_SIZE];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}
