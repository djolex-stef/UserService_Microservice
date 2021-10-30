using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Entities
{
    /// <summary>
    /// Entiti klasa za korisnike
    /// </summary>
    public class User
    {
        /// <summary>
        /// Jedinstveni kljuc za nalog
        /// </summary>
        [Column("UserId")]
        [Key]
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Email korisnika
        /// </summary>
        [Column("Email")]
        [StringLength(50)]
        [Required]
        public String Email { get; set; }

        /// <summary>
        /// Boolean vrednost koja ukazuje da li je korisnicki nalog aktivan
        /// </summary>
        [Column("IsActive")]
        [Required]
        public Boolean IsActive { get; set; }


        /// <summary>
        /// Korisnicki username
        /// </summary>
        [Column("Username")]
        [StringLength(60)]
        [Required]
        public String Username { get; set; }


        /// <summary>
        /// Telefon korisnika
        /// </summary>
        [Column("Telephone")]
        [StringLength(15)]
        [Required]
        public String Telephone { get; set; }

        /// <summary>
        /// Uloga korisnika
        /// </summary>
        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        /// <summary>
        /// Drzava korisnika
        /// </summary>
        [ForeignKey("CountryId")]
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
