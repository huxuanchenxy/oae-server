using AutoMapper;
using BusinessEntity;
using BusinessInterface;
using DataBaseModels.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService
{
    public class TestService : BaseService,ITestService
    {
        private readonly IMapper _mapper;
        public TestService(ISqlSugarClient client, IMapper mapper) : base(client)
        {
            _mapper = mapper;
        }

        public void Save(TestEntity req)
        {
            var dbObj = _mapper.Map<Test>(req);
            dbObj.CreateDate = DateTime.Now;
            this.Insert(dbObj);
        }
    }
}
