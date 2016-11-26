using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackJack.Models
{
    public class CardsViewModel
    {
        public string deckId { get; set; }
        public person dealer { get; set; }
        public person player { get; set; }     
    }
}