using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Naam { get; set; }
        public string AchterNaam { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        public UserDTO(
            int userId,
            string naam,
            string achterNaam,
            string email,
            string passwordHash,
            string salt)
        {
            UserId = userId;
            Naam = naam;
            AchterNaam = achterNaam;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
        }

    }
}
