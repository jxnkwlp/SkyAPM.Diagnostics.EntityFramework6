using System;
using System.Data.Common;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class EntityFrameworkCommandEventData
    {
        public EntityFrameworkCommandEventData(string method, DbCommand command, Exception exception = null)
        {
            Method = method;
            Command = command;
            Exception = exception;
        }

        public string Method { get; }
        public DbCommand Command { get; }
        public Exception Exception { get; }
    }
}
