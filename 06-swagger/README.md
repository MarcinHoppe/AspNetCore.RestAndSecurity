# Konfigurujemy Swaggera

## Co chcemy osiągnąć?

Chcemy automatycznie wygenerować dokumentację naszego API za pomocą Swaggera. 


## Co mamy na starcie?

Na starcie dostajemy pusty project ASP.NET Core z włączonym MVC i jednym prostym kontrolerem i jedną klasą modelu.


## Do roboty!

### Pakiet Swashbuckle.AspNetCore

1. Instalujemy pakiet `Swashbuckle.AspNetCore` do projektu `SimpleApi.csproj`:

```xml
<ItemGroup>
    <!-- ... -->
    <PackageReference Include="Swashbuckle.AspNetCore" Version="1.1.0" />
</ItemGroup>
```


### Dodajemy komponent Swashbuckle do kontenera DI i rejestrujemy middleware

1. W klasie `Startup` dodajemy:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("docs", new Info
        {
            Title = "Simple API",
            Version = "1"
        });
    });
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/docs/swagger.json", "Simple API docs");
    });

    app.UseMvcWithDefaultRoute();
}
```


## Testujemy

1. Uruchamiamy serwis (Ctrl+F5).
1. Oglądamy dokument JSON wygenerowany przez Swaggera: `http://localhost:<port>/swagger/docs/swagger.json`.
1. Korzystamy z automatycznie wygenerowanej dokumentacji: `http://localhost:<port>/swagger`.
