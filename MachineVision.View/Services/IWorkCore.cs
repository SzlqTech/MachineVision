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

        Task Load(string path);

        Task<IVmModule> RunOnce();

        void Run();

        void Stop();    
    }
}
