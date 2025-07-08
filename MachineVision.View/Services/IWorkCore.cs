using MachineVision.Core.Extensions;
using MachineVision.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMControls.Interface;

namespace MachineVision.View.Services
{
    public interface IWorkCore
    {
        IVmModule ImageSource { get; set; }

        bool Result { get; set; }

        bool IsOpen { get; set; }

        void CloseVM();

        Task<bool> Load(string path);

        Task<IVmModule> RunOnce();

        void Run();

        Task<bool> Stop();    

        event EventHandler<TEventArgs<bool>> DataResultReceived;

        ObservableCollection<InspectionResult> Results { get; set; }

        ObservableCollection<IVmModule> DefectImages { get; }

        event EventHandler<TEventArgs<IVmModule>> DefectImageReceived;
    }
}
