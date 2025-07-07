using Prism.Mvvm;
using System;


namespace MachineVision.Core.Models
{
    public class BaseModel:BindableBase
    {

        private long id;
        private DateTime createDate;
        private DateTime updateDate;
        private string code;
        private string name;

        public long Id
        {
            get { return id; }
            set { id = value; RaisePropertyChanged(); }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value;RaisePropertyChanged(); }
        }

        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value;RaisePropertyChanged(); }
        }


        public string Code
        {
            get { return code; }
            set { code = value;RaisePropertyChanged(); }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
