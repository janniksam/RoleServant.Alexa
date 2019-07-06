namespace RoleShuffle.Base
{
    public static class Constants
    {
        public static class Intents
        {
            //Basic stuff
            public const string Stop = "AMAZON.StopIntent";
            public const string Cancel = "AMAZON.CancelIntent";
            public const string Help = "AMAZON.HelpIntent";
            public const string DistributeRoles = "DistributeRolesIntent";
            public const string StartNewGame = "StartNewGameIntent";
            public const string NightPhase = "NightPhaseIntent";
        }

        public static class Slots
        {
            public const string PlayerAmount = "PlayerAmount";
            public const string GameNumber = "GameNumber";
            public const string DeckId = "DeckId";
            public const string ConfirmAction = "ConfirmAction";
        }


        public static class SlotYesNoResult
        {
            public const string Yes = "Yes";
            public const string No = "No";
        }

        public static class GameNumbers
        {
            public const short SecretHitler = 1;
            public const short OneNightUltimateWerewolf = 2;
            public const short Insider = 3;
            public const short TheResistanceAvalon = 4;
            public const short ActOfTreason = 5;
        }
    }
}