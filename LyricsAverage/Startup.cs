using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LyricsAverage.Configuration;
using LyricsAverage.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LyricsAverage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient<ISongRetriever, MusicBrainzSongRetriever>(c =>
            {
                c.BaseAddress = new Uri("http://musicbrainz.org/ws/2/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "Lyrics-Average (antonf26@gmail.com)");
            });

            services.AddHttpClient<ILyricsRetriever, OvhLyricsRetriever>(c =>
            {
                c.BaseAddress = new Uri("https://api.lyrics.ovh/v1/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddTransient<ILyricsCounter, LyricsCounter>();

            services.Configure<LyricsAverageConfig>(Configuration.GetSection("LyricsAverage"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
