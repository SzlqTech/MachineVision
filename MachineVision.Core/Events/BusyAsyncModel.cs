using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Events
{
    public class BusyAsyncModel
    {
        public bool IsOpen { get; set; }

        public string Filter { get; set; }
    }

    public class BusyAsyncEvent : PubSubEvent<BusyAsyncModel>
    {

    }
}
