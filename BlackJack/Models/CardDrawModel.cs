using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackJack.Models
{
    public class CardDrawmodel
    {
        public bool success { get; set; }
        public List<Card> cards { get; set; }
        public int remaining { get; set; }
    }
}