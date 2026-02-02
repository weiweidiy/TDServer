using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // 保持原样 配置 JSON 序列化选项，保持属性名称原样（不转换为驼峰命名）
    // 或 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 首字母小写
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7289, listenOptions =>
    {
        listenOptions.UseHttps(); // 默认使用开发证书
    });
    options.ListenAnyIP(5289); // http
});

builder.Services.AddSwaggerWithJwt(); //自定义扩展方法，为 Swagger 添加 JWT 支持
builder.Services.AddSignalR();

builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=MyDb;UserId=postgres;Password=123321qweasd;"));


builder.Services.AddSingleton<IDeserializer, JsonNetDeserilizer>();
builder.Services.AddSingleton<IConfigLoader, LocalFileConfigLoader>();
builder.Services.AddSingleton<TiktokConfigManager>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ILevelNodesService, LevelNodeService>();
builder.Services.AddScoped<ISamuraiService, SamuraiService>();
builder.Services.AddScoped<IFormationDeployService, FormationDeployService>();
builder.Services.AddScoped<ILevelNodeCombatService, LevelNodeCombatService>();
builder.Services.AddScoped<IHpPoolService, HpPoolService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IEvaluationService, TiktokCombatEvaluationService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IBagService, BagService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IDrawSamuraiService, DrawSamuraiService>();
builder.Services.AddScoped<TiktokNotifyService, TiktokNotifyService>();
builder.Services.AddScoped<IFormationService, FormationService>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IGuideService, GuideService>();


builder.Services.AddSingleton<JFramework.ILogger, ConsoleLogger>();

builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddHostedService<TimedTaskService>();


// 添加日志（默认已包含Console、Debug等）
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


//builder.Logging.AddFile("Logs/app-{Date}.log"); // 需安装第三方包，如 Serilog 或 Microsoft.Extensions.Logging.File

var app = builder.Build();


var configService = app.Services.GetRequiredService<TiktokConfigManager>();

//"D:\\Demos\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\"
//"E:\\UnityProjects\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\"

await configService.PreloadAllAsync("E:\\UnityProjects\\TiktokGame2Server\\TiktokGame2Server\\TiktokGame2Server\\Configs\\", ".json"); // 如果 Main 是 async Task

app.UseMiddleware<TokenAuthMiddleware>(); //使用自定义的 Token 认证中间件

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<TiktokGame2Server.Hubs.GameHub>("/gamehub"); //映射 SignalR Hub 到指定路径

app.UseHttpsRedirection(); //启用 HTTPS 重定向

app.UseAuthorization(); //启用 HTTPS 重定向

app.MapControllers(); //映射控制器路由

//app.MapGet("/", ()=>"hello world");

//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
//        if (exceptionHandlerPathFeature?.Error != null)
//        {
//            logger.LogError(exceptionHandlerPathFeature.Error, "Unhandled exception occurred.");
//        }
//        context.Response.StatusCode = 500;
//        await context.Response.WriteAsync("An error occurred.");
//    });
//}); //全局异常处理，防止未处理异常导致应用崩溃和暴露给客户端敏感信息

app.Run();




//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/gamehub")
//    .build();

//connection.on("ReceiveDrawSamurai", function(data) {
//    console.log("全服广播：", data);
//});

//connection.start();