using Pastel.Data.Interfaces;
using Pastel.Data.Repositories;
using Pastel.Data.UnitOfWork;
using Pastel.Handles.CommandHandle;
using Pastel.Handles.Handle;
using Pastel.Handles.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICreateUserCommandHandle, CreateUserCommandHandle>();
builder.Services.AddScoped<IImageHandle, ImageHandle>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPhoneRepository, PhoneRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
