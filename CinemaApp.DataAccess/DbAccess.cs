using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Dapper;
using CinemaApp.DataAccess;

public class DbAccess<T>
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public DbAccess(string tableName)
    {
        _connectionString = DbInitializer.GetDbPath();
        _tableName = tableName;
        DbInitializer.Initialize();
    }

    public void Write(T entity)
    {
        string sql = $"INSERT INTO {_tableName} ({GetColumnNames()}) VALUES ({GetParameterNames()})";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(sql, entity);
        }
    }

    public void Update(T entity)
    {
        string sql = $"UPDATE {_tableName} SET {GetUpdateColumns()} WHERE id = @ID";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(sql, entity);
        }
    }

    public void Delete(int id)
    {
        string sql = $"DELETE FROM {_tableName} WHERE id = @ID";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Execute(sql, new { ID = id });
        }
    }

    public T GetById(int id)
    {
        string sql = $"SELECT * FROM {_tableName} WHERE id = @ID";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            return connection.QueryFirstOrDefault<T>(sql, new { ID = id });
        }
    }

    public IEnumerable<T> GetAll()
    {
        string sql = $"SELECT * FROM {_tableName}";

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            return connection.Query<T>(sql);
        }
    }

    private string GetColumnNames()
    {
        return string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
    }

    private string GetParameterNames()
    {
        return string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name));
    }

    private string GetUpdateColumns()
    {
        return string.Join(", ", typeof(T).GetProperties()
            .Where(p => !string.Equals(p.Name, "id", StringComparison.OrdinalIgnoreCase))
            .Select(p => $"{p.Name} = @{p.Name}"));
    }
}
