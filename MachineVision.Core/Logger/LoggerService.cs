using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Logger
{
    public class LoggerService: ILoggerService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();

        public void ClearLogs()
        {
            Logs.Clear();
        }

        public void Error(string log)
        {
            Logs.Add($"{DateTime.Now.ToString("HH:mm:ss")}:" + log);
            Logger.Error(log);
        }

        public void Info(string log)
        {
            Logs.Add($"{DateTime.Now.ToString("HH:mm:ss")}:" + log);
            Logger.Info(log);
        }

        public void RemoveLog(string log)
        {
            Logs.Remove($"{DateTime.Now.ToString("HH:mm:ss")}:" + log);
        }

    }
}
