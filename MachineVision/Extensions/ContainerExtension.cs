
using AutoMapper;
using MachineVision.Core.Mapper;
using MachineVision.Core.Services;
using Prism.Ioc;


namespace MachineVision.Extensions
{
    public static class ContainerExtension
    {
        public static void AddService(this IContainerRegistry service)
        {
            service.Register<INavigationMenuService, NavigationMenuService>();
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
