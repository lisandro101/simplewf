using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core
{
    public enum WorkflowItemKind
    {
        Workflow,
        Activity,
        ChildWorkflow,
        Signal,
        Marker,
        Timer,
        ExternalCancellation,
        ExternalSignaling,
        ActivityCancellation
    }
}
