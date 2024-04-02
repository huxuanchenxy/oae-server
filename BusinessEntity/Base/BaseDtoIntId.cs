using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Base
{ 
    public abstract class BaseDtoIntId : BaseDto
    {
        public int Id { get; set; }

    }
}
