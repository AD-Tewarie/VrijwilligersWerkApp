using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class CategorieDTO
    {
        private int id { get; set; }
        private string naam { get; set; }


        public CategorieDTO(int id, string naam)
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
