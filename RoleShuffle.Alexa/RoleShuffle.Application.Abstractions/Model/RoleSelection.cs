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
    }
}