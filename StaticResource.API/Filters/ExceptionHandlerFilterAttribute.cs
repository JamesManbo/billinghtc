using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace StaticResource.API.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        public ExceptionHandlerFilterAttribute(
            IHostingEnvironment hostingEnvironment,
            ILogger logger
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            //            if (!_hostingEnvironment.IsDevelopment())
            //            {
            //                return;
            //            }

            _logger.Log(LogLevel.Error, context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}
