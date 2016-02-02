using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCards.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AzureCardsTechReadyDemo client = new AzureCardsTechReadyDemo();
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
