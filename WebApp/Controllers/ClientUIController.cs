using Microsoft.AspNetCore.Mvc;
using Soukoku.AspNetCore.ViteIntegration;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for serving ClientUI. Each method should be for an entry page
    /// in ClientUI.
    /// </summary>
    public class ClientUIController : Controller
    {
        private readonly ILogger<ClientUIController> _logger;

        public ClientUIController(ILogger<ClientUIController> logger)
        {
            _logger = logger;
        }

        // park the entry page under the same-name root so it's easier to find them.

        [HttpGet("home/{**slug}")]
        public IActionResult Home(string? slug = null)
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/entry-pages/home.ts",
                UseAntiforgery = true,
                PageData = new
                {
                    Message = $"This initial message is from aspnet with slug {slug}"
                }
            };
            return View("VuePage", model);
        }
    }
}
