using AutoMapper;
using DebtManagement.API.Protos;
using Global.Models;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ProtobufMapperConfigs : Profile
    {
        public ProtobufMapperConfigs()
        {
            CreateMap<int, Int32Value>().ConvertUsing(s => new Int32Value() { Value = s });
            CreateMap<Int32Value, int>().ConvertUsing(s => s.Value);

            CreateMap<long, Int64Value>().ConvertUsing(s => new Int64Value() { Value = s });
            CreateMap<Int64Value, long>().ConvertUsing(s => s.Value);

            CreateMap<string, StringValue>().ConvertUsing(s => new StringValue() { Value = s });
            CreateMap<StringValue, string>().ConvertUsing(s => s.Value);

            CreateMap<bool, BoolValue>().ConvertUsing(s => new BoolValue() { Value = s });
            CreateMap<BoolValue, bool>().ConvertUsing(s => s.Value);

            CreateMap<double, DoubleValue>().ConvertUsing(s => new DoubleValue() { Value = s });
            CreateMap<DoubleValue, double>().ConvertUsing(s => s.Value);

            CreateMap<float, FloatValue>().ConvertUsing(s => new FloatValue() { Value = s });
            CreateMap<FloatValue, float>().ConvertUsing(s => s.Value);

            CreateMap<Timestamp, DateTime>().ConvertUsing(s => s.ToDateTime());
            CreateMap<DateTime, Timestamp>().ConvertUsing(s => s.ToUniversalTime().ToTimestamp());

            CreateMap<ErrorGrpc, ErrorGeneric>();
            CreateMap<ActionResponseGrpc, ActionResponse>();

            CreateMap<Guid, string>().ConvertUsing(s => s.ToString());
            CreateMap<string, Guid>().ConvertUsing(s => string.IsNullOrEmpty(s) ? Guid.Empty : new Guid(s));

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
