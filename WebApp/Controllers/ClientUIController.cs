using Microsoft.AspNetCore.Mvc;
using Soukoku.AspNetCore.ViteIntegration;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for serving ClientUI. Each method should be for an entry page
    /// in ClientUI as different area of the app.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClientUIController : Controller
    {
        private readonly ILogger<ClientUIController> _logger;

        public ClientUIController(ILogger<ClientUIController> logger)
        {
            _logger = logger;
        }

        // park the entry pages under this controller so it's easier to find them.
        // you might have a landing page area, actual app area, admin area, docs, etc.

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


        [HttpGet("admin/{**slug}")]
        public IActionResult Admin(string? slug = null)
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/entry-pages/admin.ts",
                UseAntiforgery = true,
                PageData = new
                {
                    Message = $"This initial message is from aspnet with slug {slug}"
                }
            };
            return View("VuePage", model);
        }


        [HttpGet("docs/{**slug}")]
        public IActionResult Docs(string? slug = null)
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/entry-pages/docs.ts",
                UseAntiforgery = true,
                PageData = new
                {
                }
            };
            return View("VuePage", model);
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new VitePageMvcModel
            {
                Entry = "src/entry-pages/error.ts",
                UseAntiforgery = true,
                PageData = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
            };
            return View("VuePage", model);
        }
    }
}
