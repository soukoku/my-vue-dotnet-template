using Soukoku.AspNetCore.ViteIntegration;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<ViteBuildManifest>();
            builder.Services.AddAntiforgery(op =>
            {
                // enable this if need to be hosted in external iframes
                //op.SuppressXFrameOptionsHeader = true;

                op.HeaderName = "X-CSRF-TOKEN";
                op.FormFieldName = "csrf-token";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // TODO: take this out if in ssl-terminating load balancer
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
#pragma warning disable ASP0014 // using endpoints is required so controller route match overrides spa during dev
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
#pragma warning restore ASP0014 // Suggest using top level route registrations

                app.UseSpa(spa =>
                {
                    // keep this port in sync with what's in ClientUI/vite.config.ts
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:3000");
                });
            }
            else
            {
                app.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");

            }

            app.Run();
        }
    }
}
