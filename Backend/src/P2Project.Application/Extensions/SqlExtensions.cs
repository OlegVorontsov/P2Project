using System.Data;
using System.Text;
using Dapper;

namespace P2Project.Application.Extensions;

public static class SqlExtensions
{
    public static void ApplySorting(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        string? sortOrder,
        string? sortBy)
    {
        parameters.Add("@SortBy", sortBy, DbType.String);
        parameters.Add("@SortOrder", sortOrder, DbType.String);
        
        sqlBuilder.Append(" ORDER BY @SortBy @SortOrder");
    }
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        parameters.Add("@PageSize", pageSize, DbType.Int32);
        parameters.Add("@Offset", (page - 1) * pageSize, DbType.Int32);
        
        sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
    }
}