using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.Users
{
    public class CorporationUserCreatingDto: UserCreatingDto
    {
        /// <summary>
        /// Naziv korporacije
        /// </summary>
        public String CorporationName { get; set; }

        /// <summary>
        /// Pib korporacije
        /// </summary>
        public String Pib { get; set; }

        /// <summary>
        /// Grad u kojoj se korporacija nalazi
        /// </summary>
        public String CorporationCity { get; set; }

        /// <summary>
        /// Adresa na kojoj je korporacija locirana
        /// </summary>
        public String CorporationAddress { get; set; }
    }
}
