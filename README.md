# Database
```bash
sudo docker run -d --restart always --name development-database \
  -p 5432:5432 \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=developer-password \
  -e POSTGRES_DB=devdb \
  -v /home/carl/Docker/postgres:/var/lib/postgresql/data \
  postgres:17.0
```

## install entity framework cli tool
```bash
dotnet tool install --global dotnet-ef
```
or
```bash
dotnet tool update --global dotnet-ef
```

## create migration if there are changes to the database
```bash
dotnet ef migrations add InitialCreateGmod --project ./Api
```

## update database with latest migration
```bash
dotnet ef database update --project ./Api
```