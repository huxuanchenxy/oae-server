using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other
{
    /// <summary>
    /// 分页入参
    /// </summary>
    public class PaginationReqDto
    {
        public int  PageIndex { get; set; } 
        public int PageSize { get; set; }     
    }
}
