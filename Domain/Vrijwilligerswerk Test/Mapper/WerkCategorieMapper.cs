using Domain.Mapper;
using Domain.Vrijwilligerswerk_Test.Models;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Mapper
{
    public class WerkCategorieMapper : IMapper<WerkCategorie, WerkCategorieDTO>
    {
        public WerkCategorieDTO MapToDTO(WerkCategorie categorie)
        {
            if (categorie == null)
                throw new ArgumentNullException(nameof(categorie));

            return new WerkCategorieDTO
            (
                categorie.CategorieId,
                categorie.WerkId
            );
        }

        public WerkCategorie MapToDomain(WerkCategorieDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return WerkCategorie.Maak
            (
                dto.CategorieId,
                dto.WerkId
            );
        }
    }

}
