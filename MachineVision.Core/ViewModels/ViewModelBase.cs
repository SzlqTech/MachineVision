using MachineVision.Core.Events;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;


namespace MachineVision.Core.ViewModels
{
    public class ViewModelBase: BindableBase
    {
        public ViewModelBase()
        {
            aggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
        }


        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value;RaisePropertyChanged(); }
        }

        public readonly IEventAggregator aggregator;

        public void SendMessage(string msg, string filterName = "Main")
        {
            aggregator.SendSnackBarMessage(msg, filterName);
        }

        public virtual async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            aggregator.SendBusyAsyncMessage(new BusyAsyncModel() { IsOpen = IsBusy });
            try
            {
                await func();
            }
            finally
            {
                IsBusy = false;
                aggregator.SendBusyAsyncMessage(new BusyAsyncModel() { IsOpen = IsBusy });
            }
        }

    }
}
