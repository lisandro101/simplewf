using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core.Builders
{
    public class ExternalSignalingBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            throw new NotImplementedException();
        }

        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            throw new NotImplementedException();
        }
    }
}
