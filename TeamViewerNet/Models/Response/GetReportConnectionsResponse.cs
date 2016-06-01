using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamViewerNet.Models.Response
{
    public class GetReportConnectionsResponse : BaseResponseModel
    {
        public List<Objects.ReportRecord> records { get; set; }
        public int records_remaining { get; set; }
        public string next_offset { get; set; }
    }
}
