using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Models.ModelsDTO;

namespace BillManager.Interfaces
{
    public interface IUsersService
    {
        UsersDTO GetAllUsers();
        ResponseDTO EditUser(UserDTO userDTO);
        ResponseAfterAutDTO GetIdAndRoleForUserById(string mail);
    }
}
