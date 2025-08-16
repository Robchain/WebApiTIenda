using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiTIenda.Application.Interfaces.Admin;
using WebApiTIenda.Application.Interfaces.Auth;
using WebApiTIenda.Application.Interfaces.Buyer;
using WebApiTIenda.Application.Mappings;
using WebApiTIenda.Application.Services.Admin;
using WebApiTIenda.Application.Services.Auth;
using WebApiTIenda.Application.Services.Buyer;
using WebApiTIenda.Domian.Interfaces;
using WebApiTIenda.Infrastructure.Repositories;
using WebApiTIenda.Infrastucture.Data;
using WebApiTIenda.Infrastucture.Repositories;
using WebApiTIenda.Infrastucture.Security;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ILotRepository, LotRepository>();
builder.Services.AddScoped<IWishListRepository, WishListRepository>();
builder.Services.AddScoped<IProductSupplierRepository, ProductSupplierRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();


builder.Services.AddScoped<IProductBuyerService, ProductBuyerService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishListService, WishListService>();


builder.Services.AddScoped<IProductAdminService, ProductAdminService>();
builder.Services.AddScoped<ISupplierAdminService, SupplierAdminService>();
builder.Services.AddScoped<IProductSupplierAdminService, ProductSupplierAdminService>();
builder.Services.AddScoped<ILotAdminService, LotAdminService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WebApi Tienda", Version = "v1" });

    c.CustomSchemaIds(type => type.FullName);

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


builder.Services.AddSingleton<JwtTokenGenerator>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Valida el tiempo de token
        ClockSkew = TimeSpan.Zero, // Valida el tiempo exacto
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi Tienda v1");
        
    });
}


app.UseCors("AllowAll");


app.UseAuthentication(); 
app.UseAuthorization();

// Controllers
app.MapControllers();


app.Run();