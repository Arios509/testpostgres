using CSharpFunctionalExtensions;
using Domain.Aggregate.User;
using Infrastructure;
using Infrastructure.Identity.Helpers;
using MediatR;

namespace Api.Features.UserFeature
{

    public class AddUserCommand : IRequest<Result<bool, CommandErrorResponse>>
    {
    }

    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<bool, CommandErrorResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        public AddUserCommandHandler(IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<Result<bool, CommandErrorResponse>>
            Handle(AddUserCommand query, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = new TestingUser(new Detail());

                await _userRepository.Add(newUser);

                return ResultCustom.Success(true);
            }
            catch (Exception ex)
            {
                return ResultCustom.Error<bool>(ex);
            }
        }
    }
}