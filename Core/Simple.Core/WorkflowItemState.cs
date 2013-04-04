using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core
{
    public enum WorkflowItemState
    {
        Scheduled,
        Started,
        Completed,
        Failed,
        Canceled,
        Terminated,
        Signaled,
        StartFailed,
        CancelFailed,
        TimedOut,
        ContinuedAsNew,
        CancelRequested,
        ScheduledFailed,
        RequestCancelFailed,
        Marked,
        Initiated
    }
}
