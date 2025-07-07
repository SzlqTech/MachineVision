using IfModuleCs;
using MachineVision.Core.Extensions;
using MachineVision.Core.ViewModels;
using System;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;

namespace MachineVision.View.Services
{
    public class VisionBaseWorkflow :ViewModelBase, IWorkCore
    {

        public VisionBaseWorkflow()
        {
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
                        if(ifModule != null)
                        {
                            if (ifModule.ModuResult.StrResult.Trim().Equals("OK"))
                                OnRasiseDataResultReceived(true);
                            else
                                OnRasiseDataResultReceived(false);
                        }
                    }
                }
            }
            catch (VmException ex)
            {
                throw ex;
            }
        }

        public IVmModule ImageSource { get; set ; }

        public bool Result { get; set; }

        public virtual async Task Load(string path)
        {
            try
            {
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {
                        //KillProcess("VisionMasterServerApp");
                        //KillProcess("VisionMaster");
                        //KillProcess("VmModuleProxy.exe");
                        VmSolution.Load(path);
                    });
                });
              
                SendMessage("加载成功");
            }
            catch (VmException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void Run()
        {
            throw new NotImplementedException();
        }

        public async Task<IVmModule> RunOnce()
        {
            try
            {
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
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task Stop()
        {
            try
            {
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {
                        VmSolution.Instance?.Dispose();
                    });
                });        
                SendMessage("停止成功");
            }
            catch (VmException ex)
            {
                throw ex;
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
