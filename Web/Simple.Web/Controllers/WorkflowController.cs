using Amazon;
using Amazon.SimpleWorkflow;
using Amazon.SimpleWorkflow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Simple.Web.Controllers
{
    public class WorkflowController : ApiController
    {
        #region Constructors

        public WorkflowController()
            : this(AWSClientFactory.CreateAmazonSimpleWorkflowClient())
        {
        }

        public WorkflowController(AmazonSimpleWorkflow workflowClient)
        {
            WorkflowClient = workflowClient;
        }

        #endregion

        #region Properties

        public AmazonSimpleWorkflow WorkflowClient { get; set; }

        #endregion

        public WorkflowExecutionDetail Get(string domain, string workflowId, string runId)
        {
            runId = runId.FromSafe64();
            return WorkflowClient.DescribeWorkflowExecution(domain, workflowId, runId);
        }
    }
}
