using System;
using System.Collections.Generic;
using System.Linq;
using RoleShuffle.Application.Abstractions.Model;
using RoleShuffle.Application.Abstractions.RoleManager;

namespace RoleShuffle.Application.RoleManager
{
    public class OneNightUltimateWerewolfRoleManager : IOneNightUltimateWerewolfRoleManager
    {
        private readonly Dictionary<int, RoleSelection> m_roleSelections;

        public OneNightUltimateWerewolfRoleManager()
        {
            m_roleSelections = new Dictionary<int, RoleSelection>();
        }

        public int AddRoleSelection(RoleSelection roleSelection)
        {
            lock (m_roleSelections)
            {
                var exists = m_roleSelections.Any(p => p.Value.IsSameDeck(roleSelection));
                if (exists)
                {
                    var key = m_roleSelections.FirstOrDefault(p => p.Value.IsSameDeck(roleSelection)).Key;
                    m_roleSelections[key].Expiration = DateTime.UtcNow.AddDays(7);
                    return key;
                }

                roleSelection.Expiration = DateTime.UtcNow.AddDays(14);

                for (var i = 1; i <= 100000; i++)
                {
                    DeleteIfExpired(i);
                    if (!m_roleSelections.ContainsKey(i))
                    {
                        m_roleSelections.Add(i, roleSelection);
                        return i;
                    }
                }

                throw new IndexOutOfRangeException("Es gibt aktuell zu viele Decks.");
            }
        }

        private void DeleteIfExpired(int key)
        {
            if (m_roleSelections.ContainsKey(key))
            {
                var existingSelection = m_roleSelections[key];
                if (existingSelection.Expiration <= DateTime.UtcNow)
                {
                    m_roleSelections.Remove(key);
                }
            }
        }

        public RoleSelection GetRoleSelection(int deckId)
        {
            lock (m_roleSelections)
            {
                var exists = m_roleSelections.ContainsKey(deckId);
                if (exists)
                {
                    return m_roleSelections[deckId];
                }

                return null;
            }
        }

        public RoleSelection LoadRoles(string deckSelection)
        {
            var roleSelection = new RoleSelection();
            DieErsteNacht(deckSelection, roleSelection);
            Mondsucht(deckSelection, roleSelection);
            EinsameNacht(deckSelection, roleSelection);
            Konfusion(deckSelection, roleSelection);

            return roleSelection;
        }

        private static void DieErsteNacht(string deckSelection, RoleSelection roleSelection)
        {
            if (deckSelection == "DEN3")
            {
                roleSelection.Villager = 1;
                roleSelection.Werewolf = 2;
                roleSelection.Seer = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
            }
            else if (deckSelection == "DEN4")
            {
                roleSelection.Villager = 2;
                roleSelection.Werewolf = 2;
                roleSelection.Seer = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
            }
            else if (deckSelection == "DEN5")
            {
                roleSelection.Villager = 3;
                roleSelection.Werewolf = 2;
                roleSelection.Seer = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
            }
        }

        private static void Mondsucht(string deckSelection, RoleSelection roleSelection)
        {
            if (deckSelection == "MS3")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Insomniac = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 1;
            }
            else if (deckSelection == "MS4")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Insomniac = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 2;
            }
            else if (deckSelection == "MS5")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Insomniac = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 2;
                roleSelection.Seer = 1;
            }
            else if (deckSelection == "MS6")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Insomniac = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 3;
                roleSelection.Seer = 1;
            }
        }

        private static void EinsameNacht(string deckSelection, RoleSelection roleSelection)
        {
            if (deckSelection == "EN3")
            {
                roleSelection.Werewolf = 1;
                roleSelection.Seer = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 2;
            }
            else if (deckSelection == "EN4")
            {
                roleSelection.Werewolf = 1;
                roleSelection.Seer = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Villager = 2;
            }
        }
        
        private static void Konfusion(string deckSelection, RoleSelection roleSelection)
        {
            if (deckSelection == "KF3")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
            }
            else if (deckSelection == "KF4")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 1;
            }
            else if (deckSelection == "KF5")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 1;
                roleSelection.Seer = 1;
            }
            else if (deckSelection == "KF6")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 2;
                roleSelection.Seer = 1;
            }
            else if (deckSelection == "KF7")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 3;
                roleSelection.Seer = 1;
            }
            else if (deckSelection == "KF8")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 3;
                roleSelection.Seer = 1;
                roleSelection.Minion = 1;
            }
            else if (deckSelection == "KF9")
            {
                roleSelection.Werewolf = 2;
                roleSelection.Drunk = 1;
                roleSelection.Robber = 1;
                roleSelection.Troublemaker = 1;
                roleSelection.Insomniac = 1;
                roleSelection.Villager = 2;
                roleSelection.Seer = 1;
                roleSelection.Minion = 1;
                roleSelection.Mason = 2;
            }
        }
    }
}