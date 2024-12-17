using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using P2Project.Application.Interfaces.DataBase;

namespace P2Project.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public IDbConnection CreateConnection() =>
        new NpgsqlConnection(_configuration.GetConnectionString("Database"));
}