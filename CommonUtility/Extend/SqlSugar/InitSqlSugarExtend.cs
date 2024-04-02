 using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility.Extend.SqlSugar
{
    public static  class InitSqlSugarExtend
    {
        /// <summary>
        /// 初始化SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void InitSqlSugar(this WebApplicationBuilder builder)
        {
            string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("请配置数据库链接字符串~");
            }

            ConnectionConfig connection = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            };            
            builder.Services.AddScoped<ISqlSugarClient>(s =>
            {
                SqlSugarClient client = new SqlSugarClient(connection);
                //client.Aop.OnLogExecuting = (s, p) =>
                //{
                //    Console.WriteLine($"OnLogExecuting:输出Sql语句:{s} || 参数为：{string.Join(",", p.Select(p => p.Value))}");
                //};
                client.Aop.OnExecutingChangeSql = (s, p) =>
                {
                    Console.WriteLine($"*******************************OnLogExecuting************************************");
                    Console.WriteLine($"OnLogExecuting:输出Sql语句:\r\n{s} \r\n\r\n参数为：{string.Join(",", p.Select(p => p.Value))}");
                    return new KeyValuePair<string, SugarParameter[]>(s, p);
                };
                //client.Aop.OnLogExecuted = (s, p) =>
                //{
                //    Console.WriteLine($"OnLogExecuted:输出Sql语句:{s} || 参数为：{string.Join(",", p.Select(p => p.Value))}");
                //};
                client.Aop.OnError = e =>
                {
                    Console.WriteLine($"*******************************OnError************************************");
                    Console.WriteLine($"OnError:Sql语句执行异常:\r\n{e.Message}");
                };

                {
                    //可以配置对于数据库操作的过滤器--SqlSugar特有的
                }
                return client;
            });
            if (builder.Configuration["IsInitDatabase"] == "1")
            {
                using (SqlSugarClient client = new SqlSugarClient(connection))
                {
                    client.DbMaintenance.CreateDatabase();
                    Assembly assembly = Assembly.LoadFile(Path.Combine(AppContext.BaseDirectory, "DataBaseModels.dll"));
                    Type[] typeArray = assembly.GetTypes().Where(t =>
                    !t.Name.Contains("BaseModel") &&
                     !t.Name.Contains("DataBase"))
                        .ToArray();
                    client.CodeFirst.InitTables(typeArray);

                   
                }
            }
        } 
    }
}
