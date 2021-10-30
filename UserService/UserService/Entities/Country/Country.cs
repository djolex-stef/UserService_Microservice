using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entiti klasa za zemlje
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Jedinstveni kljuc za zemlju
        /// </summary>
        [Column("CountryId")]
        [Key]
        [Required]
        public Guid CountryId { get; set; }
   
        /// <summary>
        /// Naziv zemlje
        /// </summary>
        [Column("CountryName")]
        [StringLength(50)]
        [Required]
        public String CountryName { get; set; }
    }
}
