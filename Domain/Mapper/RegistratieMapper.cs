using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Mapper
{
    public class RegistratieMapper
    {
        private static WerkRegistratieRepositoryDB repository = new WerkRegistratieRepositoryDB();
        

        public static List<WerkRegistratie> MapToDomainList()
        {
            List<WerkRegistratie> registraties = new List<WerkRegistratie>();
            List<WerkRegistratieDTO> registratieDTOs = repository.GetWerkRegistraties();
            


            foreach (WerkRegistratieDTO dto in registratieDTOs)
            {
                var vrijwilligersWerk = WerkMapper.MapToVrijwilligerswerk(dto.VrijwilligersWerk);
                var user = UserMapper.MapToUser(dto.User);

                registraties.Add(new WerkRegistratie(vrijwilligersWerk, user, dto.RegistratieId));

            }
            return registraties;

        }


       

        public static WerkRegistratieDTO MapToDTO(WerkRegistratie registratie)
        {
            return new WerkRegistratieDTO(
                WerkMapper.MapToDTO(registratie.VrijwilligersWerk),
                UserMapper.MapToDTO(registratie.User),
                registratie.RegistratieId);

        }


    }
}
