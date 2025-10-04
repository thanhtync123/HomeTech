using HomeTechBlazor.Components;
using HomeTechBlazor.Components.Shared;
using HomeTechBlazor.Service;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// === SERVICES ===
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CurrentUser>();
builder.Services.AddMudServices();
builder.Services.AddScoped<HomeTechBlazor.Managers.ChatbotManager>();






// Cho phép tải file lớn
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

var app = builder.Build();

// === PIPELINE ===
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// ⚠️ Quan trọng: phải gọi UseStaticFiles TRƯỚC antiforgery và map razor
app.UseStaticFiles(); // <-- Phục vụ video, js, css, ảnh, v.v.

// Nếu bạn có routing, có thể thêm: app.UseRouting();
app.UseAntiforgery();

// ⚙️ Map Razor Components
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

// Không cần app.Map("/videos/..."), Kestrel tự xử lý
app.Run();
