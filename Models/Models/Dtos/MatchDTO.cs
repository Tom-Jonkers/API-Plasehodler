using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dtos
{
    public class MatchDTO
    {
        public int Id { get; set; }

        public string UserIdA { get; set; } = null!;

        public string PlayerNameA { get; set; } = null!;

        public string UserIdB { get; set; } = null!;

        public string PlayerNameB { get; set; } = null!;
    }
}
