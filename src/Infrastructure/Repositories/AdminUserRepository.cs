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

        public async Task<TestingUser> Get()
        {
            var query = $"""
                select * from {nameof(TestingUser)} limit 1
                """;

            var result = await _baseRepository.Get<TestingUser>(query.ToString(), null, commandType: System.Data.CommandType.Text);

            return result;
        }

        public async Task<long> Add(TestingUser user)
        {
            var query = $"""
                INSERT INTO {nameof(TestingUser)} 
                ({nameof(TestingUser.Detail)}, {nameof(TestingUser.Details)}) VALUES 
                ({JsonSerializer.Serialize(user.Detail)}, '{JsonSerializer.Serialize(user.Details)}')
                """;

            var parameters = new DynamicParameters();
            parameters.Add(nameof(TestingUser.Detail), JsonSerializer.Serialize(user.Detail));

            var result = await _baseRepository.InsertAsync<TestingUser>(query.ToString(), parameters);
            return result;
        }
    }
}