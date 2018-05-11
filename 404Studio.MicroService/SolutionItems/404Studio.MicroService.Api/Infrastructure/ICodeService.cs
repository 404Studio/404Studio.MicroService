using YH.Etms.Settlement.Api.Domain.Dtos.Code;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    public interface ICodeService
    {
        string GetTmsOrderCode(CodeRequestDto dto);
        string GetTmsAuditCode(AuditCodeRequestDto dto);
        string GetTmsTransportCode(CodeRequestDto dto);
    }
}
