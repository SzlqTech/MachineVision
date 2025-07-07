using MachineVision.Core.Extensions;
using System;
using System.Collections.Generic;
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

        void CloseVM();

        Task Load(string path);

        Task<IVmModule> RunOnce();

        void Run();

        Task Stop();    

        event EventHandler<TEventArgs<bool>> DataResultReceived;
    }
}
