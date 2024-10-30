// Copyright 2024 Entex Interactive, LLC

using System.Net.Http.Headers;
using System.Text;
using EpicGames.Web.Responses;
using Newtonsoft.Json;

namespace EpicGames.Web.Interfaces
{
    /// <summary>
    /// Represents the Connect Web API within Epic Online Services.
    /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/connect-web-api"/></para>
    /// </summary>
    public sealed class ConnectInterface
    {
        private readonly HttpClient _client;
        private readonly string _url;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectInterface"/> class.
        /// </summary>
        /// <param name="url">The url of the Epic Games Dev API.</param>
        public ConnectInterface(string url = "https://api.epicgames.dev")
        {
            _client = new HttpClient();
            _url = url;
        }
        
        #region Access Tokens

        /// <summary>
        /// Gets an access token for client access.
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/connect-web-api#token-request"/></para>
        /// </summary>
        /// <param name="clientId">Client Credentials ID.</param>
        /// <param name="clientSecret">Client Credentials Secret.</param>
        /// <returns></returns>
        public async Task<TokenResponse> RequestClientAccessToken(string clientId, string clientSecret)
        {
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            Dictionary<string, string> body = new()
            {
                { "grant_type", "client_credentials" }
            };

            return await InternalMakeTokenRequest(auth, body);
        }
        
        /// <summary>
        /// Gets an access token for user access.
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/connect-web-api#token-request"/></para>
        /// </summary>
        /// <param name="clientId">Client Credentials ID.</param>
        /// <param name="clientSecret">Client Credentials Secret.</param>
        /// <param name="nonce">An arbitrary string value provided by the client that provides added security. It is included in the API response for the client, to verify the correct nonce value. Required for user access.</param>
        /// <param name="deploymentId">Target EOS deployment to request access for.</param>
        /// <param name="externalAuthToken">External authentication token that identifies the user account in the external account system.</param>
        /// <param name="externalAuthType">Identifies the external authentication token’s type. Required for user access. The possible values are: amazon_access_token, apple_id_token, discord_access_token, epicgames_access_token, epicgames_id_token, gog_encrypted_sessionticket, google_id_token, itchio_jwt, itchio_key, nintendo_id_token, oculus_userid_nonce, openid_access_token, psn_id_token, steam_access_token, steam_encrypted_appticket, steam_session_ticket, viveport_user_token, xbl_xsts_token</param>
        /// <returns></returns>
        public async Task<TokenResponse> RequestUserAccessToken(string clientId, string clientSecret, string nonce, string deploymentId, string externalAuthToken, string externalAuthType)
        {
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            Dictionary<string, string> body = new()
            {
                { "grant_type", "external_auth" },
                { "nonce", nonce },
                { "deployment_id", deploymentId },
                { "external_auth_token", externalAuthToken },
                { "external_auth_type", externalAuthType }
            };

            return await InternalMakeTokenRequest(auth, body);
        }

        private async Task<TokenResponse> InternalMakeTokenRequest(string basicAuthStr, Dictionary<string, string> body)
        {
            using HttpRequestMessage request = new(HttpMethod.Post, $"{_url}/auth/v1/oauth/token")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Basic", basicAuthStr)},
                Content = new FormUrlEncodedContent(body)
            };
            
            using HttpResponseMessage response = await _client.SendAsync(request);
            string jsonStr = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonStr);
            
            TokenResponse? token = JsonConvert.DeserializeObject<TokenResponse>(jsonStr);
            if (token is not null) return token;

            return new TokenResponse();
        }

        #endregion
    }
}