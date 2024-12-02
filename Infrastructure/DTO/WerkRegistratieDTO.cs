using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class WerkRegistratieDTO
    {
        private VrijwilligersWerkDTO vrijwilligersWerk;
        private UserDTO user;
        private int registratieId { get; set; }



        public WerkRegistratieDTO(VrijwilligersWerkDTO werk, UserDTO user, int registratieId)
        {
            this.vrijwilligersWerk = werk;
            this.user = user;
            this.registratieId = registratieId;
        }

        public int RegistratieId
        {
            get { return registratieId; }
            set { registratieId = value; }
        }

        public VrijwilligersWerkDTO VrijwilligersWerk
        {
            get { return vrijwilligersWerk; }
            set { vrijwilligersWerk = value; }
        }

        public UserDTO User
        {
            get { return user; }
            set { user = value; }
        }

    }
}
