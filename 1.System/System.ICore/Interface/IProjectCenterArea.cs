using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Interface
{
    public interface IProjectCenterArea
    {
        string ProjectName { get; set; }

        IWFDesigner WFConstructor { get; set; }

        IWFDesigner WFDispose { get; set; }

        bool StartConstructor();

        bool StartDispose();

        bool StopConstructor();
    }
}
