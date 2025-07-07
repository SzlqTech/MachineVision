using Prism.Events;


namespace MachineVision.Core.Events
{
    public class SnackBarModel
    {
        public string Message { get; set; }

        public string Filter { get; set; }
    }

    public class SnackBarMessageEvent : PubSubEvent<SnackBarModel>
    {

    }
}
