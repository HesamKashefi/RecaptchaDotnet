using Microsoft.AspNetCore.Mvc.Filters;
using RecaptchaDotnet.Exceptions;
using RecaptchaDotnet.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace RecaptchaDotnet
{
    /// <summary>
    /// Reads ReCaptcha Response from header and validates it
    /// If it's not valid, this returns BadRequest
    /// </summary>
    public class ReCaptchaAttribute : ActionFilterAttribute
    {
        #region Dependencies
        private readonly IRecaptchaService _recaptchaService;
        private readonly IOptionsMonitor<RecaptchaSettings> _options;

        #endregion

        #region Ctor
        public ReCaptchaAttribute(IRecaptchaService recaptchaService, IOptionsMonitor<RecaptchaSettings> options)
        {
            _recaptchaService = recaptchaService;
            _options = options;
        }
        #endregion

        #region Overrides
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var mode = _options.CurrentValue.InvalidRecaptchaResponseMode;

            if (context.HttpContext.Request.Headers.All(x => !x.Key.Equals(Constants.ReCaptchaHeader, StringComparison.InvariantCultureIgnoreCase)))
            {
                RejectRequest(context, mode);
                return;
            }

            var reCaptchaResponse = context.HttpContext.Request.Headers.FirstOrDefault(x =>
                x.Key.Equals(Constants.ReCaptchaHeader, StringComparison.InvariantCultureIgnoreCase)).Value;

            if (string.IsNullOrEmpty(reCaptchaResponse))
            {
                RejectRequest(context, mode);
                return;
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
                        context.Result = new BadRequestObjectResult(new { error = msg });
                        break;
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
        #endregion

        #region Private Methods
        private static void RejectRequest(ActionExecutingContext context, InvalidRecaptchaResponseMode mode)
        {
            const string msg = "ReCaptcha header is empty";
            switch (mode)
            {
                case InvalidRecaptchaResponseMode.ThrowRecaptchaException:
                    throw new RecaptchaMissingException(msg);
                case InvalidRecaptchaResponseMode.ReturnBadRequest:
                    context.Result = new BadRequestObjectResult(new { error = msg });
                    break;
            }
        }
        #endregion
    }
}