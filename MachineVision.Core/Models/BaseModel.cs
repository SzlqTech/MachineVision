using Prism.Mvvm;
using System;


namespace MachineVision.Core.Models
{
    public class BaseModel:BindableBase
    {
        public long Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
