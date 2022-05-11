using Dapper;
using System.Data;

namespace Infrastructure.SeedWork
{
    public interface IBaseRepository
    {
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure) where T : class;
        Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure, bool isStoreProcedure = false) where T : class;
        Task<T> QueryAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure) where T : class;
        Task<long> InsertAsync<T>(string sp, DynamicParameters parms) where T : class;
        Task<long> UpdateAsync<T>(string sp, DynamicParameters parms) where T : class;
        Task<long> Insert<T>(T obj) where T : class;
        Task<int> GetCount<T>() where T : class;
    }
}
