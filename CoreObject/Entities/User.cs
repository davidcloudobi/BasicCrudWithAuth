using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.Entities
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Role { get; set; }    
        public string? Token { get; set; }
    }
}
