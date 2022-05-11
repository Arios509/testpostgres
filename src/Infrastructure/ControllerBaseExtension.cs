using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure
{
    public static class ControllerBaseExtension
    {
        public static IActionResult OkOrError<T>(this ControllerBase controller, Result<T, CommandErrorResponse> response) =>
            response.IsSuccess ? controller.Ok(response.Value) : Error(controller, response.Error);

        public static IActionResult AcceptedOrError<T>(this ControllerBase controller,
            Result<T, CommandErrorResponse> response) =>
            response.IsSuccess
                ? controller.Accepted(response.Value)
                : Error(controller, response.Error);

        private static IActionResult Error(ControllerBase controller, CommandErrorResponse commandErrroResponse) =>
            controller.StatusCode(
                (int)commandErrroResponse.HttpStatusCode,
                new
                {
                    errorCode = commandErrroResponse.ErrorCode,
                    message = commandErrroResponse.Message
                });
    }
}