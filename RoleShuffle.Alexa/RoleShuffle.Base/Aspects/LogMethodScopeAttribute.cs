using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace RoleShuffle.Base.Aspects
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Interface)]
    public class LogMethodScopeAttribute : AbstractInterceptorAttribute
    {
        private readonly LogLevel m_level;

        public LogMethodScopeAttribute(LogLevel level = LogLevel.Debug)
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

            var methodName = context.ImplementationMethod.Name;
            var sw = new Stopwatch();
            try
            {
                logger.Log(m_level, $"Entering {methodName}");

                sw.Start();
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"Exception executing method {methodName}");
                throw;
            }
            finally
            {
                sw.Stop();
                logger.Log(m_level, $"Leaving {methodName} (Took {sw.ElapsedMilliseconds}ms)");
            }
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