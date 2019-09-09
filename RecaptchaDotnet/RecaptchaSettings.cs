namespace RecaptchaDotnet
{
    /// <summary>
    /// Settings used to verify recaptcha requests
    /// </summary>
    public class RecaptchaSettings
    {
        /// <summary>
        /// Secret that you took from google
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Indicates how recaptcha must respond to Verification failures
        /// </summary>
        public InvalidRecaptchaResponseMode InvalidRecaptchaResponseMode { get; set; }
    }
}
