using System;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Grpc.Clients
{
    public class GrpcCaller
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        private readonly ILogger<GrpcCaller> _logger;

        protected readonly string GrpcServerUrl;

        public GrpcCaller(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger, string grpcServerUrl)
        {
            _wrappedConfig = wrappedConfig;
            _logger = logger;
            GrpcServerUrl = grpcServerUrl;
        }

        public async Task<TResponse> CallAsync<TResponse>(Func<GrpcChannel, Task<TResponse>> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            //var httpClientHandler = new HttpClientHandler();
            //// Return `true` to allow certificates that are untrusted/invalid
            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress(GrpcServerUrl);

            _logger.LogInformation("Creating GRPC client base address ={@grpcUrl}, BaseAddress={@BaseAddress}", GrpcServerUrl, channel.Target);

            try
            {
                return await func(channel);
            }
            catch (RpcException e)
            {
                _logger.LogError("Error calling via GRPC: {Status} - {Message}", e.Status, e.Message);
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}
