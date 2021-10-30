using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entiti klasa za uloge koje se koriste za autorizaciju
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Jedinstveni kljuc za ulogu
        /// </summary>
        [Column("RoleId")]
        [Key]
        [Required]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Naziv uloge
        /// </summary>
        [Column("RoleName")]
        [StringLength(50)]
        [Required]
        public String RoleName { get; set; }
    }
}
