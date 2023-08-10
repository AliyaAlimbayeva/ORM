using DapperDemo;
using System.Data.SqlClient;

namespace DapperDemoTests
{
    public class TestBase : IDisposable
    {
        private readonly string _connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Database = DapperDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False";

        public readonly SqlConnection Connection;
        public readonly DapperMod DapperMod;

        public TestBase()
        {
            Connection = new SqlConnection(_connectionString);
            Connection.Open();

            DapperMod = new DapperMod(Connection);
        }

        public void Dispose()
        {
            DapperMod.ClearAllData();
            Connection.Dispose();
        }
    }
}
