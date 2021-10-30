﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Data
{
    public class CorporationUserRepository : ICorporationUserRepository
    {
        private readonly UserDbContext context;
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;

        public CorporationUserRepository(UserDbContext context, IRoleRepository roleRepository, IMapper mapper)
        {
            this.context = context;
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public CorporationUserCreatingConfirmation CreateUser(CorporationUser user)
        {
            var userRole = roleRepository.GetRoles("Regular user")[0];
            user.Role = userRole;
            context.Role.Attach(userRole);
            var createdUser = context.Add(user);
            return mapper.Map<CorporationUserCreatingConfirmation>(createdUser.Entity);
        }

        public void DeleteUser(Guid userId)
        {
            var user = GetUserByUserId(userId);
            context.Remove(user);
        }

        public CorporationUser GetUserByUserId(Guid userId)
        {
            return context.CorporationUser.Include(user => user.Country).Include(user => user.Role).FirstOrDefault(e => e.UserId == userId);

        }

        public List<CorporationUser> GetUsers(string country = null, string username = null)
        {
            return context.CorporationUser.Include(user => user.Country).Include(user => user.Role).
                Where(e => country == null || e.Country.CountryName == country).Where(e => username == null || e.Username.Equals(username)).
                ToList();
        }

        public List<CorporationUser> GetUsersWithCountry(Guid id)
        {
            return context.CorporationUser.Include(user => user.Country).Where(country => country.CountryId == id).ToList();

        }

        public CorporationUser GetUserWithEmail(string email)
        {
            return context.CorporationUser.FirstOrDefault(user => user.Email == email);
        }

        public List<CorporationUser> GetUsersWithRole(Guid id)
        {
            return context.CorporationUser.Include(user => user.Role).Where(role => role.RoleId == id).ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateUser(CorporationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
