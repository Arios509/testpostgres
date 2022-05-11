namespace Domain
{
    public class DomainError
    {
        public readonly string Code;
        public readonly string Message;

        private DomainError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static DomainError New(string code, string message) => new DomainError(code, message);
    }

    public class BusinessError
    {
        public static class FailedMessage
        {
            public static string Code = "1111";
            public static string Message = "failed test";
            public static DomainError Error() => DomainError.New(Code, Message);
        }
    }
}