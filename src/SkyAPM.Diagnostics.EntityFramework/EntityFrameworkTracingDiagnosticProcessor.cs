using System;
using System.Data.Common;
using System.Linq;
using SkyApm;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Tracing;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class EntityFrameworkTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        private readonly IEntityFrameworkSegmentContextFactory _contextFactory;
        private readonly TracingConfig _tracingConfig;

        public string ListenerName => DiagnosticStrings.DiagnosticListenerName;

        public EntityFrameworkTracingDiagnosticProcessor(IEntityFrameworkSegmentContextFactory contextFactory, IConfigAccessor configAccessor)
        {
            _contextFactory = contextFactory;
            _tracingConfig = configAccessor.Get<TracingConfig>();
        }

        private static string ResolveOperationName(EntityFrameworkCommandEventData data)
        {
            var commandType = data.Command.CommandText?.Split(' ');
            return $"{DiagnosticStrings.ClientPrefix}{commandType?.FirstOrDefault() ?? data.Method}";
        }

        [DiagnosticName(DiagnosticStrings.SqlBeforeExecuteCommand)]
        public void CommandExecuting([Object] EntityFrameworkCommandEventData data)
        {
            var operationName = ResolveOperationName(data);
            var context = _contextFactory.Create(operationName, data.Command);
            context.Span.SpanLayer = SkyApm.Tracing.Segments.SpanLayer.DB;
            context.Span.Component = SkyApm.Common.Components.SQLCLIENT;
            context.Span.AddTag(SkyApm.Common.Tags.DB_TYPE, "Sql");
            context.Span.AddTag(SkyApm.Common.Tags.DB_INSTANCE, data.Command.Connection.Database);
            context.Span.AddTag(SkyApm.Common.Tags.DB_STATEMENT, data.Command.CommandText);
            context.Span.AddTag(SkyApm.Common.Tags.DB_BIND_VARIABLES, BuildParameterVariables(data.Command.Parameters));
        }

        [DiagnosticName(DiagnosticStrings.SqlAfterExecuteCommand)]
        public void CommandExecuted([Object] EntityFrameworkCommandEventData data)
        {
            var context = _contextFactory.GetCurrentContext(data.Command);
            if (context != null)
            {
                _contextFactory.Release(context);
            }
        }

        [DiagnosticName(DiagnosticStrings.SqlErrorExecuteCommand)]
        public void CommandError([Object] EntityFrameworkCommandEventData data)
        {
            var context = _contextFactory.GetCurrentContext(data.Command);
            if (context != null)
            {
                context.Span.ErrorOccurred(data.Exception, _tracingConfig);
                _contextFactory.Release(context);
            }
        }

        private string BuildParameterVariables(DbParameterCollection dbParameters)
        {
            if (dbParameters == null || dbParameters.Count == 0)
            {
                return string.Empty;
            }

            return string.Join(", ", dbParameters.Cast<DbParameter>().Select(x => $"@{x.ParameterName}='{x.Value}' (DbType = {x.DbType})"));
        }
    }
}
