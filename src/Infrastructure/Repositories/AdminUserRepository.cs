using Dapper;
using Domain.Aggregate.User;
using Infrastructure.SeedWork;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IBaseRepository _baseRepository;
        public UserRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<User> Get()
        {
            var query = new StringBuilder();

            query.Append($"select * from {nameof(User)} limit 1");

            var result = await _baseRepository.Get<User>(query.ToString(), null, commandType: System.Data.CommandType.Text);

            return result;
        }

        public async Task<long> Add(User user)
        {
            var query = $"""
                INSERT INTO {nameof(User)} 
                ({nameof(User.Detail)}) VALUES 
                (@{nameof(User.Detail)})
                """;

            var parameters = new DynamicParameters();
            parameters.Add(nameof(User.Detail), user.Detail);

            var result = await _baseRepository.InsertAsync<User>(query.ToString(), parameters);
            return result;
        }
    }
}