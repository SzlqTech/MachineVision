using IfModuleCs;
using MachineVision.Core.Extensions;
using MachineVision.Core.Models;
using MachineVision.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using TimeStatisticModuleCs;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace MachineVision.View.Services
{
    public class VisionBaseWorkflow :ViewModelBase, IWorkCore
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public int SN { get; set; } = 0;

        public bool IsOpen { get; set; }

        public VisionBaseWorkflow()
        {
            Results = new ObservableCollection<InspectionResult>();
            VmSolution.OnWorkStatusEvent -= VmSolution_OnWorkStatusEvent;
            VmSolution.OnWorkStatusEvent += VmSolution_OnWorkStatusEvent;
        }

        #region 关闭Vm
        void KillProcess(string strKillName)
        {
            foreach (var P in System.Diagnostics.Process.GetProcesses())
            {
                if (P.ProcessName.Contains(strKillName))
                {
                    try
                    {
                        P.Kill();
                        P.WaitForExit();
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                }
            }
        }
        #endregion

        public event EventHandler<TEventArgs<bool>> DataResultReceived;

        public void OnRasiseDataResultReceived(bool result)
        {
            DataResultReceived?.Invoke(this, new TEventArgs<bool>(result));
        }

        public virtual void VmSolution_OnWorkStatusEvent(ImvsSdkDefine.IMVS_MODULE_WORK_STAUS workStatusInfo)
        {
            //0表示执行完毕 1表示正在执行 10000表示流程1
            try
            {
                if (workStatusInfo.nWorkStatus == 0)
                {
                    if (workStatusInfo.nProcessID == 10000)
                    {
                        VmProcedure vmProcedure = VmSolution.Instance["流程1"] as VmProcedure;
                        IfModuleTool ifModule= vmProcedure["条件检测1"] as IfModuleTool;
                        TimeStatisticModuleTool timeStatisticModule = vmProcedure["耗时统计1"] as TimeStatisticModuleTool;                 
                        if (ifModule != null)
                        {
                            InspectionResult result = new InspectionResult();
                            if(timeStatisticModule != null)
                            {
                                result.TimeSpan =Math.Round(timeStatisticModule.ModuResult.Time,2);
                            }
                            else
                            {
                                result.TimeSpan = 0;
                            }
                           
                            if (ifModule.ModuResult.StrResult.Trim().Equals("OK"))
                            {
                                OnRasiseDataResultReceived(true);
                                result.IsSuccess = true;
                            }

                            else
                            {
                                OnRasiseDataResultReceived(false);
                                result.IsSuccess = false;
                            }
                             
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                if (Results.Count > 20)
                                {
                                    Results.RemoveAt(0);
                                }
                                SN++;
                                result.SN = SN;
                                Results.Add(result);
                            });
                          
                        }
                    }
                }
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "回调失败失败");
            }
        }

        public IVmModule ImageSource { get; set ; }

        public bool Result { get; set; }
        public ObservableCollection<InspectionResult> Results { get ; set ; }

        public virtual async Task<bool> Load(string path)
        {
            try
            {
                if(IsOpen)
                {
                    SendMessage("方案已经加载");
                    return false;
                }
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {                 
                        VmSolution.Load(path);
                    });
                    IsOpen = true;
                });

                return true;
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "加载失败");
                return false;
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "加载失败");
                return false;
                
            }
        }

        public virtual void Run()
        {
            
        }

        public async Task<IVmModule> RunOnce()
        {
            try
            {
                if (!IsOpen)
                {
                    SendMessage("方案未加载");
                    return null;
                }
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {
                        VmSolution.Instance.SyncRun();
                        VmProcedure vmProcess1 = (VmProcedure)VmSolution.Instance["流程1"];
                        ImageSource = vmProcess1;
                    });
                });           
               // SendMessage("运行成功");
                return ImageSource;
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "运行失败");   
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "运行失败");
                return null;
            }
        }

        public virtual async Task<bool> Stop()
        {
            try
            {
                if (!IsOpen)
                {
                    SendMessage("方案未加载");
                    return false;
                }
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {
                        VmSolution.Instance?.CloseSolution();                
                    });
                    IsOpen= false;
                });
                //SendMessage("停止成功");
                return true;
            }
            catch (VmException ex)
            {
                Logger.Error(ex, "停止失败");
                return false;
            }
        }

        public void CloseVM()
        {
            KillProcess("VisionMasterServerApp");
            KillProcess("VisionMaster");
            KillProcess("VmModuleProxy.exe");
        }
    }
}
