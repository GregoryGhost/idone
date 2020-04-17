# Подключения к БД
## Стандартная локальная БД
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "ConnectionStrings": {
        "default": "Server=(localdb)\\mssqllocaldb;Database=Idone.Tests;Trusted_Connection=True;ConnectRetryCount=0;"
    }
}
```

## Docker Db
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Debug",
            "System": "Information",
            "Microsoft": "Information"
        }
    },
    "ConnectionStrings": {
        "default": "Server=172.17.0.3,1433;Database=Idone.Tests;User ID=SA;pwd=<qweQWE1234>;Trusted_Connection=False;"
    }
}
```
### Использование Docker Db для Ubuntu
Перед использованием этих комманд, естественно, нужно установить Docker, Image MSSQL для Docker'а.
```
// Запуск СУБД
> sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<qweQWE1234>" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04

// Подключение к СУБД
> /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "<qweQWE1234>"

// Создание тестовой БД
> CREATE DATABASE [Idone.Tests]
```