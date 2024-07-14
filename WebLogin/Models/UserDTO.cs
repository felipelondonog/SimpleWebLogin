using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLogin.Models
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string ConfirmPwd { get; set; }
        public bool ResetPwd { get; set; }
        public bool Confirmed { get; set; }
        public string Token { get; set; }
    }
}