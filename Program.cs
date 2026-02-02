using EmailAPI.IServices;
using EmailAPI.Services;
using EmailAPI.Settings;
using Microsoft.AspNetCore.RateLimiting;
using SendGrid;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Portfolio Contact API",
        Version = "v1",
        Description = "API for portfolio contact form using SendGrid"
    });
});

// SendGrid Settings
var sendGridApiKey = builder.Configuration["SENDGRID_API_KEY"] ?? string.Empty;

builder.Services.Configure<SendGridSettings>(options =>
{
    options.ApiKey = sendGridApiKey;
});

// Optional: ISendGridClient singleton
builder.Services.AddSingleton<ISendGridClient>(x =>
    new SendGridClient(sendGridApiKey));

// DI
builder.Services.AddScoped<IEmailService, EmailService>();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("contactLimiter", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 0;
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();