using Amazon;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.SimpleWorkflow
{
    public static class AmazonSimpleWorkflowExtensions
    {
        public static IEnumerable<WorkflowExecutionInfo> ListAllWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId)
        {
            var oldestDate = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
            var latestDate = DateTime.Now.AddMinutes(2);

            return ListAllWorkflowExecutions(client, domain, workflowId, oldestDate, latestDate);
        }

        public static IEnumerable<WorkflowExecutionInfo> ListAllWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId, DateTime oldestDate, DateTime latestDate)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var openWorkflows = client.ListAllOpenWorkflowExecutions(domain, workflowId, oldestDate, latestDate);
            var closedWorkflows = client.ListAllClosedWorkflowExecutions(domain, workflowId, oldestDate, latestDate);

            return openWorkflows.Union(closedWorkflows);
        }

        public static IEnumerable<WorkflowExecutionInfo> ListAllOpenWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId)
        {
            var oldestDate = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
            var latestDate = DateTime.Now.AddMinutes(2);

            return client.ListAllOpenWorkflowExecutions(domain, workflowId, oldestDate, latestDate);
        }

        public static IEnumerable<WorkflowExecutionInfo> ListAllOpenWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId, DateTime oldestDate, DateTime latestDate)
        {
           var workflows = new List<WorkflowExecutionInfo>();

           var nextPageToken = string.Empty;
           var getNextPage = true;
           System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
           System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
           while (getNextPage)
           {
               var result = client.ListOpenWorkflowExecutions(
                   new ListOpenWorkflowExecutionsRequest
                   {
                       Domain = domain,
                       StartTimeFilter = new ExecutionTimeFilter
                       {
                           OldestDate = oldestDate,
                           LatestDate = latestDate
                       },
                       ExecutionFilter = workflowId != null 
                        ? new WorkflowExecutionFilter
                           {
                               WorkflowId = workflowId
                           } 
                        : default(WorkflowExecutionFilter),
                       NextPageToken = string.IsNullOrWhiteSpace(nextPageToken) ? null : nextPageToken
                   });

               workflows.AddRange(result.ListOpenWorkflowExecutionsResult.WorkflowExecutionInfos.ExecutionInfos);

               nextPageToken = result.ListOpenWorkflowExecutionsResult.WorkflowExecutionInfos.NextPageToken;
               getNextPage = !String.IsNullOrWhiteSpace(nextPageToken);
           }

            return workflows;
        }

        public static IEnumerable<WorkflowExecutionInfo> ListAllClosedWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId)
        {
            var oldestDate = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
            var latestDate = DateTime.Now.AddMinutes(2);

            return client.ListAllClosedWorkflowExecutions(domain, workflowId, oldestDate, latestDate);
        }

        public static IEnumerable<WorkflowExecutionInfo> ListAllClosedWorkflowExecutions(this AmazonSimpleWorkflow client, string domain, string workflowId, DateTime oldestDate, DateTime latestDate)
        {
            var workflows = new List<WorkflowExecutionInfo>();

            var nextPageToken = string.Empty;
            var getNextPage = true;
            while (getNextPage)
            {
                var result = client.ListClosedWorkflowExecutions(
                    new ListClosedWorkflowExecutionsRequest
                    {
                        Domain = domain,
                        StartTimeFilter = new ExecutionTimeFilter
                        {
                            OldestDate = oldestDate,
                            LatestDate = latestDate
                        },
                        ExecutionFilter = workflowId != null
                         ? new WorkflowExecutionFilter
                         {
                             WorkflowId = workflowId
                         }
                         : default(WorkflowExecutionFilter),
                        NextPageToken = string.IsNullOrWhiteSpace(nextPageToken) ? null : nextPageToken
                    });

                workflows.AddRange(result.ListClosedWorkflowExecutionsResult.WorkflowExecutionInfos.ExecutionInfos);

                nextPageToken = result.ListClosedWorkflowExecutionsResult.WorkflowExecutionInfos.NextPageToken;
                getNextPage = !String.IsNullOrWhiteSpace(nextPageToken);
            }

            return workflows;
        }

        public static WorkflowExecutionDetail DescribeWorkflowExecution(this AmazonSimpleWorkflow client, string domain, string workflowId, string runId)
        {
            var response = client.DescribeWorkflowExecution(
                new DescribeWorkflowExecutionRequest
                {
                    Domain = domain,
                    Execution = new WorkflowExecution
                    {
                        WorkflowId = workflowId,
                        RunId = runId
                    }
                });

            return response.DescribeWorkflowExecutionResult.WorkflowExecutionDetail;
        }

        public static IEnumerable<HistoryEvent> GetWorkflowExecutionHistory(this AmazonSimpleWorkflow client, string domain, string workflowId, string runId)
        {
            var events = new List<HistoryEvent>();

            var nextPageToken = string.Empty;
            var getNextPage = true;
            while (getNextPage)
            {
                var result = client.GetWorkflowExecutionHistory(
                    new GetWorkflowExecutionHistoryRequest
                    {
                        Domain = domain,
                        Execution = new WorkflowExecution { RunId = runId, WorkflowId = workflowId },
                        NextPageToken = string.IsNullOrWhiteSpace(nextPageToken) ? null : nextPageToken
                    });

                events.AddRange(result.GetWorkflowExecutionHistoryResult.History.Events);

                nextPageToken = result.GetWorkflowExecutionHistoryResult.History.NextPageToken;
                getNextPage = !string.IsNullOrWhiteSpace(nextPageToken);
            }

            return events;
        }

        public static IEnumerable<DomainInfo> ListAllRegisteredDomains(this AmazonSimpleWorkflow client)
        {
            var domains = new List<DomainInfo>();

            var nextPageToken = string.Empty;
            var getNextPage = true;
            while (getNextPage)
            {
                var result = client.ListDomains(
                    new ListDomainsRequest
                    {
                        RegistrationStatus = "REGISTERED",
                        NextPageToken = string.IsNullOrWhiteSpace(nextPageToken) ? null : nextPageToken
                    });

                domains.AddRange(result.ListDomainsResult.DomainInfos.Name);

                nextPageToken = result.ListDomainsResult.DomainInfos.NextPageToken;
                getNextPage = !String.IsNullOrWhiteSpace(nextPageToken);
            }

            return domains;
        } 
    }
}