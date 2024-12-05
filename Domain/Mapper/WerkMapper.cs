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
    public class WerkMapper
    {
        
        private IVrijwilligersWerkRepository dbRepos;


        public WerkMapper(IVrijwilligersWerkRepository repos)
        {
            dbRepos = repos;
        }



        public List<VrijwilligersWerk> MapToWerkLijst()
        {
            List<VrijwilligersWerk> werk = new List<VrijwilligersWerk>();
            List<VrijwilligersWerkDTO> werkDTOs = dbRepos.GetVrijwilligersWerk();

            foreach (VrijwilligersWerkDTO dto in werkDTOs) {
                werk.Add(new VrijwilligersWerk(dto.WerkId, dto.Titel, dto.Omschrijving, dto.MaxCapaciteit));
            
            }
            return werk;

        }

        
        public VrijwilligersWerkDTO MapToDTO(VrijwilligersWerk werk)
        {
            return new VrijwilligersWerkDTO(
                werk.WerkId,
                werk.Titel,
                werk.Omschrijving,
                werk.MaxCapaciteit

                );
        
                
            
        }

        public VrijwilligersWerk MapToVrijwilligerswerk(VrijwilligersWerkDTO dto)
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
