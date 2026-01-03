using System;
using System.Collections.Generic;
using System.ICore.Enum;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Interface
{
    public interface IProjectSubArea
    {

        Action<WorkFlowStatus> WorkFlowStatusEnvent { get; set; }

        string ProjectName { get; }

        string GroupName { get; }

        List<IWFDesigner> Items { get; }

        WorkFlowStatus Status { get; }


        bool Start();

        bool Stop();
    }
}
