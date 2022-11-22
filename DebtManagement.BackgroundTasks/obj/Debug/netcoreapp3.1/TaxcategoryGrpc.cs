// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/taxcategory.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace ContractManagement.API.Protos {
  public static partial class TaxCategoryServiceGrpc
  {
    static readonly string __ServiceName = "ContractManagementApi.TaxCategoryServiceGrpc";

    static readonly grpc::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO> __Marshaller_ContractManagementApi_TaxCategoryListGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.RequestFilterGrpc> __Marshaller_ContractManagementApi_RequestFilterGrpc = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.RequestFilterGrpc.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO> __Marshaller_ContractManagementApi_TaxCategoryPageListGrpcDTO = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO.Parser.ParseFrom);

    static readonly grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO> __Method_GetAll = new grpc::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetAll",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_ContractManagementApi_TaxCategoryListGrpcDTO);

    static readonly grpc::Method<global::ContractManagement.API.Protos.RequestFilterGrpc, global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO> __Method_GetTaxCategories = new grpc::Method<global::ContractManagement.API.Protos.RequestFilterGrpc, global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetTaxCategories",
        __Marshaller_ContractManagementApi_RequestFilterGrpc,
        __Marshaller_ContractManagementApi_TaxCategoryPageListGrpcDTO);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::ContractManagement.API.Protos.TaxcategoryReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for TaxCategoryServiceGrpc</summary>
    public partial class TaxCategoryServiceGrpcClient : grpc::ClientBase<TaxCategoryServiceGrpcClient>
    {
      /// <summary>Creates a new client for TaxCategoryServiceGrpc</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public TaxCategoryServiceGrpcClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for TaxCategoryServiceGrpc that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public TaxCategoryServiceGrpcClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected TaxCategoryServiceGrpcClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected TaxCategoryServiceGrpcClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO GetAll(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAll(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO GetAll(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetAll, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO> GetAllAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAllAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.TaxCategoryListGrpcDTO> GetAllAsync(global::Google.Protobuf.WellKnownTypes.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetAll, null, options, request);
      }
      public virtual global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO GetTaxCategories(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetTaxCategories(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO GetTaxCategories(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetTaxCategories, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO> GetTaxCategoriesAsync(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetTaxCategoriesAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::ContractManagement.API.Protos.TaxCategoryPageListGrpcDTO> GetTaxCategoriesAsync(global::ContractManagement.API.Protos.RequestFilterGrpc request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetTaxCategories, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override TaxCategoryServiceGrpcClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new TaxCategoryServiceGrpcClient(configuration);
      }
    }

  }
}
#endregion
