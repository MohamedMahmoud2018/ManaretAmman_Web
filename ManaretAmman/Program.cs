using DataAccessLayer.Models;
using ManaretAmman.MiddleWare;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using BusinessLogicLayer.Mapper;

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


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware(typeof(GlobalExceptionHandler));
app.UseMiddleware(typeof(ProjectMiddleware));
#region Cors
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
#endregion
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();