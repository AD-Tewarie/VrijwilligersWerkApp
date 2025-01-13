using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Repos_DB;
using Infrastructure.Interfaces;
using Domain.WachtwoordStrategy;

namespace Domain.Mapper
{
    public class UserMapper : IMapper<User, UserDTO>
    {
        private readonly IWachtwoordStrategy wachtwoordStrategy;

        public UserMapper(IWachtwoordStrategy wachtwoordStrategy)
        {
            this.wachtwoordStrategy = wachtwoordStrategy ?? throw new ArgumentNullException(nameof(wachtwoordStrategy));
        }

        public UserDTO MapToDTO(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new UserDTO(
                user.UserId,
                user.Naam,
                user.AchterNaam,
                user.Email,
                user.PasswordHash,
                user.Salt
            );
        }

        public User MapToDomain(UserDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return User.LaadVanuitDatabase(
                dto.UserId,
                dto.Naam,
                dto.AchterNaam,
                dto.Email,
                dto.PasswordHash,
                dto.Salt,
                wachtwoordStrategy
            );
        }
    }
}
