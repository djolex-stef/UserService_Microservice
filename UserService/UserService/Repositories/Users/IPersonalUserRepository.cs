using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public interface IPersonalUserRepository
    {
        List<PersonalUser> GetUsers(string country = null, string username = null);
        List<PersonalUser> GetUsersWithRole(Guid id);
        List<PersonalUser> GetUsersWithCountry(Guid id);
        PersonalUser GetUserWithEmail(string email);
        PersonalUser GetUserByUserId(Guid userId);
        PersonalUserCreatingConfirmation CreateUser(PersonalUser user);
        PersonalUserCreatingConfirmation CreateAdmin(PersonalUser user);
        void UpdateUser(PersonalUser user);
        void DeleteUser(Guid userId);
        bool SaveChanges();
        
    }
}
