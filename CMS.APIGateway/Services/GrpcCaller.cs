using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace CMS.APIGateway.Services
{
    public class GrpcCaller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcCaller> _logger;

        protected readonly string GrpcServerUrl;

        public GrpcCaller(IMapper mapper, ILogger<GrpcCaller> logger, string grpcServerUrl)
        {
            _mapper = mapper;
            _logger = logger;
            GrpcServerUrl = grpcServerUrl;
        }

        public async Task<TResponse> Call<TResponse>(Func<GrpcChannel, Task<object>> func)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var httpClientHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress(GrpcServerUrl,
                new GrpcChannelOptions { HttpClient = httpClient });

            _logger.LogInformation("Creating GRPC client base address ={@grpcUrl}, BaseAddress={@BaseAddress}", GrpcServerUrl, channel.Target);

            try
            {
                var result = await func(channel);

                if (result.GetType() == typeof(TResponse)) return (TResponse) result;

                return _mapper.Map<TResponse>(result);
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
