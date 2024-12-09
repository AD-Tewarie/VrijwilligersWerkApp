using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test
{
    public class Categorie
    {
        private int id { get; set; }
        private string naam { get; set; }


        public Categorie(int id, string naam)
        {
            this.id = id;
            this.naam = naam;
            
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Naam
        {
            get { return naam; }
            set { naam = value; }
        }
    }
}
