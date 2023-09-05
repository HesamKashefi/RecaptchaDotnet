# RecaptchaDotnet

A simple, easy to use library for Google Recaptcha verification in .net apps specially `ASP.NET Core`.

## Build Status

[![build](https://github.com/HesamKashefi/RecaptchaDotnet/actions/workflows/build.yml/badge.svg)](https://github.com/HesamKashefi/RecaptchaDotnet/actions/workflows/build.yml)

## Installation

With dotnet cli

    dotnet add package RecaptchaDotnet
----
Or with nuget package manager console
    
    Install-Package RecaptchaDotnet

## How to use


### Configure services

In yuour `Startup.cs`'s ConfigureServices method add the following

    public void ConfigureServices(IServiceCollection services) 
    {
        services.AddRecaptcha(config => 
        {
            // your secret code from google
            config.Secret = "YOUR_SECRET_CODE";
            
            //configure reaction to unsuccessful verifications
            config.InvalidRecaptchaResponseMode = InvalidRecaptchaResponseMode.ReturnBadRequest;
        })
    }

### Recaptcha Attribute

Add `ReCaptchaAttribute` to your actions or controllers to enable recaptcha for it

    
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        ...

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ReCaptchaAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            ...
        }
    }

### Recaptcha Middleware

You can also use Recaptcha Middleware to enable recaptcha for every request

    public void Configure(IApplicationBuilder app) 
    {
        // Add this before Useing mvc and Authentication middlewares
        app.UseRecaptcha();

        app.UseAuthentication();
        app.UseMvcWithDefaultRoute();
    }

## Client Side

All you need to do is to add a Header with name `Recaptcha` with the verification code that Google Recaptcha generates for the user after interaction of user.
