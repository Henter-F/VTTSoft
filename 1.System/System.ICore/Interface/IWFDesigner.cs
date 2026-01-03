using System;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.ICore.Enum;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ICore.Interface
{
    public interface IWFDesigner
    {
        string Name { get; set; }

        WorkFlowStatus Status { get; set; }

        string FilePath { get; set; }

        Action<string, WorkFlowStatus> UpdateStatusEvent { get; set; }

        WorkflowDesigner Designer { get; set; }


        void CreatActivity();

        void OpenActivity();

        void SaveActivity();

        void Start();

        void Stop();
    }
}
