using Prism.Mvvm;
using Prism.Regions;

namespace MachineVision.Core.ViewModels
{
    public class NavigationViewMdoel : BindableBase, INavigationAware
    {
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
