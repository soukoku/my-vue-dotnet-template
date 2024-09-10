using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                      .AddJsonOptions(op =>
                      {
                          op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                      });
            builder.Services.AddViteManifest("https://localhost:3000/template/");
            builder.Services.AddAntiforgery(op =>
            {
                // enable this if need to be hosted in external iframes
                //op.SuppressXFrameOptionsHeader = true;

                op.HeaderName = "X-CSRF-TOKEN";
                op.FormFieldName = "csrf-token";
            });

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/svg+xml"]);
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                var xmlDocFile = Path.Combine(AppContext.BaseDirectory, $"{nameof(WebApp)}.xml");
                if (File.Exists(xmlDocFile))
                {
                    op.IncludeXmlComments(xmlDocFile);
                }
            });

            var app = builder.Build();
            app.UsePathBase("/template"); // TODO: remove if running in iis site root (and remove from vite.config.ts)

            app.UseResponseCompression();
            app.UseSwagger();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts(); // TODO: add if requires https
            }

            //app.UseHttpsRedirection(); // TODO: add if not in ssl-terminating load balancer and requires https
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
