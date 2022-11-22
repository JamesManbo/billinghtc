using AttchmentFile.StaticResource.API.Proto;
using AutoMapper;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Commands.Commons;
using ContractManagement.Domain.Models;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class AttachmentFileMapperConfigs : Profile
    {
        public AttachmentFileMapperConfigs()
        {
            CreateMap<AttachmentFile, AttachmentFileDTO>().ReverseMap();
            CreateMap<CreateUpdateFileCommand, AttachmentFile> ().ReverseMap();
            CreateMap<CreateUpdateFileCommand, AttachmentFileGrpcDto> ().ReverseMap();
        }
    }
}