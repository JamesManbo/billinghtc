using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CMS.APIGateway.Infrastructure.DelegateHandlers
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private const string ACCESS_TOKEN = "access_token";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<HttpClientAuthorizationDelegatingHandler> _logger;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor, ILogger<HttpClientAuthorizationDelegatingHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Version = new System.Version(2, 0);
            request.Method = HttpMethod.Get;

            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>{ authorizationHeader });
            }

            var token = await GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetToken()
        {
            return await _httpContextAccessor.HttpContext.GetTokenAsync(ACCESS_TOKEN);
        }
    }
}
