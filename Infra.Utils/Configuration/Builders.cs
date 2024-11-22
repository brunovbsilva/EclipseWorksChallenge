using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infra.Utils.Configuration
{
    public class Builders
    {
        public static string BuildSqlConnectionString(IConfiguration configuration)
        {
            var connBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
            {
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ConnectTimeout = 30
            };
            return connBuilder.ConnectionString;
        }
    }
}
