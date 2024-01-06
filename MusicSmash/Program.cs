using MusicSmash;
using MusicSmash.Components;
using MusicSmash.Controllers;
using MusicSmash.Controllers.Api.Spotify;
using MusicSmash.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<RoundService>();

builder.Services.AddScoped<RoundController>();
builder.Services.AddScoped<VoteController>();
builder.Services.AddScoped<Events>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<Api>();

builder.Services.AddControllers();
builder.Services.AddCookiePolicy(configureOptions =>
{
    configureOptions.MinimumSameSitePolicy = SameSiteMode.Lax;
    configureOptions.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    configureOptions.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
