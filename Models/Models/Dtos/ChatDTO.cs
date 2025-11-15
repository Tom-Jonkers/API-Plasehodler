using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dtos
{
    public class ChatDTO
    {
        public string Pseudo { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool isSpectator { get; set; }

    }
}
