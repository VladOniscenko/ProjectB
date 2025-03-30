using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProjectB.DataAccess
{
    public class DbAccess<T>
    {
        public readonly string ConnectionString;
        private readonly string _tableName;
        private readonly string _dbPath;

        public DbAccess(string tableName)
        {
            _tableName = tableName;
            _dbPath = GetDatabasePath();
            ConnectionString = $"Data Source={_dbPath}";
            EnsureDatabaseExists();
        }

        private string GetDatabasePath()
        {
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(projectRoot, "../../../../../DataSources");
            string dbPath = Path.Combine(directoryPath, "project.db");

            return dbPath;
        }

        private void EnsureDatabaseExists()
        {
            string directoryPath = Path.GetDirectoryName(_dbPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Created missing directory: {directoryPath}");
            }

            if (!File.Exists(_dbPath))
            {
                File.Create(_dbPath).Close();
                Console.WriteLine($"Database file not found, created a new one at: {_dbPath}");
            }
        }

        public void Write(T entity)
        {
            string sql = $"INSERT INTO {_tableName} ({GetColumnNames()}) VALUES ({GetParameterNames()})";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sql, entity);
            }
        }

        public void Update(T entity)
        {
            string sql = $"UPDATE {_tableName} SET {GetUpdateColumns()} WHERE Id = @Id";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sql, entity);
            }
        }

        public void Delete(int id)
        {
            string sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(sql, new { Id = id });
            }
        }

        private string GetColumnNames() =>
            string.Join(", ", typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != "Id")
                .Select(p => p.Name));

        private string GetParameterNames() =>
            string.Join(", ", typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != "Id")
                .Select(p => $"@{p.Name}"));

        private string GetUpdateColumns() =>
            string.Join(", ", typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.Name != "Id")
                .Select(p => $"{p.Name} = @{p.Name}"));
    }
}
