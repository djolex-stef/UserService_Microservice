using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public class RoleCreatingConfirmation
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
