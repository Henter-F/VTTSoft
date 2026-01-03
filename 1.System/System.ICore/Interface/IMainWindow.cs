using AvalonDock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Interface
{
    public interface IMainWindow
    {
        DockingManager DockManagers { get; }
    }
}
