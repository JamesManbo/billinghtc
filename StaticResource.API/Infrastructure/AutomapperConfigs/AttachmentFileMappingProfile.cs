using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttchmentFile.StaticResource.API.Proto;
using AutoMapper;
using Picture.StaticResource.API.Proto;
using StaticResource.API.Models;

namespace StaticResource.API.Infrastructure.AutomapperConfigs
{
    public class AttachmentFileMappingProfile : Profile
    {
        public AttachmentFileMappingProfile()
        {
            CreateMap<FileAttachmentModel, AttachmentFileGrpcDto>().ReverseMap();
        }
    }
}
