using AutoMapper;
using YH.Etms.Settlement.Api.Application.Commands;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init.Dto;
using YH.Etms.Settlement.Api.Application.Commands.TransportTasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Dtos;
using YH.Etms.Settlement.Api.Domain.Events;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.IntegrationEvents.Events.TransportTask.Create;

namespace YH.Etms.Settlement.Api.Infrastructure.Configuration.AutoMapper
{
    public class SettlementAutoMapperProfile : Profile
    {
        public SettlementAutoMapperProfile()
        {
            CreateMap<TransportTaskCreateDto, TransportTask>();
            CreateMap<InitPayableCommand, Payable>();
            CreateMap<TransportTaskCreateEvent, InitPayableCommand>();
            CreateMap<TransportTaskDto, CreateTransportTaskCommand>();
            CreateMap<CreateTransportTaskCommand,TransportTask>();
            CreateMap<PayableItem, SettlementPayableItem>();
            CreateMap<SettlementPayable, FinishPriceEvent>();

            //CreateMap<CreateBatchOrdersCommand, List<CreateOrderCommand>>();
            //this.CreatMap(typeof(UpdateVehicleDto), typeof(Vehicle));
            //this.CreatMap(typeof(AddDriverDto), typeof(Driver));
            //this.CreatMap(typeof(UpdateDriverDto), typeof(Driver));
            //this.CreatMap(typeof(UpdateVehicleDriverDto), typeof(VehicleDriver));
            //this.CreatMap(typeof(AddVehicleDriverDto), typeof(VehicleDriver));
        }
    }
}
