using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class WerkCategorieDTO
    {
        private int werkId { get; set; }
        private int categorieId { get; set; }

        public WerkCategorieDTO(int werkId, int categorieId)
        {
            this.werkId = werkId;
            this.categorieId = categorieId;
        }

        public int WerkId
        {
            get { return werkId; }
            set { werkId = value; }

        }

        public int CategorieId
        {
            get { return categorieId; }
            set { categorieId = value; }

        }



    }
}
