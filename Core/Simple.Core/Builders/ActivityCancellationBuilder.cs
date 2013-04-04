using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Simple.Core.Builders
{
    public class ActivityCancellationBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var eventId = historyEvent.GetInitialEventId();
            var state = GetStatus(eventId, history);

            var item = new WorkflowItem
            {
                EventId = historyEvent.EventId,
                Kind = WorkflowItemKind.ActivityCancellation,
                State = state,
                Result = state == WorkflowItemState.Completed ? history.GetActivityResult(eventId) : null,
                Reason = state == WorkflowItemState.Failed ? history.GetActivityReason(eventId) : null,
                Details =
                    state == WorkflowItemState.Failed ||
                    state == WorkflowItemState.Canceled ? history.GetActivityDetails(eventId) : null
            };

            if (state == WorkflowItemState.CancelRequested)
            {
                var attributes = historyEvent.ActivityTaskCancelRequestedEventAttributes;
                var decisionId = historyEvent.ActivityTaskCancelRequestedEventAttributes.DecisionTaskCompletedEventId;

                item.Order = decisionId;
                item.Id = attributes.ActivityId;
            }
            else if (state == WorkflowItemState.CancelFailed)
            {
                var attributes = historyEvent.RequestCancelActivityTaskFailedEventAttributes;
                var decisionId = historyEvent.RequestCancelActivityTaskFailedEventAttributes.DecisionTaskCompletedEventId;

                item.Order = decisionId;
                item.Id = attributes.ActivityId;
                item.Reason = attributes.Cause;
            }

            return item;
        }

        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            if (history.IsActivityCancellationRequested(eventId))
            {
                return WorkflowItemState.CancelRequested;
            }

            if (history.IsActivityCancellationRequestFailed(eventId))
            {
                return WorkflowItemState.RequestCancelFailed;
            }

            throw new InvalidOperationException("Cannot determine the activity cancellation status.");
        }
    }
}
