
using AutoMapper;
using FreeSql;
using MachineVision.Core.Enum;
using MachineVision.Core.Logger;
using MachineVision.Core.Mapper;
using MachineVision.Core.Services;
using MachineVision.Core.Services.Carmer;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using Prism.Ioc;

namespace MachineVision.Extensions
{
    public static class ContainerExtension
    {
        public static void AddService(this IContainerRegistry service)
        {
            service.Register<INavigationMenuService, NavigationMenuService>();
            service.Register<BaseService,ProductService>();
            service.Register<BaseService, AppSettingService>();
            var Sql = new FreeSqlBuilder()
                  .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=freedb.db")
                  .UseAutoSyncStructure(true)  // 开发环境自动同步实体结构 
                  .Build();
            // 注册 FreeSql 为单例服务
            service.RegisterSingleton<IFreeSql>(() => Sql);
            service.Register<ILoggerService, LoggerService>();
            service.Register<IHostDialogAware, DialogViewModel>();
            service.RegisterSingleton<IHostDialogService, HostDialogService>();
            service.Register<ICarmerService,HIKCarmerService>(nameof(CameraType.HIKVision));
            service.AddAutoMapper();
        }

        public static void AddAutoMapper(this IContainerRegistry service)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AppMapper());
            });
            service.RegisterInstance<IMapper>(configuration.CreateMapper());
        }
    }
}
