# EquiMarket

Projektuppgift för DT191G

## Uppgiften

Projektuppgiften gick ut på att skapa en webbplats med .NET och EF Core som på något vis har stöd för autentisering i form av inloggning. Det ska finnas CRUD-funktionalitet och de genererade sidorna ska validera enligt W3C:s standarder (både CSS och HTML).
Data från databasen ska presenteras på något vis för oinloggade användare.

## Lösning

EquiMarket är en köp- och säljsida för hästar, där en oinloggad användare kan bläddra bland annonser och se enskilda annonser. För att lägga till, uppdatera eller radera annonser krävs ett användarkonto. Det går endast att redigera eller radera annonser skapade av det konto man är inloggad på.

### Extra funktionalitet

Det finns ett filtreringsformulär för annonser med diverse fält. Det finns också paginering, och man kan som inloggad användare se alla sina annonser från sin profil för en enkel överblick.

## Utveckling

Projektet är skapat med MVC och EF Core mot en SQLite-databas. Projektet är inte publicerat på en hostingplattform utan finns endast på github, och testning görs lokalt.
