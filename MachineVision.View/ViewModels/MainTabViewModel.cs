using MachineVision.Core.Extensions;
using MachineVision.Core.Logs;
using MachineVision.Core.Models;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using MachineVision.View.Services;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace MachineVision.View.ViewModels
{
    public class MainTabViewModel: NavigationViewMdoel
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ProductService productService;
        public  IWorkCore VisionWork { get; }
        public ILogService LogService { get; }

        /// <summary>
        /// 缺陷检测图片
        /// </summary>
        public ObservableCollection<IVmModule> DefectImages { get; set; } = new ObservableCollection<IVmModule>();

        public MainTabViewModel(ProductService productService, IWorkCore visionWork, ILogService logService)
        {
            this.productService = productService;
            this.VisionWork = visionWork;
            LogService = logService;
            ExcuteCommand = new DelegateCommand<string>(Excute);
            visionWork.DataResultReceived -= VisionWork_DataResultReceived;
            visionWork.DataResultReceived += VisionWork_DataResultReceived;
            visionWork.DefectImageReceived -= VisionWork_DefectImageReceived;
            visionWork.DefectImageReceived += VisionWork_DefectImageReceived;
        }

        private void VisionWork_DefectImageReceived(object sender, TEventArgs<IVmModule> e)
        {
            if (DefectImages.Count>=3)
            {
                DefectImages.RemoveAt(0);
            }
            DefectImages.Add(e.Data);

        }

        private async void VisionWork_DataResultReceived(object sender, Core.Extensions.TEventArgs<bool> e)
        {
            if (e.Data)
            {
                SelectProduct.OK += 1;
            }
            else
            {
               SelectProduct.NG += 1;
            }
            await productService.UpdateAsync(SelectProduct);
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
                   await Stop(); break;
            }
        }

        private async Task Load()
        {
            if (SelectProduct == null || string.IsNullOrEmpty(SelectProduct.Path)) return;        
            try
            {            
                if(await VisionWork.Load(SelectProduct?.Path))
                {
                    IsEnable = false;
                    LogService.Info($"加载产品：{SelectProduct?.Name} 成功！");
                }      
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "加载错误");
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

                Logger.Error(ex, "运行错误");
            }
        }

        public void Run()
        {

        }

        public async Task Stop()
        {         
            try
            {
                if(await VisionWork.Stop())
                {
                    IsEnable = true;
                    LogService.Info($"停止产品：{SelectProduct?.Name} 成功！");
                }    
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "停止错误");
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
