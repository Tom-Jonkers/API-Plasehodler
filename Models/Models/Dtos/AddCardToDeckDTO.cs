using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dtos
{
    public class AddCardToDeckDTO
    {
        public int OwnedCardId { get; set; }
        public int DeckId { get; set; }
    }
}
