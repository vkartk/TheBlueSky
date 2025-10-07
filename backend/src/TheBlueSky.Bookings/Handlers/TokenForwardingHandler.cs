using System.Net.Http;
using System.Net.Http.Headers;

namespace TheBlueSky.Bookings.Handlers
{
    public class TokenForwardingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenForwardingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = _httpContextAccessor.HttpContext?.Request.Headers.Authorization;

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);



            return await base.SendAsync(request, cancellationToken);
        }

    }
}
