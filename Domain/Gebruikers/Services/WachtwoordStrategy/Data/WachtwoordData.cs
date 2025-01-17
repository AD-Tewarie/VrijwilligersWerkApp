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


    }
}
