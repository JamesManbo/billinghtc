using AutoMapper;
using Feedback.API.Infrastructure.Helpers;
using Feedback.API.Models;
using Feedback.API.Protos;
using Global.Models.PagedList;
using System.Collections.Generic;

namespace Feedback.API.Infrastructure.MapperConfigs
{
    public class FeedbackAndRequestMapperConfigs : Profile
    {
        public FeedbackAndRequestMapperConfigs()
        {
            CreateMap<FeedbackAndRequest, FeedbackAndRequestDTOGrpc>().ReverseMap();
            CreateMap<FeedbackAndRequest, CreateFeedbackAndRequestGrpc>().ReverseMap();
            CreateMap<FeedbackAndRequestFilterModel, FeedbackAndRequestFilterGrpc>().ReverseMap();
            CreateMap<IPagedList<FeedbackAndRequest>, FeedbackAndRequestPageListGrpcDTO>()
                .ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

            CreateMap<IEnumerable<FeedbackAndRequest>, FeedbackAndRequestAllGrpc>()
                .ForMember(s => s.Result, m => m.MapFrom(d => d));

            CreateMap<FeedbackAndRequest, HTCTicketTransferModel>()
                .ForMember(s => s.code, m => m.MapFrom(d => d.RequestCode))
                .ForMember(s => s.guid, m => m.MapFrom(d => d.GlobalId))
                .ForMember(s => s.tinh_su_co, m => m.MapFrom(d => d.City.ToAscii()))
                .ForMember(s => s.cid, m => m.MapFrom(d => d.CId))
                .ForMember(s => s.thoi_gian_bat_dau, 
                    m => m.MapFrom(d => d.StartTime.HasValue ? d.StartTime.Value.ToString("dd/MM/yyyy HH:mm") : d.DateRequested.ToString("dd/MM/yyyy HH:mm")))
                .ForMember(s => s.thoi_gian_ket_thuc,
                    m => m.MapFrom(d => d.StopTime.HasValue ? d.StopTime.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty))
                .ForMember(s => s.nguyen_nhan_so_bo, m => m.MapFrom(d => d.Content))
                .ForMember(s => s.noi_dung_kn, m => m.MapFrom(d => d.Content))
                .ForMember(s => s.so_hop_dong, m => m.MapFrom(d => d.ContractCode))
                .ForMember(s => s.ten_khach_hang, m => m.MapFrom(d => d.CustomerName))
                .ForMember(s => s.ma_kh, m => m.MapFrom(d => d.CustomerCode))
                .ForMember(s => s.sdt, m => m.MapFrom(d => d.CustomerPhone))
                .ForMember(s => s.note, m => m.MapFrom(d => d.Note))
                .ForMember(s => s.status, m => m.MapFrom(d => d.Status))
                .ForMember(s => s.danh_muc, m => m.MapFrom(d => "ftth"));
        }
    }
}
