using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Core
{
    public class WorkflowItem
    {
        public long EventId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public WorkflowItemKind Kind { get; set; }

        public WorkflowItemState State { get; set; }

        public string Input { get; set; }

        public string Result { get; set; }

        public string Details { get; set; }

        public string Reason { get; set; }

        public IEnumerable<WorkflowItem> Events { get; set; }

        public long Order { get; set; }
    }
}
