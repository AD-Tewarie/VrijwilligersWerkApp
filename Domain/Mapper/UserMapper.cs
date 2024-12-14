using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Infrastructure.Repos_DB;
using Infrastructure.Interfaces;

namespace Domain.Mapper
{
    public class UserMapper
    {
        private  IUserRepository userRepo;


        public UserMapper(IUserRepository repos)
        {
            userRepo = repos;
        }


        public List<User> MapToUserLijst()
        {
            List<User> user = new List<User>();
            List<UserDTO> userDTO = userRepo.GetUsers();

            foreach (UserDTO dto in userDTO)
            {
                user.Add(new User(dto.UserId, dto.Naam, dto.AchterNaam,dto.Email, dto.PasswordHash, dto.Salt));
                

            }
            return user;

        }


        public User MapToUser(UserDTO dto)
        {
            return new User(
                dto.UserId,
                dto.Naam,
                dto.AchterNaam,
                dto.Email,
                dto.PasswordHash,
                dto.Salt


                );
        }

        public UserDTO MapToDTO(User user)
        {
            return new UserDTO(
                user.UserId,
                user.Naam,
                user.AchterNaam,
                user.Email,
                user.PasswordHash,
                user.Salt
                );
        }
    }
}
