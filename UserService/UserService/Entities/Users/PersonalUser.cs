using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entiti klasa za personalne naloge
    /// </summary>
    public class PersonalUser: User
    {
        /// <summary>
        /// Ime korisnika
        /// </summary>
        [Column("FirstName")]
        [StringLength(100)]
        [Required]
        public String FirstName { get; set; }

        /// <summary>
        /// Prezime korisnika
        /// </summary>
        [Column("LastName")]
        [StringLength(100)]
        [Required]
        public String LastName { get; set; }
    }
}
