
using AutoMapper;
using FreeSql;
using MachineVision.Core.Mapper;
using MachineVision.Core.Services;
using MachineVision.Core.Services.DataBase;
using Prism.Ioc;
using System.ComponentModel.DataAnnotations;


namespace MachineVision.Extensions
{
    public static class ContainerExtension
    {
        public static void AddService(this IContainerRegistry service)
        {
            service.Register<INavigationMenuService, NavigationMenuService>();
            service.Register<BaseService,ProductService>();
            var Sql = new FreeSqlBuilder()
          .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=freedb.db")
          .UseAutoSyncStructure(true)  // 开发环境自动同步实体结构 
          .Build();
            // 注册 FreeSql 为单例服务
           service.RegisterSingleton<IFreeSql>(() => Sql);
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
