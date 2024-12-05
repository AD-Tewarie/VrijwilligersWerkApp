using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public List<UserDTO> GetUsers();
        public UserDTO GetUserOnId(int id);
        public void AddUser(UserDTO userDTO);
        public bool VerwijderUser(int userId);


    }
}
