using System.Data.Common;

namespace SkyAPM.Diagnostics.EntityFramework
{
    public class SqlServerEntityFrameworkSpanMetadataProvider : IEntityFrameworkSpanMetadataProvider
    {
        public string Component { get; } = SkyApm.Common.Components.SQLCLIENT.GetStringValue(); // Components.ENTITYFRAMEWORK_SQLSERVER.GetStringValue();

        public bool Match(DbConnection connection)
        {
            // Microsoft.Data.Sql.SqlConnection
            return connection.GetType().FullName == "System.Data.SqlClient.SqlConnection";
        }

        public string GetPeer(DbConnection connection)
        {
            return connection.DataSource;
        }
    }
}
