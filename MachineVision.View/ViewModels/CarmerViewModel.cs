using MachineVision.Core.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.View.ViewModels
{
    public class CarmerViewModel: NavigationViewMdoel
    {
        public CarmerViewModel()
        {
            ExcuteCommand = new DelegateCommand<string>(Excute);
        }

        #region Command
        private void Excute(string obj)
        {
            switch (obj)
            {
                case "FindDevice":
                    FindDevice(); break;
                case "OpenDevice":
                    OpenDevice(); break;
                case "CloseDevice":
                    CloseDevice(); break;
                case "StartGrab":
                    StartGrab(); break;
                case "StopGrab":
                    StopGrab(); break;
                case "TriggerExec":
                    TriggerExec(); break;
                case "GetParam":
                    GetParam(); break;
                case "SetParam":
                    SetParam(); break;
            }

        }

        private void SetParam()
        {
            
        }

        private void GetParam()
        {
            
        }

        private void TriggerExec()
        {
            
        }

        private void StopGrab()
        {
            
        }

        private void StartGrab()
        {
            
        }

        private void CloseDevice()
        {
            
        }

        private void OpenDevice()
        {
           
        }

        private void FindDevice()
        {
            
        }

        public DelegateCommand<string> ExcuteCommand { get; set; }  
        #endregion
    }
}
