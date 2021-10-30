using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class PersonalUserRepository : IPersonalUserRepository
    {
        private readonly UserDbContext context;
        private readonly IMapper mapper;
        private readonly IRoleRepository roleRepository;

        public PersonalUserRepository(UserDbContext context, IMapper mapper, IRoleRepository roleRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.roleRepository = roleRepository;
            this.context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public void DeleteUser(Guid userId)
        {
            var user = GetUserByUserId(userId);
            context.Remove(user);
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            return context.PersonalUser.Include(user => user.Country).Include(user => user.Role).FirstOrDefault(e => e.UserId == userId);
        }


        public List<PersonalUser> GetUsers(string country = null, string username = null)
        {
            return context.PersonalUser.Include(user => user.Country).Include(user => user.Role).
              Where(e => country == null || e.Country.CountryName == country).Where(e => username == null || e.Username.Equals(username)).
              ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateUser(PersonalUser user)
        {
            throw new NotImplementedException();
        }

        public PersonalUserCreatingConfirmation CreateUser(PersonalUser user)
        {
            var userRole = roleRepository.GetRoles("Regular user")[0];
            user.Role = userRole;
            context.Role.Attach(userRole);
            var createdUser = context.Add(user);
            return mapper.Map<PersonalUserCreatingConfirmation>(createdUser.Entity);
        }

        public PersonalUserCreatingConfirmation CreateAdmin(PersonalUser user)
        {
            var userRole = roleRepository.GetRoles("Admin")[0];
            user.Role = userRole;
            context.Role.Attach(userRole);
            var createdUser = context.Add(user);
            return mapper.Map<PersonalUserCreatingConfirmation>(createdUser.Entity);
        }

        public List<PersonalUser> GetUsersWithRole(Guid id)
        {
            return context.PersonalUser.Include(user => user.Role).Where(role => role.RoleId == id).ToList();

        }

        public List<PersonalUser> GetUsersWithCountry(Guid id)
        {
            return context.PersonalUser.Include(user => user.Country).Where(country => country.CountryId == id).ToList();

        }

        public PersonalUser GetUserWithEmail(string email)
        {
            return context.PersonalUser.FirstOrDefault(user => user.Email == email);

        }
    }
}
