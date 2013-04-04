using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core.Builders
{
    public class ChildWorkflowBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var eventId = historyEvent.GetInitialEventId();
            var attributes = historyEvent.StartChildWorkflowExecutionInitiatedEventAttributes;
            var state = GetStatus(eventId, history);

            var decisionId = historyEvent.StartChildWorkflowExecutionInitiatedEventAttributes.DecisionTaskCompletedEventId;

            return new WorkflowItem
            {
                Id = attributes.WorkflowId,
                EventId = historyEvent.EventId,
                Name = attributes.WorkflowType.Name,
                Input = attributes.Input,
                Kind = WorkflowItemKind.ChildWorkflow,
                Version = attributes.WorkflowType.Version,
                Events = new List<WorkflowItem>(),
                Order = decisionId,
                State = state,
                Result = state == WorkflowItemState.Completed ? history.GetChildWorkflowResult(eventId) : null,
                Reason = state == WorkflowItemState.Failed ? history.GetChildWorkflowReason(eventId) : null,
                Details =
                    state == WorkflowItemState.Failed ||
                    state == WorkflowItemState.Canceled ? history.GetChildWorkflowDetails(eventId) : null
            };
        }

        /// <summary>
        /// Determines the child status state browsing the related life-cycle event entries.
        /// </summary>
        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            if (history.IsChildWorkflowCompleted(eventId))
            {
                return WorkflowItemState.Completed;
            }

            if (history.IsChildWorkflowFailed(eventId))
            {
                return WorkflowItemState.Failed;
            }

            if (history.IsChildWorkflowCanceled(eventId))
            {
                return WorkflowItemState.Canceled;
            }

            if (history.IsChildWorkflowTerminated(eventId))
            {
                return WorkflowItemState.Terminated;
            }

            if (history.IsChildWorkflowTimedOut(eventId))
            {
                return WorkflowItemState.TimedOut;
            }

            if (history.IsChildWorkflowStarted(eventId))
            {
                return WorkflowItemState.Started;
            }

            if (history.IsChildWorkflowScheduleFailed(eventId))
            {
                return WorkflowItemState.ScheduledFailed;
            }

            if (history.IsChildWorkflowInitiated(eventId))
            {
                return WorkflowItemState.Scheduled;
            }

            throw new InvalidOperationException("Cannot determine the child workflow status.");
        }
    }
}
