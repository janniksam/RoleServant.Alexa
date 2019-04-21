using System.ComponentModel;
using Alexa.NET.Request;

namespace RoleShuffle.Application.Extensions
{
    public static class IntentExtensions
    {
        public static string GetSlot(this Intent intent, string slotName)
        {
            if (intent?.Slots == null)
            {
                return null;
            }

            if (intent.Slots.ContainsKey(slotName))
            {
                var slot = intent.Slots[slotName];
                if (string.IsNullOrEmpty(slot.Value))
                {
                    return slot.Value;
                }

                if (slot.Resolution?.Authorities?.Length == 1 &&
                    slot.Resolution.Authorities[0].Status?.Code == ResolutionStatusCode.SuccessfulMatch &&
                    slot.Resolution.Authorities[0].Values?.Length == 1)
                {
                    return slot.Resolution.Authorities[0].Values[0].Value?.Name ?? slot.Value;
                }

                return slot.Value;
            }

          return null;

        }
    }
}