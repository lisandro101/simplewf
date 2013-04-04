using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core
{
    public interface IWorkflowItemBuilder
    {
        WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history);

        WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history);
    }
}
