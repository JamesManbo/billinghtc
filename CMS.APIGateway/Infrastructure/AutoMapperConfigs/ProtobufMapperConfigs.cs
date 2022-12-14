using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace CMS.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ProtobufMapperConfigs : Profile
    {
        private const long BclSecondsAtUnixEpoch = 62135596800;
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

            CreateMap<Guid, string>().ConvertUsing(s => s.ToString());
            CreateMap<string, Guid>().ConvertUsing(s => string.IsNullOrEmpty(s) ? Guid.Empty : new Guid(s));

        }
    }
}
