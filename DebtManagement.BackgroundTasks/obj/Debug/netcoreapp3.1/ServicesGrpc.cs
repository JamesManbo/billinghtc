// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/services.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace ContractManagement.API.Protos {
  public static partial class ServiceGrpc
  {
    static readonly string __ServiceName = "ContractManagementApi.ServiceGrpc";

    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.RequestFilterGrpc> __Marshaller_ContractManagementApi_RequestFilterGrpc = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.RequestFilterGrpc.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.ServicePageListGrpcDTO> __Marshaller_ContractManagementApi_ServicePageListGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.ServicePageListGrpcDTO.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.ServiceListGrpcDTO> __Marshaller_ContractManagementApi_ServiceListGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.ServiceListGrpcDTO.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Int32Value> __Marshaller_google_protobuf_Int32Value = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Int32Value.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.ServiceGrpcDTO> __Marshaller_ContractManagementApi_ServiceGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.ServiceGrpcDTO.Parser.ParseFrom);

    static readonly grpc::Method<global::ContractManagement.API.Protos.RequestFilterGrpc, global::ContractManagement.API.Protos.ServicePageListGrpcDTO> __Method_GetServices = new grpc::Method<global::ContractManagement.API.Protos.RequestFilterGrpc, global::ContractManagement.API.Protos.ServicePageListGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetServices",
        __Marshaller_ContractManagementApi_RequestFilterGrpc,
        __Marshaller_ContractManagementApi_ServicePageListGrpcDTO);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.ServiceListGrpcDTO> __Method_GetAllService = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.ServiceListGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAllService",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ContractManagementApi_ServiceListGrpcDTO);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::ContractManagement.API.Protos.ServiceGrpcDTO> __Method_GetDetail = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::ContractManagement.API.Protos.ServiceGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetDetail",
        __Marshaller_google_protobuf_Int32Value,
        __Marshaller_ContractManagementApi_ServiceGrpcDTO);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ContractManagement.API.Protos.ServicesReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for ServiceGrpc</summary>
    public partial class ServiceGrpcClient : grpc::ClientBase<ServiceGrpcClient>
    {
      /// <summary>Creates a new client for ServiceGrpc</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public ServiceGrpcClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ServiceGrpc that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public ServiceGrpcClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected ServiceGrpcClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected ServiceGrpcClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::ContractManagement.API.Protos.ServicePageListGrpcDTO GetServices(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetServices(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.ServicePageListGrpcDTO GetServices(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetServices, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServicePageListGrpcDTO> GetServicesAsync(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetServicesAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServicePageListGrpcDTO> GetServicesAsync(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetServices, null, options, request);
      }
      public virtual global::ContractManagement.API.Protos.ServiceListGrpcDTO GetAllService(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAllService(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.ServiceListGrpcDTO GetAllService(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetAllService, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServiceListGrpcDTO> GetAllServiceAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAllServiceAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServiceListGrpcDTO> GetAllServiceAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetAllService, null, options, request);
      }
      public virtual global::ContractManagement.API.Protos.ServiceGrpcDTO GetDetail(global::Google.Protobuf.WellKnownTypes.Int32Value request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDetail(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.ServiceGrpcDTO GetDetail(global::Google.Protobuf.WellKnownTypes.Int32Value request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetDetail, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServiceGrpcDTO> GetDetailAsync(global::Google.Protobuf.WellKnownTypes.Int32Value request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDetailAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.ServiceGrpcDTO> GetDetailAsync(global::Google.Protobuf.WellKnownTypes.Int32Value request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetDetail, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override ServiceGrpcClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ServiceGrpcClient(configuration);
      }
    }

  }
}
#endregion