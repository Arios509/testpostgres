namespace Domain.Aggregate.User
{
    public interface IUserRepository
    {
        Task<User> Get();
        Task<long> Add(User user);
    }
}
