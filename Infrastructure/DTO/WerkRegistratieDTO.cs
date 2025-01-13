using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class WerkRegistratieDTO
    {
        public int RegistratieId { get; set; }
        public VrijwilligersWerkDTO VrijwilligersWerk { get; set; }
        public UserDTO User { get; set; }

        public WerkRegistratieDTO(
            VrijwilligersWerkDTO vrijwilligersWerk,
            UserDTO user,
            int registratieId)
        {
            RegistratieId = registratieId;
            VrijwilligersWerk = vrijwilligersWerk;
            User = user;
        }

    }
}
