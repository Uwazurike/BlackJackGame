using BlackJack.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlackJack.Controllers
{
    public class CardController : Controller
    {
        HttpClient client = new HttpClient();
        string url = "http://deckofcardsapi.com/api/deck";
        Deck currentDeck;

        public CardController()
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/new/shuffle/?deck_count=1");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                currentDeck = JsonConvert.DeserializeObject<Deck>(responseData);
                return View(currentDeck);
            }
            return View("Error");
        }

        public async Task<ActionResult> PlayGame()
        {
            CardsViewModel game = new CardsViewModel();
            game.deckId = Request.QueryString["deck_id"];
            game.dealer = new person();
            game.player = new person();
            game.dealer.hand = new List<Card>();
            game.player.hand = new List<Card>();

            for (int i = 0; i < 2; i++)
            {
                game.dealer.hand.Add(await GetACard(game.deckId));
                game.player.hand.Add(await GetACard(game.deckId));               
            }
            Session["gameSession"] = game;
            return View(game);
        }

        [HttpPost]
        public async Task<ActionResult> PlayGame(string playerHitButton, string playerStayButton)
        {
            CardsViewModel game = (CardsViewModel)Session["gameSession"];
            if (playerHitButton != null)
            {
                game.player.hand.Add(await GetACard(game.deckId));
                if (playerHitButton != null && game.dealer.HandValue() <= 15)
                {
                    game.dealer.hand.Add(await GetACard(game.deckId));
                    return View(game);
                }
                if (game.player.HandValue() == 21 || game.dealer.HandValue() > 21)
                {
                    return RedirectToAction("Won");
                }
                else if (game.dealer.HandValue() == 21 || game.player.HandValue() > 21)
                {
                    return RedirectToAction("Lose");
                }           
                if (playerStayButton != null && game.dealer.HandValue() <= 15)
                {
                    game.dealer.hand.Add(await GetACard(game.deckId));
                    return View(game);
                }
                else if (game.player.HandValue() == 21 || game.dealer.HandValue() > 21)
                {
                    return RedirectToAction("Won");
                }
                else if (game.dealer.HandValue() == 21 || game.player.HandValue() > 21)
                {
                    return RedirectToAction("Lose");
                }
                else
                {
                    return View(game);
                }
            }
            return View(game);
        }

        public async Task<Card> GetACard(string deckID)
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + "/" + deckID + "/draw/?count=1");

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var drawnCard = JsonConvert.DeserializeObject<CardDrawmodel>(responseData);
                if (drawnCard.remaining == 0)
                    RedirectToAction("Index");
                return drawnCard.cards[0];
            }
            return null;
        }
        public ActionResult Won()
        {
            return View();
        }
        public ActionResult Lose()
        {
            return View();
        }
    }
}