# Add initial migration
```sh
cd .\TimeManagementService.DataAccess.Migrations\
dotnet ef migrations add InitialCreate
```

# Build migration script
```sh
dotnet ef migrations script --idempotent --project TimeManagementService.DataAccess.Migrations --startup-project TimeManagementService.DataAccess.Migrations --output Databases/TimeManagementServiceDB.sql
```