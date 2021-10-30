using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{

    /// <summary>
    /// Entitet koji modeluje uloge koje se koriste za autorizaciju
    /// </summary>
    public class AccountRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Opis uloge
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        public AccountRole() : base()
        {
            Description = "No Role Description";
        }

        public AccountRole(string description, string roleName) : base(roleName)
        {
            Description = description;
        }

        public AccountRole(Guid id, string description, string roleName) : base(roleName)
        {
            base.Id = id;
            Description = description;
        }
    }
}
