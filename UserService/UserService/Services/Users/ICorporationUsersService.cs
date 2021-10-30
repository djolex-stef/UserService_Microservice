using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services.Users
{
    public interface ICorporationUsersService
    {
        List<CorporationUser> GetUsers(string country = null, string username = null);
        CorporationUser GetUserByUserId(Guid userId);
        CorporationUserCreatingConfirmation CreateUser(CorporationUser user, string password);
        void UpdateUser(CorporationUser user, CorporationUser userWithId);
        void DeleteUser(Guid userId);
    }
}
