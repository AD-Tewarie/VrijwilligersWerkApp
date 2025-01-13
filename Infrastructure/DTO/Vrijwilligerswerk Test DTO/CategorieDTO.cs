using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class CategorieDTO
    {
        public int Id { get; set; }
        public string Naam { get; set; }

        public CategorieDTO(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }


    }
}
