namespace RoleShuffle.Application.ResponseMessages
{
    public static class MessageCreator
    {
        public static IMessages CreateMessages()
        {
            var deDeResource = new Messages("de-de")
            {
                SkillName = "Rollen Knecht",
                InvocationName = "Rollen Knecht",
                HelpMessage =
                    "Der Rollen Knecht kann den Spielleiter für mehrere Spiele übernehmen. " +
                    "Ich helfe unter anderem bei der Rollenverteilung von Secret Hitler helfen, kann aber auch in Nachtphase von Werfwolf die einzelnen Phasen ansagen." +
                    "Sage mir zum Beispiel: Alexa sage Rollen Knecht, ich möchte ein neues Spiel starten. " +
                    "Alles weitere erfahrt ihr dann im Anschluss.",
                StopMessage = "Bis bald!",
                Launch = "Rollen Knecht wurde gestartet. Wie kann ich weiterhelfen?",
                
                ErrorNotFound = "Entschuldigung, das habe ich nicht verstanden.",
                ErrorNotFoundIntent = "Der Intent wurde nicht gefunden.",
                ErrorNotFoundUser = "Es wurde keine Benutzerkennung übermittelt.",
                ErrorRequestTypeNotSupported = "Der Requesttype \"{0}\" wird von diesem Skill nicht unterstützt.",
            };

            return deDeResource;
        }
    }
}
