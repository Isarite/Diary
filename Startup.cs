using DiaryApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VueCliMiddleware;

namespace DiaryApp
{
    //add
    // dotnet ef migrations add MyCommand1 -v
    //remove
    //dotnet ef migrations remove
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename = MyDatabase.db"));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename = MyDatabase.db"));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("test"));

            //string connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb").ToString();
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultProvider;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>().
                AddDefaultTokenProviders();
            services.TryAddScoped<RoleManager<IdentityRole>>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var SecretKey = Encoding.ASCII.GetBytes
            ("lyTfdO0pRUuvjCD7ICfDQ5LBnkcfWL2luGG_HUjvuwubg9_D7qwJCmAJ2Fo3i30hHS4unlSr0Dk8Unm0MEikvqNvJVEsrqQuLvrGRWWHUJts5zyZlp1WAxUOocuf5gWbRlrHfgsi09rZqRcdbtGnNkdQttKrZ26i0vdJuF6npw3lLCWvwi4FRiVkBZYzybyHQ5nLa5xy5xFpTCrubs-GEKN5ErJQr44sUy0JAg2A03OHImx9iKOcRF_02cNNLcMWCgeGu0jSVfi5JonP1fw4bkjYsoNq-FnjJM2A-WgtyVeDlESft1HbfKtpNQKcHi6JUjQ1nnQ7lNAZ_c-blPB7BA");
            //Configure JWT Token Authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
            //Same Secret key will be used while creating the token
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = true,
            //Usually, this is your application base URL
                    ValidIssuer = "http://localhost:5000/",
                    ValidateAudience = true,
            //Here, we are creating and using JWT within the same application.
            //In this case, base URL is fine.
            //If the JWT is created using a web service, then this would be the consumer URL.
                    ValidAudience = "http://localhost:5000/",
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });



            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext _context, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            //var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole<string>>>();
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    //create database schema if none exists
            //    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            //    context.Database.EnsureCreated();

            //    //If there is already an Administrator role, abort
            //    var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            //    //Seed roles
            //    roleManager.CreateAsync(new IdentityRole("Administrator"));
            //    roleManager.CreateAsync(new IdentityRole("User"));
            //}

            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            //app.UseHttpsRedirection();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // run npm process with client app
                    spa.UseVueCli(npmScript: "serve", port: 8080);
                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead:
                    // app should be already running before starting a .NET client
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080"); // your Vue app port
                }
            });
            _context.Database.EnsureCreated();
            SeedRoles(serviceProvider).Wait();
        }

        private async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleExist = await roleManager.RoleExistsAsync("Administrator");
            if(roleExist)
                return;
            await roleManager.CreateAsync(new IdentityRole("Administrator"));
            await roleManager.CreateAsync(new IdentityRole("User"));
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var userExists = await userManager.FindByNameAsync("admin");
            if (userExists != null)
                return;

            await userManager.CreateAsync(new User { UserName = "admin" });
            var user = await userManager.FindByNameAsync("admin");
            var res = await userManager.AddPasswordAsync(user, "12345");

            String hashedNewPassword = userManager.PasswordHasher.HashPassword(user, "12345");
            //UserStore<User> store = new UserStore<User>("");
            //await store.SetPasswordHashAsync(user, hashedNewPassword);

            await userManager.UpdateAsync(user);
            var last = await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
}
