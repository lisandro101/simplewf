using Simple.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Simple.Web.Controllers
{
    public class FlowController : ApiController
    {
        #region Constructors

        public FlowController()
            : this(new WorkflowReader())
        {
        }

        public FlowController(WorkflowReader reader)
        {
            Reader = reader;
        }

        #endregion

        #region Properties

        public WorkflowReader Reader { get; set; }

        #endregion

        public WorkflowItem Get(string domain, string workflowId, string runId, long lastEventId)
        {
            runId = runId.FromSafe64();
            return Reader.GetState(domain, workflowId, runId);
        }
    }
}
