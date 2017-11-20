# Dodajemy wsparcie dla HATEOAS

## Co chcemy osiągnąć?

Chcemy przenieść nasze API na poziom 3 w modelu Richardsona. W tym celu musimy do reprezentacji zasobów dodać linki do powiązanych zasobów.


## Co mamy na starcie?

Na starcie mamy API ze zidentyfikowanymi zasobami i metodami `POST`, `PUT` i `GET` implementującymi operacje.


## Do roboty!

### Pobranie zasoby typu witryna

1. W kontrolerze `WebsiteController` dodajemy operację pobrania zasobu typu witryna:

```csharp
[HttpGet("/websites/{url}")]
public IActionResult GetWebsite(string url)
{
    var website = context.Websites.FirstOrDefault(w => w.Url == url);
    if (website == null)
    {
        return NotFound($"Witryna {url} nie została zarejestrowana.");
    }

    return Json(new ViewModel.Website
    {
        Url = website.Url
    });
}
```


### Rejestracja witryny

1. Po zarejestrowaniu witryny zwracamy kod `HTTP 201` i URL do zasobu w nagłówku `Location`:

```csharp
[HttpPost("/websites")]
public IActionResult RegisterWebsite([FromBody] ViewModel.Website website)
{
    context.Websites.Add(new Website
    {
        Url = website.Url
    });
    context.SaveChanges();

    return CreatedAtAction(nameof(GetWebsite), new { url = website.Url }, null);
}
```


### Identyfikator witryny w reprezentacji hasła

1. W klasie `Credentials` w folderze `ViewModel` zmieniamy nazwę `Url` na `WebsiteUri`.
1. Podczas konstrukcji reprezentacji hasła zwracamy URI zasobu typu witryna:

```csharp
// ...
var websiteUri = Url.Action("GetWebsite", "Website", new { url = website.Url }, Request.Scheme);

return Json(new ViewModel.Credentials
{
    WebsiteUri = websiteUri,
    Login = website.Credentials.Login,
    Password = website.Credentials.Password
});
```


### Identyfikator hasła w reprezentacji witryny

1. W klasie `Website` w folderze `ViewModel` dodajemy nowe pole `PasswordUri`.
1. Zwracając reprezentację witryny nadajemy wartość temu polu tylko, jeżeli zostało wygenerowane lub przypisane hasło:

```csharp
// ...
var website = context.Websites
    .Include(w => w.Credentials)
    .FirstOrDefault(w => w.Url == url);
    
string passwordUri = null;

if (website.Credentials != null)
{
    passwordUri = Url.Action("GetPassword", "Credentials", new {url = website.Url}, Request.Scheme);
}

return Json(new ViewModel.Website
{
    Url = website.Url,
    PasswordUri = passwordUri
}, new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore
});
```


## Testujemy

1. Uruchamiamy serwis (Ctrl+F5).
1. Za pomocą Postmana testujemy następujący scenariusz:
  1. Rejestrujemy nową witrynę
  1. Pobieramy reprezentację zarejestrowanej witryny
  1. Generujemy hasło dla loginu w tej witrynie
  1. Pobieramy reprezentację zarejestrowanej witryny

Wszystkie żądania powinny używać odpowiedniej dla typu operacji metody HTTP. Używamy tylko linków zwróconych w reprezentacji zasobów.
