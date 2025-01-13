using Domain.Models;
using Infrastructure;
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
    public class RegistratieMapper : IMapper<WerkRegistratie, WerkRegistratieDTO>
    {
        private readonly IMapper<VrijwilligersWerk, VrijwilligersWerkDTO> werkMapper;
        private readonly IMapper<User, UserDTO> userMapper;

        public RegistratieMapper(
            IMapper<VrijwilligersWerk, VrijwilligersWerkDTO> werkMapper,
            IMapper<User, UserDTO> userMapper)
        {
            this.werkMapper = werkMapper ?? throw new ArgumentNullException(nameof(werkMapper));
            this.userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        }

        public WerkRegistratieDTO MapToDTO(WerkRegistratie registratie)
        {
            if (registratie == null)
                throw new ArgumentNullException(nameof(registratie));

            return new WerkRegistratieDTO(
                werkMapper.MapToDTO(registratie.VrijwilligersWerk),
                userMapper.MapToDTO(registratie.User),
                registratie.RegistratieId
            );
        }

        public WerkRegistratie MapToDomain(WerkRegistratieDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var vrijwilligersWerk = werkMapper.MapToDomain(dto.VrijwilligersWerk);
            var user = userMapper.MapToDomain(dto.User);


            return WerkRegistratie.LaadVanuitDatabase(dto.RegistratieId, vrijwilligersWerk, user);
        }


    }
}
