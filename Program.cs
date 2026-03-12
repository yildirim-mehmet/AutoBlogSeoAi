using BlogSeoAi.Data;
using BlogSeoAi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpClient();

// Servislr
builder.Services.AddScoped<ISlugService, SlugService>();
builder.Services.AddScoped<IAiClient, GeminiAiClient>();
builder.Services.AddScoped<IDailyPostGenerator, DailyPostGenerator>();

// Otomatik günlük üretimi “kendi içinde” çalýþtýrmak istersen ama dikkkt et hosting tarzý yðaýda problem yalanabiliyor
builder.Services.AddHostedService<DailyAutoPostHostedService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// (Admin için auth eklemek istersen buraya) yönetici yönlendirme adresi
app.MapControllerRoute(
    name: "post",
    pattern: "blog/{slug}",
    defaults: new { controller = "Post", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

