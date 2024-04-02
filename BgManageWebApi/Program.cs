using BusinessInterface; 
using BusinessService; 
using CommonUtility;
using CommonUtility.Extend;
using CommonUtility.Extend.FileMiddleware;
using CommonUtility.Extend.SqlSugar;
using CommonUtility.Extend.Swagger;
using CommonUtility.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.CrosDomainsPolicy(); 
builder.Services.AddControllers(); 
builder.AddSwaggerExtend();
builder.InitSqlSugar();  
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs)); 
builder.Services.AddTransient<ITestService, TestService>();
var app = builder.Build();
 // Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{ 
    app.UseSwaggerExtend();
} 
app.UseCrosDomainsPolicy(); 
app.UseAuthorization(); 
app.MapControllers(); 
app.Run(); 