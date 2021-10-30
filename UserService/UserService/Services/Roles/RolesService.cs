using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Services.Users;

namespace UserService.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPersonalUserRepository _personalUserRepository;
        private readonly ICorporationUserRepository _corporationUserRepository;
        private readonly RoleManager<AccountRole> _roleManager;
        public RolesService(IRoleRepository roleRepository, ICorporationUserRepository corporationUserRepository, IPersonalUserRepository personalUserRepository,
            RoleManager<AccountRole> roleManager)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _personalUserRepository = personalUserRepository;
            _corporationUserRepository = corporationUserRepository;
        }

        public RoleCreatingConfirmation CreateRole(Role role)
        {
            if (!_roleManager.RoleExistsAsync(role.RoleName).Result)
            {
                RoleCreatingConfirmation createdRole = _roleRepository.CreateRole(role);
                _roleRepository.SaveChanges();
                AccountRole accRole = new AccountRole(createdRole.RoleId, role.RoleName, role.RoleName);
                _roleManager.CreateAsync(accRole).Wait();
                return createdRole;

            }
            throw new UniqueValueViolationException("Role name should be unique");
        }

        public void DeleteRole(Guid roleId)
        {
            List<PersonalUser> persUsers = _personalUserRepository.GetUsersWithRole(roleId);
            if(persUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of role is referenced in another table");
            }
            List<CorporationUser> corpUsers = _corporationUserRepository.GetUsersWithRole(roleId);
            if(corpUsers.Count > 0)
            {
                throw new ReferentialConstraintViolationException("Value of role is referenced in another table");
            }
            var role = _roleManager.Roles.First(r => r.Id == roleId);
            _roleManager.DeleteAsync(role).Wait();
            _roleRepository.DeleteRole(roleId);
            _roleRepository.SaveChanges();
        }

        public Role GetRoleByRoleId(Guid roleId)
        {
            return _roleRepository.GetRoleByRoleId(roleId);
        }

        public List<Role> GetRoles(string roleName)
        {
            return _roleRepository.GetRoles(roleName);
        }

        public void UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
