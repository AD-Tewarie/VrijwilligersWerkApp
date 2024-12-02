using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class VrijwilligersWerkDTO
    {
        private int werkId { get; set; }
        private string titel { get; set; }
        private string omschrijving { get; set; }
        private int maxCapaciteit { get; set; }
        private int aantalRegistraties { get; set; }


        public VrijwilligersWerkDTO(int werkId, string titel, string omschrijving, int maxCapaciteit) 
        {   
            this.werkId = werkId;
            this.titel = titel;
            this.omschrijving = omschrijving;
            this.maxCapaciteit = maxCapaciteit;
            
     
            
        }


        public int WerkId
        {
            get { return werkId; } 
            set { werkId = value; } 
        }

        public string Titel
        {
            get { return titel; }
            set { titel = value; }
        }

        public string Omschrijving
        {
            get { return omschrijving; }
            set { omschrijving = value; }
        }

        public int MaxCapaciteit
        {
            get { return maxCapaciteit; }
            set { maxCapaciteit = value; }
        }

        public int Aantalregistraties
        {
            get { return aantalRegistraties; }
            set { aantalRegistraties = value; }
        }

      






    }
}
