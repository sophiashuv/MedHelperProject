# MedHelperProject

## Clone

- Clone this repo to your local machine using `https://github.com/pavlozab/lnu-team-project`

## Migrations

- Create new migration.

CLI

```shell
$ dotnet ef migrations add NewMigration
```

PM

```shell
> add-migration NewMigration
```

- Update database.

CLI

```shell
$ dotnet ef database update
```

PM

```shell
> update-database -verbose
```
