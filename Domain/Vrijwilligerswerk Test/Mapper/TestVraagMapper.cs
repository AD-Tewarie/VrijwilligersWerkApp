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
    public class TestVraagMapper : IMapper<TestVraag, TestVraagDTO>
    {
        public TestVraagDTO MapToDTO(TestVraag vraag)
        {
            if (vraag == null)
                throw new ArgumentNullException(nameof(vraag));

            return new TestVraagDTO
            (
                vraag.Id,
                vraag.Tekst,
                vraag.CategorieId
            );
        }

        public TestVraag MapToDomain(TestVraagDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return TestVraag.Maak
            (
                dto.Id,
                dto.Tekst,
                dto.CategorieId
            );
        }
    }
}
