using AutoMapper;

namespace YH.Etms.Settlement.Api.Infrastructure.Configuration.AutoMapper
{
    /// <summary>
    /// 领域事件映射配置
    /// </summary>
    public class SettlementAggregateEventMapperProfile : Profile
    {
        public SettlementAggregateEventMapperProfile()
        {
            //CreateMap<CreateContactFromLoadUnloadCommand, CreateOrUpdateContactCommand>();
        }
    }
}
