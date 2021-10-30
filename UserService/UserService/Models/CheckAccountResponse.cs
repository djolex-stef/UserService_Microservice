using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Models
{
    public class CheckAccountResponse
    {
        /// <summary>
        /// Boolean vrednost koja ukazuje na to da li autentifikacija uspesna
        /// </summary>
        public bool Succes { get; set; }


        /// <summary>
        ///Poruka, ukoliko autentifikacija nije uspesna
        /// </summary>
        public string Message { get; set; }
        public AccountInfoDto AccountInfo { get; set; }


        public CheckAccountResponse()
        {

        }

        public CheckAccountResponse(bool succes, string message, AccountInfoDto account)
        {
            this.Succes = succes;
            this.Message = message;
            this.AccountInfo = account;
        }
    }
}
