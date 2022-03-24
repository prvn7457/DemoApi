using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.Contracts.Response
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public bool Ok { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
