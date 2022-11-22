using AttchmentFile.StaticResource.API.Proto;
using AutoMapper;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Commands.Commons;
using DebtManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class AttachmentFileMapperConfigs : Profile
    {
        public AttachmentFileMapperConfigs()
        {
            CreateMap<AttachmentFile, AttachmentFileDTO>().ReverseMap();
            CreateMap<AttachmentFile, AttachmentFileCommand>().ReverseMap();
            CreateMap<AttachmentFileCommand, AttachmentFileGrpcDto>().ReverseMap();
        }
    }
}
