# Modyfikacja żądań za pomocą proxy

## Co chcemy osiągnąć?

Chcemy zmodyfikować żądania i odpowiedzi HTTP za pomocą proxy Fiddler.


## Co mamy na starcie?

Postman, Fiddler i gotowe API REST na postman-echo.com. To wszystko, czego będziemy potrzebowali.


## Do roboty!

### Instalujemy Fiddlera

Pobieramy go ze strony [https://www.telerik.com/download/fiddler](https://www.telerik.com/download/fiddler) i instalujemy.


### Testujemy API bez atakującego

Za pomocą Postmana wysyłamy pod adres `http://postman-echo.com/post` żądanie `POST` z dwoma parametrami: `email=marcin.hoppe@acm.org` i `amount=1000`.


### Przechwytujemy żądanie

1. Zastawiamy pułapkę w Fiddlerze za pomocą polecenia `bpu postman-echo.com`.
1. Wysyłamy to samo żądanie.
1. W Fiddlerze modyfikujemy zwartość żądania i wybieramy opcję `Run to completion`.
1. Czy wszystkie pola mają wartość, której się spodziewaliśmy?


### Przechwytujemy odpowiedź

1. Wysyłamy to samo żądanie.
1. W Fiddlerze wybieramy opcję `Break on response`.
1. W Fiddlerze modyfikujemy zwartość odpowiedzi i wybieramy opcję `Run to completion`.
1. Czy wszystkie pola mają wartość, której się spodziewaliśmy?


### Sprzątamy po sobie

1. W Fiddlerze usuwamy pułapkę za pomocą polecenia `bpu`.
