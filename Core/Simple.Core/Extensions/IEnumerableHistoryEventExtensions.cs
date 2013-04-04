using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.SimpleWorkflow
{
    public static class IEnumerableHistoryEventExtensions
    {
        #region Timer events

        public static bool IsTimerStarted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsTimerStarted()
                select e).Any();
        }

        public static bool IsTimerFired(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsTimerFired()
                select e).Any();
        }

        public static bool IsTimerCanceled(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsTimerCanceled()
                select e).Any();
        }

        public static bool IsTimerCancelFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsTimerCancelFailed()
                select e).Any();
        }

        public static bool IsTimerStartFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsTimerStartFailed()
                select e).Any();
        }

        #endregion

        #region Activity events

        public static bool IsActivityTaskCompleted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskCompleted()
                select e).Any();
        }

        public static bool IsActivityTaskFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskFailed()
                select e).Any();
        }

        public static bool IsActivityTaskCanceled(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskCanceled()
                select e).Any();
        }

        public static bool IsActivityTaskTimedOut(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskTimedOut()
                select e).Any();
        }

        public static bool IsActivityTaskScheduleFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskScheduleFailed()
                select e).Any();
        }

        public static bool IsActivityTaskStarted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskStarted()
                select e).Any();
        }

        public static bool IsActivityTaskScheduled(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskScheduled()
                select e).Any();
        }

        #endregion

        #region Child workflow events

        public static bool IsChildWorkflowCompleted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowCompleted()
                select e).Any();
        }

        public static bool IsChildWorkflowFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowFailed()
                select e).Any();
        }

        public static bool IsChildWorkflowCanceled(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowCanceled()
                select e).Any();
        }

        public static bool IsChildWorkflowTimedOut(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowTimedOut()
                select e).Any();
        }

        public static bool IsChildWorkflowStarted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowStarted()
                select e).Any();
        }

        public static bool IsChildWorkflowInitiated(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowInitiated()
                select e).Any();
        }

        public static bool IsChildWorkflowTerminated(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowTerminated()
                select e).Any();
        }

        public static bool IsChildWorkflowScheduleFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowScheduleFailed()
                select e).Any();
        }

        #endregion

        #region Workflow events

        public static bool IsWorkflowCompleted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowCompleted()
                select e).Any();
        }

        public static bool IsWorkflowFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowFailed()
                select e).Any();
        }

        public static bool IsWorkflowCanceled(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowCanceled()
                select e).Any();
        }

        public static bool IsWorkflowStarted(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowStarted()
                select e).Any();
        }

        public static bool IsWorkflowTerminated(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowTerminated()
                select e).Any();
        }

        #endregion

        #region Activity cancellation events

        public static bool IsActivityCancellationRequested(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityCancellationRequested()
                select e).Any();
        }

        public static bool IsActivityCancellationRequestFailed(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityCancellationRequestFailed()
                select e).Any();
        }

        #endregion

        public static string GetActivityResult(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskCompleted()
                select e.ActivityTaskCompletedEventAttributes.Result).FirstOrDefault();
        }

        public static string GetActivityDetails(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            var historyEvent = (
                from e in history
                where e.GetInitialEventId() == initialEventId && (e.IsActivityTaskCanceled() || e.IsActivityTaskFailed())
                select e).FirstOrDefault();

            if (historyEvent.IsActivityTaskCanceled())
            {
                return historyEvent.ActivityTaskCanceledEventAttributes.Details;
            }
            else
            {
                return historyEvent.ActivityTaskFailedEventAttributes.Details;
            }
        }

        public static string GetActivityReason(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsActivityTaskFailed()
                select e.ActivityTaskFailedEventAttributes.Reason).FirstOrDefault();
        }

        public static string GetChildWorkflowResult(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowCompleted()
                select e.ChildWorkflowExecutionCompletedEventAttributes.Result).FirstOrDefault();
        }

        public static string GetChildWorkflowDetails(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            var historyEvent = (
                from e in history
                where e.GetInitialEventId() == initialEventId && (e.IsChildWorkflowCanceled() || e.IsChildWorkflowFailed())
                select e).FirstOrDefault();

            if (historyEvent.IsChildWorkflowCanceled())
            {
                return historyEvent.ChildWorkflowExecutionCanceledEventAttributes.Details;
            }
            else
            {
                return historyEvent.ChildWorkflowExecutionFailedEventAttributes.Details;
            }
        }

        public static string GetChildWorkflowReason(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsChildWorkflowFailed()
                select e.ChildWorkflowExecutionFailedEventAttributes.Reason).FirstOrDefault();
        }

        public static string GetWorkflowResult(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowCompleted()
                select e.WorkflowExecutionCompletedEventAttributes.Result).FirstOrDefault();
        }

        public static string GetWorkflowDetails(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            var historyEvent = (
                from e in history
                where e.GetInitialEventId() == initialEventId && (e.IsWorkflowCanceled() || e.IsWorkflowFailed())
                select e).FirstOrDefault();

            if (historyEvent.IsWorkflowCanceled())
            {
                return historyEvent.WorkflowExecutionCanceledEventAttributes.Details;
            }
            else
            {
                return historyEvent.WorkflowExecutionFailedEventAttributes.Details;
            }
        }

        public static string GetWorkflowReason(this IEnumerable<HistoryEvent> history, long initialEventId)
        {
            return (
                from e in history
                where e.GetInitialEventId() == initialEventId && e.IsWorkflowFailed()
                select e.WorkflowExecutionFailedEventAttributes.Reason).FirstOrDefault();
        }
    }
}
