using MachineVision.Core.Events;
using MachineVision.Core.Models;
using MachineVision.Core.Services;
using MachineVision.Core.ViewModels;
using MachineVision.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace MachineVision.ViewModels
{
    
    public class MainWindowViewModel: NavigationViewMdoel
    {
        private readonly IRegionManager manager;
        private readonly IEventAggregator aggregator;
        private readonly IHostDialogService dialog;

        public INavigationMenuService NavigationMenuService { get; }
        public MainWindowViewModel(INavigationMenuService navigationMenuService, IRegionManager manager, 
            IEventAggregator aggregator, IHostDialogService dialog)
        {
            NavigationMenuService = navigationMenuService;
            this.manager = manager;
            this.aggregator = aggregator;
            this.dialog = dialog;
            InitCommand();
            aggregator.ResgiterBusyAsyncMessage(arg =>
            {
                IsOpen = arg.IsOpen;
            }, "Main");
            GoHomeCommand= new DelegateCommand(() =>
            {
                NavigatePage("MainTabView");
                IsTopDrawerOpen = false;
            });
            SettingCommand = new DelegateCommand(ShowDevice);
           
        }

        public bool IsTopDrawerOpen { get; set; }

        public bool IsOpen { get; set; }

        public DelegateCommand<NavigationItem> NavigateCommand { get;set; }

        public DelegateCommand LoadedCommand { get;set; }

        public DelegateCommand GoHomeCommand { get; set; }

        public DelegateCommand SettingCommand { get; set; }

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

        private async void ShowDevice()
        {
            var dialogResult = await dialog.ShowDialogAsync(nameof(SettingsView));
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
