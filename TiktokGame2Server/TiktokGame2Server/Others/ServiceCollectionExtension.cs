using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace TiktokGame2Server.Others
{
    public class AppConst
    {
        public const string User = "User";
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
    }
    public static class ServiceCollectionExtension
    {
        public static void AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                // 添加JWT认证
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "请输入JWT Token，格式为：Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });
        }

        public static IServiceCollection AddAuthorize(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // 策略1：要求管理员角色
                options.AddPolicy(AppConst.User, policy =>
                    policy.RequireRole(AppConst.User));

                options.AddPolicy(AppConst.Admin, policy => policy.RequireRole(AppConst.Admin, AppConst.SuperAdmin));

                options.AddPolicy(AppConst.SuperAdmin, policy => policy.RequireRole(AppConst.SuperAdmin));

                //// 策略2：要求编辑权限且年满18岁
                //options.AddPolicy("AdultEditors", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireRole("Editor");
                //    policy.RequireClaim("Age", "18", "19", "20"); // 假设年龄存储在声明中
                //});

                //// 策略3：自定义要求（需要实现IAuthorizationRequirement）
                //options.AddPolicy("DepartmentAccess", policy =>
                //    policy.Requirements.Add(new DepartmentRequirement("IT", "Finance")));

                //// 策略4：混合要求
                //options.AddPolicy("SeniorStaff", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireAssertion(context =>
                //    {
                //        var hireDateClaim = context.User.FindFirst(c => c.FormationType == "HireDate");
                //        if (hireDateClaim == null) return false;

                //        var hireDate = DateTime.Parse(hireDateClaim.Value);
                //        return hireDate < DateTime.Now.AddYears(-5); // 入职超过5年
                //    });
                //});          
            });

            // 注册自定义要求的处理器
            //services.AddSingleton<IAuthorizationHandler, DepartmentRequirementHandler>();

            return services;
        }
    }

    // 自定义授权要求
    public class DepartmentRequirement : IAuthorizationRequirement
    {
        public string[] AllowedDepartments { get; }

        public DepartmentRequirement(params string[] departments)
        {
            AllowedDepartments = departments;
        }
    }

    // 自定义要求处理器
    public class DepartmentRequirementHandler : AuthorizationHandler<DepartmentRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DepartmentRequirement requirement)
        {
            var departmentClaim = context.User.FindFirst(c => c.Type == "Department");
            if (departmentClaim != null &&
                requirement.AllowedDepartments.Contains(departmentClaim.Value))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
