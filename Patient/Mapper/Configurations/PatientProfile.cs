using AutoMapper;
using Patient.Api.Models;
using Patient.Data.Models;

namespace Patient.Api.Mapper.Configurations
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Data.Models.Patient, Data.Models.Patient>();

            CreateMap<PatientDto, Data.Models.Patient>()
                .ForMember(dest => dest.Use, opt =>
                    {
                        opt.PreCondition(src => src.Name is not null);
                        opt.MapFrom(src => src.Name.Use);
                    })
                .ForMember(dest => dest.GivenNames, opt =>
                    {
                        opt.PreCondition(src => src.Name != null && src.Name.Given != null);
                        opt.MapFrom(src => src.Name.Given.Select(x => new PatientGivenName
                        {
                            PatientId = src.Name != null && src.Name.Id.HasValue ? src.Name.Id.Value : default,
                            GivenName = x,
                        }));
                    })
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.PreCondition(src => src.Name is not null);
                    opt.MapFrom(src => src.Name.Id);
                })
                .ForMember(dest => dest.Family, opt =>
                {
                    opt.PreCondition(src => src.Name is not null);
                    opt.MapFrom(src => src.Name.Family);
                });

            CreateMap<Data.Models.Patient, PatientDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new PatientName
                {
                    Id = src.Id,
                    Family = src.Family,
                    Use = src.Use,   
                    Given = src.GivenNames != null ? src.GivenNames.Select(x => x.GivenName) : Enumerable.Empty<string>()
                }));
        }
    }
}
