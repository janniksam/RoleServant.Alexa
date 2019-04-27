using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace RoleShuffle.Base.Aspects
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface)]
    public class LogResultAttribute : AbstractInterceptorAttribute
    {
        private readonly LogLevel m_level;

        public LogResultAttribute(LogLevel level = LogLevel.Debug)
        {
            m_level = level;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var logger = GetLogger(context);
            if (!logger.IsEnabled(m_level))
            {
                await next(context);
                return;
            }

            await next(context);
            logger.Log(m_level, $"{context.ImplementationMethod.Name} returned: {context.ReturnValue}");
        }

        private static ILogger GetLogger(AspectContext context)
        {
            var type = context.Implementation.GetType();
            var genericBase = typeof(ILogger<>);
            var combinedType = genericBase.MakeGenericType(type);
            var logger = (ILogger) context.ServiceProvider.GetService(combinedType);
            return logger;
        }
    }
}