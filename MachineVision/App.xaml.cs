using MachineVision.Extensions;
using MachineVision.View;
using MachineVision.ViewModels;
using MachineVision.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace MachineVision
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        

        protected override void RegisterTypes(IContainerRegistry service)
        {
            service.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            service.AddService();
            service.AddAutoMapper();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MachineViewModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
