using System;

namespace frontend.Services
{
    public class ServiceResult
    {
        public string? Error { get; set; }

        public ServiceResult(string? error = null)
        {
            Error = error;
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }

        public ServiceResult(T? data = default, string? error = null)
            : base(error)
        {
            Data = data;
        }
    }
}
