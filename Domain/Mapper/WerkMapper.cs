using Domain.Models;
using Infrastructure;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public class WerkMapper
    {
        
        private static VrijwilligersWerkRepositoryDB dbRepos = new VrijwilligersWerkRepositoryDB();

        public static List<VrijwilligersWerk> MapToWerkLijst()
        {
            List<VrijwilligersWerk> werk = new List<VrijwilligersWerk>();
            List<VrijwilligersWerkDTO> werkDTOs = dbRepos.GetVrijwilligersWerk();

            foreach (VrijwilligersWerkDTO dto in werkDTOs) {
                werk.Add(new VrijwilligersWerk(dto.WerkId, dto.Titel, dto.Omschrijving, dto.MaxCapaciteit));
            
            }
            return werk;

        }

        
        public static VrijwilligersWerkDTO MapToDTO(VrijwilligersWerk werk)
        {
            return new VrijwilligersWerkDTO(
                werk.WerkId,
                werk.Titel,
                werk.Omschrijving,
                werk.MaxCapaciteit

                );
        
                
            
        }

        public static VrijwilligersWerk MapToVrijwilligerswerk(VrijwilligersWerkDTO dto)
        {
            return new VrijwilligersWerk(
                dto.WerkId,
                dto.Titel,
                dto.Omschrijving,
                dto.MaxCapaciteit
                );
        }

    }
}
