# Konfigurujemy HTTPS

## Co chcemy osiągnąć?

Chcemy skonfigurować HTTPS dla prostego API REST hostowanego za serwerem IIS.


## Co mamy na starcie?

Na starcie dostajemy pusty project ASP.NET Core z włączonym MVC i jednym prostym kontrolerem i jedną klasą modelu. Domyślnie są one hostowane w IIS Express.


## Do roboty!

### Instalacja IIS

1. Wchodzimy do Panelu Sterowania i wybieramy opcję Programy a następnie Włącz lub wyłącz funkcje systemu Windows.
1. Instalujemy pełen serwer IIS.
1. Wybieramy wszystko poza serwerem FTP i zgodnością narzędzi do zarządzania z IIS 6.
1. Właczamy Visual Studio Installer i włączamy obsługę IIS w trakcie developmentu.


### Hostujemy nasze API w pełnym serwerze IIS

1. Wchodzimy do karty właściwości projektu.
1. W zakładce Debug tworzymy nowy profil i nazywamy go `IIS`.
1. W polu Launch wybieramy opcję IIS.
1. Opcję Enable Anonymous Authentication pozostawiamy *włączoną*.
1. W pliku `Program.cs` podczas konfiguracji hosta dodajemy wywołanie metody `UseIISIntegration`.
1. Uruchamiamy API i testujemy je za pomocą Postmana.


### Generujemy certyfikat HTTPS

1. Uruchamiamy program IIS Manager.
1. Wybieramy opcję Certyfikaty serwera.
1. Wybieramy opcję Utwórz certyfikat z podpisem własnym...
1. Generujemy certyfikat szkoleniowy.


### Odnajdujemy certyfikat w systemie

1. Uruchamiamy program mmc.exe.
1. Korzystając z menu Plik dodajemy przystawkę Certyfikaty (Konto komputera dla komputera lokalnego).
1. Szukamy nowoutworzonego certyfikatu w folderze Osobiste -> Certyfikaty.


### Konfigurujemy certyfikat

1. W programie IIS Manager wybieramy domyślną witrynę.
1. Wybieramy opcję Powiązania...
1. Dodajemy powiązanie dla protokołu HTTPS (port 443).
1. Wybieramy nowoutworzony certyfikat.


### Testujemy

1. W ustawieniach Postmana (File -> Settings -> General) *odznaczamy* opcję weryfikacji certyfikatu HTTPS.
1. Testujemy API (korzystając z nowego profilu IIS) za pomocą Postmana zarówno za pomocą protokołu HTTP i HTTPS.


### Wymuszamy HTTPS

1. W metodzie `ConfigureServices` dodajemy wymuszenie protokołu HTTPS:

```csharp
services.Configure<MvcOptions>(options =>
{
    options.Filters.Add(new RequireHttpsAttribute());
});
```

### Konfigurujemy przekierowanie z HTTP na HTTPS

1. W metodzie `Configure` konfigurujemy middleware, który przekieruje żądania HTTP na HTTPS:

```csharp
var rewriteOptions = new RewriteOptions();
rewriteOptions.AddRedirectToHttps();

app.UseRewriter(rewriteOptions);
```

## Testujemy

1. Uruchamiamy API (Ctrl + F5).
1. Testujemy API (korzystając z nowego profilu IIS) za pomocą protokołu HTTPS.
1. Uruchamiamy Fiddlera.
1. Wykonujemy żądanie za pomocą protokołu HTTP.
1. Weryfikujemy, że występuje przekierowanie (może być za pomocą przeglądarki).
