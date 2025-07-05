using MachineVision.Core.Services;
using MachineVision.Core.ViewModels;
using Prism.Regions;


namespace MachineVision.ViewModels
{
    public class MainWindowViewModel: NavigationViewMdoel
    {
        private readonly IRegionManager manager;

        public INavigationMenuService NavigationMenuService { get; }
        public MainWindowViewModel(INavigationMenuService navigationMenuService, IRegionManager manager )
        {
            NavigationMenuService = navigationMenuService;
            this.manager = manager;
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationMenuService.InitMenus();
        }
    }
}
