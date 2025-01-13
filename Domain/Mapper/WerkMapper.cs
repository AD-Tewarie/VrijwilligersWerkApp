using Domain.Models;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public class WerkMapper : IMapper<VrijwilligersWerk, VrijwilligersWerkDTO>
    {
        public VrijwilligersWerkDTO MapToDTO(VrijwilligersWerk werk)
        {
            if (werk == null)
                throw new ArgumentNullException(nameof(werk));

            
            return new VrijwilligersWerkDTO
            (
                werk.WerkId,  
                werk.Titel,
                werk.Omschrijving,
                werk.MaxCapaciteit,
                werk.AantalRegistraties
            );
        }

        public VrijwilligersWerk MapToDomain(VrijwilligersWerkDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

     

            return VrijwilligersWerk.LaadVanuitDatabase(
                dto.WerkId,
                dto.Titel,
                dto.Omschrijving,
                dto.MaxCapaciteit,
                dto.AantalRegistraties
            );
        }
    }
}