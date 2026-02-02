using Game.Share;
using JFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class TimedTaskService : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(5); // 5分钟执行一次，可根据需要调整

        private readonly IServiceProvider _serviceProvider;


        JFramework.ILogger logger;

        public TimedTaskService(JFramework.ILogger logger, IServiceProvider serviceProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    await DoWorkAsync(stoppingToken);
                    await Task.Delay(_interval, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.Log("定时器执行异常 " + ex.Message);
                }
            }
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            logger.Log("定时器执行");
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
            var notifyService = scope.ServiceProvider.GetRequiredService<TiktokNotifyService>();


            var now = DateTime.UtcNow;
            var schedules = await db.Set<BuildingUpgradSchedule>()
                .Where(x => x.UpgradeEndAt <= now)
                .ToListAsync(stoppingToken);

            // 处理建筑升级完成检查
            var tasks = ProcessBuildingUpgrade(scope, db, schedules, notifyService);

            await Task.WhenAll(tasks);

            await db.SaveChangesAsync(stoppingToken);
        }

        //private async Task DoWorkAsync(CancellationToken stoppingToken)
        //{
        //    logger.Log("定时器执行");
        //    using var scope = _serviceProvider.CreateScope();
        //    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        //    var notifyService = scope.ServiceProvider.GetRequiredService<TiktokNotifyService>();
        //    var buildingService = scope.ServiceProvider.GetRequiredService<IBuildingService>();

        //    var now = DateTime.UtcNow;
        //    var schedules = await db.Set<BuildingUpgradSchedule>()
        //        .Where(x => x.UpgradeEndAt <= now)
        //        .ToListAsync(stoppingToken);

        //    foreach (var schedule in schedules)
        //    {
        //        var success = await buildingService.TryToCompleteUpgrade(db, schedule.BuildingId, schedule.PlayerId);
        //        if (success)
        //        {
        //            Console.WriteLine($"Building upgrade completed for BuildingId: {schedule.BuildingId}, PlayerId: {schedule.PlayerId}");
        //            var builiding = await db.Buildings.FirstOrDefaultAsync(b => b.Id == schedule.BuildingId && b.PlayerId == schedule.PlayerId);
        //            if (builiding != null)
        //            {
        //                var buildingDTO = new BuildingDTO
        //                {
        //                    BusinessId = builiding.BusinessId,
        //                    Level = builiding.Level,
        //                };
        //                await notifyService.NotifyBuildingUpdate(schedule.PlayerId, buildingDTO);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 处理建筑升级完成检查
        /// </summary>
        /// <param name="schedules"></param>
        /// <param name="notifyService"></param>
        /// <returns></returns>
        List<Task> ProcessBuildingUpgrade(IServiceScope scope, MyDbContext db, List<BuildingUpgradSchedule> schedules, TiktokNotifyService notifyService)
        {
            //using var scope = _serviceProvider.CreateScope();
            //var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var buildingService = scope.ServiceProvider.GetRequiredService<IBuildingService>();
            return schedules.Select(async schedule =>
            {
                var success = await buildingService.TryToCompleteUpgrade(db,schedule.BuildingId, schedule.PlayerId);
                if (success)
                {
                    Console.WriteLine($"Building upgrade completed for BuildingId: {schedule.BuildingId}, PlayerId: {schedule.PlayerId}");
                    var builiding = await buildingService.GetBuildingAsync(schedule.BuildingId, schedule.PlayerId);
                    if (builiding != null)
                    {
                        var buildingDTO = new BuildingDTO
                        {
                            BusinessId = builiding.BusinessId,
                            Level = builiding.Level,
                        };
                        await notifyService.NotifyBuildingUpdate(schedule.PlayerId, buildingDTO);
                    }
                }
            }).ToList();
        }
    }
}
