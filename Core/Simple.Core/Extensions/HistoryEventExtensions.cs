using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.SimpleWorkflow
{
    public static class HistoryEventExtensions
    {
        public static long GetInitialEventId(this HistoryEvent e)
        {
            // Workflow events
            if (e.WorkflowExecutionCanceledEventAttributes != null) return 1;
            if (e.WorkflowExecutionCompletedEventAttributes != null) return 1;
            if (e.WorkflowExecutionFailedEventAttributes != null) return 1;
            if (e.WorkflowExecutionStartedEventAttributes != null) return 1;
            if (e.WorkflowExecutionTerminatedEventAttributes != null) return 1;
            if (e.WorkflowExecutionTimedOutEventAttributes != null) return 1;
            if (e.WorkflowExecutionContinuedAsNewEventAttributes != null) return 1;
            if (e.WorkflowExecutionCancelRequestedEventAttributes != null) return 1;
       
            // Decision events
            if (e.DecisionTaskScheduledEventAttributes != null) return e.EventId;
            if (e.DecisionTaskStartedEventAttributes != null) return e.DecisionTaskStartedEventAttributes.ScheduledEventId;
            if (e.DecisionTaskCompletedEventAttributes != null) return e.DecisionTaskCompletedEventAttributes.ScheduledEventId;
            if (e.DecisionTaskTimedOutEventAttributes != null) return e.DecisionTaskTimedOutEventAttributes.ScheduledEventId;

            // Activity events
            if (e.ActivityTaskCanceledEventAttributes != null) return e.ActivityTaskCanceledEventAttributes.ScheduledEventId;
            if (e.ActivityTaskCompletedEventAttributes != null) return e.ActivityTaskCompletedEventAttributes.ScheduledEventId;
            if (e.ActivityTaskFailedEventAttributes != null) return e.ActivityTaskFailedEventAttributes.ScheduledEventId;
            if (e.ActivityTaskScheduledEventAttributes != null) return e.EventId;
            if (e.ActivityTaskStartedEventAttributes != null) return e.ActivityTaskStartedEventAttributes.ScheduledEventId;
            if (e.ActivityTaskTimedOutEventAttributes != null) return e.ActivityTaskTimedOutEventAttributes.ScheduledEventId;
            if (e.ScheduleActivityTaskFailedEventAttributes != null) return e.EventId;

            // Activity cancellation event
            if (e.ActivityTaskCancelRequestedEventAttributes != null) return e.EventId;
            if (e.RequestCancelActivityTaskFailedEventAttributes != null) return e.EventId;

            // Signal events
            if (e.WorkflowExecutionSignaledEventAttributes != null) return e.EventId;

            // Marker events
            if (e.MarkerRecordedEventAttributes != null) return e.EventId;
            if (e.RecordMarkerFailedEventAttributes != null) return e.EventId;

            // Timer events
            if (e.TimerCanceledEventAttributes != null) return e.TimerCanceledEventAttributes.StartedEventId;
            if (e.TimerFiredEventAttributes != null) return e.TimerFiredEventAttributes.StartedEventId;
            if (e.TimerStartedEventAttributes != null) return e.EventId;
            if (e.StartTimerFailedEventAttributes != null) return e.EventId;
            if (e.CancelTimerFailedEventAttributes != null) return e.EventId;

            // Child workflow events
            if (e.ChildWorkflowExecutionCanceledEventAttributes != null) return e.ChildWorkflowExecutionCanceledEventAttributes.InitiatedEventId;
            if (e.ChildWorkflowExecutionCompletedEventAttributes != null) return e.ChildWorkflowExecutionCompletedEventAttributes.InitiatedEventId;
            if (e.ChildWorkflowExecutionFailedEventAttributes != null) return e.ChildWorkflowExecutionFailedEventAttributes.InitiatedEventId;
            if (e.ChildWorkflowExecutionStartedEventAttributes != null) return e.ChildWorkflowExecutionStartedEventAttributes.InitiatedEventId;
            if (e.ChildWorkflowExecutionTerminatedEventAttributes != null) return e.ChildWorkflowExecutionTerminatedEventAttributes.InitiatedEventId;
            if (e.ChildWorkflowExecutionTimedOutEventAttributes != null) return e.ChildWorkflowExecutionTimedOutEventAttributes.InitiatedEventId;
            if (e.StartChildWorkflowExecutionInitiatedEventAttributes != null) return e.EventId;
            if (e.StartChildWorkflowExecutionFailedEventAttributes != null) return e.StartChildWorkflowExecutionFailedEventAttributes.InitiatedEventId;

            // External signaling events
            if (e.SignalExternalWorkflowExecutionInitiatedEventAttributes != null) return e.ChildWorkflowExecutionTimedOutEventAttributes.InitiatedEventId;
            if (e.ExternalWorkflowExecutionSignaledEventAttributes != null) return e.EventId;
            if (e.SignalExternalWorkflowExecutionFailedEventAttributes != null) return e.StartChildWorkflowExecutionFailedEventAttributes.InitiatedEventId;

            // External cancellation events
            if (e.RequestCancelExternalWorkflowExecutionInitiatedEventAttributes != null) return e.ChildWorkflowExecutionTimedOutEventAttributes.InitiatedEventId;
            if (e.ExternalWorkflowExecutionCancelRequestedEventAttributes != null) return e.EventId;
            if (e.RequestCancelExternalWorkflowExecutionFailedEventAttributes != null) return e.StartChildWorkflowExecutionFailedEventAttributes.InitiatedEventId;
    
            // Default to the event itself
            return e.EventId;
        }

        #region Child workflow events

        public static bool IsChildWorkflow(this HistoryEvent e)
        {
            return IsChildWorkflowCompleted(e) ||
                   IsChildWorkflowCanceled(e) ||
                   IsChildWorkflowTerminated(e) ||
                   IsChildWorkflowFailed(e) ||
                   IsChildWorkflowInitiated(e) ||
                   IsChildWorkflowTimedOut(e) ||
                   IsChildWorkflowStarted(e);
        }

        public static bool IsChildWorkflowCompleted(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionCompletedEventAttributes != null;
        }

        public static bool IsChildWorkflowCanceled(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionCanceledEventAttributes != null;
        }

        public static bool IsChildWorkflowTerminated(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionTerminatedEventAttributes != null;
        }

        public static bool IsChildWorkflowFailed(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionFailedEventAttributes != null;
        }

        public static bool IsChildWorkflowInitiated(this HistoryEvent e)
        {
            return e.StartChildWorkflowExecutionInitiatedEventAttributes != null;
        }

        public static bool IsChildWorkflowTimedOut(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionTimedOutEventAttributes != null;
        }

        public static bool IsChildWorkflowStarted(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionStartedEventAttributes != null;
        }

        public static bool IsChildWorkflowScheduleFailed(this HistoryEvent e)
        {
            return e.StartChildWorkflowExecutionFailedEventAttributes != null;
        }

        #endregion

        #region Workflow events

        public static bool IsWorkflow(this HistoryEvent e)
        {
            return IsWorkflowStarted(e) ||
                   IsWorkflowCompleted(e) ||
                   IsWorkflowCanceled(e) ||
                   IsWorkflowTerminated(e) ||
                   IsWorkflowFailed(e) ||
                   IsWorkflowTimedOut(e) ||
                   IsWorkflowContinuedAsNew(e) ||
                   IsWorkflowCancelRequested(e);
        }

        public static bool IsWorkflowStarted(this HistoryEvent e)
        {
            return e.WorkflowExecutionStartedEventAttributes != null;
        }

        public static bool IsWorkflowCompleted(this HistoryEvent e)
        {
            return e.WorkflowExecutionCompletedEventAttributes != null;
        }

        public static bool IsWorkflowCanceled(this HistoryEvent e)
        {
            return e.WorkflowExecutionCanceledEventAttributes != null;
        }

        public static bool IsWorkflowTerminated(this HistoryEvent e)
        {
            return e.WorkflowExecutionTerminatedEventAttributes != null;
        }

        public static bool IsWorkflowFailed(this HistoryEvent e)
        {
            return e.ChildWorkflowExecutionFailedEventAttributes != null;
        }

        public static bool IsWorkflowTimedOut(this HistoryEvent e)
        {
            return e.WorkflowExecutionTimedOutEventAttributes != null;
        }

        public static bool IsWorkflowContinuedAsNew(this HistoryEvent e)
        {
            return e.WorkflowExecutionContinuedAsNewEventAttributes != null;
        }

        public static bool IsWorkflowContinuedAsNewFailed(this HistoryEvent e)
        {
            return e.ContinueAsNewWorkflowExecutionFailedEventAttributes != null;
        }

        public static bool IsWorkflowCancelRequested(this HistoryEvent e)
        {
            return e.WorkflowExecutionCancelRequestedEventAttributes != null;
        }

        #endregion

        #region Activity events

        public static bool IsActivityTask(this HistoryEvent e)
        {
            return e.ActivityTaskCanceledEventAttributes != null ||
                   e.ActivityTaskCompletedEventAttributes != null ||
                   e.ActivityTaskFailedEventAttributes != null ||
                   e.ActivityTaskScheduledEventAttributes != null ||
                   e.ActivityTaskStartedEventAttributes != null ||
                   e.ActivityTaskTimedOutEventAttributes != null;
        }

        public static bool IsActivityTaskScheduled(this HistoryEvent e)
        {
            return e.ActivityTaskScheduledEventAttributes != null;
        }

        public static bool IsActivityTaskCompleted(this HistoryEvent e)
        {
            return e.ActivityTaskCompletedEventAttributes != null;
        }

        public static bool IsActivityTaskFailed(this HistoryEvent e)
        {
            return e.ActivityTaskFailedEventAttributes != null;
        }

        public static bool IsActivityTaskCanceled(this HistoryEvent e)
        {
            return e.ActivityTaskCanceledEventAttributes != null;
        }

        public static bool IsActivityTaskTimedOut(this HistoryEvent e)
        {
            return e.ActivityTaskTimedOutEventAttributes != null;
        }

        public static bool IsActivityTaskScheduleFailed(this HistoryEvent e)
        {
            return e.ScheduleActivityTaskFailedEventAttributes != null;
        }

        public static bool IsActivityTaskStarted(this HistoryEvent e)
        {
            return e.ActivityTaskStartedEventAttributes != null;
        }

        #endregion

        #region Decision events

        public static bool IsDecisionTask(this HistoryEvent e)
        {
            return e.DecisionTaskCompletedEventAttributes != null ||
                   e.DecisionTaskScheduledEventAttributes != null ||
                   e.DecisionTaskStartedEventAttributes != null ||
                   e.DecisionTaskTimedOutEventAttributes != null;
        }

        #endregion
        
        #region Signal events

        public static bool IsSignal(this HistoryEvent e)
        {
            return e.WorkflowExecutionSignaledEventAttributes != null;
        }

        #endregion

        #region Marker events

        public static bool IsMarker(this HistoryEvent e)
        {
            return e.MarkerRecordedEventAttributes != null ||
                   e.RecordMarkerFailedEventAttributes != null;
        }

        #endregion

        #region Timer events

        public static bool IsTimer(this HistoryEvent e)
        {
            return e.TimerStartedEventAttributes != null ||
                   e.TimerFiredEventAttributes != null ||
                   e.TimerCanceledEventAttributes != null ||
                   e.StartTimerFailedEventAttributes != null ||
                   e.CancelTimerFailedEventAttributes != null;
        }

        public static bool IsTimerStarted(this HistoryEvent e)
        {
            return e.TimerStartedEventAttributes != null;
        }

        public static bool IsTimerFired(this HistoryEvent e)
        {
            return e.TimerFiredEventAttributes != null;
        }

        public static bool IsTimerCanceled(this HistoryEvent e)
        {
            return e.TimerCanceledEventAttributes != null;
        }

        public static bool IsTimerStartFailed(this HistoryEvent e)
        {
            return e.StartTimerFailedEventAttributes != null;
        }

        public static bool IsTimerCancelFailed(this HistoryEvent e)
        {
            return e.CancelTimerFailedEventAttributes != null;
        }

        #endregion

        #region External Cancellation

        public static bool IsExternalCancellation(this HistoryEvent e)
        {
            return IsExternalCancellationInitiated(e) ||
                   IsExternalCancellationRequested(e) ||
                   IsExternalCancellationFailed(e);
        }

        public static bool IsExternalCancellationInitiated(this HistoryEvent e)
        {
            return e.RequestCancelExternalWorkflowExecutionInitiatedEventAttributes != null;
        }

        public static bool IsExternalCancellationRequested(this HistoryEvent e)
        {
            return e.ExternalWorkflowExecutionCancelRequestedEventAttributes != null;
        }

        public static bool IsExternalCancellationFailed(this HistoryEvent e)
        {
            return e.RequestCancelExternalWorkflowExecutionFailedEventAttributes != null;
        }

        #endregion

        #region External Signaling

        public static bool IsExternalSignaling(this HistoryEvent e)
        {
            return IsExternalSignalingSignaled(e) ||
                   IsExternalSignalingFailed(e) ||
                   IsExternalSignalingInitiated(e);
        }

        public static bool IsExternalSignalingSignaled(this HistoryEvent e)
        {
            return e.ExternalWorkflowExecutionSignaledEventAttributes != null;
        }

        public static bool IsExternalSignalingFailed(this HistoryEvent e)
        {
            return e.SignalExternalWorkflowExecutionFailedEventAttributes != null;
        }

        public static bool IsExternalSignalingInitiated(this HistoryEvent e)
        {
            return e.SignalExternalWorkflowExecutionInitiatedEventAttributes != null;
        }

        #endregion

        #region Activity Cancellation

        public static bool IsActivityCancellation(this HistoryEvent e)
        {
            return IsActivityCancellationRequested(e) ||
                   IsActivityCancellationRequestFailed(e);
        }

        public static bool IsActivityCancellationRequested(this HistoryEvent e)
        {
            return e.ActivityTaskCancelRequestedEventAttributes != null;
        }

        public static bool IsActivityCancellationRequestFailed(this HistoryEvent e)
        {
            return e.RequestCancelActivityTaskFailedEventAttributes != null;
        }

        #endregion
    }
}