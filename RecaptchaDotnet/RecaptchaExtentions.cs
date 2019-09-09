using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using RecaptchaDotnet.Services;
using RecaptchaDotnet.Services.Implementations;

namespace RecaptchaDotnet
{
    public static class RecaptchaExtensions
    {
        /// <summary>
        /// Adds Recaptcha Services to <paramref name="services"/>
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configure">Configure recaptcha settings</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddRecaptcha(this IServiceCollection services, Action<RecaptchaSettings> configure)
        {
            services.Configure(configure);

            services.AddSingleton<ReCaptchaAttribute>();

            services.AddSingleton<IRecaptchaService, RecaptchaService>();
            
            services.AddHttpClient(Constants.RecaptchaHttpClient)
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(i * 200)));

            return services;
        }

        /// <summary>
        /// Adds recaptcha verification to each request coming though
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns>Application builder</returns>
        public static IApplicationBuilder UseRecaptcha(this IApplicationBuilder app)
        {
            app.UseMiddleware<RecaptchaMiddleware>();
            return app;
        }
    }
}
