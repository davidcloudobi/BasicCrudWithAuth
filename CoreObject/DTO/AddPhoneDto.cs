using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreObject.DTO
{
    public class AddPhoneDto
    {
        [Required]
        public string Model { get; set; }
    }
}

