using CSharpFunctionalExtensions;
using Domain;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Infrastructure
{
    public class CommandErrorResponse
    {
        public readonly string ErrorCode;
        public readonly string Message;
        public readonly HttpStatusCode HttpStatusCode;
        public readonly JObject Context;

        public CommandErrorResponse(string errorCode, string message, HttpStatusCode httpStatusCode,
            JObject context = null)
        {
            ErrorCode = errorCode;
            Message = message;
            HttpStatusCode = httpStatusCode;
            Context = context;
        }

        public CommandErrorResponse(string errorCode, string message, HttpStatusCode httpStatusCode,
            dynamic context) : this(errorCode, message, httpStatusCode,
                (JObject)(context != null ? JObject.FromObject(context) : null))
        {

        }

        public static CommandErrorResponse UnknowError(string message, string errorCodes = "UnknowError")
        {
            return new CommandErrorResponse(errorCode: errorCodes, message: message,
                httpStatusCode: HttpStatusCode.InternalServerError);
        }

        public static CommandErrorResponse BusinessError(DomainError domainError)
            => new CommandErrorResponse(
                errorCode: domainError.Code,
                message: domainError.Message,
                httpStatusCode: HttpStatusCode.BadRequest);

        public static CommandErrorResponse BusinessError(string message, string errorCodes = "error")
            => new CommandErrorResponse(
                errorCode: errorCodes,
                message: message,
                httpStatusCode: HttpStatusCode.BadRequest);

        public static CommandErrorResponse NotFound(string message, string errorCodes = "NotFound")
            => new CommandErrorResponse(
                errorCode: errorCodes,
                message: message,
                httpStatusCode: HttpStatusCode.NotFound);

        public static CommandErrorResponse NotAuthorized(string message, string errorCodes = "NotAuthorized")
           => new CommandErrorResponse(
               errorCode: errorCodes,
               message: message,
               httpStatusCode: HttpStatusCode.Unauthorized);

        public static CommandErrorResponse Parse(string responseContent, HttpStatusCode statusCode)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                return new CommandErrorResponse(errorCode: statusCode.ToString(),
                    message: "",
                    httpStatusCode: statusCode,
                    context: null);
            }

            var jo = JObject.Parse(responseContent);

            return new CommandErrorResponse(
                errorCode: jo["errorCode"]?.ToString(),
                message: jo["message"]?.ToString(),
                httpStatusCode: statusCode,
                context: jo["context"] as JObject
                );
        }

        public static implicit operator CommandErrorResponse(string errorMessage) => CommandErrorResponse.BusinessError(errorMessage);

    }

    public static class ResultCustom
    {
        public static Result<T, CommandErrorResponse> Success<T>(T value) => Result.Success<T, CommandErrorResponse>(value);

        public static Result<T, CommandErrorResponse> NotAuthorized<T>(string message, string errorCodes = "NotAuthorized")
            => Result.Failure<T, CommandErrorResponse>(CommandErrorResponse.NotAuthorized(message, errorCodes));

        public static Result<T, CommandErrorResponse> NotFound<T>(string message, string errorCodes = "NotFound") =>
            Result.Failure<T, CommandErrorResponse>(CommandErrorResponse.NotFound(message, errorCodes));

        public static Result<T, CommandErrorResponse> Error<T>(CommandErrorResponse error) =>
            Result.Failure<T, CommandErrorResponse>(error);

        public static Result<T, CommandErrorResponse> Error<T>(DomainError domainError) =>
           Result.Failure<T, CommandErrorResponse>(CommandErrorResponse.BusinessError(domainError.Message, domainError.Code));

        public static Result<T, CommandErrorResponse> Error<T>(Exception ex)
        {
            var innerException = ex.InnerException;
            if(innerException == null)
            {
                return Result.Failure<T, CommandErrorResponse>(CommandErrorResponse.UnknowError(ex.Message, ex.GetType().ToString()));
            }
            else
            {
                var cer = new CommandErrorResponse(
                    errorCode: ex.GetType().ToString(),
                    message: ex.Message,
                    httpStatusCode: HttpStatusCode.InternalServerError,
                    context: new JObject
                    {
                        {"innerException", innerException.GetType().ToString()},
                        {"innerExceptionMessage", innerException.Message}
                    });

                return Result.Failure<T, CommandErrorResponse>(cer);
            }
        }

        public static Result<T, CommandErrorResponse> Error<T>(string businessMessage, Exception ex, string errorCode = "custom-error")
        {
            var context = new JObject
            {
                {"exceptionMessage", ex.Message },
                {"exception", ex.GetType().ToString() }
            };

            var innerException = ex.InnerException;
            if(innerException != null)
            {
                context.Add("innerException", innerException.GetType().ToString());
                context.Add("innerExceptionMessage", innerException.Message);
            }

            var cer = new CommandErrorResponse(
                errorCode: errorCode,
                message: $"{businessMessage} see context for more detail",
                httpStatusCode: HttpStatusCode.InternalServerError,
                context: context);

            return Result.Failure<T, CommandErrorResponse>(cer);
        }

        public static Result<T, CommandErrorResponse> Error<T>(string message, string errorCodes) =>
            Result.Failure<T, CommandErrorResponse>(CommandErrorResponse.BusinessError(message, errorCodes));
    }
}