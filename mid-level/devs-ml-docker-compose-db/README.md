# 🐳 MSSQL Server - Docker Compose

SQL Server 2022 database infrastructure ready for local development use, pre-configured with industrial sample data.

## 🚀 Getting Started

1.  Ensure you have **Docker Desktop** or **Docker Compose** installed.
2.  (Optional) Create a `.env` file based on `.env.example`:
    - `COMPOSE_PROJECT_NAME`: Customizes the Docker prefix for containers/volumes.
    - `MSSQL_PORT`: Customizes the external port (default is 1433).
    - `MSSQL_SA_PASSWORD`: Sets the `sa` user password.
    - `MSSQL_DB`: Sets the initial database name (default is IndustrialDb).
3.  Run the command:
    ```bash
    docker-compose up -d
    ```

**Note:** The system will automatically create the `IndustrialDb` and seed it with initial data upon startup through the `sqlserver-db-init` container.

## 📊 Initial Seed Data

The database is pre-populated with:
- **Tasks**: Industrial maintenance and operation tasks (Boilers, CNC, Safety).
- **SensorReadings**: 20 historical readings per sensor (Temperature, Pressure, Vibration) simulating the last 2 hours.

## 🛠 Connection Details

- **Server:** `localhost`
- **Port:** `${MSSQL_PORT:-1433}`
- **User:** `sa`
- **Password:** `YourStrong@Password123` (or as defined in your `.env`)
- **Database:** `${MSSQL_DB:-IndustrialDb}`

## 🔗 TaskMaster API Integration

In your API's `appsettings.json` file, use the following Connection String:

```json
"DefaultConnection": "Server=localhost;Database=IndustrialDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
```

> [!IMPORTANT]
> Raw data is persisted through a named Docker volume called `sql-data`. This ensures your data remains intact even if the container is restarted or recreated.
