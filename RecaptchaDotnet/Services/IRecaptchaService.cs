using System.Threading;
using System.Threading.Tasks;

namespace RecaptchaDotnet.Services
{
    /// <summary>
    /// Used to verify recaptcha response
    /// </summary>
    public interface IRecaptchaService
    {
        /// <summary>
        /// Verifies recaptcha response
        /// </summary>
        /// <param name="response">ReCaptcha response from the client</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Recaptcha Response</returns>
        Task<RecaptchaResponse> VerifyAsync(string response, CancellationToken cancellationToken);
    }
}