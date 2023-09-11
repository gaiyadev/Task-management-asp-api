# Entity Framework Core (EF Core) CLI Commands

Here is a list of essential EF Core CLI commands for managing database migrations in your .NET Core or .NET 5/6 project.

## Creating Migrations

### `dotnet ef migrations add [Name]`

- **Description:** Creates a new migration with the specified name.
- **Example:** 
```bash 
 dotnet ef migrations add InitialCreate 
 ```

## Applying Migrations

### `dotnet ef database update`

- **Description:** Applies pending migrations to update the database schema.
- **Example:** 
```bash 
 dotnet ef database update
```

## Listing Migrations

### `dotnet ef migrations list`

- **Description:** Lists all available migrations.
- **Example:** 
```bash 
dotnet ef migrations list
```

## Removing Migrations

### `dotnet ef migrations remove`

- **Description:** Removes the last applied migration.
- **Example:** 
```bash 
dotnet ef migrations remove
```

## Dropping the Database

### `dotnet ef database drop`

- **Description:** Drops the database.
- **Example:** 
```bash
dotnet ef database drop
```

### `dotnet ef database drop --force`

- **Description:** Drops the database without asking for confirmation.
- **Example:** 
```bash
dotnet ef database drop --force
```

## Scaffolding DbContext and Entity Types

### `dotnet ef dbcontext scaffold`

- **Description:** Scaffold the DbContext and entity types for an existing database.
- **Example:** 
```bash
dotnet ef dbcontext scaffold "Server=YourServer;Database=YourDatabase;User=YourUser;Password=YourPassword;" Microsoft.EntityFrameworkCore.SqlServer -o Models
```

Please note that these commands should be executed from the root directory of your project, and you should have the necessary EF Core tools installed. These commands are vital for managing database migrations when using Entity Framework Core in your project.

Make sure to replace `[Name]`, `YourServer`, `YourDatabase`, `YourUser`, and `YourPassword` with appropriate values for your specific project and database configuration. Keep in mind that command syntax and options may change with newer versions of EF Core, so always refer to the official documentation or use `dotnet ef --help` for the most up-to-date information.

# Dropping All Migrations

To drop all migrations in Entity Framework Core (EF Core), follow these steps:

1. **Delete the Migrations Folder:**

    - In your project directory, locate the "Migrations" folder. This folder contains all the migration files.
    - Delete the entire "Migrations" folder and its contents.

2. **Remove Migrations from the Database:**

    - Open a command prompt or terminal in the root directory of your project.

    - Run the following command to remove all migrations from the database:

      ```bash
      dotnet ef database update 0
      ```

    - The `0` in the command specifies the target migration. By setting it to `0`, you are effectively rolling back all migrations to the initial state.

After following these steps, all migrations will be removed from both your project and the database, effectively resetting your database to its initial state. Please exercise caution when dropping migrations, as it will result in data loss if your database was previously populated with data from migrations.
