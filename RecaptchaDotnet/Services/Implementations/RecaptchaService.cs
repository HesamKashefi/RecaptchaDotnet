using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecaptchaDotnet.Exceptions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RecaptchaDotnet.Services.Implementations
{
    public class RecaptchaService : IRecaptchaService
    {
        #region Constants
        const string Endpoint = "https://www.google.com/recaptcha/api/siteverify";
        #endregion

        #region Dependecies
        private readonly IOptionsMonitor<RecaptchaSettings> _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        #endregion

        #region Ctor
        public RecaptchaService(IOptionsMonitor<RecaptchaSettings> options, IHttpClientFactory httpClientFactory, ILogger<RecaptchaService> logger)
        {
            _options = options;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        #endregion

        #region Implementation of IRecaptchaService
        public async Task<RecaptchaResponse> VerifyAsync(string response, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Requesting ReCaptcha verification - response = {response}", response);

            var data = new MultipartFormDataContent
            {
                { new StringContent(response), "response" },
                { new StringContent(_options.CurrentValue.Secret), "secret" }
            };
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
            request.Headers.Clear();
            request.Headers.Add("Accept", "application/json");
            request.Content = data;

            var client = _httpClientFactory.CreateClient(Constants.RecaptchaHttpClient);

            var httpResponse = await client.SendAsync(request, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var error = await httpResponse.Content.ReadAsStringAsync();
                _logger.LogError("ReCaptcha verification failed - message : {error}", error);
                throw new RecaptchaVerificationFailureException(error);
            }

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadAsAsync<RecaptchaResponse>(cancellationToken);

            _logger.LogTrace("ReCaptcha Response Received : {@result}", result);

            return result;
        }
        #endregion
    }
}
