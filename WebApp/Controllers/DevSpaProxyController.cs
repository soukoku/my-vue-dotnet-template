#if DEBUG
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Debug-time only controller for proxying requests to vite's dev server during development.'
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DevSpaProxyController : ControllerBase
    {
        // port is set in vite.config.ts
        private static readonly HttpClient __proxyClient =
            new() { BaseAddress = new Uri("https://localhost:3000/template/") };

        /// <summary>
        /// Proxies requests to vite's dev server during dev time.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("@vite/{*vitePath}")]
        [Route("@id/{*idPath}")]
        [Route("src/{*srcPath}")]
        [Route("node_modules/{*nmPath}")]
        [Route("assets/{*assetPath}")]
        public async Task<IActionResult> SpaDevServerProxy(
            string? vitePath = null,
            string? idPath = null,
            string? srcPath = null,
            string? nmPath = null,
            string? assetPath = null)
        {
            var url = vitePath != null ? $"@vite/{vitePath}" :
                idPath != null ? $"@id/{idPath}" :
                srcPath != null ? $"src/{srcPath}" :
                nmPath != null ? $"node_modules/{nmPath}" :
                assetPath != null ? $"assets/{assetPath}" : "";

            return new HttpResponseMessageResult(await __proxyClient.GetAsync(url));
        }
    }

    class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage; // could add throw if null
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;


            if (_responseMessage == null)
            {
                var message = "Response message cannot be null";

                throw new InvalidOperationException(message);
            }

            using (_responseMessage)
            {
                response.StatusCode = (int)_responseMessage.StatusCode;

                var responseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
                if (responseFeature != null)
                {
                    responseFeature.ReasonPhrase = _responseMessage.ReasonPhrase;
                }

                var responseHeaders = _responseMessage.Headers;

                // Ignore the Transfer-Encoding header if it is just "chunked".
                // We let the host decide about whether the response should be chunked or not.
                if (responseHeaders.TransferEncodingChunked == true &&
                    responseHeaders.TransferEncoding.Count == 1)
                {
                    responseHeaders.TransferEncoding.Clear();
                }

                foreach (var header in responseHeaders)
                {
                    response.Headers.Append(header.Key, header.Value.ToArray());
                }

                if (_responseMessage.Content != null)
                {
                    var contentHeaders = _responseMessage.Content.Headers;

                    // Copy the response content headers only after ensuring they are complete.
                    // We ask for Content-Length first because HttpContent lazily computes this
                    // and only afterwards writes the value into the content headers.
                    var unused = contentHeaders.ContentLength;

                    foreach (var header in contentHeaders)
                    {
                        response.Headers.Append(header.Key, header.Value.ToArray());
                    }

                    await _responseMessage.Content.CopyToAsync(response.Body);
                }
            }
        }
    }
}
#endif