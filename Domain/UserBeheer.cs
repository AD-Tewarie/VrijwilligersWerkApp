using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repos_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserBeheer : IUserBeheer
    {

 
        private IUserRepository repositoryDB;
        private List<User> users = new List<User>();
        private readonly UserMapper userMapper;

        public UserBeheer(IUserRepository userRepository, UserMapper userMap)
        {
            repositoryDB = userRepository;
            userMapper = userMap;
        }


        // voeg user toe
        public void VoegGebruikerToe(string naam, string achterNaam)
        {
            int userId = GenereerId();
            var user = new User(userId, naam, achterNaam);
            var userDTO = userMapper.MapToDTO(user); // Convert to UserDTO
            repositoryDB.AddUser(userDTO);
        }


        // haal alle users op
        public List<User> GetAllUsers()
        {
            
            var users = new List<User>();

            foreach (var user in userMapper.MapToUserLijst())
            {
                users.Add(user);
            }

            return users;
        }


        // verwijder gebruiker
        public void VerwijderGebruiker(int userId)
        {
            repositoryDB.VerwijderUser(userId);
        }


        // genereer user id
        public int GenereerId()
        {
            users = userMapper.MapToUserLijst();
            int maxId = 0;
            foreach (var user in users)
            {
                if (user.UserId > maxId)
                {
                    maxId = user.UserId;
                }
            }
            return maxId + 1;

        }


    }
}
