using Dapper;
using System.Data;
using System.Text;

namespace Infrastructure.SeedWork
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DapperExtensionsProxy _dapperExtensionsProxy;
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _dapperExtensionsProxy = new DapperExtensionsProxy(unitOfWork);
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms = null,
            CommandType commandType = CommandType.Text) where T : class
        {
            var result = await _dapperExtensionsProxy.GetList<T>(sp, parms, commandType: commandType);
            return result.ToList();
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text, bool isStoreProcedure = false) where T : class
        {
            if (isStoreProcedure)
            {
                var storeProcedureResult = await _dapperExtensionsProxy.Get<T>(sp: sp, parms: parms, commandType: commandType);
                return storeProcedureResult;
            }
            var result = await _dapperExtensionsProxy.Get<T>(sp, parms, commandType: commandType);
            return result;
        }

        public async Task<long> Insert<T>(T obj) where T : class
        {
            var result = await _dapperExtensionsProxy.Insert(obj);
            return result;
        }

        public async Task<T> QueryAsync<T>(string sp, DynamicParameters parms,
            CommandType commandType = CommandType.Text) where T : class
        {
            var result = await _dapperExtensionsProxy.QueryAsync<T>(sp, parms, commandType: commandType);
            return result;
        }

        public async Task<long> InsertAsync<T>(string sp, DynamicParameters parms) where T : class
        {
            var result = await _dapperExtensionsProxy.InsertAsync<T>(sp, parms);
            return result;
        }

        public async Task<long> UpdateAsync<T>(string sp, DynamicParameters parms) where T : class
        {
            var result = await _dapperExtensionsProxy.UpdateAsync<T>(sp, parms);
            return result;
        }

        public async Task<int> GetCount<T>() where T : class
        {
            var name = typeof(T).Name;

            var query = new StringBuilder();

            query.Append($"select count(*) from {name}");

            var result = await _dapperExtensionsProxy.GetCount(query.ToString());
            return result;
        }


        //protected void Delete(T poco)
        //{
        //    var result = _dapperExtensionsProxy.Delete(poco);
        //}

        //protected void DeleteById(Guid id)
        //{
        //    T poco = (T)Activator.CreateInstance(typeof(T));
        //    poco.SetDbId(id);
        //    var result = dapperExtensionsProxy.Delete(poco);
        //}

        //protected void DeleteById(string id)
        //{
        //    T poco = (T)Activator.CreateInstance(typeof(T));
        //    poco.SetDbId(id);
        //    var result = dapperExtensionsProxy.Delete(poco);
        //}

        //protected void DeleteAll()
        //{
        //    var predicateGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
        //    var result = dapperExtensionsProxy.Delete<T>(predicateGroup);//Send empty predicateGroup to delete all records.
        //}
    }
}
