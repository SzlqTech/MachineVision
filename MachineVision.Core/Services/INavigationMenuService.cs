using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Services
{
    public interface INavigationMenuService
    {
        void InitMenus();

        void RefreshMenus();
    }
}
