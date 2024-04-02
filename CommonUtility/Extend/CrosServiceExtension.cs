using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility.Extend
{
    public static  class CrosServiceExtension
    {
        /// <summary>
        /// 配置支持跨域的策略
        /// </summary>
        /// <param name="builder"></param>
        public static void CrosDomainsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                //所有的Api都支持跨域
                option.AddPolicy("AllCrosDomainsPolicy", corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// 配置生效
        /// </summary>
        /// <param name="app"></param>
        public static void UseCrosDomainsPolicy(this WebApplication app) => app.UseCors("AllCrosDomainsPolicy");

    }
}
