using INZ_API_POSTGRES.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddRazorPages();




builder.Services.AddDbContext<DatabaseContext>(o=> o.UseNpgsql(builder.Configuration.GetConnectionString("connection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddIdentity<Users, IdentityRole>()
        .AddEntityFrameworkStores<DatabaseContext>()
        .AddDefaultTokenProviders();

builder.WebHost.UseUrls("http://*:5000", "https://*:5001");

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Set the login path
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true; // Sliding expiration for the cookie
    options.ExpireTimeSpan = TimeSpan.FromDays(2);
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/App");
    options.Conventions.AuthorizePage("/Account/Settings");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
  
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();
app.MapRazorPages();


app.Run();
