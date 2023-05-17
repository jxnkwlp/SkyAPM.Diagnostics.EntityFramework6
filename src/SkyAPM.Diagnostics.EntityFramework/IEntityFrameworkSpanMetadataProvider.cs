using System.Data.Common;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public interface IEntityFrameworkSpanMetadataProvider
    {
        string Component { get; }

        bool Match(DbConnection connection);

        string GetPeer(DbConnection connection);
    }
}
