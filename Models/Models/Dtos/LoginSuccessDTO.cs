using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dtos
{
    public class LoginSuccessDTO
    {
        [Required]
        public string Token { get; set; } = "";
        [Required]
        public string Username { get; set; } = "";
        [Required]
        public string UserId { get; set; } = "";
        [Required]
        public int PlayerMoney { get; set; }

    }
}
