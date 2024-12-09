using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class TestVraagDTO
    {
        private int id { get; set; }
        private string tekst { get; set; }
        private int categorieId { get; set; }

        public TestVraagDTO(int id, string tekst, int categorieId)
        {
            this.id = id;
            this.tekst = tekst;
            this.categorieId = categorieId;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


        public string Tekst
        {
            get { return tekst; }
            set { tekst = value; }
        }

        public int CategorieId
        {
            get { return categorieId; }
            set { categorieId = value; }
        }


    }
}
