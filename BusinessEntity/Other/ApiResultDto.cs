using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other
{
    public class ApiResultDto
    {
        public bool IsSuccess { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
    }
}
