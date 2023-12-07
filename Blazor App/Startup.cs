using Microsoft.AspNetCore.Identity;

namespace NazwaTwojegoProjektu
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin"));
                // Dodaj inne polityki według potrzeb
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            // Dodaj inne konfiguracje/usługi, jeśli są potrzebne
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            // Konfiguracje środowiska, middleware itp.

            string[] roles = { "Admin", "User", "Manager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Dalsze konfiguracje aplikacji
        }
    }
}
