using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Services
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public string? Error { get; set; }
    }

    public class ServiceResult
    {
        public string? Error { get; set; }
    }
}
