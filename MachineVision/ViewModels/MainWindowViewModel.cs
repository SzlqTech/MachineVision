using MachineVision.Core.Events;
using MachineVision.Core.Models;
using MachineVision.Core.Services;
using MachineVision.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace MachineVision.ViewModels
{
    
    public class MainWindowViewModel: NavigationViewMdoel
    {
        private readonly IRegionManager manager;
        private readonly IEventAggregator aggregator;

        public INavigationMenuService NavigationMenuService { get; }
        public MainWindowViewModel(INavigationMenuService navigationMenuService, IRegionManager manager, IEventAggregator aggregator )
        {
            NavigationMenuService = navigationMenuService;
            this.manager = manager;
            this.aggregator = aggregator;
            InitCommand();
            aggregator.ResgiterBusyAsyncMessage(arg =>
            {
                IsOpen = arg.IsOpen;
            }, "Main");
        }

        public bool IsTopDrawerOpen { get; set; }

        public bool IsOpen { get; set; }

        public DelegateCommand<NavigationItem> NavigateCommand { get;set; }

        public DelegateCommand LoadedCommand { get;set; }


        public void Navigate(NavigationItem item)
        {
            if (item == null) return;
            if (!string.IsNullOrEmpty(item.PageName))
            {
                manager.Regions["MainRegion"].RequestNavigate(item.PageName, back =>
                {
                    if ((bool)back.Result)
                    {
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(back.Error.Message);
                    }
                });
            }
            if (item.Name.Equals("全部"))
            {
                IsTopDrawerOpen = true;
                return;
            }
            IsTopDrawerOpen = false;
        }

       
        public void Loaded()
        {
            NavigationMenuService.InitMenus();
            NavigatePage("MainTabView");
        }

        public void InitCommand()
        {
            NavigateCommand = new DelegateCommand<NavigationItem>(Navigate);
            LoadedCommand = new DelegateCommand(Loaded);
        }

        public void NavigatePage(string pageName)
        {
            manager.Regions["MainRegion"].RequestNavigate(pageName, back =>
            {
                if (!(bool)back.Result)
                {
                    System.Diagnostics.Debug.WriteLine(back.Error.Message);
                }
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
           
        }
    }
}
