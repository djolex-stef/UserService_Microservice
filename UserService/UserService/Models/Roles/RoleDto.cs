using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Roles
{
    /// <summary>
    /// DTO klasa koja modeluje uloge
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// Jedinstveni kljuc uloge
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Naziv uloge
        /// </summary>
        public String RoleName { get; set; }
    }
}
