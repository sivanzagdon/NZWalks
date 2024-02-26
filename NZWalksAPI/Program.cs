using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using Microsoft.IdentityModel.Tokens; //new TokenValidationParameters line 29
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title="NZ Walks API", Version="v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                },
                Scheme="Oauth2",
                Name=JwtBearerDefaults.AuthenticationScheme,
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


// Dependancy injection
// Scoped
// Singlton
// AddTransient
builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))); //
builder.Services.AddDbContext<NZWalksAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString"))); //

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();//
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddIdentityCore<IdentityUser>() // adds the basic Identity services
    .AddRoles<IdentityRole>() //adds role management capabilities to the Identity services
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks") // adds a token provider for generating and validating tokens used for various features like email confirmation, password reset
    .AddEntityFrameworkStores<NZWalksAuthDbContext>() //`NZWalksAuthDbContext` as the database context to use.
    .AddDefaultTokenProviders(); //adds the default token providers

// This configures Identity options such as password requirements
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //In-app authentication method as token-based authentication JWT
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters //Configures the validation of JWT tokens using validation parameters.
    {
        //Defines the actions required to verify that the token is valid and reliable
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        //Specifies the actual values of your user and application credentials used to validate the token.
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider= new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    RequestPath="/Images",
    // https://localhost:1234/Images
});

app.MapControllers();

app.Run();
