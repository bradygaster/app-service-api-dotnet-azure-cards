using AzureCards.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace AzureCards
{
    public class DeckIsolatedStorage
    {
        private string _filePath;

        public DeckIsolatedStorage()
        {
            var webAppsHome = Environment.GetEnvironmentVariable("HOME")?.ToString();
            if (String.IsNullOrEmpty(webAppsHome))
            {
                _filePath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) + "\\";
            }
            else
            {
                _filePath = webAppsHome + "\\site\\wwwroot\\";
            }
        }

        public string New(Deck deck)
        {
            var deckId = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            Save(deckId, deck);
            return deckId;
        }

        public string Save(string deckId, Deck deck)
        {
            var filename = string.Format("{0}{1}.json", _filePath, deckId);
            var json = JsonConvert.SerializeObject(deck);
            var data = Encoding.ASCII.GetBytes(json);
            File.WriteAllText(filename, json);
            return deckId;
        }

        public Deck GetById(string deckId)
        {
            var filename = string.Format("{0}{1}.json", _filePath, deckId);
            var json = File.ReadAllText(filename);
            return JsonConvert.DeserializeObject<Deck>(json);
        }
    }
}
