using framework.Domain.Persistence;
using framework.Infrastructure.Specs;
using Microsoft.AspNetCore.Diagnostics;
using users_backend.Application.Mappings;
using users_backend.Application.Services;
using users_backend.Domain.Persistence;
using users_backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(RoleMapperProfile));
builder.Services.AddAutoMapper(typeof(UserMapperProfile));
builder.Services.AddScoped(typeof(ISpecificationParser<>), typeof(SpecificationParser<>));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<UsersContext>(options =>
        options.UseSqlServer(connectionString)
    );
}

var app = builder.Build();
ConfigureExceptionhandler(app);
app.UseCors("AllowAll");

if (builder.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UsersContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    DataLoader dataLoader = new(context);
    dataLoader.LoadData();
}

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

static void ConfigureExceptionhandler(WebApplication app)
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            // Obtén los detalles de la excepción
            IExceptionHandlerPathFeature? exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // Configura el encabezado y el contenido de la respuesta
            context.Response.ContentType = "application/json";
            int statusCode = 500; // Código predeterminado para errores internos
            string errorMessage = "An unexpected error occurred.";

            if (exceptionHandlerPathFeature?.Error != null)
            {
                var exception = exceptionHandlerPathFeature.Error;

                // Manejo de excepciones personalizadas
                if (exception is ConcurrencyException concurrencyEx)
                {
                    statusCode = 409; // Código HTTP para conflictos de concurrencia
                    errorMessage = concurrencyEx.Message; // Mensaje específico de la excepción
                    logger.LogWarning(concurrencyEx, "A concurrency conflict occurred.");
                }
                else
                {
                    // Loguea cualquier otra excepción no controlada
                    logger.LogError(exception, "An unhandled exception occurred while processing the request.");
                }
            }
            else
            {
                logger.LogError("An unhandled exception occurred while processing the request.");
            }

            // Establece el código de estado y escribe la respuesta JSON
            context.Response.StatusCode = statusCode;
            var response = new
            {
                statusCode,
                error = errorMessage
            };

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        });
    });
}
