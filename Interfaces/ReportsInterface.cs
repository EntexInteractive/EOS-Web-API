﻿using EpicGames.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EpicGames.Web.Interfaces
{
    /// <summary>
    /// The report reason Id.
    /// </summary>
    public enum ReportReason
    {
        Cheating = 1,
        Exploiting = 2,
        OffensiveProfile = 3,
        VerbalAbuse = 4,
        Scamming = 5,
        Spamming = 6,
        Other = 7
    }

    /// <summary>
    /// Represents the Player Reports Web API within Epic Online Services.
    /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/player-reports-web-apis"/></para>
    /// </summary>
    public class ReportsInterface(string accessToken, string url = "https://api.epicgames.dev")
    {
        private readonly HttpClient _client = new();
        private readonly string _accessToken = accessToken;
        private readonly string _url = url;

        /// <summary>
        /// Sends a new player report. 
        /// <para>The client policy used must have the following permission: 'playerreports:sendReportForAnyUser'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/player-reports-web-apis#sending-new-player-reports"/></para>
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
        public async Task<bool> SendNewPlayerReportAsync(Report report)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();
            body.Add("reportingPlayerId", report.ReportingPlayerId);
            body.Add("reportedPlayerId", report.ReportedPlayerId);
            body.Add("time", report.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
            body.Add("reasonId", ((int)report.Reason).ToString());
            if (!string.IsNullOrEmpty(report.Message)) body.Add("message", report.Message);
            if (!string.IsNullOrEmpty(report.Context)) body.Add("context", report.Context);

            using HttpRequestMessage request = new(HttpMethod.Post, $"{_url}/player-reports/v1/report")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) },
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "text/plain")
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Finds player reports from a deployment id. 
        /// <para>The client policy used must have the following permission: 'playerreports:findReportsForAnyUser'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/player-reports-web-apis#find-player-reports"/></para>
        /// </summary>
        /// <param name="deploymentId"></param>
        /// <returns>An array of found <see cref="Report"/>s.</returns>
        public async Task<Report[]> GetPlayerReportsAsync(string deploymentId)
        {
            List<Report> reportList = new List<Report>();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{_url}/player-reports/v1/report/{deploymentId}?endTime={DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach(JObject item in jsonObject["elements"])
            {
                reportList.Add(new Report(item));
            }

            return reportList.ToArray();
        }

        /// <summary>
        /// Finds player reports from a deployment id and a reported player id. 
        /// <para>The client policy used must have the following permission: 'playerreports:findReportsForAnyUser'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/player-reports-web-apis#find-player-reports"/></para>
        /// </summary>
        /// <param name="deploymentId"></param>
        /// <param name="reportedPlayerId"></param>
        /// <returns>An array of found <see cref="Report"/>s.</returns>
        public async Task<Report[]> GetPlayerReportsAsync(string deploymentId, string reportedPlayerId)
        {
            List<Report> reportList = new List<Report>();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{_url}/player-reports/v1/report/{deploymentId}?reportedPlayerId={reportedPlayerId}")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach (JObject item in jsonObject["elements"])
            {
                reportList.Add(new Report(item));
            }

            return reportList.ToArray();
        }

        /// <summary>
        /// Finds player reports from a deployment id and <see cref="ReportReason"/>.
        /// <para>The client policy used must have the following permission: 'playerreports:findReportsForAnyUser'</para>
        /// <para><seealso href="https://dev.epicgames.com/docs/web-api-ref/player-reports-web-apis#find-player-reports"/></para>
        /// </summary>
        /// <param name="deploymentId"></param>
        /// <param name="reason"></param>
        /// <returns>An array of found <see cref="Report"/>s.</returns>
        public async Task<Report[]> GetPlayerReportsAsync(string deploymentId, ReportReason reason)
        {
            List<Report> reportList = new List<Report>();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{_url}/player-reports/v1/report/{deploymentId}?reasonId={(int)reason}")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
            };

            using HttpResponseMessage response = await _client.SendAsync(request);
            JObject jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            foreach (JObject item in jsonObject["elements"])
            {
                reportList.Add(new Report(item));
            }

            return reportList.ToArray();
        }
    }
}
