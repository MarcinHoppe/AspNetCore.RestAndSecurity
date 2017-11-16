# Implementujemy API w stylu RPC

## Co chcemy osiągnąć?

Chcemy wyposażyć nasze API w możliwość:

1. Rejestrację nowej witryny
1. Wygenerowania hasła dla danego loginu i witryny
1. Zapisania własnego hasła dla danego loginu i witryny
1. Pobrania loginu i hasła dla danej witryny

## Co mamy na starcie?

Na starcie dostajemy pusty project ASP.NET Core z włączonym MVC. Projekt nie ma na razie żadnego kontrolera ani modelu.

## Do roboty!

## Model

1. Dodajemy nowy folder `Model`.
1. Dodajemy nową klasę `Credentials`:

```csharp
public class Credentials
{
    public string Login { get; set; }
    public string Password { get; set; }
}
```

### Kontroler

1. Dodajemy nowy folder `Controllers`.
1. Dodajemy nową klasę `ApiController`:

```csharp
public class ApiController : Controller
{
}
```

### Rejestracja nowej witryny

1. Dodajemy do kontrolera akcję, która zarejestruje nową witrynę w systemie:

```csharp
public class ApiController : Controller
{
    private static Dictionary<string, Credentials> database = new Dictionary<string, Credentials>();

    [HttpPost]
    public IActionResult RegisterWebsite(string website)
    {
        database[website] = new Credentials();
        return Ok();
    }
}
```

### Generowanie hasła

1. Dodajemy do kontrolera akcję generującą hasło dla loginu i witryny:

```csharp
[HttpPost]
public IActionResult GeneratePassword(string website, string login)
{
    var password = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
    var credentials = database[website];
    credentials.Login = login;
    credentials.Password = password;
    return Ok();
}
```

### Zapisanie własnego hasła

1. Dodajemy do kontrolera akcję zapisującą hasło dla loginu i witryny:

```csharp
[HttpPost]
public IActionResult StorePassword(string website, string login, string password)
{
    var credentials = database[website];
    credentials.Login = login;
    credentials.Password = password;
    return Ok();
}
```

### Pobranie hasła

1. Dodajemy do kontrolera akcję odczytującą hasło dla danej witryny:

```csharp
[HttpPost]
public IActionResult GetPassword(string website)
{
    var credentials = database[website];
    return Json(credentials);
}
```

## Testujemy

1. Uruchamiamy serwis (Ctrl+F5).
1. Za pomocą Postmana testujemy następujący scenariusz:
  1. Rejestrujemy nową witrynę
  1. Generujemy hasło dla loginu w tej witrynie
  1. Odczytujemy hasło dla loginu w tej witrynie
  1. Zapisujemy własne hasło dla loginu w tej witrynie
  1. Odczytujemy hasło dla loginu w tej witrynie

Wszystkie żądania powinny być żądaniami typu `POST`. Ciało każdego żądania wypełniamy za pomocą `form-data` w zakładce `Body` w Postmanie.