using System.Data;

namespace P2Project.Core.Interfaces.DataBase;

public interface ISqlConnectionFactory
{
    public IDbConnection CreateConnection();
}