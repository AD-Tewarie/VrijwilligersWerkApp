using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Repos_DB;

namespace Domain.Mapper
{
    public class UserMapper
    {
        private static UserRepositoryDB UserRepo = new UserRepositoryDB();


        public static List<User> MapToUserLijst()
        {
            List<User> user = new List<User>();
            List<UserDTO> userDTO = UserRepo.GetUsers();

            foreach (UserDTO dto in userDTO)
            {
                user.Add(new User(dto.UserId, dto.Naam, dto.AchterNaam));
                

            }
            return user;

        }


        public static User MapToUser(UserDTO dto)
        {
            return new User(
                dto.UserId,
                dto.Naam,
                dto.AchterNaam


                );
        }

        public static UserDTO MapToDTO(User user)
        {
            return new UserDTO(
                user.UserId,
                user.Naam,
                user.AchterNaam
                );
        }
    }
}
