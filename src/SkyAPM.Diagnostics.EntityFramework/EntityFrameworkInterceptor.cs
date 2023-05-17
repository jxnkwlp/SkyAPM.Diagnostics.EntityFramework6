using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class EntityFrameworkInterceptor : IDbCommandInterceptor
    {
        public static readonly DiagnosticListener Listener = new DiagnosticListener(DiagnosticStrings.DiagnosticListenerName);

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                Listener.Write(DiagnosticStrings.SqlErrorExecuteCommand, new EntityFrameworkCommandEventData("NonQuery", command, interceptionContext.Exception));
            else
                Listener.Write(DiagnosticStrings.SqlAfterExecuteCommand, new EntityFrameworkCommandEventData("NonQuery", command));
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Listener.Write(DiagnosticStrings.SqlBeforeExecuteCommand, new EntityFrameworkCommandEventData("NonQuery", command));
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                Listener.Write(DiagnosticStrings.SqlErrorExecuteCommand, new EntityFrameworkCommandEventData("Reader", command, interceptionContext.Exception));
            else
                Listener.Write(DiagnosticStrings.SqlAfterExecuteCommand, new EntityFrameworkCommandEventData("Reader", command));
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Listener.Write(DiagnosticStrings.SqlBeforeExecuteCommand, new EntityFrameworkCommandEventData("Reader", command));
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            if (interceptionContext.Exception != null)
                Listener.Write(DiagnosticStrings.SqlErrorExecuteCommand, new EntityFrameworkCommandEventData("Scalar", command, interceptionContext.Exception));
            else
                Listener.Write(DiagnosticStrings.SqlAfterExecuteCommand, new EntityFrameworkCommandEventData("Scalar", command));
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Listener.Write(DiagnosticStrings.SqlBeforeExecuteCommand, new EntityFrameworkCommandEventData("Scalar", command));
        }
    }
}
