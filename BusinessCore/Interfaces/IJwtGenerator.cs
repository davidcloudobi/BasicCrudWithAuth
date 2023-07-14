using CoreObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}
