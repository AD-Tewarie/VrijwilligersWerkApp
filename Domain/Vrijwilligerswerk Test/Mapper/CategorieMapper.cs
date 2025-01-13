using Domain.Mapper;
using Infrastructure.DTO.Vrijwilligerswerk_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vrijwilligerswerk_Test.Mapper
{
    public class CategorieMapper : IMapper<Categorie, CategorieDTO>
    {
        public CategorieDTO MapToDTO(Categorie categorie)
        {
            if (categorie == null)
                throw new ArgumentNullException(nameof(categorie));

            return new CategorieDTO
            (
                categorie.Id,
                categorie.Naam
               
            );
        }

        public Categorie MapToDomain(CategorieDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return Categorie.Maak
            (
                dto.Id,
                dto.Naam
                
            );
        }
    }
}
