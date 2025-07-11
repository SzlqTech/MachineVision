﻿using Prism.Mvvm;


namespace MachineVision.Core.Models
{
    /// <summary>
    /// 应用程序系统配置参数
    /// </summary>
    public class AppSettingParameter : BindableBase
    {
        private string deviceType;
        private string cameraType;
        private string productCode;
        private string productName;
        private int id;
        

        /// <summary>
        /// ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 相机类型
        /// </summary>
        public string CameraType
        {
            get { return cameraType; }
            set { cameraType = value; RaisePropertyChanged(); }
        }

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; RaisePropertyChanged(); }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; RaisePropertyChanged(); }
        }
    }
}
