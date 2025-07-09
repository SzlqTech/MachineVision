using MachineVision.Core.Enum;
using MachineVision.Core.Models;
using MachineVision.Core.Services.DataBase;
using MachineVision.Core.ViewModels;
using Newtonsoft.Json;
using Prism.Services.Dialogs;
using System;

namespace MachineVision.ViewModels
{
    public class SettingsViewModel:DialogViewModel
    {
        public AppSettingService appService { get; }
        public SettingsViewModel(AppSettingService appService)
        {
            CameraList = Enum.GetValues(typeof(CameraType));
            DeviceList = Enum.GetValues(typeof(DeviceType));
            this.appService = appService;
        }

        public Array CameraList { get; set; }

        public Array DeviceList { get; set; }

        private AppSettingParameter parameter;

        public AppSettingParameter Parameter
        {
            get { return parameter; }
            set { parameter = value; RaisePropertyChanged(); }
        }

 

        public override async void Save(DialogResult result)
        {
            await appService.UpdateSettingAsync(Parameter);
            base.Save(result);
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            var appSetting = await appService.GetSettingAsync();
            if (appSetting == null)
                Parameter = new AppSettingParameter();
            else
                Parameter = JsonConvert.DeserializeObject<AppSettingParameter>(appSetting.Content);
            base.OnDialogOpened(parameters);
        }
    }
}
