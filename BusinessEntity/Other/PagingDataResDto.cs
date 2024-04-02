using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other
{
    public class PagingDataResDto  
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int RecordCount { get; set; } 
        /// <summary>
        /// 当前页的数据集合
        /// </summary>
        public object? DataList { get; set; } 
    }
}
