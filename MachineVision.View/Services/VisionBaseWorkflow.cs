using System;
using System.Threading.Tasks;
using VM.Core;
using VM.PlatformSDKCS;
using VMControls.Interface;
using MachineVision.Core.ViewModels;

namespace MachineVision.View.Services
{
    public class VisionBaseWorkflow :ViewModelBase, IWorkCore
    {
       
        public IVmModule ImageSource { get; set ; }

        public virtual async Task Load(string path)
        {
            try
            {
                await SetBusyAsync(async () =>
                {
                    await Task.Run(() =>
                    {
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

        public void Run()
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

        public void Stop()
        {
            try
            {
                VmSolution.Instance?.Dispose();
                SendMessage("停止成功");
            }
            catch (VmException ex)
            {
                throw ex;
            }
        }
    }
}
