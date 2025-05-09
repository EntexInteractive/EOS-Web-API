﻿using EpicGames.Web.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicGames.Web.Models
{
    public class Report
    {
        /// <summary>
        /// The game EOS productId.
        /// </summary>
        public string ProductId { get; }

        /// <summary>
        /// The game EOS sandboxId.
        /// </summary>
        public string SandboxId { get; }

        /// <summary>
        /// The game EOS deploymentId.
        /// </summary>
        public string DeploymentId { get; }

        /// <summary>
        /// The EOS productUserId making the report.
        /// </summary>
        public string ReportingPlayerId { get; }

        /// <summary>
        /// The reported EOS productUserId. It must be different from the reportingPlayerId.
        /// </summary>
        public string ReportedPlayerId { get; }

        /// <summary>
        /// Time when the report was issued.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        /// <summary>
        /// The report reason.
        /// </summary>
        public ReportReason Reason { get; }

        /// <summary>
        /// The report content.
        /// </summary>
        public string? Message { get; set; } = null;

        /// <summary>
        /// The report context.
        /// </summary>
        public string? Context { get; set; } = null;


        public Report(string reportingPlayerId, string reportedPlayerId, ReportReason reason)
        {
            ReportingPlayerId = reportingPlayerId;
            ReportedPlayerId = reportedPlayerId;
            Timestamp = DateTime.UtcNow;
            Reason = reason;
        }

        [JsonConstructor]
        public Report(string productId, string sandboxId, string deploymentId, string reportingPlayerId, string reportedPlayerId, string time, int reasonId, string message, string context)
        {
            ProductId = productId;
            SandboxId = sandboxId;
            DeploymentId = deploymentId;
            ReportingPlayerId = reportingPlayerId;
            ReportedPlayerId = reportedPlayerId;
            Timestamp = DateTime.Parse(time);
            Reason = (ReportReason)reasonId;
            Message = message;
            Context = context;
        }

        public Report(JObject json)
        {
            ProductId = (string)json["productId"];
            SandboxId = (string)json["sandboxId"];
            DeploymentId = (string)json["deploymentId"];
            ReportingPlayerId = (string)json["reportingPlayerId"];
            ReportedPlayerId = (string)json["reportedPlayerId"];
            Timestamp = DateTime.Parse((string)json["time"]);
            Reason = (ReportReason)(int)json["reasonId"];
            Message = (string)json["message"];
            Context = (string)json["context"];
        }
    }
}
