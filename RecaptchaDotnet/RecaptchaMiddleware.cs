using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RecaptchaDotnet.Exceptions;
using RecaptchaDotnet.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecaptchaDotnet
{
    public class RecaptchaMiddleware
    {
        #region Dependencies
        private readonly RequestDelegate _next;
        private readonly IRecaptchaService _recaptchaService;
        private readonly IOptionsMonitor<RecaptchaSettings> _options;

        #endregion

        #region Ctor
        public RecaptchaMiddleware(RequestDelegate next, IRecaptchaService recaptchaService, IOptionsMonitor<RecaptchaSettings> options)
        {
            _next = next;
            _recaptchaService = recaptchaService;
            _options = options;
        }
        #endregion

        public async Task Invoke(HttpContext httpContext)
        {
            var mode = _options.CurrentValue.InvalidRecaptchaResponseMode;

            if (httpContext.Request.Headers.All(x => !x.Key.Equals(Constants.ReCaptchaHeader, StringComparison.InvariantCultureIgnoreCase)))
            {
                await RejectRequestAsync(httpContext, mode);
            }

            var reCaptchaResponse = httpContext.Request.Headers.FirstOrDefault(x =>
                x.Key.Equals(Constants.ReCaptchaHeader, StringComparison.InvariantCultureIgnoreCase)).Value;

            if (string.IsNullOrEmpty(reCaptchaResponse))
            {
                await RejectRequestAsync(httpContext, mode);
            }

            var recaptchaVerificationResult = await _recaptchaService.VerifyAsync(reCaptchaResponse, CancellationToken.None);

            if (!recaptchaVerificationResult.Success)
            {
                const string msg = "Recaptcha verification failed";
                switch (mode)
                {
                    case InvalidRecaptchaResponseMode.ThrowRecaptchaException:
                        throw new RecaptchaVerificationFailureException(msg);
                    case InvalidRecaptchaResponseMode.ReturnBadRequest:
                        httpContext.Response.StatusCode = 400;
                        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { msg }));
                        break;
                }
            }

            await _next.Invoke(httpContext);
        }

        #region Private Methods
        private static async Task RejectRequestAsync(HttpContext httpContext, InvalidRecaptchaResponseMode mode)
        {
            const string msg = "ReCaptcha header is empty";
            switch (mode)
            {
                case InvalidRecaptchaResponseMode.ThrowRecaptchaException:
                    throw new RecaptchaMissingException(msg);
                case InvalidRecaptchaResponseMode.ReturnBadRequest:
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { msg }));
                    break;
            }
        }
        #endregion
    }
}
