using Alexa.NET.Response;

namespace RoleShuffle.Application.ResponseMessages
{
    public interface IMessages
    {
        string HelpMessage { get; set; }

        string StopMessage { get; set; }

        string Launch { get; set; }

        string ErrorNotFound { get; }

        string ErrorNotFoundIntent { get; }

        string ErrorRequestTypeNotSupported { get; }

        string ErrorNoOpenGame { get; }
    }
}