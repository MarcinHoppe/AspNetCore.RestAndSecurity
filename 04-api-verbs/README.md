# Poprawnie używamy metod

## Co chcemy osiągnąć?

Chcemy przenieść nasze API na poziom 2 w modelu Richardsona. W tym celu musimy poprawnie używać metod i kodów odpowiedzi HTTP:

1. Rejestracja nowej witryny pozostanie operacją `POST` (nic nie musimy zmieniać).
1. Operacja wygenerowania hasła pozostanie operację typu `POST`.
1. Operacja zapisu własnego hasła będzie operacją typu `PUT`.
1. Operacja pobrania hasła będzie operacją typu `GET`.
1. Poprawnie obsłużymy sytuację, w której witryna nie jest zarejestrowana.


## Co mamy na starcie?

Na starcie mamy API ze zidentyfikowanymi zasobami, ale wszystkimi operacjami realizowanymi jako operacje `POST`.

## Do roboty!

### Operacja wygenerowania hasła

1. W metodzie `CredentialsController.GeneratePassword` zmieniamy szablon URL w atrybucie `[HttpPost]`:

```csharp
[HttpPost("/websites/{url}/password/{login}")]
public IActionResult GeneratePassword(string url, string login)
{
    // ...
}
```

### Operacja zapisu własnego hasła

1. W metodzie `CredentialsController.StorePassword` zmieniamy atrybut `[HttpPost]` na `[HttpPut]` z szablonem URL poprzedniego punktu:

```csharp
[HttpPut("/websites/{url}/password/{login}")]
public IActionResult StorePassword(string url, string login, string password)
{
    // ...
}
```

### Operacja pobrania hasła

1. W metodzie `CredentialsController.GetPassword` zmieniamy atrybut `[HttpPost]` na `[HttpGet]`:

```csharp
[HttpGet("/websites/{url}/password")]
public IActionResult GetPassword(string url)
{
    // ...
}
```

### Poprawna obsługa braku zarejestrowanej witryny

1. W każdej z metod w klasie `CredentialsController` obsługujemy brak zarejestrowanej witryny:

```csharp
// ...
var website = context.Websites
    .Include(w => w.Credentials)
    .FirstOrDefault(w => w.Url == url);

if (website == null)
{
    return NotFound($"Witryna {url} nie została zarejestrowana.");
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
  1. Odczytujemy hasło dla niezarejestrowanej witryny
  1. Generujemy nowe hasło dla niezarejestrowanej witryny

Wszystkie żądania powinny używać odpowiedniej dla typu operacji metody HTTP.
