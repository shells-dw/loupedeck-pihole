namespace Loupedeck.PiholePlugin
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Loupedeck.PiholePlugin.Models;

    public class PiHoleApiClient : IPiHoleApiClient
    {
        private readonly String _baseUrl;
        private readonly String _token;
        private HttpClient _httpClient;

        public PiHoleApiClient(HttpClient httpClient, String baseUrl, String token = "")
        {
            this._httpClient = httpClient;
            this._baseUrl = baseUrl;
            this._token = token;
        }

        private async Task<String> GetResultAsString(String url)
        {
            try
            {
                var result = await this._httpClient.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
            catch
            {
                return null;
            }
        }
        public async Task<Boolean> VerifyConnectivity(String check)
        {
            var resultString = await this.GetResultAsString($"{this._baseUrl}?{check}&auth={this._token}");
            return resultString != "Not authorized!";
        }

        public async Task<PiStatus> Disable(Int32 seconds = 0)
        {
            try
            {
                var s = seconds > 0 ? $"disable={seconds}" : "disable";
                var resultString = await this.GetResultAsString($"{this._baseUrl}?{s}&auth={this._token}");
                return JsonHelpers.DeserializeObject<PiStatus>(resultString);
            }
            catch
            {
                return null;
            }
        }

        public async Task<PiStatus> Enable()
        {
            return JsonHelpers.DeserializeObject<PiStatus>
                (await this.GetResultAsString($"{this._baseUrl}?enable&auth={this._token}"));
        }

        public async Task<Summary> GetSummaryRawAsync()
        {
            var resultString = await this.GetResultAsString($"{this._baseUrl}?summaryRaw&auth={this._token}");
            if (resultString == null )
            {
                return new Summary();
            }
            return JsonHelpers.DeserializeObject<Summary>(resultString);
        }
    }
}
