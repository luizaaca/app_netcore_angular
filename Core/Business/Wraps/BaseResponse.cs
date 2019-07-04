using System;

namespace Core.Business.Wraps
{
    public class BaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Result { get; set; }
    }
}
