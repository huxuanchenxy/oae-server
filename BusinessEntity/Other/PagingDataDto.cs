using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Other
{
   
    public class PagingDataDto<T> where T : class
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int RecordCount { get; set; }

       
        /// <summary>
        /// 当前页的数据集合
        /// </summary>
        public List<T>? DataList { get; set; } 

       
    }
}
