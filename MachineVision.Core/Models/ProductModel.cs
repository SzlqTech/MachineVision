

namespace MachineVision.Core.Models
{
    public class ProductModel: BaseModel
    {
        public string Name { get; set; }

        public int OK { get; set; }

        public int NG { get; set; }

        public string Path { get; set; }
    }
}
