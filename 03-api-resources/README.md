# Identifikujemy zasoby

## Co chcemy osiągnąć?

Chcemy przenieść nasze API na poziom 1 w modelu Richardsona. Zidentyfikowaliśmy następujące zasoby i reprezentujące je URI:

1. Witryna: `http://<serwer>/webites/<url>` (np. `http://localhost/webites/www.wp.pl`)
2. Hasło: `http://<serwer>/webites/<url>/password/<login>` (np. `http://localhost/webites/www.wp.pl/password/marcin.hoppe`)

Chcemy również, żeby reprezentacją zasobu typu witryna był następujący dokument JSON:

```json
{
    "url": "www.wp.pl"
}
```

a reprezentacją zasobu typu hasło:

```json
{
    "url": "www.wp.pl",
    "login": "marcin.hoppe",
    "password": "ala ma kota"
}
```

## Co mamy na starcie?

Na starcie mamy API w stylu RPC, które stworzyliśmy w poprzednim ćwiczeniu. Dane są przechowywane w pamięci przez Entity Frameworke Core.

## Do roboty!

### Kontroler obsługujący witryny

1. Dodajemy nowy kontroler `WebsiteController`.
1. Przenosimy do kontrolera metodę rejestrującą witrynę:

```csharp
public class WebsiteController : Controller
{
    private readonly WebsiteContext context;

    public WebsiteController(WebsiteContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public IActionResult RegisterWebsite(string url)
    {
        context.Websites.Add(new Website
        {
            Url = url
        });
        context.SaveChanges();

        return Ok();
    }
}
```

### Kontroler obsługujący hasła

1. Zmieniamy nazwę kontrolera `ApiController` na `CredentialsController`.

### Routing w kontrolerze obsługującym witryny

1. Zmieniamy parametry atrybutu `[HttpPost]`:

```csharp
public class WebsiteController : Controller
{
    // ...
    [HttpPost("/websites")]
    public IActionResult RegisterWebsite(string url)
    {
        // ...
    }
}
```

### Routing w kontrolerze obsługującym hasła

1. Zmieniamy parametry atrybutów `[HttpPost]`:

```csharp
public class CredentialsController : Controller
{
    // ...
    [HttpPost("/websites/{url}/password/{login}/generate")]
    public IActionResult GeneratePassword(string url, string login)
    {
        // ...
    }

    [HttpPost("/websites/{url}/password/{login}/store")]
    public IActionResult StorePassword(string url, string login, string password)
    {
        // ...
    }

    [HttpPost("/websites/{url}/password/get")]
    public IActionResult GetPassword(string url)
    {
        // ...
    }
}
```

### Reprezentacja zasobu typu witryna

1. Dodajemy nowy folder `ViewModel`.
1. W nowym folderze dodajemy klasę `Website`:

```csharp
public class Website
{
    public string Url { get; set; }
}
```

i modyfikujemy metodę rejestrującą witrynę:

```csharp
[HttpPost("/websites")]
public IActionResult RegisterWebsite([FromBody] ViewModel.Website website)
{
    context.Websites.Add(new Website
    {
        Url = website.Url
    });
    context.SaveChanges();

    return Ok();
}
```

### Reprezentacja zasobu typu hasło

1. W folderze `ViewModel` dodajemy klasę `Credentials`:

```csharp
public class Credentials
{
    public string Url { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}
```

i modyfikujemy metodę zwracającą hasło:

```csharp
[HttpPost("/websites/{url}/password/get")]
public IActionResult GetPassword(string url)
{
    var website = context.Websites
        .Include(w => w.Credentials)
        .FirstOrDefault(w => w.Url == url);

    return Json(new ViewModel.Credentials
    {
        Url = website.Url,
        Login = website.Credentials.Login,
        Password = website.Credentials.Password
    });
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

Wszystkie żądania powinny być żądaniami typu `POST`. Ciało pierwszego żądania wypełniamy za pomocą `Raw` w zakładce `Body` w Postmanie. Jako format danych wybieramy `JSON (application/json)`.
