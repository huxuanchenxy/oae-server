using BusinessInterface; 
using BusinessService; 
using CommonUtility;
using CommonUtility.Extend;
using CommonUtility.Extend.FileMiddleware;
using CommonUtility.Extend.SqlSugar;
using CommonUtility.Extend.Swagger;
using CommonUtility.Filters;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.CrosDomainsPolicy(); 
builder.Services.AddControllers(); 
builder.AddSwaggerExtend();
builder.InitSqlSugar();  
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs)); 
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ICusModuleService, CusModuleService>();
builder.Services.AddTransient<ISysFuncService, SysFuncService>();
builder.Services.AddTransient<IFunctionsService, FunctionsService>();

var app = builder.Build();
 // Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{ 
    app.UseSwaggerExtend();
}
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = (c) =>
    {
        c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        c.Context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, DELETE, PUT, OPTIONS, TRACE, HEAD, PATCH");
        c.Context.Response.Headers.Add("Access-Control-Allow-Headers", "*");
    },
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "DevImgs")),
    RequestPath = "/devimgs"
});
app.UseCrosDomainsPolicy(); 
app.UseAuthorization(); 
app.MapControllers(); 
app.Run(); 