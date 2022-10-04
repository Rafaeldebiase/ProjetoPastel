using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pastel.Data.Interfaces;
using Pastel.Data.Repositories;
using Pastel.Data.UnitOfWork;
using Pastel.Handles.CommandHandle;
using Pastel.Handles.Handle;
using Pastel.Handles.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICreateUserCommandHandle, CreateUserCommandHandle>();
builder.Services.AddScoped<IImageHandle, ImageHandle>();
builder.Services.AddScoped<IEditUserCommandHandle, EditUserCommandHandle>();
builder.Services.AddScoped<IDeleteUserCommandHandle, DeleteUserCommandHandle>();
builder.Services.AddTransient<IAutenticateCommandHandle, AutenticateCommandHandle>();
builder.Services.AddScoped<ICreateTaskCommandHandle, CreateTaskCommandHandle>();
builder.Services.AddScoped<IEditTaskCommandHandle, EditTaskCommandHandle>();
builder.Services.AddScoped<ICloseTaskCommandHandle, CloseTaskCommandHandle>();
builder.Services.AddScoped<IEmailHandle, EmailHanle>();
builder.Services.AddScoped<IUserTaskHandle, UserTaskHandle>();
builder.Services.AddScoped<IDeleteTaskCommandHandle, DeleteTaskCommandHandle>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPhoneRepository, PhoneRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("secret").Value);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
