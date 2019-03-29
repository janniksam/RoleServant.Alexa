namespace RoleShuffle.Application.ResponseMessages
{
    internal class Messages : IMessages
    {
        public Messages(string language)
        {
            Language = language;
        }

        public string Language { get; }

        public string SkillName { get; set; }

        public string InvocationName { get; set; }

        public string HelpMessage { get; set; }

        public string StopMessage { get; set; }

        public string Launch { get; set; }

        public string ErrorNotFound { get; set; }

        public string ErrorNotFoundIntent { get; set; }

        public string ErrorRequestTypeNotSupported { get; set; }

        public string ErrorNoOpenGame { get; set; }
    }
}
