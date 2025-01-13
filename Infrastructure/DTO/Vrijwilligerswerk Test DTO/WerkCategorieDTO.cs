using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO.Vrijwilligerswerk_Test
{
    public class WerkCategorieDTO
    {
        public int WerkId { get; set; }
        public int CategorieId { get; set; }

        public WerkCategorieDTO(int werkId, int categorieId)
        {
            WerkId = werkId;
            CategorieId = categorieId;
        }

    }
}
