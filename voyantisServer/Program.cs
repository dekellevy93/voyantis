var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
// In ConfigureServices:
// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCORS",
        builder => builder
            .WithOrigins("http://localhost:8080") // Replace with your Vue.js dev server URL
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("DevelopmentCORS");

app.UseStaticFiles();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers(); // *** IMPORTANT: Add this line to map your controllers ***


app.Run();
