using System;
using AutoMapper;

namespace YH.Etms.Settlement.Api.Infrastructure.Configuration.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static void CreatMap(this Profile profile, Type source,Type destination) {
            profile.CreateMap(source,destination);
            profile.CreateMap(destination, source); 
        }
    }
}
