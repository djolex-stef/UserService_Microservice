using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface ICorporationUserRepository
    {
        List<CorporationUser> GetUsers(string country = null, string username = null);
        List<CorporationUser> GetUsersWithRole(Guid id);
        List<CorporationUser> GetUsersWithCountry(Guid id);
        CorporationUser GetUserByUserId(Guid userId);
        CorporationUser GetUserWithEmail(string email);

        CorporationUserCreatingConfirmation CreateUser(CorporationUser user);
        void UpdateUser(CorporationUser user);
        void DeleteUser(Guid userId);
        bool SaveChanges();
    }
}
