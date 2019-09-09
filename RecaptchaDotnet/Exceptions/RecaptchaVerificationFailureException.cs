namespace RecaptchaDotnet.Exceptions
{
    /// <summary>
    /// Indicates a failed recaptcha (unsuccessful state)
    /// </summary>
    public class RecaptchaVerificationFailureException : RecaptchaException
    {
        public RecaptchaVerificationFailureException(string message) : base(message)
        {

        }
    }
}