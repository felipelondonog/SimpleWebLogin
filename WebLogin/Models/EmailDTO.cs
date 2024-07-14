using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLogin.Models
{
    public class EmailDTO
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}