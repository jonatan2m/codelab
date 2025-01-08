using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web3_1.MapperExamples
{
    //TODO AutoMapper ADO.NET - How does it work?
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }        
        public int EventBriteCode { get; set; }
    }

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Event, EventViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.EventId))
                .ReverseMap();
        }
    }
}
