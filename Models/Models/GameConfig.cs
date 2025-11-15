using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Cartes_Infinies.Models
{
    public class GameConfig
    {
        public GameConfig() { }

        public int Id { get; set; }

        public int nbCardToDraw { get; set; }

        public int qtManaPerTurn { get; set; }


        public int MonnaieRecueCreation { get; set; }

        public int MonnaieRecueVictoire { get; set; }

        public int MonnaieRecueDefaite { get; set; }

        
        public int maxDecks { get; set; } = 5;
        
        public int maxCardsPerDeck { get; set; } = 30;

    }
}
