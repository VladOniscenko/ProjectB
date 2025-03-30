using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using System.Reflection;

namespace ProjectB.DataAccess
{
    public class DbAccess<T>
    {
        public readonly string ConnectionString;
        private readonly string _tableName;

        public DbAccess(string tableName)
        {
            _tableName = tableName;
            ConnectionString = $"{DbInitializer.GetDbPath()}";
            DbInitializer.Initialize();
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