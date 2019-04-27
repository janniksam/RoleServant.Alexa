using System;
using System.IO;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace RoleShuffle.Web.Validation
{
    public class AlexaRequestValidator : IAlexaRequestValidator
    {
        public async Task<bool> ValidateRequest(HttpRequest request, SkillRequest skillRequest)
        {
            var isTimestampValid = RequestVerification.RequestTimestampWithinTolerance(skillRequest);
            try
            {
                request.Headers.TryGetValue("SignatureCertChainUrl", out var signatureChainUrl);
                if (string.IsNullOrWhiteSpace(signatureChainUrl))
                {
                    return false;
                }

                Uri certUrl;
                try
                {
                    certUrl = new Uri(signatureChainUrl);
                }
                catch
                {
                    return false;
                }

                request.Headers.TryGetValue("Signature", out var signature);
                if (string.IsNullOrWhiteSpace(signature))
                {
                    return false;
                }

                var body = "";
                request.EnableRewind();
                using (var stream = new StreamReader(request.Body))
                {
                    stream.BaseStream.Position = 0;
                    body = stream.ReadToEnd();
                    stream.BaseStream.Position = 0;
                }

                if (string.IsNullOrWhiteSpace(body))
                {
                    return false;
                }

                var valid = await RequestVerification.Verify(signature, certUrl, body).ConfigureAwait(false);
                return valid && isTimestampValid;
            }
            catch
            {
                return false;
            }
        }
    }
}