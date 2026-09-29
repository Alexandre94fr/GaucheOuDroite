using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;


namespace GaucheOuDroiteBackEndTests.Tools
{
    public sealed class TestDataBase : IDisposable
    {
        readonly SqliteConnection _connection;

        public DataBaseContext Context { get; }


        public TestDataBase()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            DbContextOptions<DataBaseContext> options = new DbContextOptionsBuilder<DataBaseContext>().UseSqlite(_connection).Options;

            Context = new DataBaseContext(options);

            Context.Database.EnsureCreated();
        }


        public void Dispose()
        {
            Context.Dispose();
            _connection.Dispose();
        }
    }
}