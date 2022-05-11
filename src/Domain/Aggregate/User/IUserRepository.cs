namespace Domain.Aggregate.User
{
    public interface IUserRepository
    {
        Task<TestingUser> Get();
        Task<long> Add(TestingUser user);
    }
}
