using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Core.Builders
{
    public class ActivityBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var eventId = historyEvent.GetInitialEventId();
            var attributes = historyEvent.ActivityTaskScheduledEventAttributes;
            var state = GetStatus(eventId, history);
            var decisionId = historyEvent.ActivityTaskScheduledEventAttributes.DecisionTaskCompletedEventId;

            return new WorkflowItem
            {
                Id = attributes.ActivityId,
                EventId = historyEvent.EventId,
                Name = attributes.ActivityType.Name,
                Input = attributes.Input,
                Kind = WorkflowItemKind.Activity,
                Version = attributes.ActivityType.Version,
                Order = decisionId,
                State = state,
                Result = state == WorkflowItemState.Completed ? history.GetActivityResult(eventId) : null,
                Reason = state == WorkflowItemState.Failed ? history.GetActivityReason(eventId) : null,
                Details =
                    state == WorkflowItemState.Failed ||
                    state == WorkflowItemState.Canceled ? history.GetActivityDetails(eventId) : null
            };
        }

        /// <summary>
        /// Determines the activity status browsing the related life-cycle event entries.
        /// </summary>
        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            if (history.IsActivityTaskCompleted(eventId))
            {
                return WorkflowItemState.Completed;
            }

            if (history.IsActivityTaskFailed(eventId))
            {
                return WorkflowItemState.Failed;
            }

            if (history.IsActivityTaskCanceled(eventId))
            {
                return WorkflowItemState.Canceled;
            }

            if (history.IsActivityTaskTimedOut(eventId))
            {
                return WorkflowItemState.TimedOut;
            }

            if (history.IsActivityTaskScheduleFailed(eventId))
            {
                return WorkflowItemState.ScheduledFailed;
            }

            if (history.IsActivityTaskStarted(eventId))
            {
                return WorkflowItemState.Started;
            }

            if (history.IsActivityTaskScheduled(eventId))
            {
                return WorkflowItemState.Scheduled;
            }

            throw new InvalidOperationException("Cannot determine the activity status.");
        }
    }
}