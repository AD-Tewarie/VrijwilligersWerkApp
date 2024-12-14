using Domain.Interfaces;
using Domain.Mapper;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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


        // voeg gebruiker toe
        public void VoegGebruikerToe(string naam, string achterNaam,string email, string password)
        {
            var salt = GenereerSalt();
            var passwordHash = HashPassword(password, salt);
            
            var user = new User(naam , achterNaam,email, passwordHash, salt);
            var userDTO = userMapper.MapToDTO(user);
            repositoryDB.AddUser(userDTO);
        }



        // valideer gebruiker
        public bool ValideerGebruiker(string email, string password)
        {
            
            var user = repositoryDB.GetUsers().FirstOrDefault(x => x.Email == email);

            if (user == null) return false;

            var hash = HashPassword(password, user.Salt);

            return hash == user.PasswordHash;

            
        }



        // haal gebruiker op naam en email
        public User HaalGebruikerOpEmail(string email)
        {
            var userDTO = repositoryDB.GetUserByEmail(email);
            var user = userMapper.MapToUser(userDTO);
            return user;
        }


        // haal alle gebruikers op
        public List<User> HaalAlleGebruikersOp()
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


      


        // Methods voor hashing en salt

      

        private string HashPassword (string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create()) 
            {
                var combined = Encoding.UTF8.GetBytes (salt + password);
                var hash = sha256.ComputeHash (combined);
                return Convert.ToBase64String (hash);

            }
            
        }



        private string GenereerSalt()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }




    }
}
