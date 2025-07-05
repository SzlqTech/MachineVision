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
    }
}
