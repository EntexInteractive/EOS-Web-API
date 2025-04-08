using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicGames.Web.Models
{
    public class Sanction
    {
        /// <summary>
        /// Sanctioned user’s EOS ProductUserId.
        /// </summary>
        public string ProductUserId { get; set; }

        /// <summary>
        /// Action string associated with this sanction
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Justification string associated with this sanction.
        /// </summary>
        public string Justification { get; set; }

        /// <summary>
        /// Source which created this sanction.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Identity provider that the sanctioned user authenticated with.
        /// </summary>
        public string? IdentityProvider { get; set; }

        /// <summary>
        /// Sanctioned user's account ID with the specified <see cref="IdentityProvider"/>.
        /// </summary>
        public string? AccountId { get; set; }

        /// <summary>
        /// List of tags associated with this sanction.
        /// </summary>
        public string[]? Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Unique identifier for this sanction.
        /// </summary>
        public string? ReferenceId { get; set; }

        /// <summary>
        /// Time when this sanction was placed.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// Time when this expiration expires.
        /// </summary>
        public DateTime Expiration { get; } = DateTime.MaxValue;

        /// <summary>
        /// UUID for the request associated with this sanction. One request may create multiple sanctions as a batch.
        /// </summary>
        public string? BatchUuid { get; set; }


        public Sanction(string productUserId, string action, string justification, string source)
        {
            ProductUserId = productUserId;
            Action = action;
            Justification = justification;
            Source = source;
        }

        public Sanction(JObject item)
        {
            ProductUserId = (string)item["productUserId"];
            Action = (string)item["action"];
            Justification = (string)item["justification"];
            Source = (string)item["source"];
            IdentityProvider = (string)item["identityProvider"];
            AccountId = (string)item["accountId"];
            Tags = item["tags"].ToObject<string[]>();
            ReferenceId = (string)item["referenceId"];
            Timestamp = DateTime.Parse((string)item["timestamp"]);
            Expiration = string.IsNullOrEmpty((string)item["expirationTimestamp"]) ? DateTime.MaxValue : DateTime.Parse((string)item["expirationTimestamp"]);
            BatchUuid = (string)item["batchUuid"];
        }
    }
}
