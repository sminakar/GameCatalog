using System;

namespace Commons.Results
{
    public sealed class ErrorResult<T> : Result<T>
    {
        public ErrorResult(ErrorCode code, string message, string errorSource)
        {
            Code = code;
            Message = message;
            ErrorSource = errorSource;
        }
        public override bool IsSuccess { get { return false; } }
        public ErrorCode Code { get; set; }
        public string Message { get; set; }
        public string ErrorSource { get; set; }
        public Exception Exception { get; set; }
    }
}
