using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Logger
{
    public interface ILoggerService
    {
        ObservableCollection<string> Logs { get; set; }

        void Info(string log);

        void Error(string log);

        void RemoveLog(string log);

        void ClearLogs();

    }
}
