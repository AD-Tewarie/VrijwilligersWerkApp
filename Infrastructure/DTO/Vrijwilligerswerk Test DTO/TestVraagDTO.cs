using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class TestVraagDTO
    {
        public int Id { get; set; }
        public string Tekst { get; set; }
        public int CategorieId { get; set; }

        public TestVraagDTO(int id, string tekst, int categorieId)
        {
            Id = id;
            Tekst = tekst;
            CategorieId = categorieId;
        }


    }
}
