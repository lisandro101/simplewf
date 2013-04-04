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
    public class DomainsController : ApiController
    {
        #region Constructors

        public DomainsController()
            : this(AWSClientFactory.CreateAmazonSimpleWorkflowClient())
        {
        }

        public DomainsController(AmazonSimpleWorkflow workflowClient)
        {
            WorkflowClient = workflowClient;
        }

        #endregion

        #region Properties

        public AmazonSimpleWorkflow WorkflowClient { get; set; }

        #endregion

        public IEnumerable<DomainInfo> Get()
        {
            return WorkflowClient.ListAllRegisteredDomains();
        }
    }
}
