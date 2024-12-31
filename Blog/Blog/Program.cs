using Microsoft.EntityFrameworkCore;
using System;
using Blog.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Optional: Set session timeout
    options.Cookie.HttpOnly = true; // Security setting for the session cookie
    options.Cookie.IsEssential = true; // Set to true if the session is essential for the app
});

builder.Services.AddDbContext<BlogsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Data Source=DESKTOP-KDHUHUT;Initial Catalog=blogs;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True")));

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Ovo omogućava ispisivanje logova u konzolu
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
