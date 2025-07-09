using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MachineVision.Core.Services.Carmer
{
    public class HIKCarmerService : ICarmerService
    {
        List<IDeviceInfo> deviceInfoList = new List<IDeviceInfo>();
        public int SelectedIndex = 0;
        bool isGrabbing = false;        // ch:是否正在取图 | en: Grabbing flag
        public readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice;
        public IDevice device = null;
        Thread receiveThread = null;    // ch:接收图像线程 | en: Receive image thread
        IntPtr pictureBoxHandle = IntPtr.Zero; // ch:显示图像的控件句柄 | en: Control handle for image display
        public void CloseDevice()
        {
            // ch:取流标志位清零 | en:Reset flow flag bit
            if (isGrabbing == true)
            {
                StopGrab();
            }

            // ch:关闭设备 | en:Close Device
            if (device != null)
            {
                device.Close();
                device.Dispose();
            }
        }

        public void FindDevice()
        {
            // ch:创建设备列表 | en:Create Device List
            int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
            if (nRet != MvError.MV_OK)
            {
                //ShowErrorMsg("Enumerate devices fail!", nRet);
                return;
            }
        }

        public void GetParam()
        {
            GetTriggerMode();

            IFloatValue floatValue;
            int result = device.Parameters.GetFloatValue("ExposureTime", out floatValue);
            if (result == MvError.MV_OK)
            {
                //tbExposure.Text = floatValue.CurValue.ToString("F1");
            }

            result = device.Parameters.GetFloatValue("Gain", out floatValue);
            if (result == MvError.MV_OK)
            {
              //  tbGain.Text = floatValue.CurValue.ToString("F1");
            }

            result = device.Parameters.GetFloatValue("ResultingFrameRate", out floatValue);
            if (result == MvError.MV_OK)
            {
                //tbFrameRate.Text = floatValue.CurValue.ToString("F1");
            }

            IEnumValue enumValue;
            result = device.Parameters.GetEnumValue("PixelFormat", out enumValue);
            if (result == MvError.MV_OK)
            {
                //tbPixelFormat.Text = enumValue.CurEnumEntry.Symbolic;
            }
        }

        public void OpenDevice()
        {
            // ch:获取选择的设备信息 | en:Get selected device information
            IDeviceInfo deviceInfo = deviceInfoList[SelectedIndex];

            try
            {
                // ch:打开设备 | en:Open device
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create Device fail!" + ex.Message);
                return;
            }

            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                // ShowErrorMsg("Open Device fail!", result);
                return;
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                if (result != MvError.MV_OK)
                {
                    // ShowErrorMsg("Warning: Get Packet Size failed!", result);
                }
                else
                {
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        //ShowErrorMsg("Warning: Set Packet Size failed!", result);
                    }
                }
            }
        }

        public void SetParam(string exposure,string gain,string frameRate)
        {
            try
            {
                float.Parse(exposure);
                float.Parse(gain);
                float.Parse(frameRate);
            }
            catch
            {
               // ShowErrorMsg("Please enter correct type!", 0);
                return;
            }

            device.Parameters.SetEnumValue("ExposureAuto", 0);
            int result = device.Parameters.SetFloatValue("ExposureTime", float.Parse(exposure));
            if (result != MvError.MV_OK)
            {
               // ShowErrorMsg("Set Exposure Time Fail!", result);
            }

            device.Parameters.SetEnumValue("GainAuto", 0);
            result = device.Parameters.SetFloatValue("Gain", float.Parse(gain));
            if (result != MvError.MV_OK)
            {
                //ShowErrorMsg("Set Gain Fail!", result);
            }

            result = device.Parameters.SetBoolValue("AcquisitionFrameRateEnable", true);
            if (result != MvError.MV_OK)
            {
                //ShowErrorMsg("Set AcquisitionFrameRateEnable Fail!", result);
            }
            else
            {
                result = device.Parameters.SetFloatValue("AcquisitionFrameRate", float.Parse(frameRate));
                if (result != MvError.MV_OK)
                {
                    //ShowErrorMsg("Set Frame Rate Fail!", result);
                }
            }
        }

        public void StartGrab()
        {
            try
            {
                // ch:标志位置位true | en:Set position bit true
                isGrabbing = true;
               // pictureBoxHandle = displayArea.Handle;

                receiveThread = new Thread(ReceiveThreadProcess);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Start thread failed!, " + ex.Message);
                return;
            }

            // ch:开始采集 | en:Start Grabbing
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)
            {
                isGrabbing = false;
                receiveThread.Join();
                //ShowErrorMsg("Start Grabbing Fail!", result);
                return;
            }

        }

        public void StopGrab()
        {
            // ch:标志位设为false | en:Set flag bit false
            isGrabbing = false;
            receiveThread.Join();

            // ch:停止采集 | en:Stop Grabbing
            int result = device.StreamGrabber.StopGrabbing();
            if (result != MvError.MV_OK)
            {
               // ShowErrorMsg("Stop Grabbing Fail!", result);
            }
        }

        public void ReceiveThreadProcess()
        {
            IFrameOut frameOut = null;
            int result = MvError.MV_OK;

            while (isGrabbing)
            {
                result = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
                if (result == MvError.MV_OK)
                {
                    //device.ImageRender.DisplayOneFrame(pictureBoxHandle, frameOut.Image);

                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
            }
        }

        public void TriggerExec()
        {
            int result = device.Parameters.SetCommandValue("TriggerSoftware");
            if (result != MvError.MV_OK)
            {
               // ShowErrorMsg("Trigger Software Fail!", result);
            }
        }

        /// <summary>
        /// ch:获取触发模式 | en:Get Trigger Mode
        /// </summary>
        private void GetTriggerMode()
        {
            IEnumValue enumValue;
            int result = device.Parameters.GetEnumValue("TriggerMode", out enumValue);
            if (result == MvError.MV_OK)
            {
                if (enumValue.CurEnumEntry.Symbolic == "On")
                {
                   // bnTriggerMode.IsChecked = true;
                    //bnContinuesMode.IsChecked = false;

                    result = device.Parameters.GetEnumValue("TriggerSource", out enumValue);
                    if (result == MvError.MV_OK)
                    {
                        if (enumValue.CurEnumEntry.Symbolic == "TriggerSoftware")
                        {
                           // cbSoftTrigger.IsEnabled = true;
                            //cbSoftTrigger.IsChecked = true;
                            if (isGrabbing)
                            {
                               // bnTriggerExec.IsEnabled = true;
                            }
                        }
                    }
                }
                else
                {
                   // bnContinuesMode.IsChecked = true;
                    //bnTriggerMode.IsChecked = false;
                }
            }
        }
    }
}
