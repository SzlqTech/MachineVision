using MachineVision.Core.Models;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using MachineVision.View.Services;
using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace MachineVision.View.ViewModels
{
    public class MainTabViewModel: NavigationViewMdoel
    {
        private readonly ProductService productService;
        public  IWorkCore VisionWork { get; }

        public MainTabViewModel(ProductService productService, IWorkCore visionWork)
        {
            this.productService = productService;
            this.VisionWork = visionWork;
            ExcuteCommand = new DelegateCommand<string>(Excute);       
        }

        private async void Excute(string obj)
        {
            switch (obj)
            {
                case "Load":
                    await Load();break;
                case "RunOnce":
                    await RunOnce(); break;
                case "Run":
                    Run(); break;
                case "Stop":
                    Stop(); break;
            }
        }

        private async Task Load()
        {
            if (SelectProduct == null || string.IsNullOrEmpty(SelectProduct.Path)) return;        
            try
            {
               await VisionWork.Load(SelectProduct.Path);
                IsEnable = false;
            }
            catch (VmException ex)
            {
                throw ex;
            }
        }

        private async Task RunOnce()
        {
            try
            {
                ImageSource=await VisionWork.RunOnce();
            }
            catch (VmException ex)
            {

                throw ex;
            }
        }

        public void Run()
        {

        }

        public void Stop()
        {         
            try
            {
                VisionWork.Stop();
                IsEnable = true;
            }
            catch (VmException ex)
            {
                throw ex;
            }
        }

        public bool IsEnable { get; set; } = true;

        public ObservableCollection<ProductModel> Products { get; set; }

        public ProductModel SelectProduct { get;set; }

        public IVmModule ImageSource { get; set; }

        public DelegateCommand<string> ExcuteCommand { get; set; }


        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Products = new ObservableCollection<ProductModel>();
            List<ProductModel> products =await productService.GetListAsync();
            Products.AddRange(products);
        }
    }
}
