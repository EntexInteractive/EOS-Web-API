using EpicGames.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EpicGames.Web.Interfaces
{
    /// <summary>
    /// Represents the Sanctions Web API within Epic Online Services.
    /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/sanctions-web-apis"/></para>
    /// </summary>
    public class SanctionsInterface(string accessToken, string url = "https://api.epicgames.dev")
    {
        private readonly HttpClient _client = new();
        private readonly string _accessToken = accessToken;
        private readonly string _url = url;

        /// <summary>
        /// Creates a batch of new sanctions.
        /// <para>The client policy used must have the following permission: 'sanctions:createSanction'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/sanctions-web-apis#creating-sanctions"/></para>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateSanctionsAsync(string deploymentId, Sanction[] sanctions)
        {
            JArray sanctionPayload = new JArray();
            foreach(Sanction sanction in sanctions)
            {
                JObject sple = new JObject();
                sple.Add("productUserId", sanction.ProductUserId);
                sple.Add("action", sanction.Action);
                sple.Add("justification", sanction.Justification);
                sple.Add("source", sanction.Source);
                if (!string.IsNullOrEmpty(sanction.IdentityProvider)) sple.Add("identityProvider", sanction.IdentityProvider);
                if (!string.IsNullOrEmpty(sanction.AccountId)) sple.Add("accountId", sanction.AccountId);
                if (sanction.Tags?.Length > 0) sple.Add("tags", JArray.FromObject(sanction.Tags));
                if (sanction.Expiration != DateTime.MaxValue) sple.Add("duration", sanction.Expiration.Subtract(sanction.Timestamp).TotalSeconds);
                sanctionPayload.Add(sple);
            }

            using HttpRequestMessage request = new(HttpMethod.Post, $"{_url}/sanctions/v1/{deploymentId}/sanctions")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) },
                Content = new StringContent(JsonConvert.SerializeObject(sanctionPayload), Encoding.UTF8, "application/json")
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        //public async Task<SanctionEvent[]> SyncSanctionsAsync()
        //{

        //}

        /// <summary>
        /// Get active sanctions for a specific player.
        /// <para>The client policy used must have the following permission: 'sanctions:findActiveSanctionsForAnyUser'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/sanctions-web-apis#querying-active-sanctions-for-a-specific-player"/></para>
        /// </summary>
        /// <param name="productUserId">The EOS ProductUserId to query sanctions for.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public async Task<Sanction[]> GetSanctionsAsync(string productUserId)
        {
            List<Sanction> sanctionList = new List<Sanction>();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{_url}/sanctions/v1/productUser/{productUserId}/active")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach (JObject item in jsonObject["elements"])
            {
                sanctionList.Add(new Sanction(item));
            }

            return sanctionList.ToArray();
        }

        public async Task<Sanction[]> GetSanctionsAsync(string deploymentId, int limit)
        {
            List<Sanction> sanctionList = new List<Sanction>();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{_url}/sanctions/v1/{deploymentId}/sanctions?limit={limit}")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach (JObject item in jsonObject["elements"])
            {
                sanctionList.Add(new Sanction(item));
            }

            return sanctionList.ToArray();
        }
    }
}
