using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.SeedWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IOptions<ConnectionStringOptions> opt)
        {
            _id = Guid.NewGuid();
            _connection = new NpgsqlConnection(opt.Value.DefaultConnection);
        }

        IDbConnection _connection = null;
        IDbTransaction _transaction = null;
        Guid _id = Guid.Empty;

        IDbConnection IUnitOfWork.Connection
        {
            get { return _connection; }
        }
        IDbTransaction IUnitOfWork.Transaction
        {
            get { return _transaction; }
        }
        Guid IUnitOfWork.Id
        {
            get { return _id; }
        }

        public void Begin()
        {
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
        }
    }
}
