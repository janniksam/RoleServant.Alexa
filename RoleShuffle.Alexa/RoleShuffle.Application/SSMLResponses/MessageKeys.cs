namespace RoleShuffle.Application.SSMLResponses
{
    public static class MessageKeys
    {
        public static class AllLanguages
        {
            public const string ErrorMalformedRequest = "ErrorMalformedRequest";
        }

        public const string HelpMessage = "HelpMessage";
        public const string StopMessage = "StopMessage";
        public const string LaunchMessage = "LaunchMessage";

        public const string GameNoActiveGame = "GameNoActiveGame";
        public const string GameNoNightPhase = "GameNoNightPhase";
        public const string GameNoDistributionPhase = "GameNoDistributionPhase";
        public const string GameStartContinueWithDistributionPhase = "GameStartContinueWithDistributionPhase";
        public const string GameStartContinueWithNightPhase = "GameStartContinueWithNightPhase";

        public const string ErrorIntentNotFound = "ErrorIntentNotFound";
        public const string ErrorRequestTypeNotSupported = "ErrorRequestTypeNotSupported";
        public const string ErrorNoOpenGame = "ErrorNoOpenGame";
    }
}