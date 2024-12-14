using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class UserDTO
    {
        private int userId { get; }
        private string naam { get; set; }
        private string passwordHash { get; set; }
        private string salt { get; set; }
        private string achterNaam { get; set; }
        private string email { get; set; }



        public UserDTO(int userId, string naam, string achternaam,string email, string passwordHash, string salt)
        {
            this.userId = userId;
            this.naam = naam;
            this.achterNaam = achternaam;
            this.email = email;
            this.passwordHash = passwordHash;
            this.salt = salt;
        }


        public UserDTO(string naam, string achterNaam, string email, string passwordHash, string salt) 
        {
            this.naam = naam;
            this.achterNaam = achterNaam;
            this.passwordHash = passwordHash;
            this.salt = salt;
            this.email = email;
        }

        public int UserId
        {
            get { return userId; }
            
        }

        public string Naam
        {
            get { return naam; }
            set { naam = value; }
        }

        public string AchterNaam
        {
            get { return achterNaam; }
            set { achterNaam = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }

        }

        public string PasswordHash
        {
            get { return passwordHash; }
            set { passwordHash = value; }
        }

        public string Salt
        {
            get { return salt; }
            set { salt = value; }

        }

    }
}
