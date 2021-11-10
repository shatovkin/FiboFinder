using FiboFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiboFinder
{
    public class ToolInfo
    {
        public string ClassCode { get; set; }
        public string SecCode { get; set; }
        public string TimeFrame { get; set; }
        public decimal PreisPlane { get; set; }
        public string Quantity { get; set; }
        public string Direction { get; set; }
        public string Variant { get; set; }
        public string ToolName { get; set; }
        public string ClientCode { get; set; }
        public string FirmId { get; set; }
    }
}
