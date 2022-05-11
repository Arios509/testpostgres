using CSharpFunctionalExtensions;
using Domain.Aggregate.User;
using Infrastructure;
using MediatR;

namespace Api.Features.UserFeature
{
    public class GetUserQuery : IRequest<Result<TestingUser, CommandErrorResponse>>
    {
        public int PageSize { get; set; } = 10;
    }


    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<TestingUser, CommandErrorResponse>>
    {
        private readonly IUserRepository _adminUserRepository;
        public GetUserQueryHandler(IUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        public async Task<Result<TestingUser, CommandErrorResponse>>
            Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _adminUserRepository.Get();

                return ResultCustom.Success(result);
            }
            catch (Exception ex)
            {
                return ResultCustom.Error<TestingUser>(ex);
            }
        }
    }
}