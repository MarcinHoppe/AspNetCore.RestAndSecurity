# Chronimy dostęp do API za pomocą IdentityServer4

## Co chcemy osiągnąć?

Chcemy zdobyć token, który da nam dostęp do naszego API.


## Co mamy na starcie?

Na starcie mamy minimalne API oraz IdentityServer4 ze skonfigurowanym pojedynczym klientem.


## Do roboty!

### IdentityServer4 z client credentials

1. Budujemy rozwiązanie.
1. Uruchamiamy (Ctrl+F5) obie aplikacje.
1. Rozglądamy się po kodzie :).

### Testujemy API anonimowo

1. Testujemy API za pomocą Postmana.
1. Czy udało się zdobyć dostęp?


### Pobieramy token z IdentityServera

1. W Postmanie wybieramy zakładkę Authorization i typ OAuth 2.0.
1. Generujemy nowy _access token_.
1. Wybieramy _grant type_: `Client Credentials`.
1. Podajemy _access token URL_: http://localhost:5000/connect/token.
1. Podajemy identyfikator klienta, sekret i pożądany scope: `client`, `secret`, `api1`.
  1. Skąd pochodzą te wartości?

### Testujemy API z tokenem

1. Testujemy API za pomocą Postmana, tym razem z tokenem.
1. Czy udało się zdobyć dostęp?


### Zadanie z *

1. Wklejamy token na stronie jwt.io.
1. Jakie informacje są zawarte w tokenie?
