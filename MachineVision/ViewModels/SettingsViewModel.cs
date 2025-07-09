using MachineVision.Core.Constants;
using MachineVision.Core.Enum;
using MachineVision.Core.Models;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace MachineVision.ViewModels
{
    public class SettingsViewModel:DialogViewModel
    {
        public AppSettingService appService { get; }
        private readonly ProductService productService;
        public SettingsViewModel(AppSettingService appService,ProductService productService)
        {
            CameraList = Enum.GetValues(typeof(CameraType));
            DeviceList = Enum.GetValues(typeof(DeviceType));
            this.appService = appService;
            this.productService = productService;
        }

        public Array CameraList { get; set; }

        public Array DeviceList { get; set; }

        public ProductModel SelectedProduct { get; set; }

        public List<ProductModel> Products { get; set; } = new List<ProductModel>();

        private AppSettingParameter parameter;
   

        public AppSettingParameter Parameter
        {
            get { return parameter; }
            set { parameter = value; RaisePropertyChanged(); }
        }

 

        public override async void Save(DialogResult result)
        {
            Parameter.ProductName = SelectedProduct?.Name;
            await appService.UpdateSettingAsync(Parameter);
            AppConstant.CurrProduct = SelectedProduct;
            base.Save(result);
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            var appSetting = await appService.GetSettingAsync();
            var productList = await productService.GetListAsync();
            Products.AddRange(productList);
            if (appSetting == null)
                Parameter = new AppSettingParameter();
            else
                Parameter = JsonConvert.DeserializeObject<AppSettingParameter>(appSetting.Content);
            SelectedProduct= Products?.Find(s => s.Name == Parameter.ProductCode) ?? new ProductModel();    
            base.OnDialogOpened(parameters);
        }
    }
}
