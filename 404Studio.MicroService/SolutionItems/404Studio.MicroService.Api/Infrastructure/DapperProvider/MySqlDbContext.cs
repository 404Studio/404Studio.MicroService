using System;
using MySql.Data.MySqlClient;
using NPoco;

namespace YH.Etms.Settlement.Api.Infrastructure.DapperProvider
{
    public class MySqlDbContext : IDatabaseContext
    {
        private readonly string _connectionString;
        public MySqlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Database Database
        {
            get {
                if (_database != null) return _database;
                var conn = new MySqlConnection(_connectionString);
                conn.Open();
                _database = new Database(conn);
                return _database;
            }
        }

        private Database _database;

        public void Dispose()
        {
            if (_database != null)
            {
                if (_database.Connection != null)
                {
                    _database.Connection.Close();
                    _database.Connection.Dispose();
                }
                _database.Dispose();
            }
            GC.SuppressFinalize(this);
        }
    }
}
