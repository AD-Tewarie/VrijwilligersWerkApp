using Infrastructure.DTO;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class WerkRegistratie
    {


        private VrijwilligersWerk vrijwilligersWerk;

        private User user;

        private int registratieId;



        public WerkRegistratie(VrijwilligersWerk vrijwilligersWerk, User user, int registratieId)
        {
            this.vrijwilligersWerk = vrijwilligersWerk;
            this.user = user;
            this.registratieId = registratieId;
        }


        public int RegistratieId
        {
            get { return registratieId; }
            set { registratieId = value; }
        }

        public VrijwilligersWerk VrijwilligersWerk
        {
            get { return vrijwilligersWerk; }
            set { vrijwilligersWerk = value; }
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

    }
}
