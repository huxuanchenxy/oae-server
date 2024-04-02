using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Base
{
    public abstract class BaseModel
    {  

        [SugarColumn(IsNullable = true)]
        public int Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? OrderNum { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Creater { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? Updater { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateDate { get; set; }
    }
}
