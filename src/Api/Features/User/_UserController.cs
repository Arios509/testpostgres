using Microsoft.AspNetCore.Mvc;
using MediatR;
using Infrastructure;
using Infrastructure.Identity.Helpers;
using Infrastructure.Identity;
using Api.Common.Security;

namespace Api.Features.UserFeature
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserQuery([FromQuery] GetUserQuery command) =>
            this.OkOrError(await _mediator.Send(command));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddUserCommand([FromQuery] AddUserCommand command) =>
            this.OkOrError(await _mediator.Send(command));

    }
}