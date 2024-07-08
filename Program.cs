using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BancoKRT.API.Domain.Services;
using BancoKRT.API.Domain.Services.Interfaces;
using BancoKRT.API.Infrastructure.Repositories.Interfaces;
using BancoKRT.Infrastructure.Repository;
using BancoKRT.API.Infrastructure.Services;
using BancoKRT.API.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);


ConfigureDynamoDB(builder.Services);
ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();
ConfigureMiddleware(app, builder.Environment);
ConfigureEndpoints(app);

app.Run();

void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment)
{
    services.AddRazorPages();
    services.AddControllers();
    services.AddSwaggerGen();
    services.AddEndpointsApiExplorer();

    // Configurar serviços de aplicação
    services.AddScoped<ExceptionMiddleware>();
    services.AddScoped<IClientService, ClientService>();
    services.AddScoped<IPIXService, PIXService>();

    // Configurar repositórios
    services.AddScoped<IClientRepository, ClientRepository>();
    services.AddScoped<IPIXRepository, PIXRepository>();

    if (!environment.IsDevelopment())
    {
        if (!Directory.Exists(@"/var/afKeys"))
        {
            Directory.CreateDirectory(@"/var/afKeys");
        }
        services.AddDataProtection()
                .SetApplicationName("BancoKRT")
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/afKeys/"));
    }

    if (environment.IsStaging())
    {
        builder.WebHost.UseStaticWebAssets();
    }
}

void ConfigureDynamoDB(IServiceCollection services)
{
    var awsOptions = builder.Configuration.GetAWSOptions();
    awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
       builder.Configuration["AWS:AccessKey"],
       builder.Configuration["AWS:SecretKey"]
    );
    services.AddDefaultAWSOptions(awsOptions);
    services.AddAWSService<IAmazonDynamoDB>();
    services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
    services.AddSingleton<DynamoDBTableService>();
}

void ConfigureMiddleware(WebApplication app, IWebHostEnvironment environment)
{
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    if (!environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
}

void ConfigureEndpoints(WebApplication app)
{
    app.MapRazorPages();
    app.MapControllers();
}