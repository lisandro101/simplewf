using Amazon;
using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Simple.Web.Controllers
{
    public class WorkflowsController : ApiController
    {
        #region Constructors

        public WorkflowsController()
            : this(AWSClientFactory.CreateAmazonSimpleWorkflowClient())
        {
        }

        public WorkflowsController(AmazonSimpleWorkflow workflowClient)
        {
            WorkflowClient = workflowClient;
        }

        #endregion

        #region Properties

        public AmazonSimpleWorkflow WorkflowClient { get; set; }

        #endregion

        public IEnumerable<WorkflowExecutionInfo> Get(string domain, string workflowId = null)
        {
            return WorkflowClient.ListAllWorkflowExecutions(domain, workflowId);
        }
    }
}
