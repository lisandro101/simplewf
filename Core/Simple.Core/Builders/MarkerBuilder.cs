using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core.Builders
{
    public class MarkerBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var attributes = historyEvent.MarkerRecordedEventAttributes;

            var decisionId = historyEvent.EventId;

            return new WorkflowItem
            {
                EventId = historyEvent.EventId,
                Name = attributes.MarkerName,
                Order = decisionId,
                Kind = WorkflowItemKind.Marker,
                State = WorkflowItemState.Completed
            };
        }

        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            throw new NotImplementedException();
        }
    }
}
