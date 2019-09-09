namespace RecaptchaDotnet
{
    public enum InvalidRecaptchaResponseMode : byte
    {
        ReturnBadRequest = 0,
        ThrowRecaptchaException = 1
    }
}