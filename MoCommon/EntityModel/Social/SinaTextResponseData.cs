using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.EntityModel.Social
{
    public class SinaTextResponseData
    {
        public string Created_At { get; set; }
        public string Id { get; set; }
        public string Mid { get; set; }
        public string IdStr { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }


    }

    public class SinaTextErrorData
    {
        public string Request { get; set; }
        public string Error_Code { get; set; }
        public string Error { get; set; }
    }
}
