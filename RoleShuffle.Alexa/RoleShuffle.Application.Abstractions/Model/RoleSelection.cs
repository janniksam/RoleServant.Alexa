using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RoleShuffle.Application.Abstractions.Validation;

namespace RoleShuffle.Application.Abstractions.Model
{
    public class RoleSelection
    {
        #region Deck
        public short Villager { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Betrunkene geben")]
        public short Drunk { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Dorfgängerinnen geben")]
        public short Doppelganger { get; set; }

        [NullOrTwo(ErrorMessage = "Es kann nur 0 oder 2 Freimaurer geben")]
        public short Mason { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Gerber geben")]
        public short Tanner { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Günstlinge geben")]
        public short Minion { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Jäger geben")]
        public short Hunter { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Räuber geben")]
        public short Robber { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Schlaflose geben")]
        public short Insomniac { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Seherinnen geben")]
        public short Seer { get; set; }

        [Range(0, 1, ErrorMessage = "Es kann nur 0 oder 1 Unruhestifterinnen geben")]
        public short Troublemaker { get; set; }

        public short Werewolf { get; set; }

        [Range(6, int.MaxValue, ErrorMessage = "Die Kartenanzahl muss mindestens 6 betragen")]
        public int? PlayerCount => PreSelection == null
            ? (Villager + Doppelganger + Drunk + Mason + Tanner + Minion + Hunter + Robber + Insomniac + Seer + Troublemaker + Werewolf)
            : (int?) null;

        public bool IsSameDeck(RoleSelection selection)
        {
            return Villager == selection.Villager &&
                   Drunk == selection.Drunk &&
                   Doppelganger == selection.Doppelganger &&
                   Mason == selection.Mason &&
                   Tanner == selection.Tanner &&
                   Minion == selection.Minion &&
                   Hunter == selection.Hunter &&
                   Robber == selection.Robber &&
                   Insomniac == selection.Insomniac &&
                   Seer == selection.Seer &&
                   Troublemaker == selection.Troublemaker &&
                   Werewolf == selection.Werewolf;
        }
        #endregion

        public int? DeckId { get; set; }

        public DateTime Expiration { get; set; }

        public string PreSelection { get; set; }

        public string DeckSummary
        {
            get
            {
                var rolesIncluded = new List<string>
                {
                    AddRoleToSummary(Drunk, "Ein Betrunkener", "Betrunkene"),
                    AddRoleToSummary(Villager, "Ein Dorfbewohner", "Dorfbewohner"),
                    AddRoleToSummary(Mason, "Ein Freimaurer", "Freimaurer"),
                    AddRoleToSummary(Hunter, "Ein Jäger", "Jäger"),
                    AddRoleToSummary(Robber, "Ein Räuber", "Räuber"),
                    AddRoleToSummary(Insomniac, "Eine Schlaflose", "Schlaflose"),
                    AddRoleToSummary(Seer, "Eine Seherin", "Seher"),
                    AddRoleToSummary(Troublemaker, "Eine Unruhestifterin", "Unruhestifter"),
                    AddRoleToSummary(Doppelganger, "Eine Doppelgängerin", "Doppelgänger"),
                    AddRoleToSummary(Tanner, "Ein Gerber", "Gerber"),
                    AddRoleToSummary(Minion, "Ein Günstling", "Günstlinge"),
                    AddRoleToSummary(Werewolf, "Ein Werwolf", "Werwölfe")
                };

                rolesIncluded = rolesIncluded.Where(p => !string.IsNullOrEmpty(p)).ToList();

                if (rolesIncluded.Count == 0)
                {
                    return null;
                }

                var summary = string.Join(", ", rolesIncluded.Take(rolesIncluded.Count - 1));
                return $"{summary} und {rolesIncluded.Last()}";
            }
        }

        private string AddRoleToSummary(short amount, string singular, string plural)
        {
            if (amount > 0)
            {
                if (amount == 1)
                {
                    return $"{singular}";
                }

                return $"{amount} {plural}";
            }

            return null;
        }
    }
}