using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Interfaces;
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
        private  IWerkRegistratieRepository repository;
        private WerkMapper werkMapper;
        private UserMapper userMapper;

        public RegistratieMapper(IWerkRegistratieRepository repos, WerkMapper werk, UserMapper user)
        {
            repository = repos;
            werkMapper = werk;
            userMapper = user;
        }



        public  List<WerkRegistratie> MapToDomainList()
        {
            List<WerkRegistratie> registraties = new List<WerkRegistratie>();
            List<WerkRegistratieDTO> registratieDTOs = repository.GetWerkRegistraties();
            


            foreach (WerkRegistratieDTO dto in registratieDTOs)
            {
                var vrijwilligersWerk = werkMapper.MapToVrijwilligerswerk(dto.VrijwilligersWerk);
                var user = userMapper.MapToUser(dto.User);

                registraties.Add(new WerkRegistratie(vrijwilligersWerk, user, dto.RegistratieId));

            }
            return registraties;

        }


       

        public  WerkRegistratieDTO MapToDTO(WerkRegistratie registratie)
        {
            return new WerkRegistratieDTO(
                werkMapper.MapToDTO(registratie.VrijwilligersWerk),
                userMapper.MapToDTO(registratie.User),
                registratie.RegistratieId);

        }


    }
}
