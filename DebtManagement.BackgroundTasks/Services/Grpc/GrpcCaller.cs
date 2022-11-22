using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace DebtManagement.BackgroundTasks.Services.Grpc
{
    public class GrpcCaller
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<GrpcCaller> _logger;

        protected readonly string GrpcServerUrl;

        public GrpcCaller(IMapper mapper, ILogger<GrpcCaller> logger, string grpcServerUrl)
        {
            _mapper = mapper;
            _logger = logger;
            GrpcServerUrl = grpcServerUrl;
        }

        public async Task<TResponse> CallAsync<TResponse>(Func<GrpcChannel, Task<TResponse>> func)
        {
            try
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                // Return `true` to allow certificates that are untrusted/invalid
                var httpClient = new HttpClient(httpClientHandler);
                var channel = GrpcChannel.ForAddress(GrpcServerUrl,
                    new GrpcChannelOptions { 
                        HttpClient = httpClient,
                        MaxReceiveMessageSize = 25 * 1024 * 1024
                    });

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
