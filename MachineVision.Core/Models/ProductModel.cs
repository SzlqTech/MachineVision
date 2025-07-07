

namespace MachineVision.Core.Models
{
    public class ProductModel: BaseModel
    {

        private int ok;
        private int ng;
        private string path;

        public int OK
        {
            get { return ok; }
            set { ok = value;RaisePropertyChanged(); }
        }

        public int NG
        {
            get { return ng; }
            set { ng = value;RaisePropertyChanged(); }
        }


        public string Path
        {
            get { return path; }
            set { path = value;RaisePropertyChanged(); }
        }

    }
}
