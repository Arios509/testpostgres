using Dapper;
using System.Data;
using System.Data.Common;

namespace Api.Infrastructure
{    
    public interface IDapper : IDisposable    
    {    
        DbConnection GetDbconnection();
        Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);    
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);    
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);    
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);    
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);    
    }    
}     