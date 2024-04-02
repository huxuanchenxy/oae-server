using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Base
{
    public abstract class BaseDtoStringId : BaseDto
    {
        public string? Id { get; set; }
    }
}
