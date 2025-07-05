using AutoMapper;
using Prism.Ioc;

namespace MachineVision.Core.Services.DataBase
{
    public class BaseService
    {
        public  static IMapper mapper { get; set; }

        public static IFreeSql Sql { get;set; }

        public BaseService()
        {
            mapper = ContainerLocator.Container.Resolve<IMapper>();
            Sql= ContainerLocator.Container.Resolve<IFreeSql>();
        }

        //public static IFreeSql Sql = new FreeSql.FreeSqlBuilder()
        //       .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=freedb.db")
        //       .UseAutoSyncStructure(true) //自动同步实体结构到数据库，只有CRUD时才会生成表
        //       .Build();
    }
}
