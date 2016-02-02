using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AzureCards.ConsoleClient
{
    internal class ApimDelegatingHandler : DelegatingHandler
    {
        internal const string BASE_URI_CARDS = "https://bradysapis.azure-api.net/AzureCardsTechReadyDemo";
        internal const string APIM_AUTH_HEADER = "[YOUR KEY HERE]";

        private string _apimAuthHeader;

        public ApimDelegatingHandler(string apimAuthHeader)
        {
            _apimAuthHeader = apimAuthHeader;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", _apimAuthHeader);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
