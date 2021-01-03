# MTCG

## :package: Requirements

PostgreSQL Server

Tested with `Postgres 12` under `WSL2`.

## 🛠 Build 

```
dotnet build --configuration Release
```

## 🚴‍♂️Run

Run MTCG server (port defaults to `8080`)

```
dotnet run --project ./MTCG/MTCG.csproj
```

## 🧪 Test

Run unit tests

```
dotnet test
```

For the integration CURL and/or [Postman](https://www.postman.com/) is needed.

The curl-batch scripts can be found under `MTCG-Test/Integration/curl`.

The test collections for Postman can be found under `MTCG-Test/Integration/postman`.

For both cases two integrations test-suites can be found, one for the basic requirements and one that tests bonus features.

> Note: After every integration test run, the database needs to be dropped:
>
> ```sql
> DROP DATABASE mtcg;
> ```



## `MTCG` Protocol

### Design

![](res/MTCG_UML_Diagram.png)







### Lessons Learned



### Unit test design



### Time spent



### Link to git







