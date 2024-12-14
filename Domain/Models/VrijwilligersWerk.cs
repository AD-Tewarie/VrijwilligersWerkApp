using Domain.Vrijwilligerswerk_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class VrijwilligersWerk
    {


        private int werkId;
        private string titel;
        private string omschrijving;
        private int maxCapaciteit;
        private int aantalRegistraties;
      



        public VrijwilligersWerk(int werkId, string titel, string omschrijving, int maxCapaciteit)
        {
            this.werkId = werkId;
            this.titel = titel;
            this.omschrijving = omschrijving;
            this.maxCapaciteit = maxCapaciteit;
           
            
        }

        // ctor voor het toevoegen van werk
        public VrijwilligersWerk(string titel, string omschrijving, int maxCapaciteit)
        {
            this.titel=titel;
            this.omschrijving=omschrijving;
            this.maxCapaciteit=maxCapaciteit;
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
            set { maxCapaciteit = value; }
        }


    }
}
