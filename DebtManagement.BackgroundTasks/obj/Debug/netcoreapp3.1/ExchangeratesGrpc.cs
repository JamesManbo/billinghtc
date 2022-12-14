// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: exchangerates.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace ContractManagement.API.Protos {
  public static partial class ExchangeRateGrpc
  {
    static readonly string __ServiceName = "ContractManagementApi.ExchangeRateGrpc";

    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.RequestExchangeRateGrpc> __Marshaller_ContractManagementApi_RequestExchangeRateGrpc = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.RequestExchangeRateGrpc.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.ExchangeRateGrpcResult> __Marshaller_ContractManagementApi_ExchangeRateGrpcResult = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.ExchangeRateGrpcResult.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO> __Marshaller_ContractManagementApi_ListExchangeRateGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.BoolValue> __Marshaller_google_protobuf_BoolValue = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.BoolValue.Parser.ParseFrom);

    static readonly grpc::Method<global::ContractManagement.API.Protos.RequestExchangeRateGrpc, global::ContractManagement.API.Protos.ExchangeRateGrpcResult> __Method_ExchangeRate = new grpc::Method<global::ContractManagement.API.Protos.RequestExchangeRateGrpc, global::ContractManagement.API.Protos.ExchangeRateGrpcResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ExchangeRate",
        __Marshaller_ContractManagementApi_RequestExchangeRateGrpc,
        __Marshaller_ContractManagementApi_ExchangeRateGrpcResult);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO> __Method_GetExchangeRates = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetExchangeRates",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ContractManagementApi_ListExchangeRateGrpcDTO);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.BoolValue> __Method_Synchronize = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.BoolValue>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Synchronize",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_BoolValue);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ContractManagement.API.Protos.ExchangeratesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for ExchangeRateGrpc</summary>
    public partial class ExchangeRateGrpcClient : grpc::ClientBase<ExchangeRateGrpcClient>
    {
      /// <summary>Creates a new client for ExchangeRateGrpc</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ExchangeRateGrpcClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ExchangeRateGrpc that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ExchangeRateGrpcClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ExchangeRateGrpcClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ExchangeRateGrpcClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::ContractManagement.API.Protos.ExchangeRateGrpcResult ExchangeRate(global::ContractManagement.API.Protos.RequestExchangeRateGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ExchangeRate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.ExchangeRateGrpcResult ExchangeRate(global::ContractManagement.API.Protos.RequestExchangeRateGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ExchangeRate, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ExchangeRateGrpcResult> ExchangeRateAsync(global::ContractManagement.API.Protos.RequestExchangeRateGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ExchangeRateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ExchangeRateGrpcResult> ExchangeRateAsync(global::ContractManagement.API.Protos.RequestExchangeRateGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ExchangeRate, null, options, request);
      }
      public virtual global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO GetExchangeRates(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetExchangeRates(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO GetExchangeRates(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetExchangeRates, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO> GetExchangeRatesAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetExchangeRatesAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ListExchangeRateGrpcDTO> GetExchangeRatesAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetExchangeRates, null, options, request);
      }
      public virtual global::Google.Protobuf.WellKnownTypes.BoolValue Synchronize(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Synchronize(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Google.Protobuf.WellKnownTypes.BoolValue Synchronize(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Synchronize, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Google.Protobuf.WellKnownTypes.BoolValue> SynchronizeAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SynchronizeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Google.Protobuf.WellKnownTypes.BoolValue> SynchronizeAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Synchronize, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ExchangeRateGrpcClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ExchangeRateGrpcClient(configuration);
      }
    }

  }
}
#endregion
