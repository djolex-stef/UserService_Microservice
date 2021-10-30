using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;

namespace UserService.Services.Users
{
    public class PersonalUsersService : IPersonalUsersService
    {
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly ICorporationUserRepository _corporationUserRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly UserManager<AccountInfo> _userManager;
        private readonly IMapper _mapper;



        public PersonalUsersService(IMapper mapper, IPersonalUserRepository personalUserRepository, IRoleRepository roleRepository, ICountryRepository countryRepository,
            ICorporationUserRepository corporationUserRepository, UserManager<AccountInfo> userManager)
        {
            _personalUserRepository = personalUserRepository;
            _corporationUserRepository = corporationUserRepository;
            _countryRepository = countryRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _mapper = mapper;

        }

        public PersonalUserCreatingConfirmation CreateAdmin(PersonalUser user, string password)
        {
            if (!CheckUniqueUsername(user.Username, false, null))
            {
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CkeckUniqueEmail(user.Email))
            {
                throw new UniqueValueViolationException("Email should be unique");
            }
            if (!CheckCountry(user.CountryId))
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");

            }
            //Dodavanja u userDbContext
            PersonalUserCreatingConfirmation userCreated = _personalUserRepository.CreateAdmin(user);
            _personalUserRepository.SaveChanges();

            //Dodavanje u identityUserDbContext
            string username = string.Join("", user.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var persuser = new AccountInfo(username, user.Email, userCreated.UserId);
            IdentityResult result = _userManager.CreateAsync(persuser, password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(persuser, "Admin").Wait();
            }
            else
            {
                _personalUserRepository.DeleteUser(userCreated.UserId);

            }
            return userCreated;

        }

        public PersonalUserCreatingConfirmation CreateUser(PersonalUser user, string password)
        {
            if (!CheckUniqueUsername(user.Username, false, null))
            {
                throw new UniqueValueViolationException("Username should be unique");
            }
            if (!CkeckUniqueEmail(user.Email))
            {
                throw new UniqueValueViolationException("Email should be unique");
            }
            if (!CheckCountry(user.CountryId))
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");
            }

            ////Dodavanje u userDbContext
            PersonalUserCreatingConfirmation persUserCreated = _personalUserRepository.CreateUser(user);
            _personalUserRepository.SaveChanges();

            //Dodavanje u IdentityUserDbContext
            string username = string.Join("", user.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            var persuser = new AccountInfo(username, user.Email, persUserCreated.UserId);
            IdentityResult result = _userManager.CreateAsync(persuser, password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(persuser, "Regular user").Wait();
            }
            else
            {
                _personalUserRepository.DeleteUser(persUserCreated.UserId);
            }
            return persUserCreated;
        }

        public void DeleteUser(Guid userId)
        {
            _personalUserRepository.DeleteUser(userId);
            _personalUserRepository.SaveChanges();
            //Brisanje iz identity tabele
            var persuser = _userManager.FindByIdAsync(userId.ToString()).Result;
            _userManager.DeleteAsync(persuser).Wait();
        }

        public PersonalUser GetUserByUserId(Guid userId)
        {
            return _personalUserRepository.GetUserByUserId(userId);
        }

        public List<PersonalUser> GetUsers(string country = null, string username = null)
        {
            return _personalUserRepository.GetUsers(country, username);
        }

        public void UpdateUser(PersonalUser user, PersonalUser userWithId)
        {
            if (!CheckUniqueUsername(user.Username, true, userWithId.UserId))
            {
                throw new UniqueValueViolationException("Username should be unique");
            }
            Country country = _countryRepository.GetCountryByCountryId(user.CountryId);
            if (country == null)
            {
                throw new ForeignKeyConstraintViolationException("Foreign key constraint violated");
            }

            user.RoleId = userWithId.RoleId;
            user.Email = userWithId.Email;
            user.Role = _roleRepository.GetRoleByRoleId(userWithId.RoleId);

            user.Country = country;
            user.UserId = userWithId.UserId;
            _mapper.Map(user, userWithId);
            _personalUserRepository.SaveChanges();

            AccountInfo persuser = _userManager.FindByIdAsync(userWithId.UserId.ToString()).Result;
            string username = string.Join("", user.Username.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            persuser.UserName = username;
            _userManager.UpdateAsync(persuser).Wait();
        }
        private bool CheckUniqueUsername(string username, bool forUpdate, Guid? userId)
        {
           // ((corpUsers != null && corpUsers.Count > 0) && (!forUpdate || (forUpdate && corpUsers[0].UserId != userId)))
            var corpUsers = _corporationUserRepository.GetUsers(null, username);
            if (corpUsers != null && corpUsers.Count > 0)
            {
                if (!forUpdate || (forUpdate && corpUsers[0].UserId != userId))
                {
                    return false;
                }
            }
            var personalUsers = _personalUserRepository.GetUsers(null, username);
            if (personalUsers != null && personalUsers.Count > 0)
            {
                if (!forUpdate || (forUpdate && personalUsers[0].UserId != userId))
                {

                    return false;
                }

            }
            return true;
        }

        private bool CheckCountry(Guid countryId)
        {
            return (_countryRepository.GetCountryByCountryId(countryId) != null);
        }
        private bool CkeckUniqueEmail(string email)
        {
            var corpUsers = _corporationUserRepository.GetUserWithEmail(email);
            if (corpUsers != null)
            {
                return false;
            }
            var personalUsers = _personalUserRepository.GetUserWithEmail(email);
            if (personalUsers != null)
            {
                return false;
            }
            return true;
        }

    }
}
