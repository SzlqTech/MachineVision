﻿using MachineVision.View.Services;
using MachineVision.View.ViewModels;
using MachineVision.View.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace MachineVision.View
{
    public class MachineViewModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
           
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {       
            containerRegistry.RegisterForNavigation<MainTabView,MainTabViewModel>();
            containerRegistry.RegisterForNavigation<ProductView,ProductViewModel>();
            containerRegistry.Register<IWorkCore, VisionBaseWorkflow>();
            containerRegistry.RegisterForNavigation<CarmerView, CarmerViewModel>();
        }
    }
}
