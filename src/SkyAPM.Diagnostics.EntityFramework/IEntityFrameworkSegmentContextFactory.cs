using System.Data.Common;
using SkyApm.Tracing.Segments;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public interface IEntityFrameworkSegmentContextFactory
    {
        SegmentContext GetCurrentContext(DbCommand dbCommand);

        SegmentContext Create(string operationName, DbCommand dbCommand);

        void Release(SegmentContext segmentContext);
    }
}
