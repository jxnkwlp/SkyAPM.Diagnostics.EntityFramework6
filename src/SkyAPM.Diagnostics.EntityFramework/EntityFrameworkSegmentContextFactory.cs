using System.Collections.Generic;
using System.Data.Common;
using SkyApm.Common;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class EntityFrameworkSegmentContextFactory : IEntityFrameworkSegmentContextFactory
    {
        private readonly IEnumerable<IEntityFrameworkSpanMetadataProvider> _spanMetadataProviders;
        private readonly ITracingContext _tracingContext;
        private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;
        private readonly IExitSegmentContextAccessor _exitSegmentContextAccessor;

        public EntityFrameworkSegmentContextFactory(
            IEnumerable<IEntityFrameworkSpanMetadataProvider> spanMetadataProviders,
            ITracingContext tracingContext, ILocalSegmentContextAccessor localSegmentContextAccessor,
            IExitSegmentContextAccessor exitSegmentContextAccessor)
        {
            _spanMetadataProviders = spanMetadataProviders;
            _tracingContext = tracingContext;
            _localSegmentContextAccessor = localSegmentContextAccessor;
            _exitSegmentContextAccessor = exitSegmentContextAccessor;
        }

        public SegmentContext GetCurrentContext(DbCommand dbCommand)
        {
            foreach (var provider in _spanMetadataProviders)
                if (provider.Match(dbCommand.Connection))
                    return _exitSegmentContextAccessor.Context;

            return _localSegmentContextAccessor.Context;
        }

        public SegmentContext Create(string operationName, DbCommand dbCommand)
        {
            foreach (var provider in _spanMetadataProviders)
                if (provider.Match(dbCommand.Connection))
                    return CreateExitSegment(operationName, dbCommand, provider);

            return CreateLocalSegment(operationName, dbCommand);
        }

        public void Release(SegmentContext segmentContext)
        {
            _tracingContext.Release(segmentContext);
        }

        private SegmentContext CreateExitSegment(string operationName, DbCommand dbCommand, IEntityFrameworkSpanMetadataProvider metadataProvider)
        {
            var context = _tracingContext.CreateExitSegmentContext(operationName, metadataProvider.GetPeer(dbCommand.Connection));
            context.Span.Component = new StringOrIntValue(metadataProvider.Component);
            return context;
        }

        private SegmentContext CreateLocalSegment(string operationName, DbCommand dbCommand)
        {
            var context = _tracingContext.CreateLocalSegmentContext(operationName);
            context.Span.Component = SkyApm.Common.Components.SQLCLIENT;
            return context;
        }
    }
}
