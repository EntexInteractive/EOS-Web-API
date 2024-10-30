// Copyright 2024 Entex Interactive, LLC

using Newtonsoft.Json;

namespace EpicGames.Web.Responses
{
    /// <summary>
    /// Represents an access token response.
    /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/connect-web-api#example-request"/></para>
    /// </summary>
    public sealed class TokenResponse
    {
        #region Properties

        /// <summary>
        /// Generated access token as a JSON Web Token (JWT) string. The access token describes the authenticated client and user.
        /// </summary>
        public string? AccessToken { get; } = null;

        /// <summary>
        /// Token expiration time.
        /// </summary>
        public DateTime ExpiresAt { get; } = DateTime.UnixEpoch;

        /// <summary>
        /// Arbitrary string value provided by the client in the token request. Used by clients for added security. When receiving an access token in HTTP response, the client can verify that the token response includes the expected nonce value.
        /// </summary>
        public string? Nonce { get; } = null;

        /// <summary>
        /// Your organization identifier.
        /// </summary>
        public string? OrganizationId { get; } = null;

        /// <summary>
        /// Your product identifier.
        /// </summary>
        public string? ProductId { get; } = null;

        /// <summary>
        /// Your sandbox identifier.
        /// </summary>
        public string? SandBoxId { get; } = null;

        /// <summary>
        /// Your deployment identifier.
        /// </summary>
        public string? DeploymentId { get; } = null;

        /// <summary>
        /// Identifies the EOS user uniquely across products within your organization. Included for user access tokens.
        /// </summary>
        public string? OrganizationUserId { get; } = null;

        /// <summary>
        /// The authenticated EOS Product User ID. Included for user access tokens.
        /// </summary>
        public string? ProductUserId { get; } = null;

        /// <summary>
        /// An ID token that securely identifies the EOS Product User. Included for user access tokens.
        /// </summary>
        public string? IdToken { get; } = null;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenResponse"/> class.
        /// </summary>
        public TokenResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenResponse"/> class.
        /// </summary>
        [JsonConstructor]
        public TokenResponse(string access_token, string expires_at, string nouce, string organization_id, string product_id, string sandbox_id, string deployment_id, string organization_user_id, string product_user_id, string id_token)
        {
            AccessToken = access_token;
            ExpiresAt = DateTime.Parse(expires_at);
            Nonce = nouce;
            OrganizationId = organization_id;
            ProductId = product_id;
            SandBoxId = sandbox_id;
            DeploymentId = deployment_id;
            OrganizationUserId = organization_user_id;
            ProductId = product_user_id;
            IdToken = id_token;
        }
    }
}