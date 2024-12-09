using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Models
{
    public class WerkCategorie
    {
        private int werkId { get; set; }
        private int categorieId { get; set; }

        public WerkCategorie(int werkId, int categorieId)
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
