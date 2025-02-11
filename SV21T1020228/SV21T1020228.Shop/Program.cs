﻿using Microsoft.AspNetCore.Authentication.Cookies;
using SV21T1020228.Shop.AppCodes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews()
	.AddMvcOptions(options =>
	{
		options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
	});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(option =>
	{
		option.Cookie.Name = "AuthenticationCookie";        // Tên
		option.LoginPath = "/Account/Login";                // Đường dẫn
		option.AccessDeniedPath = "/Account/AccessDenined"; // URL trong trường hợp bị cấm truy cập
		option.ExpireTimeSpan = TimeSpan.FromDays(360);     // Thời gian tồn tại của Cookie
	});
builder.Services.AddSession(option =>
{
	option.IdleTimeout = TimeSpan.FromMinutes(60);
	option.Cookie.HttpOnly = true;
	option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();

/*app.MapGet("/", context =>
{
	context.Response.Redirect("/Shop/Home/Index", permanent: false);
	return Task.CompletedTask;
});*/
app.UseRouting();

app.UseAuthentication();    // Trước Author
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

ApplicationContext.Configure
	(
		context: app.Services.GetRequiredService<IHttpContextAccessor>(),
		enviroment: app.Services.GetRequiredService<IWebHostEnvironment>()
	);

// Khởi tạo cấu hình cho BusinessLayer
string connectionString = builder.Configuration.GetConnectionString("LiteCommerceDB");
SV21T1020228.BusinessLayers.Configuration.Initialize(connectionString);

app.Run();
