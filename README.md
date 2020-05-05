# Currency Fetcher

Currency Fetcher is Web API project to fetch data from https://sdw-wsrest.ecb.europa.eu/help/ web service and generate currency information based on 

  - The currency being measured (e.g.: US dollar - code USD),
  - The currency against which a currency is being measured (e.g.: Euro - code EUR),
  - With range of dates

To set up web service read below informations:

### Project code overview

* [CurrencyFetcherApi] - Main project contains web api controllers
* [CurrencyFetcherApi.Tests] - Unit tests project for CurrencyFetcherApi project
* [CurrencyFetcher.Core] - Core project contains Services, model, entities, db context classes
* [CurrencyFetcher.Core.Tests] - Unit tests project for CurrencyFetcher.Core project

### Installation

1. To set up web service change connection strings value in localizations:
- {path_to_repo}/webAPI/CurrencyFetcherApi/CurrencyFetcherApi/appsettings.json
- change ConnectionString value into own database connection string
- Provide the connection string in second localization:
```sh
$ {path_to_repo}/webAPI/CurrencyFetcherApi/CurrencyFetcherApi/NLog.config
``` 
in node:
```sh
$ targets/target.connectionString
```

This configures nlog logger to save logs into database

2. Install dotnet ef tool. The installation link is below:
https://docs.microsoft.com/pl-pl/ef/core/miscellaneous/cli/dotnet

3. Run any common line terminal tool and move to directory:
{path_to_repo}/webAPI/CurrencyFetcherApi/CurrencyFetcher.Core/

4. Run command:
```sh
$ dotnet ef database update
```

After that scripts will create database and configure it to use web service.
Run application with any c# editor like Visual Studio 2019 and on screen will be show
Swagger UI to help communicate with web api service


- Default authentication for generate JWT Token is
```sh
$ Username=> currency
$ Password=> Currency123)(*
```

### Web API Endpoints:

1. /api/Token
- method: POST
- in default token expires in 7 hours
- this can be change in appsettings file -> [appsettings] (Jwt.ExpiresInHours)
- example of body:
```sh
{
	"Username": "currency",
	"Password": "Currency123)(*"
}
```

- example of response:
```sh
{
  "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjZjdhZDViMC05Y2Y5LTRjYjktOGYyOS0wZTgyOTdmMGVlMzciLCJqdGkiOiJmOWIzZjExYi1lODI4LTQyODktYjM0OC1iNzRiYzRjMWUxZDkiLCJleHAiOjE1ODg3MDU5MDYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzE3IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzMTcifQ.LCK99o3Fd8IviunPAbL2qWxXwSnTvA1C7X9l6ctRuO0"
}
```

2. /api/Currency 
- method: POST
- example of body:
```sh
 {
 	"CurrencyCodes": {
 		"PLN": "EUR",
 		"USD": "EUR",
		"NOK": "EUR"
},
"StartDate": "2010-05-28",
"EndDate": "2010-06-30",
"apiKey": "secret token string"
}

```
- example of response:
```sh
[
  {
    "currencyBeingMeasured": "PLN",
    "currencyMatched": "EUR",
    "currencyValue": 3.99,
    "dailyDataOfCurrency": "2010-05-14T00:00:00"
  },
  {
    "currencyBeingMeasured": "PLN",
    "currencyMatched": "EUR",
    "currencyValue": 4.02,
    "dailyDataOfCurrency": "2010-05-17T00:00:00"
  },
  {
    "currencyBeingMeasured": "PLN",
    "currencyMatched": "EUR",
    "currencyValue": 4,
    "dailyDataOfCurrency": "2010-05-18T00:00:00"
  },
  {
    "currencyBeingMeasured": "PLN",
    "currencyMatched": "EUR",
    "currencyValue": 4.08,
    "dailyDataOfCurrency": "2010-05-19T00:00:00"
  },
  {
    "currencyBeingMeasured": "PLN",
    "currencyMatched": "EUR",
    "currencyValue": 4.19,
    "dailyDataOfCurrency": "2010-05-20T00:00:00"
  }
]
```

### Techechnologies used in this project

Dillinger uses a number of open source projects to work properly:

* [ASP.NET Core 3.1] - The better vesion of MVC 5
* [Entity Framework] - The data access technology that provide functionality
to work with database with linq queries
* [Entity Framework Migrations] - The solution to provide migrations that can build databases by code
* [Sql Server] - The great database engine provide fexibility and is excelent to work with Enity Framework
* [JWT] - JSON web token to provide login functionality
* [nlog] - To better create logger functionlity in service
* [nUnit] - The ane of the best solution for creating unit tests
* [Moq] - To mock interfaces as testing object in test scenarioes
* [Swagger] - To better tests service by Swagger UI

License
----
apache 2.0

**Free Software, Hell Yeah!**

[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)


   [ASP.NET Core 3.1]: <https://dotnet.microsoft.com/download/dotnet-core/3.1>
   [Entity Framework]: <https://docs.microsoft.com/pl-pl/ef/core/>
   [Entity Framework Migrations]: <https://docs.microsoft.com/en-gb/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli>
   [Sql Server]: <https://www.microsoft.com/en-gb/sql-server/sql-server-downloads>
   [nlog]: <https://github.com/NLog>
   [nUnit]: <https://nunit.org/>
   [Moq]: <https://www.nuget.org/packages/moq/>
   [Swagger]: <https://swagger.io/>
   [JWT]: <https://jwt.io/>
   [CurrencyFetcherApi]: <https://github.com/kmaraszkiewicz86/currency_fetcher/tree/master/webAPI/CurrencyFetcherApi/CurrencyFetcherApi>
   [CurrencyFetcherApi.Tests]: <https://github.com/kmaraszkiewicz86/currency_fetcher/tree/master/webAPI/CurrencyFetcherApi/Tests/CurrencyFetcherApi.Tests>
   [CurrencyFetcher.Core]: <https://github.com/kmaraszkiewicz86/currency_fetcher/tree/master/webAPI/CurrencyFetcherApi/CurrencyFetcher.Core>
   [CurrencyFetcher.Core.Tests]: <https://github.com/kmaraszkiewicz86/currency_fetcher/tree/master/webAPI/CurrencyFetcherApi/Tests/CurrencyFetcher.Core.Tests>
   [appsettings]: <https://github.com/kmaraszkiewicz86/currency_fetcher/blob/master/webAPI/CurrencyFetcherApi/appsettings.json>

