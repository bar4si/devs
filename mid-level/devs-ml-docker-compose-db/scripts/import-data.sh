#!/bin/bash

# Wait for MSSQL to be ready
echo "Starting readiness check for SQL Server..."
for i in {1..60};
do
    echo "Attempt $i: Checking if SQL Server is ready..."
    /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P $MSSQL_SA_PASSWORD -C -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]
    then
        echo "SQL Server is ready - starting data import"
        /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P $MSSQL_SA_PASSWORD -C -v dbname=$MSSQL_DB -i /usr/config/init.sql
        echo "Data import completed"
        break
    else
        echo "SQL Server is not ready yet... (attempt $i)"
        sleep 2
    fi
done
