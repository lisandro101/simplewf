using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core.Builders
{
    public class SignalBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var attributes = historyEvent.WorkflowExecutionSignaledEventAttributes;

            var decisionId = historyEvent.EventId;

            return new WorkflowItem
            {
                EventId = historyEvent.EventId,
                Name = attributes.SignalName,
                Input = attributes.Input,
                Order = decisionId,
                Kind = WorkflowItemKind.Signal,
                State = WorkflowItemState.Signaled
            };
        }

        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            throw new NotImplementedException();
        }
    }
}
