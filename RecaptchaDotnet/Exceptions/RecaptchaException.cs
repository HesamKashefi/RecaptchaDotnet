using System;

namespace RecaptchaDotnet.Exceptions
{
    /// <summary>
    /// Base class for recaptcha exceptions
    /// </summary>
    public abstract class RecaptchaException : Exception
    {
        protected RecaptchaException(string message) : base(message)
        {
            
        }
    }
}
