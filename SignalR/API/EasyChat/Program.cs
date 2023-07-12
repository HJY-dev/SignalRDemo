using EasyChat;
using EasyChat.Auth;
using EasyChat.Hubs;
using FreeScheduler;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000","https://localhost:3000")
                          .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    // ��Ӷ�JWT�������֤
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// �����Ȩ����
builder.Services.AddMyJWTBearerAuth();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ��ʱ����
var hub = app.Services.GetService<IHubContext<ChatHub>>();
Scheduler scheduler = new Scheduler(new MyTaskHandler(hub));
scheduler.AddTask("topic1", "body1", round: -1, 5);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

// ��Ȩ·��
app.MapGet("generatetoken", c => c.Response.WriteAsync(MyJWTBearer.GenerateToken(c)));

app.MapControllers();
app.MapHub<ChatHub>("/ChatHub", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});

app.Run();