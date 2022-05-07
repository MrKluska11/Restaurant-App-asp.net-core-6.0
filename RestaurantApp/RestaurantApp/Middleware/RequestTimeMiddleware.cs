using System.Diagnostics;

namespace RestaurantApp.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopWatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            int seconds = _stopWatch.Elapsed.Seconds;
            string? path = context.Request.Path.Value;
            string? method = context.Request.Method;

            if (seconds > 4)
            {
                _logger.LogInformation($"Request: {method} at {path} took {seconds} seconds.");
            }
           
        }
    }
}
