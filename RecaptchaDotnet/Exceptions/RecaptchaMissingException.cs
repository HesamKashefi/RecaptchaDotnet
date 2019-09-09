namespace RecaptchaDotnet.Exceptions
{
    /// <summary>
    /// Indicates missing recaptcha in the incoming request
    /// </summary>
    public class RecaptchaMissingException : RecaptchaException
    {
        public RecaptchaMissingException(string message) : base(message)
        {

        }
    }
}
