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
        public static void CrosDomainsPolicy(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllCrosDomainsPolicy", corsbuilder =>
                {
                    corsbuilder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        } 
           
        public static void UseCrosDomainsPolicy(this WebApplication app) => app.UseCors("AllCrosDomainsPolicy");

    }
}
