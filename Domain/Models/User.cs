using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {


        private int userId;
        private string naam;
        private string achterNaam;
        private string passwordHash;
        private string salt;
        private string email;


        public User(int userId, string naam, string achternaam, string email, string passwordHash, string salt) 
        {
            this.userId = userId;
            this.naam = naam;
            this.achterNaam = achternaam;
            this.email = email;
            this.passwordHash = passwordHash;
            this.salt = salt;
        }

        public User( string naam , string achterNaam,string email, string passwordHash, string salt) //ctor for adding new users
        {
            this.naam = naam;
            this.achterNaam= achterNaam;
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