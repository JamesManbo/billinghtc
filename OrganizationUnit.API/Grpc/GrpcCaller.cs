using System;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace OrganizationUnit.API.Grpc
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

        public async Task<TResponse> Call<TResponse>(Func<GrpcChannel, Task<object>> func)
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
                var result = await func(channel);

                if (result.GetType() == typeof(TResponse)) return (TResponse) result;

                return result.MapTo<TResponse>(_wrappedConfig.MapperConfig);
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

        public async Task Call(Func<GrpcChannel, Task> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            /*
            using var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            */

            var channel = GrpcChannel.ForAddress(GrpcServerUrl);

            _logger.LogDebug("Creating grpc client base address {@httpClient.BaseAddress} ", channel.Target);

            try
            {
                await func(channel);
            }
            catch (RpcException e)
            {
                _logger.LogError("Error calling via grpc: {Status} - {Message}", e.Status, e.Message);
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}
