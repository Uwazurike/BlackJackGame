using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackJack.Models
{
    public class person
    {
        public List<Card> hand { get; set; }

        public int HandValue()
        {
            int returnValue = 0;
            foreach (Card c in hand)
            {
                switch (c.value.ToLower())
                {
                    case "ace":
                        returnValue = returnValue + 1;
                        break;
                    case "2":
                        returnValue = returnValue + 2;
                        break;
                    case "3":
                        returnValue = returnValue + 3;
                        break;
                    case "4":
                        returnValue = returnValue + 4;
                        break;
                    case "5":
                        returnValue = returnValue + 5;
                        break;
                    case "6":
                        returnValue = returnValue + 6;
                        break;
                    case "7":
                        returnValue = returnValue + 7;
                        break;
                    case "8":
                        returnValue = returnValue + 8;
                        break;
                    case "9":
                        returnValue = returnValue + 9;
                        break;
                    case "king":
                    case "queen":
                    case "jack":
                        returnValue = returnValue + 10;
                        break;
                    default:
                        int num;
                        int.TryParse(c.value, out num);
                        returnValue = returnValue + num;
                        break;
                }
            }
            return returnValue;
        }
    }   
}