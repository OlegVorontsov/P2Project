using System.Data;

namespace P2Project.Application.Interfaces.DataBase;

public interface ISqlConnectionFactory
{
    public IDbConnection CreateConnection();
}