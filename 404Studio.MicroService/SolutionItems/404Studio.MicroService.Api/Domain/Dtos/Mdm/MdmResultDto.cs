using System;

namespace YH.Etms.Settlement.Api.Domain.Dtos.Mdm
{
    public class MdmResultDto
    {
        public bool Successful { get; set; }
        public Exception Exception { get; set; }
        public object Data { get; set; }
    }
}
