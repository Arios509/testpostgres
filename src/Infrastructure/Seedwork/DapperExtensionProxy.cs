using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace Infrastructure.SeedWork
{
    public class DapperExtensionsProxy
    {
        private readonly IUnitOfWork _unitOfWork;
        public DapperExtensionsProxy(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // internal int Count<T>(object predicate) where T : class
        // {
        //    var result = _unitOfWork.Connection.Count<T>(predicate, _unitOfWork.Transaction);
        //    return result;
        // }

        //public T Get<T>(object id) where T : class
        //{
        //    var result = _unitOfWork.Connection.Get<T>(id, _unitOfWork.Transaction);
        //    return result;
        //}

        public async Task<int> GetCount(string sp)
        {
            var result = await _unitOfWork.Connection.QuerySingleAsync<int>(sp);
            return result;
        }
        public async Task<IEnumerable<T>> GetList<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure) where T : class
        {
            var result = await _unitOfWork.Connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: _unitOfWork.Transaction);
            return result;
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure) where T : class
        {
            var result = await _unitOfWork.Connection.QueryAsync<T>(sp, parms, commandType: commandType, transaction: _unitOfWork.Transaction);
            return result.FirstOrDefault();
        }

        public async Task<long> Insert<T>(T poco) where T : class
        {
            var result = await _unitOfWork.Connection.InsertAsync<T>(poco, _unitOfWork.Transaction);
            return result;
        }

        public async Task<long> InsertAsync<T>(string sp, DynamicParameters parms) where T : class
        {
            var result = await _unitOfWork.Connection.ExecuteAsync(
                sp,
                parms,
                transaction: _unitOfWork.Transaction);

            return result;
        }

        public async Task<long> UpdateAsync<T>(string sp, DynamicParameters parms) where T : class
        {
            var result = await _unitOfWork.Connection.ExecuteAsync(
                sp,
                parms,
                transaction: _unitOfWork.Transaction);

            return result;
        }

        public async Task<T> QueryAsync<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.StoredProcedure) where T : class
        {
            var result = await _unitOfWork.Connection.QueryAsync<T>(sp, parms,
                commandType: commandType,
                transaction: _unitOfWork.Transaction);
            return result.FirstOrDefault();
        }

        public bool Update<T>(T poco) where T : class
        {
            var result = _unitOfWork.Connection.Update<T>(poco, _unitOfWork.Transaction);
            return result;
        }

        public bool Delete<T>(T poco) where T : class
        {
            var result = _unitOfWork.Connection.Delete<T>(poco, _unitOfWork.Transaction);
            return result;
        }
    }
}
