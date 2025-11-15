using Super_Cartes_Infinies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Probability
    {
        public int Id { get; set; }
        public string PaquetName { get; set; } = "";
        public Raretes Rarity { get; set; }
        public decimal Value { get; set; }
        public int BaseQty { get; set; }
    }
}
