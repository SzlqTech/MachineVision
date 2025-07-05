

namespace MachineVision.Core.Entity
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }

        public int OK { get; set; }

        public int NG { get; set; }

        public string Path { get; set; }
    }
}
