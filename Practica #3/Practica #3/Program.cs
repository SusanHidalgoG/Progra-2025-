using Practica__3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("Api", (sp, client) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var url = (cfg["Start:ApiUrl"] ?? "").Trim();
    if (!url.EndsWith("/")) url += "/";
    client.BaseAddress = new Uri(url);

    var key = cfg["Start:LlaveSegura"];
    if (!string.IsNullOrWhiteSpace(key) && !client.DefaultRequestHeaders.Contains("X-App-Key"))
        client.DefaultRequestHeaders.Add("X-App-Key", key);
});

builder.Services.AddSession();
builder.Services.AddScoped<IUtilitarios, Utilitarios>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
