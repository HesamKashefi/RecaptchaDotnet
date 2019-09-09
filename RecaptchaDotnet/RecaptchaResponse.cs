using Newtonsoft.Json;
using System.Collections.Generic;

namespace RecaptchaDotnet
{
    /// <summary>
    /// Response of google verification
    /// </summary>
    public class RecaptchaResponse
    {
        /// <summary>
        /// Indicates success or failure
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        [JsonProperty("challenge_ts")]
        public string ChallengeTimestamp { get; set; }

        /// <summary>
        /// the hostname of the site where the reCAPTCHA was solved
        /// </summary>
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// missing-input-secret | invalid-input-secret | missing-input-response | invalid-input-response | bad-request
        /// </summary>
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}