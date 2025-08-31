using Microsoft.Data.SqlClient;
using System.Data;
namespace ArithmeticChat.Helpers;
public sealed class DbConnectionFactory : IDisposable
{
    private readonly string _connectionString;
    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("DefaultConnection missing");
    }
    public IDbConnection CreateConnection()
    {
        var conn = new SqlConnection(_connectionString);
        conn.Open();
        return conn; 
    }
    public void Dispose() { }
}
