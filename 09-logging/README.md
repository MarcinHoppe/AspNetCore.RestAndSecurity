# Piszemy middleware logujący

## Co chcemy osiągnąć?

Chcemy napisać middleware, który będzie na wyjście debugowe wypisywał informację o każdym żądaniu i odpowiedzi.


## Co mamy na starcie?

Na starcie dostajemy pusty project ASP.NET Core z włączonym MVC i jednym prostym kontrolerem i jedną klasą modelu.


## Do roboty!

### Implementujemy middleware

1. Dodajemy nowy folder `Middleware`.
1. W nowym folderze dodajemy klasę `LoggingMiddleware`. Klasa powinna:
  1. Mieć publiczny konstruktor `LoggingMiddleware(RequestDelegate next)`.
  1. Mieć publiczną metodę `Task Invoke(HttpContext context`), która:
    1. Będzie logowała informację za pomocą `System.Diagnostics.Debug.WriteLine`.
    1. Wywoła resztę potoku za pomocą zmiennej `next`, którą przyjęła w konstruktorze.
1. W nowym folderze dodajemy statyczną klasę `LoggingMiddlewareExtensions`, która będzie miała jedną metodę:

```csharp
public static IApplicationBuilder UseDebugLogging(this IApplicationBuilder builder)
{
    return builder.UseMiddleware<LoggingMiddleware>();
}
```


### Konfigurujemy middleware

1. W metodzie `Configure` konfigurujemy nowy middleware:

```csharp
app.UseDebugLogging();
```

### Zadanie z *

1. Zalogujmy metodę HTTP oraz względną ścieżkę URL.
1. Obie informacje należy wyciągnąć z obiektu `HttpContext`.


## Testujemy

1. Uruchamiamy API pod kontrolą debuggera (F5).
1. Testujemy API.
1. Weryfikujemy, że 
