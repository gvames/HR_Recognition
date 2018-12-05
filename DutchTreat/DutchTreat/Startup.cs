using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DutchTreat
{
    public class Startup 
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // AddIdentity folosete ca suport pt autenticare - cooki-uri-le
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
               //  cfg.Password.RequireDigit // an example of how to configure Password
            })
            .AddEntityFrameworkStores<DutchContext>();

            //Autentificare pentru APIs bazata pe token-uri si cooki-uri
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = config["Tokens:Issuer"],
                        ValidAudience = config["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]))
                    };
                }
                );


            services.AddDbContext<DutchContext>(cfg=>
            cfg.UseSqlServer(config.GetConnectionString("DutchConnectionString")));

            services.AddAutoMapper();

            services.AddTransient<DutchSeeder>();

           // services.AddScoped<ILogger>();

            services.AddScoped<IDutchRepository, DutchRepository>();
            
            
            // Add Support for real mail service
            services.AddTransient<IMailService, NullMailService > ();


          //  services.AddDefaultIdentity<StoreUser>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt=>opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) // mediul se citeste din variabila ASPNETCORE_ENVIRONMENT care se seteaza in proprietatile proiectului tab-ul Debug
            {
                app.UseDeveloperExceptionPage(); //se arunca eroarea detaliata in browser
            }
            else
            {
                app.UseExceptionHandler("/error"); // In caz de eroare in aplicatie, utilizatorul va fi redirectat catre pagina Razor <Error.cshtml>
            }
           

            // app.UseDefaultFiles(); // se configureaza apps sa deserveasca fisiere default    

            app.UseStaticFiles(); //am configurat aplicatia sa deserveasca fisiere. Fisierele deservite trebuie puse in folderul wwwroot

            app.UseNodeModules(env); //trebuie adaugat pachetul NuGet OdeToCode.UseNodeModules

            app.UseAuthentication();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default","/{controller}/{action}/{id?}", new { controller = "App",Action = "Index"});
            });    //se adauga functionalitatea MVC  si se configureaza ruta

            //app.Run( async (context) =>
            //{
            //    await  context.Response.WriteAsync("<html><body><h1>Hello PluralSight</h1></body></html>");
            //});
        }
    }
}
