using AzureCards;
using AzureCards.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Cards.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DeckController : ApiController
    {
        private DeckIsolatedStorage _deckStorage;
        public DeckController()
        {
            _deckStorage = new DeckIsolatedStorage();
        }

        [HttpPost]
        [Route("deck")]
        public string New()
        {
            var deckId = _deckStorage.New(new Deck());
            return deckId;
        }

        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("deck/{deckId}/shuffle")]
        public HttpResponseMessage Shuffle(string deckId)
        {
            var deck = _deckStorage.GetById(deckId);

            if (deck == null)
            {
                var notFoundResponse = Request.CreateResponse<bool>(false);
                notFoundResponse.StatusCode = HttpStatusCode.NotFound;
                return notFoundResponse;
            }

            deck.Shuffle();
            _deckStorage.Save(deckId, deck);
            var foundResponse = Request.CreateResponse<bool>(true);
            foundResponse.StatusCode = HttpStatusCode.OK;
            return foundResponse;
        }

        [HttpGet]
        [ResponseType(typeof(DealResponseMessage))]
        [Route("deck/{deckId}/deal/{cardCount}")]
        public HttpResponseMessage Deal(string deckId, int cardCount)
        {
            var deck = _deckStorage.GetById(deckId);

            // if the deck's already been played return a not found
            if (deck.RemainingCards.Count == 0)
                return new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            // make sure we don't take more than we have
            if (deck.RemainingCards.Count < cardCount)
                cardCount = deck.RemainingCards.Count;

            // remove the cards we'll return from the remaining
            var deal = deck.RemainingCards.Take(cardCount).ToList();
            deck.RemainingCards.RemoveRange(0, cardCount);

            // update the deck
            _deckStorage.Save(deckId, deck);

            // respond with the content
            var response = Request.CreateResponse<DealResponseMessage>(new DealResponseMessage
                {
                    Cards = deal
                });

            response.StatusCode = deal.Count() > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }
    }
}
