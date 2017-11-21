# Uwierzytelniamy i autoryzujemy

## Co chcemy osiągnąć?

Chcemy zaimplementować własny mechanizm uwierzytelniania i autoryzacji oparty o dane przesłane w dwóch nagłówkach:

1. Nazwa użytkownika: `Login`.
1. Rola: `Role`.


## Co mamy na starcie?

Na starcie dostajemy pusty project ASP.NET Core z włączonym MVC i jednym prostym kontrolerem i jedną klasą modelu. Wszystkie żądania są akceptowane a użytkownicy pozostają anonimowi.


## Do roboty!

### Tworzymy middleware uwierzytelniający

1. Tworzymy i rejestrujemy nowy middleware, którego zadaniem będzie uwierzytelnienie użytkownika: `HeaderAuthenticationMiddleware`.
1. W metodzie `Configure` konfigurujemy użycie nowego pustego komponentu w potoku przetwarzania.


### Uwierzytelniamy użytkownika na podstawie nagłówków

1. W `HeaderAuthenticationMiddleware` middleware oczytujemy wartości nagłówków `Login` i `Role`.
1. Jeżeli oba są ustawione, tworzymy dwa obiekty `Claim`, po jednej dla każdej z wartości.
1. Nazwy typów claimów to, odpowiednio, `ClaimTypes.Name` oraz `ClaimTypes.Role`.
1. Na podstawie tych dwóch claimów tworzymy obiekt klasy `ClaimsIdentity`.
1. Jako parametr `authenticationType` podajmy wartość `"HeaderAuth"`.
1. Na podstawie obiektu opisującego tożsamość użytkownika (`ClaimsIdentity`) tworzymy obiekt `ClaimsPrincipal`.
1. Ten z kolei obiekt przypisujemy do właściwości `context.User`.
1. W ten sposób uwierzytelniliśmy użytkownika.


### Autoryzujemy dostęp

1. Tworzymy nowy filtr autoryzacyjny, czyli klasę implementującą `IAuthorizationFilter`.
1. W jedynej metodzie tego interfejsu sprawdzamy, czy:
  1. Użytkownik jest uwierzytelniony: `context.HttpContext.User.Identity.IsAuthenticated`
  1. Użytkownik jest w roli `"User"`: `context.HttpContext.User.IsInRole`.
1. Jeżeli chociaż jeden z powyższych warunków nie jest spełniony, zwracamy odpowiedź `HTTP 401 Forbidden`.
  1. Korzystamy z właściwości `context.Result` i klasy `UnauthorizedResult`.
1. Rejestrujemy filtr. Przypomnij sobie jak zarejestrowałeś filtr `RequireHttpsAttribute` w [ćwiczeniu 08](../08-https/README.md).

## Testujemy

1. Uruchamiamy API (Ctrl + F5).
1. Weryfikujemy, że jako nieuwierzytelniony użytkownik nie mamy dostępu do żadnych operacji.
1. W Postmanie dodajemy nagłówek z nazwą użytkownika i rolą `User`.
1. Weryfikujemy, że mamy dostęp do operacji `GET` i `PUT`, ale nie mamy dostępu do operacji `DELETE`.
1. W Postmanie dodajemy nagłówek z nazwą użytkownika i rolą `Guest`.
1. Weryfikujemy, że nie mamy dostępu do żadnych operacji.