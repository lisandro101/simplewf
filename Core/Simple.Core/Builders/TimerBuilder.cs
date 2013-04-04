using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core.Builders
{
    public class TimerBuilder : IWorkflowItemBuilder
    {
        public WorkflowItem BuildItem(HistoryEvent historyEvent, IEnumerable<HistoryEvent> history)
        {
            var attributes = historyEvent.TimerStartedEventAttributes;
            var eventId = historyEvent.GetInitialEventId();
            var state = GetStatus(eventId, history);

            var decisionId = historyEvent.EventId;

            return new WorkflowItem
            {
                EventId = historyEvent.EventId,
                Name = attributes.TimerId,
                Input = string.Empty,
                Order = decisionId,
                Kind = WorkflowItemKind.Timer,
                State = state
            };
        }

        /// <summary>
        /// Determines the timer status browsing the related life-cycle event entries.
        /// </summary>
        public WorkflowItemState GetStatus(long eventId, IEnumerable<HistoryEvent> history)
        {
            if (history.IsTimerCanceled(eventId))
            {
                return WorkflowItemState.Canceled;
            }

            if (history.IsTimerStartFailed(eventId))
            {
                return WorkflowItemState.StartFailed;
            }

            if (history.IsTimerFired(eventId))
            {
                return WorkflowItemState.Completed;
            }

            if (history.IsTimerCancelFailed(eventId))
            {
                return WorkflowItemState.CancelFailed;
            }

            if (history.IsTimerStarted(eventId))
            {
                return WorkflowItemState.Started;
            }

            throw new InvalidOperationException("Cannot determine the workflow status.");
        }
    }
}
