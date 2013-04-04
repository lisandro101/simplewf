using Amazon;
using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using Simple.Core.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core
{
    public class WorkflowReader
    {
        #region Constructors

        public WorkflowReader()
            : this(AWSClientFactory.CreateAmazonSimpleWorkflowClient())
        {
        }

        public WorkflowReader(AmazonSimpleWorkflow workflowClient)
        {
            WorkflowClient = workflowClient;

            WorkflowBuilder = new WorkflowBuilder();
            ChildWorkflowBuilder = new ChildWorkflowBuilder();
            ActivityBuilder = new ActivityBuilder();
            SignalBuilder = new SignalBuilder();
            TimerBuilder = new TimerBuilder();
            MarkerBuilder = new MarkerBuilder();
            ExternalSignalingBuilder = new ExternalSignalingBuilder();
            ExternalCancellationBuilder = new ExternalCancellationBuilder();
            ActivityCancellationBuilder = new ActivityCancellationBuilder();
        }

        #endregion

        #region Properties

        public AmazonSimpleWorkflow WorkflowClient { get; set; }

        public WorkflowBuilder WorkflowBuilder { get; set; }

        public ChildWorkflowBuilder ChildWorkflowBuilder { get; set; }

        public ActivityBuilder ActivityBuilder { get; set; }

        public SignalBuilder SignalBuilder { get; set; }

        public TimerBuilder TimerBuilder { get; set; }

        public MarkerBuilder MarkerBuilder { get; set; }

        public ExternalSignalingBuilder ExternalSignalingBuilder { get; set; }

        public ExternalCancellationBuilder ExternalCancellationBuilder { get; set; }

        public ActivityCancellationBuilder ActivityCancellationBuilder { get; set; }

        #endregion

        public WorkflowItem GetState(string domain, string workflowId)
        {
            var executions = WorkflowClient.ListAllWorkflowExecutions(domain, workflowId);

            if (executions.Count() < 1)
            {
                throw new InvalidOperationException("There is no workflow execution");
            }
            
            return GetState(domain, workflowId, executions.First().Execution.RunId);
        }

        public WorkflowItem GetState(string domain, string workflowId, string runId)
        {
            var history = WorkflowClient.GetWorkflowExecutionHistory(domain, workflowId, runId);
            return BuildWorkflowHierarchy(domain ,history);
        }

        #region Build items by kind

        public WorkflowItem BuildWorkflowHierarchy(string domain, IEnumerable<HistoryEvent> history)
        {
            // Group related events (that represents an item), and filter out
            // all decision events
            var mainEventGroups = 
                from item in history
                where !item.IsDecisionTask()
                group item by item.GetInitialEventId() into eventGroup
                select eventGroup;

            // The first item is the workflow itself
            var workflow = WorkflowBuilder.BuildItem(mainEventGroups.First().First(), mainEventGroups.First());
            
            var children = new List<WorkflowItem>();
            workflow.Events = children;

            // Skip the workflow item and iterate over the rest
            foreach (var mainEventGroup in mainEventGroups.Skip(1))
            {
                var firstEvent = mainEventGroup.First();

                if (firstEvent.IsActivityTask())
                {
                    children.Add(ActivityBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if (firstEvent.IsChildWorkflow())
                {
                    // Get the ChildWorkflowExecutionStarted event, from where
                    // we can extract the workflowId and runId and then call
                    // GetState to retrieve the child workflow state.
                    var execution = mainEventGroup.Skip(1).First();
                    var workflowId = execution.ChildWorkflowExecutionStartedEventAttributes.WorkflowExecution.WorkflowId;
                    var runId = execution.ChildWorkflowExecutionStartedEventAttributes.WorkflowExecution.RunId;

                    var childWorkflow = GetState(domain, workflowId, runId);

                    childWorkflow.Order = firstEvent.EventId;
                    childWorkflow.EventId = firstEvent.GetInitialEventId();
                    childWorkflow.Kind = WorkflowItemKind.ChildWorkflow;
                    children.Add(childWorkflow);
                    continue;
                }

                if (firstEvent.IsSignal())
                {
                    children.Add(SignalBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if (firstEvent.IsMarker())
                {
                    children.Add(MarkerBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if(firstEvent.IsTimer())
                {
                    children.Add(TimerBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if (firstEvent.IsExternalSignaling())
                {
                    children.Add(ExternalSignalingBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if (firstEvent.IsExternalCancellation())
                {
                    children.Add(ExternalCancellationBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }

                if (firstEvent.IsActivityCancellation())
                {
                    children.Add(ActivityCancellationBuilder.BuildItem(firstEvent, mainEventGroup));
                    continue;
                }
            }

            return workflow;
        }

        #endregion
    }
}
