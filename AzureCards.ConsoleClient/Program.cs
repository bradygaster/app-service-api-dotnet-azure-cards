using System;
using System.Linq;

namespace AzureCards.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureCardsTechReadyDemo client = new AzureCardsTechReadyDemo(
                #region For use with API Management
                /*
                new Uri(ApimDelegatingHandler.BASE_URI_CARDS),
                new ApimDelegatingHandler(ApimDelegatingHandler.APIM_AUTH_HEADER)
                */
                #endregion
            );

            var deckId = client.Deck.New();
            Console.WriteLine($"Deck {deckId} created, shuffling");

            for (int i = 0; i < 10; i++)
                client.Deck.Shuffle(deckId);

            Console.WriteLine("Dealing");

            var response = client.Deck.Deal(deckId, 5);
            while(response.Cards != null && response.Cards.Any())
            {
                foreach (var card in response.Cards)
                    Console.WriteLine($"{card.Face} of {card.Suit}");

                response = client.Deck.Deal(deckId, 5);
            }

            Console.WriteLine("---------------");
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
