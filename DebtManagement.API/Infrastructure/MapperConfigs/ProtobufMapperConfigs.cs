using AutoMapper;
using DebtManagement.API.Protos;
using ContractManagement.API.Protos;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Global.Models.StateChangedResponse;
using Global.Models;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class ProtobufMapperConfigs : Profile
    {
        public ProtobufMapperConfigs()
        {
            CreateMap<int, Int32Value>().ConvertUsing(s => new Int32Value() { Value = s });
            CreateMap<Int32Value, int>().ConvertUsing(s => s.Value);

            CreateMap<long, Int64Value>().ConvertUsing(s => new Int64Value() { Value = s });
            CreateMap<Int64Value, long>().ConvertUsing(s => s.Value);

            CreateMap<ContractManagement.API.Protos.Money, decimal>().ConvertUsing(s => Convert.ToDecimal(s.Value));
            CreateMap<decimal, ContractManagement.API.Protos.Money>().ConvertUsing(s => new ContractManagement.API.Protos.Money()
            {
                Value = s.ToString(),
                FormatValue = s == 0 ? "0" : string.Format(CultureInfo.CreateSpecificCulture("es-ES"),"{0:#,#}", s),
                CurrencyCode = "VNĐ"
            });

            CreateMap<Protos.Money, decimal>().ConvertUsing(s => s == null ? 0m : Convert.ToDecimal(s.Value));
            CreateMap<decimal, Protos.Money>().ConvertUsing(s => new Protos.Money()
            {
                Value = s.ToString(CultureInfo.InvariantCulture),
                FormatValue = s == 0 ? "0" : string.Format(CultureInfo.CreateSpecificCulture("es-ES"),"{0:#,#}", s),
                CurrencyCode = "VNĐ"
            });

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
            //CreateMap(typeof(RepeatedField<>), typeof(List<>)).ReverseMap();

            CreateMap<ErrorGrpc, ErrorGeneric>();
            CreateMap<ActionResponseGrpc, ActionResponse>();
        }
    }
}
