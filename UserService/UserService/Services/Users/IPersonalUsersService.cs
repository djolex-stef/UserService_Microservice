using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Services.Users
{
    public interface IPersonalUsersService
    {
        List<PersonalUser> GetUsers(string country = null, string username = null);
        PersonalUser GetUserByUserId(Guid userId);
        PersonalUserCreatingConfirmation CreateUser(PersonalUser user, string password);
        PersonalUserCreatingConfirmation CreateAdmin(PersonalUser user, string password);
        void UpdateUser(PersonalUser updatedUser, PersonalUser userWithId);
        void DeleteUser(Guid userId);
    }
}
