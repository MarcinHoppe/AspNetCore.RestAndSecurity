# Konfigurujemy CORS

## Co chcemy osiągnąć?

Chcemy skonfigurować CORS, żebyśmy mogli z aplikacji Webowej korzystać z API REST na innym hoście.


## Co mamy na starcie?

Na starcie dostajemy dwa projekty ASP.NET Core: API REST i aplikację Webową. Każda z aplikacji działa na innym porcie.


## Do roboty!

### Testujemy stan początkowy

1. Testujemy metodę `GET` na zasobie `/echo/{message}` za pomocą Postmana i sprawdzamy czy działa.
1. W Visual Studio ustawiamy projekty Echo i Echo.Web jako projekty startowe.
1. Uruchamiamy oba projekty (Ctrl+F5).
1. W przeglądarce włączamy narzędzia dla programistów i przechodzimy do nasłuchu żądań HTTP.
1. Klikamy przycisk Echo w aplikacji Webowej.
1. W narzędziach dla programistów weryfikujemy, że żądanie zostało wysłane a odpowiedź odebrana.
1. Weryfikujemy, że odpowiedź została zablokowana zanim dotarła do kodu JavaScript:

```
Failed to load http://localhost:63509/echo/echo%20message: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin 'http://localhost:64694' is therefore not allowed access.
```


### Konfigurujemy CORS globalnie

1. Dodajemy obsługę CORS do kontenera Dependency Injection za pomocą metody `AddCors` w metodzie `ConfigureServices`.
1. Włączamy obsługę CORS globalnie za pomocą metody `UseCors` w metodzie `Configure`.
1. W metodzie `UseCors` korzystamy z przeciążonej wersji, która przyjmuje pozwala na skorzystanie z instancji klasy `CorsPolicyBuilder`.
  1. Dopuszczamy odwołania jedynie z domeny aplikacji Web (pamiętaj o porcie!).
  1. Dopuszczamy wszystkie nagłówki (`AllowAnyHeader`).
  1. Dopuszczamy jedynie metodę `GET` (`WithMethods`).
 
## Testujemy

1. Ponownie klikamy przycisk Echo w aplikacji Webowej (z otwartymi narzędziamy dla programistów).
1. Weryfikujemy, że odwołanie działa.
1. W narzędziach dla programistów weryfikujemy zawartość nagłówka `Origin` w żądaniu i `Access-Control-Allow-Origin` w odpowiedzi.

### Test z gwiazdką

1. Pozostawiamy konfigurację CORS niezmienioną.
1. Zmieniamy metodę `Echo` w kontrolerze z `GET` na `PUT`.
1. Analogiczną zmianę wykonujemy w kodzie JavaScript:

```javascript
$.ajax({
    url: "http://localhost:63509/echo/echo%20message",
    type: "PUT"
})
```

Co się stało? Zwróć uwagę na wysyłane żądania HTTP i ich metody.
