using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Data
{
    public class WerkRegistratieData
    {
        public int RegistratieId { get; set; }
        public WerkData Werk { get; set; }
        public UserData User { get; set; }

        public WerkRegistratieData(
            int registratieId,
            WerkData werk,
            UserData user)
        {
            RegistratieId = registratieId;
            Werk = werk;
            User = user;
        }
    }
}
