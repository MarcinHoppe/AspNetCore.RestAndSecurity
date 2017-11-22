# Własna logika autoryzacji

## Co chcemy osiągnąć?

Chcemy zaimplementować dwie polityki autoryzacji, które:

1. Wymuszą dostęp jedynie dla użytkowników z domeny `@szkolenie.com`.
1. Wymuszą dostęp do metody `DELETE` jedynie dla użytkowników z działu `IT`.


## Co mamy na starcie?

Na starcie mamy API z jednym kontrolerem oraz middleware, którzy uwierzytelnia użytkownika na podstawie następujących nagłówków:

1. `username`: musi być obecny, żeby użytkownik mógł zostać uwierzytelniony.
1. `email`: opcjonalny przy uwierzytelnianiu, ale wymagany przez polityki autoryzacji.
1. `department`: opcjonalny przy uwierzytelnianiu, ale wymagany przez polityki autoryzacji.


## Do roboty!

### Wymaganie: e-mail musi pochodzić z określonej domeny

1. Dodajemy nowy folder `Authorization`.
1. W nowym folderze dodajemy klasę `DomainRequirement`, która implementuje interfejs `IAuthorizationRequirement` (marker).
1. Dodajemy do klasy właściwość `Domain` typu `string` i inicjalizujemy ją za pomocą parametru konstruktora.
1. Implementujemy handler dla tego wymagania:

```csharp
public class DomainRequirementHandler : AuthorizationHandler<DomainRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DomainRequirement requirement)
    {
        if (context.User.Claims.Any(c => c.Type == ClaimTypes.Email && c.Value.EndsWith(requirement.Domain)))
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
```


### Wymaganie: pracownik musi być z określonego działu

1. Analogicznie jak powyżej dodajemy klasę `DepartmentRequirement` i `DepartmentRequirementHandler`.
1. Autoryzację wykonujemy w oparciu o claim `"department"`. Sprawdzamy całą wartość claimu a nie tylko końcówkę jak w w przypadku e-maila.


### Rejestrujemy wymagania i handlery oraz dodajemy polityki autoryzacji

1. W metodzie `ConfigureServices` rejestujemy wymagania w postaci polityk autoryzacji i dodajemy handlery do kontenera Dependency Injection:

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("OurDomain", builder => builder.AddRequirements(new DomainRequirement("@szkolenie.com")));
    options.AddPolicy("ITDept", builder => builder.AddRequirements(new DepartmentRequirement("IT")));
});

services.AddSingleton<IAuthorizationHandler, DomainRequirementHandler>();
services.AddSingleton<IAuthorizationHandler, DepartmentRequirementHandler>();
```


### Stosujemy polityki w kontrolerze

1. W kontrolerze stosujemy odpowiednie polityki w atrybucie `[Authorize]`:

```csharp
[Route("contacts")]
[Authorize(Policy = "OurDomain", AuthenticationSchemes = "HeaderAuth")]
public class ContactController : Controller
{
    // ...
    [HttpDelete("{email}")]
    [Authorize(Policy = "ITDept")]
    public IActionResult Delete(string email)
    {
        // ...
    }
}
```


## Testujemy

1. Uruchamiamy nasze API (Ctrl+F5).
1. Weryfikujemy, że anonimowe żądania (bez nagłówków) są odrzucane z kodem HTTP 401 Unauthorized.
1. Dodajemy nagłówek `username` i weryfikujemy, że użytkownik jest uwierzytelniony, ale nie ma uprawnień do wykonania operacji.
  1. Jaki kod HTTP możemy zaobserwować?
1. Dodajemy nagłówek `email` z wartością z domeny `@szkolenie.com` i weryfikujemy, że operacje `GET` i `PUT` działają prawidłowo.
  1. Jaki wynik zwraca operacja `DELETE`?
  1. Co się stanie, jeżeli e-mail użytkownika będzie z innej domeny?
1. Dodajemy nagłówek `department` o wartości `IT` i weryfikujemy, że operacje `DELETE` działa poprawnie.
  1. Co się stanie, jeżeli operację będzie próbował wykonać pracownik innego działu?
