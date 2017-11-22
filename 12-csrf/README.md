# Bronimy się przed CSRF

## Co chcemy osiągnąć?

Chcemy obronić się przed atakiem typu Cross-Site Request Forgery.


## Co mamy na starcie?

Na starcie mamy przykładową aplikację z frontendem opartym o jQuery i backendem w postaci API REST. Mamy też fałszywą witrynę, na której można wygrać iPhone'a.


## Do roboty!

### Konfigurujemy hosty web.local i evil.local

1. Do pliku `C:\windows\system32\drivers\etc\hosts` dodajemy następujące wpisy:

```
127.0.0.1 evil.local
127.0.0.1 web.local
```


### Testujemy stan początkowy

1. W Visual Studio ustawiamy projekty AspNetCore.Csrf.Sample i AspNetCore.Csrf.EvilSite jako projekty startowe.
1. Uruchamiamy oba projekty (Ctrl+F5).
1. Logujemy się w aplikacji.
1. Idziemy na stronę profilową.
1. Otwieramy stronę "złą" i próbujemy wygrać iPhone'a.
1. Odświeżamy stronę profilową i sprawdzamy, że dane zostały nadpisane.


### Generujemy tokeny anty-CSRF

1. W metodzie `ConfigureServices` włączamy usługę generowania tokenów (`AddAntiforgery`).
1. W widoku generujemy token:

```csharp
@inject Microsoft.AspNetCore.Antiforgery.IAntiforgery Csrf
@functions{
    public string GetAntiForgeryRequestToken()
    {
        return Csrf.GetAndStoreTokens(Context).RequestToken;
    }
}
```

### Wykorzystujemy tokeny

1. Metodę `POST` w kontrolerze dekorujemy atrybutem `[ValidateAntiForgeryToken]`.
1. W wywołaniu funkcji `ajax` w jQuery dodajemy nagłówek `RequestVerificationToken`:

```javascript
// ...
headers: {
    "RequestVerificationToken": '@GetAntiForgeryRequestToken()'
}
// ...
```


### Zadania z *

1. Zmieniamy parametr `SameSite` ciasteczka uwierzytelniającego z `SameSiteMode.None` na `SameSiteMode.Strict`.
1. Wylogowujemy się i logujemy się do aplikacji ponownie.
1. Czy atak został powstrzymany? Co się zmieniło?


### Zadania z *

1. Przywracamy parametr `SameSite` do wartości początkowej.
1. Włączamy `OriginCheckMiddleware` do potoku przetwarzania żądań.
1. Wylogowujemy się i logujemy się do aplikacji ponownie.
1. Czy atak został powstrzymany? Co się zmieniło?
1. Jak działa `OriginCheckMiddleware`?
