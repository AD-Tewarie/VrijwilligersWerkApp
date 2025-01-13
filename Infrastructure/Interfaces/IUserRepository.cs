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
        List<UserDTO> GetUsers();
        UserDTO GetUserByEmail(string email);
        UserDTO GetUserOnId(int id);
        void AddUser(UserDTO userDTO);
        bool VerwijderUser(int userId);

    }
}
