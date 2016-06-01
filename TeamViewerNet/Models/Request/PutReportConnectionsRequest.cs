using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Request
{
    public class PutReportConnectionsRequest : BaseRequestModel
    {
        public string rID { get; set; }
        public string billing_state { get; set; }
        public string notes { get; set; }

        public PutReportConnectionsRequest(string rID)
        {
            rID = rID;
        }
    }
}
