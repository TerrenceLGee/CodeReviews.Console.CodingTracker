using System.Data;
using Dapper;

namespace CodingTracker.TerrenceLGee.Data.Handlers;

public abstract class SqliteTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameters, T? value)
        => parameters.Value = value;
}