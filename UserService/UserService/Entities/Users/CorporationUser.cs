using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    /// <summary>
    /// Entiti klasa za korporacijske naloge
    /// </summary>
    public class CorporationUser: User
    {
        /// <summary>
        /// Naziv korporacije
        /// </summary>
        [Column("CorporationName")]
        [StringLength(100)]
        [Required]
        public String CorporationName { get; set; }

        /// <summary>
        /// Pib korporacije
        /// </summary>
        [Column("Pib")]
        [StringLength(30)]
        [Required]
        public String Pib { get; set; }

        /// <summary>
        /// Grad u kojoj se korporacija nalazi
        /// </summary>
        [Column("CorporationCity")]
        [StringLength(40)]
        [Required]
        public String CorporationCity { get; set; }

        /// <summary>
        /// Adresa na kojoj se korporacija nalazi
        /// </summary>
        [Column("CorporationAddress")]
        [StringLength(70)]
        [Required]
        public String CorporationAddress { get; set; }
    }
}
