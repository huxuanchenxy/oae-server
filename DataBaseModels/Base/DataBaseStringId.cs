using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModels.Base
{
 
    public abstract class DataBaseStringId : BaseModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string? Id { get; set; }
    }
}
