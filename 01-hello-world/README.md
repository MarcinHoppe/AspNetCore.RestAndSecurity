# Hello, world! w świecie REST

## Co chcemy osiągnąć?

Chcemy stworzyć najprostsze możliwe API oparte o HTTP i JSON za pomocą ASP.NET Core.

## Co mamy na starcie?

Nic, startujemy od zera :).

## Do roboty!

### Pusty projekt

1. W Visual Studio tworzymy nowy projekt (File-> New -> Project...).
1. Tworzymy nowy projekt ASP.NET Core: Visual C# -> Web -> ASP.NET Core Web Application.
1. Wybieramy pusty projekt (Empty), .NET Core i ASP.NET Core 2.0. Nie zawracamy sobie głowy Dockerem :).

W pliku `Program.cs` mamy:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
    }
}
```

Natomiast w pliku `Startup.cs` mamy:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.Run(async (context) =>
        {
            await context.Response.WriteAsync("Hello World!");
        });
    }
}
```

To dobry start, ale potrzebujemy kilku elementów.

### Dodajemy wsparcie dla MVC

1. Modyfikujemy klasę `Startup`:

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // ...

        app.UseMvc();
    }
}
```

### Model

1. Dodajemy folder `Models`.
1. W folderze Models dodajemy klasę `HelloMessage`:

```csharp
public class HelloMessage
{
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
```

### Kontroler

1. Dodajemy folder `Controllers`.
1. W folderze Models dodajemy klasę `HelloController`:

```csharp
public class HelloController : Controller
{
    [HttpGet]
    public IActionResult GetHelloMessage()
    {
        var response = new HelloMessage
        {
            Message = "Hello, world!",
            Timestamp = DateTime.Now
        };
        return Json(response);
    }
}
```

### Routing

Na razie nasz kontroler nie będzie wywołany, potrzebujemy konfiguracji routingu:

```csharp
public class HelloController : Controller
{
    [HttpGet("/api/hello")]
    public IActionResult GetHelloMessage()
    {
        // ...
    }
}
```

## Testujemy

1. Uruchamiamy serwis (Ctrl+F5).
1. Za pomocą Postmana wysyłamy żądanie GET pod URL: http://localhost:<PORT>/api/hello.