using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.Services;
using RoleShuffle.Application.SSMLResponses;

namespace RoleShuffle.Application.Services
{
    public class ApplicationWarmUp : IApplicationWarmUp
    {
        private readonly IEnumerable<IGame> m_games;

        public ApplicationWarmUp(IEnumerable<IGame> games)
        {
            m_games = games;
        }

        public async Task WarmUp()
        {
            foreach (var view in MessageKeys.GetRequiredLocalizedSSMLViews())
            {
                foreach (var supportedLocale in Localization.GetSupportedLocales())
                {
                    try
                    {
                        await CommonResponseCreator.GetSSMLAsync(view, supportedLocale);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            foreach (var game in m_games)
            {
                foreach (var requiredSSMLView in game.GetRequiredSSMLViews())
                {
                    foreach (var locale in Localization.GetSupportedLocales())
                    {
                        try
                        {
                            await CommonResponseCreator.GetGameSpecificSSMLAsync(game.GameId, requiredSSMLView, locale);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}