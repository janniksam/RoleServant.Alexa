using Microsoft.Extensions.DependencyInjection;
using RoleShuffle.Application.Abstractions.Games;
using RoleShuffle.Application.Abstractions.RequestHandler;
using RoleShuffle.Application.Abstractions.RoleManager;
using RoleShuffle.Application.Games.Insider;
using RoleShuffle.Application.Games.OneNightUltimateWerewolf;
using RoleShuffle.Application.Games.SecretHitler;
using RoleShuffle.Application.Games.TheResistanceAvalon;
using RoleShuffle.Application.Intents;
using RoleShuffle.Application.RequestHandler;
using RoleShuffle.Application.RoleManager;

namespace RoleShuffle.IoC
{
    public static class DependencyInjectionExtensions
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAlexaHandler, AlexaHandler>();
            services.AddTransient<IIntentHandler, IntentHandler>();

            services.AddTransient<IIntent, AmazonHelpIntent>();
            services.AddTransient<IIntent, AmazonStopIntent>();
            services.AddTransient<IIntent, AmazonCancelIntent>();
            services.AddTransient<IIntent, StartNewGameIntent>();
            services.AddTransient<IIntent, DistributeRolesIntent>();
            services.AddTransient<IIntent, NightPhaseIntent>();

            services.AddSingleton<IGame, SecretHitlerGame>();
            services.AddSingleton<IGame, OneNightUltimateWerewolfGame>();
            services.AddSingleton<IGame, InsiderGame>();
            services.AddSingleton<IGame, TheResistanceAvalonGame>();

            services.AddSingleton<IOneNightUltimateWerewolfRoleManager, OneNightUltimateWerewolfRoleManager>();
        }
    }
}