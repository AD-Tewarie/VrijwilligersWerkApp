using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
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

 
        private UserRepositoryDB repositoryDB = new UserRepositoryDB();
        private List<User> users = new List<User>();




        // voeg user toe
        public void VoegGebruikerToe(string naam, string achterNaam)
        {
            int userId = GenereerId();
            var user = new User(userId, naam, achterNaam);
            var userDTO = UserMapper.MapToDTO(user); // Convert to UserDTO
            repositoryDB.AddUser(userDTO);
        }


        // haal alle users op
        public List<string> GetAllUsers()
        {
            
            var users = new List<string>();

            foreach (var user in UserMapper.MapToUserLijst())
            {
                users.Add($"Gebruiker ID: {user.UserId}, Voornaam: {user.Naam}, Achternaam: {user.AchterNaam}");
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
            users = UserMapper.MapToUserLijst();
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
