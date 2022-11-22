using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.CommonModels;
using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class CommonMapperConfigs : Profile
    {
        public CommonMapperConfigs()
        {
            CreateMap<RequestFilterModel, RequestFilterGrpc>().ReverseMap();
            CreateMap<PictureDTOGrpc, PictureViewModel>().ReverseMap();

            CreateMap<MoneyDTO, decimal>().ConvertUsing(s => s == null ? 0m : Convert.ToDecimal(s.Value));
            CreateMap<decimal, MoneyDTO>().ConvertUsing(s => new MoneyDTO()
            {
                Value = s.ToString(CultureInfo.InvariantCulture),
                //FormatValue = s == 0 ? "0" : string.Format(CultureInfo.CreateSpecificCulture("es-ES"),"{0:#,#}", s),
                FormatValue = s == 0 ? "0" : string.Format("{0:#,#.##}", s),
                CurrencyCode = "VND"
            });
        }
    }
}
