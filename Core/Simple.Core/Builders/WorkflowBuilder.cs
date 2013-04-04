using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using Simple.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simple.Core.Builders
{
    public class WorkflowBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var eventId = historyEvent.GetInitialEventId();
            var attributes = historyEvent.WorkflowExecutionStartedEventAttributes;
            var state = GetStatus(eventId, history);

            var decisionId = 1;

            return new WorkflowItem
            {
                EventId = historyEvent.EventId,
                Name = attributes.WorkflowType.Name,
                Input = attributes.Input,
                Kind = WorkflowItemKind.Workflow,
                Version = attributes.WorkflowType.Version,
                Order = decisionId,
                State = state,
                Result = state == WorkflowItemState.Completed ? history.GetWorkflowResult(eventId) : null,
                Reason = state == WorkflowItemState.Failed ? history.GetWorkflowReason(eventId) : null,
                Details =
                    state == WorkflowItemState.Failed ||
                    state == WorkflowItemState.Canceled ? history.GetWorkflowDetails(eventId) : null
            };
        }

        /// <summary>
        /// Determines the workflow status browsing the related life-cycle event entries.
        /// </summary>
        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            if (history.IsWorkflowCompleted(eventId))
            {
                return WorkflowItemState.Completed;
            }

            if (history.IsWorkflowFailed(eventId))
            {
                return WorkflowItemState.Failed;
            }

            if (history.IsWorkflowCanceled(eventId))
            {
                return WorkflowItemState.Canceled;
            }

            if (history.IsWorkflowTerminated(eventId))
            {
                return WorkflowItemState.Terminated;
            }

            if (history.IsWorkflowStarted(eventId))
            {
                return WorkflowItemState.Started;
            }

            throw new InvalidOperationException("Cannot determine the workflow status.");
        }
    }
}