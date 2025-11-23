# WebHost Modules

A modular framework for organizing ASP.NET Core application configuration using a pipeline-based architecture.

## Key Features

- **Modular Architecture**: Organize configuration into reusable modules
- **Pipeline-Based Configuration**: Fluent API for building applications
- **Conditional Module Loading**: Apply modules based on environment or configuration
- **Dependency Management**: Automatic dependency validation

## Quick Start

### 1. Create a Module

```csharp
public sealed class DatabaseModule : WebHostModuleBase
{
    public override void ConfigureAppBuilder(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(appBuilder.Configuration.GetConnectionString("DefaultConnection")));
    }

    public override async Task ConfigureAppAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        await context.Database.MigrateAsync();
    }
}
```

### 2. Configure Pipeline

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = await builder
    .ToPipeline()
    .AddModule(new DatabaseModule())
    
    // Configure services
    .ConfigureServices(services => services.AddAuthentication())
    .ConfigureServices((services, config) => services.Configure<EmailSettings>(config.GetSection("Email")))
    
    // Configure pipeline
    .ConfigurePipeline(app => app.UseAuthentication())
    .ConfigurePipeline(async app => await app.SeedDatabaseAsync())
    
    // Conditional operations
    .WhenEnvironment("Development")
        .ConfigureServices(services => services.AddSwaggerGen())
        .ConfigurePipeline(app => app.UseSwagger())
    
    .When(config => config.GetValue<bool>("Features:EnableCaching"))
        .AddModule(new CachingModule())
    
    .When(() => DateTime.Now.Hour > 9)
        .ConfigureServices(services => services.AddSingleton<IBusinessHoursService>())
    
    .BuildAsync();

await app.RunAsync();
```

## Available Methods

### Pipeline Creation
```csharp
builder.ToPipeline()
```

### Configuration Methods
```csharp
.ConfigureServices(Action<IServiceCollection> configuration)
.ConfigureServices(Action<IServiceCollection, IConfiguration> configuration)
.ConfigureBuilder(Action<WebApplicationBuilder> configuration)
.ConfigurePipeline(Action<WebApplication> configuration)
.ConfigurePipeline(Func<WebApplication, Task> configuration)
```

### Conditional Methods
```csharp
.When(bool condition)
.When(Func<bool> conditionFunc)
.When(Func<IConfiguration, bool> conditionFunc)
.WhenEnvironment(string environmentName)
```

### Module Registration
```csharp
.AddModule(WebHostModuleBase module)
.BuildAsync() // Returns Task<WebApplication>
```

## Examples

### Before
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthentication();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseAuthentication();
await app.RunAsync();
```

### After
```csharp
var app = await builder
    .ToPipeline()
    .AddModule(new DatabaseModule())
    .ConfigureServices(services => services.AddAuthentication())
    .WhenEnvironment("Development")
        .ConfigureServices(services => services.AddSwaggerGen())
        .ConfigurePipeline(app => app.UseSwagger())
    .ConfigurePipeline(app => app.UseAuthentication())
    .BuildAsync();

await app.RunAsync();
