using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class VraagCategorie
    {

        private int id { get; set; }

        private string naam { get; set; }

        private List<TestVraag> Vragen {  get; set; }


        public VraagCategorie(int id, string naam, List<TestVraag> vragen)
        {
            this.id = id;
            this.naam = naam;
            this.Vragen = vragen;
            

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

        public List<TestVraag> GetVragen
        {
            get { return Vragen; }
            set { Vragen = value; }
        }



    }
}
