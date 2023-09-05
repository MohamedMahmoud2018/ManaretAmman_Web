using DataAccessLayer.Models;
using ManaretAmman.MiddleWare;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using BusinessLogicLayer.Mapper;
using BusinessLogicLayer.Services.ProjectProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var TypesToRegister = Assembly.Load("BusinessLogicLayer").GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsClass).ToList();
var ITypesToRegister = Assembly.Load("BusinessLogicLayer").GetTypes()
                .Where(x => !string.IsNullOrEmpty(x.Namespace))
                .Where(x => x.IsInterface).ToList();

#region Cors Origin
string defaultpolicy = "default";
builder.Services.AddCors(
    options => options.AddPolicy(
        defaultpolicy,
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    )
);
#endregion

#region Atuo Mapping
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Mapping());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region DbContext

builder.Services.AddDbContext<PayrolLogOnlyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#endregion

// Add services to the container.
#region Injection
builder.Services.AddSingleton<IProjectProvider, ProjectProvider>();

builder.Services.AddScoped<DbContext, PayrolLogOnlyContext>();

for (int i = 0; i < TypesToRegister.Count; i++)
{
    var itype = ITypesToRegister
        .Find(t => (t.Name == "I" + TypesToRegister[i].Name));
    if (itype != null)
        builder.Services.AddScoped(itype, TypesToRegister[i]);
}

#endregion

//#region Identity
//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
//    .AddEntityFrameworkStores<BapetcoContext>()
//    .AddDefaultTokenProviders();
//#endregion
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:ValidAudience"],
        ValidAudience = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(GlobalExceptionHandler));

#region Cors
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
#endregion

//app.UseExceptionHandler(); 

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.UseMiddleware(typeof(ProjectMiddleware));

app.MapControllers();

app.Run();