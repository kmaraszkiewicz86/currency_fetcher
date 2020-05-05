using System;
using System.Collections.Generic;
using System.Text;
using CurrencyFetcherApi.Controllers;
using Microsoft.Extensions.Logging;

namespace CurrencyFetcherApi.Tests.Wrappers
{
    public class LoggerWrapper<TCategoryName> : ILogger<TCategoryName>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotSupportedException();
        }
    }
}
