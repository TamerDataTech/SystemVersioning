using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using DatabaseUtils.Model.Database;
using DataTech.System.Versioning.Models.Common;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using DataTech.System.Versioning.Extensions;
using static System.Reflection.Metadata.BlobBuilder;
using DataTech.System.Versioning.Models.Database.Request;
using System.Text;
using Sylvan.Data.Csv;
using System.IO;
using System.Formats.Asn1;

namespace DataTech.System.Versioning.Service.Db
{
    public class DatabaseService
    {

        public DatabaseService()
        {

        }

        #region Help Classes
        protected static void HandleError(Exception exception)
        {

        }

        public static void SetReaderValues(SqlDataReader reader, object parent)
        {
            var props = parent.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                // Ignore indexers
                .Where(prop => prop.GetIndexParameters().Length == 0)
                // Must be both readable and writable
                .Where(prop => prop.CanWrite && prop.CanRead);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                try
                {
                    var prop = props.FirstOrDefault(x => x.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

                    if (prop == null)
                        continue;

                    object value = null;

                    if (prop.PropertyType == typeof(string))
                    {
                        value = reader[prop.Name] == DBNull.Value ? String.Empty : reader[prop.Name].ToString();
                    }
                    else if (prop.PropertyType == typeof(byte))
                    {
                        value = reader[prop.Name] == DBNull.Value ? (byte)0 : Convert.ToByte(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        value = reader[prop.Name] == DBNull.Value ? 0 : Convert.ToInt32(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(long))
                    {
                        value = reader[prop.Name] == DBNull.Value ? 0 : Convert.ToInt64(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(decimal))
                    {
                        value = reader[prop.Name] == DBNull.Value ? 0 : Convert.ToDecimal(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        value = reader[prop.Name] == DBNull.Value ? 0 : Convert.ToDouble(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        value = reader[prop.Name] != DBNull.Value && Convert.ToBoolean(reader[prop.Name]);
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        value = reader[prop.Name] == DBNull.Value
                            ? new DateTime(1900, 1, 1)
                            : Convert.ToDateTime(reader[prop.Name]);
                    }

                    prop.SetValue(parent, value, null);
                }
                catch (Exception ex)
                {
                    //RollbarLog.WriteRollBarstringBuilder.AppendLine(ex, null, "BaseSErvice->SetReaderValues");
                }
            }
        }

        protected static void PrepareSqlCommand(SqlCommand sqlCommand)
        {
            sqlCommand.CommandTimeout = 0;
            sqlCommand.CommandType = CommandType.StoredProcedure;
        }
        #endregion

        public async Task<OperationResult<List<Database>>> GetDataBases(DbQuery<Database> query)
        {
            OperationResult<List<Database>> result = new OperationResult<List<Database>>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionStringNoCatalog()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = @"SELECT dbid AS Id, name AS Name 
	                                            FROM master.dbo.sysdatabases
                                                ORDER BY 2";

                        await conn.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            result.Response = new List<Database>();

                            while (await reader.ReadAsync())
                            {
                                Database database = new Database();
                                SetReaderValues(reader, database);
                                result.Response.Add(database);
                            }

                            result.Result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        public async Task<OperationResult<List<DatabaseTable>>> GetDataBaseTables(DbQuery<Database> query)
        {
            OperationResult<List<DatabaseTable>> result = new OperationResult<List<DatabaseTable>>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionString()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = @"SELECT 1 AS Id, TABLE_NAME AS Name
                                                FROM  INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME <> 'sysdiagrams' 
                                                ORDER BY 2";


                        //if (!string.IsNullOrEmpty(query.Conn.InitialCatalog))
                        //    comm.Parameters.AddWithValue("@pDatabase", query.Conn.InitialCatalog);


                        await conn.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            result.Response = new List<DatabaseTable>();

                            while (await reader.ReadAsync())
                            {
                                DatabaseTable databaseTable = new DatabaseTable();
                                SetReaderValues(reader, databaseTable);
                                result.Response.Add(databaseTable);
                            }

                            result.Result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        public async Task<OperationResult<List<DatabaseTable>>> GetLookupDataBaseTables(DbQuery<Database> query)
        {
            OperationResult<List<DatabaseTable>> result = new OperationResult<List<DatabaseTable>>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionString()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = @"SELECT [tableID] AS Id, [tableName] AS Name
                                                FROM  [dbo].[__prtTableTypes]
                                                WHERE [tableType] = 'Lookup' 
                                                ORDER BY 2";


                        await conn.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            result.Response = new List<DatabaseTable>();

                            while (await reader.ReadAsync())
                            {
                                DatabaseTable databaseTable = new DatabaseTable();
                                SetReaderValues(reader, databaseTable);
                                result.Response.Add(databaseTable);
                            }

                            result.Result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }


        public async Task<OperationResult<List<DatabaseColumn>>> GetDataBaseTableColumns(DbQuery<DatabaseTable> query)
        {
            OperationResult<List<DatabaseColumn>> result = new OperationResult<List<DatabaseColumn>>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionString()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = @"SELECT ORDINAL_POSITION AS Id, COLUMN_NAME AS Name, DATA_TYPE AS DataType, ISNULL(CHARACTER_MAXIMUM_LENGTH, 0) AS Length
                                                FROM INFORMATION_SCHEMA.COLUMNS
                                                WHERE TABLE_NAME = @TableName ";

                        comm.Parameters.AddWithValue("@TableName", query.Parameter.Name);
                        await conn.OpenAsync();

                        using (SqlDataReader reader = await comm.ExecuteReaderAsync())
                        {
                            result.Response = new List<DatabaseColumn>();

                            while (await reader.ReadAsync())
                            {
                                DatabaseColumn databaseColumn = new DatabaseColumn();
                                SetReaderValues(reader, databaseColumn);
                                result.Response.Add(databaseColumn);
                            }

                            result.Result = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        public async Task<OperationResult<Database>> CreateDatabase(DbQuery<Database> query)
        {
            OperationResult<Database> result = new OperationResult<Database>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionStringNoCatalog()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = $"CREATE DATABASE {query.Parameter.Name}";
                        await conn.OpenAsync();
                        await comm.ExecuteNonQueryAsync();
                        result.Result = true;
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        public async Task<OperationResult<bool>> ExecuteSql(DbQuery<DatabaseCommand> query)
        {
            OperationResult<bool> result = new OperationResult<bool>();

            try
            {

                using (SqlConnection conn = new SqlConnection(query.Conn.GetConnectionString()))
                {
                    using (SqlCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = query.Parameter.Command;
                        await conn.OpenAsync();
                        await comm.ExecuteNonQueryAsync();
                        result.Result = true;
                    }
                }

            }
            catch (Exception e)
            {
                HandleError(e);
                result.ErrorMessage = e.Message;
            }

            return result;
        }


        public async Task<OperationResult<string>> CreateAuditTables(DbQuery<CreateAuditTablesRequest> query)
        {
            OperationResult<string> result = new OperationResult<string>();
            StringBuilder stringBuilder = new StringBuilder();

            try
            {

                string targetDb = query.Parameter.TargetDatabase;
                string tableSuffix = query.Parameter.Suffix;
                // --------------------------------------------------------------------------------
                var sourceDb = query.Parameter.SourceDatabase;

                var newDb = query.Parameter.CreateNewDatabase;
                if (newDb)
                {
                    targetDb = query.Parameter.NewDatabaseName;

                    if (targetDb.IsEmpty())
                    {
                        result.ErrorMessage = "New Db name is not defined. Please write DB Name.";
                        return result;
                    }
                }

                if (targetDb.IsEmpty())
                {
                    result.ErrorMessage = "Target Db is not defined. Please Check target DB.";
                    return result;
                }

                //TODO: Shall we leave this
                if (targetDb.EqualsInsensitive(sourceDb))
                {
                    result.ErrorMessage = "Target Db and Source Db can not be same. Please Check target DB.";
                    return result;
                }

                var checkedTables = query.Parameter.Tables;

                if (checkedTables.Count < 1)
                {
                    result.ErrorMessage = "At least one table has to be checked.";
                    return result;
                }

                DbConnectionString dbConnectionString = query.Conn;

                // Generate New db if __New__
                // if new we have to check if same db exists before  
                if (newDb)
                {
                    var getDatabasesResult = await GetDataBases(new DbQuery<Database>
                    {
                        Conn = dbConnectionString
                    });

                    if (getDatabasesResult.Result)
                    {
                        if (getDatabasesResult.Response.Any(x => x.Name.Equals(targetDb, StringComparison.OrdinalIgnoreCase)))
                        {

                            result.ErrorMessage = "There is already target database with the same name.";
                            return result;
                        }
                    }

                    // Create Database -------------------------------------------------------------
                    //CREATE DATABASE testDB; 
                    stringBuilder.AppendLine($"Create Database {targetDb}");
                    var createDbResult = await CreateDatabase(new DbQuery<Database>
                    {
                        Conn = dbConnectionString,
                        Parameter = new Database { Name = targetDb }
                    });

                    if (createDbResult.Result)
                    {
                        stringBuilder.AppendLine($"{targetDb} Database created successfully!");
                    }
                    else
                    {
                        stringBuilder.AppendLine($"{targetDb} Database was not created! Error: {createDbResult.ErrorMessage}");

                        return StopCleanAndReturn(stringBuilder);

                    }
                }
                // For each table ------------------------------------------------------------------
                stringBuilder.AppendLine($"Checking Tables");
                var targetTables = new List<DatabaseTable>();
                if (!newDb)
                {
                    dbConnectionString.InitialCatalog = targetDb;
                    var getDatabaseTables = await GetDataBaseTables(new DbQuery<Database>
                    {
                        Conn = dbConnectionString
                    });

                    if (!getDatabaseTables.Result)
                    {
                        stringBuilder.AppendLine($"Could not get {targetDb} database tables!");
                        return StopCleanAndReturn(stringBuilder);
                    }

                    targetTables = getDatabaseTables.Response;
                }


                foreach (var checkedTable in checkedTables)
                {
                    var sourceTable = checkedTable;

                    var logPrefix = "\t";

                    stringBuilder.AppendLine($"Checking table {sourceTable}!");
                    // Get table columns from source -----------------------------------------------------------  
                    dbConnectionString.InitialCatalog = sourceDb;
                    var columnsResult = await GetDataBaseTableColumns(new DbQuery<DatabaseTable>
                    {
                        Conn = dbConnectionString,
                        Parameter = new DatabaseTable
                        {
                            Name = sourceTable
                        }
                    });

                    if (!columnsResult.Result)
                    {
                        stringBuilder.AppendLine($"{logPrefix} Could not get {sourceTable} database table columns! Error: {columnsResult.ErrorMessage}");
                        return StopCleanAndReturn(stringBuilder);
                    }

                    var tableColumns = columnsResult.Response;

                    string logTableName = sourceTable + tableSuffix;

                    var targetTable = targetTables.FirstOrDefault(t => t.Name.EqualsInsensitive(logTableName));

                    if (targetTable == null)
                    {
                        stringBuilder.AppendLine($"{logPrefix} Creating log table {logTableName} on target Db {targetDb} ");

                        string createTableCommandText = $" USE {targetDb}; ";
                        createTableCommandText += $"CREATE TABLE {logTableName} ( ";
                        createTableCommandText += "AuditId BIGINT IDENTITY(1,1) NOT NULL, [AuditAction] [CHAR](1) NOT NULL, [AuditDate] [datetime] NOT NULL, [AuditUser] [NVARCHAR](50) NULL, [AuditApp] [NVARCHAR](128) NULL";
                        for (int i = 0; i < tableColumns.Count; i++)
                        {
                            var tableColumn = tableColumns[i];

                            createTableCommandText += ", ";
                            createTableCommandText += "[" + tableColumn.Name + "] " + tableColumn.DataType + GetColumnLengthPhrase(tableColumn);
                        }
                        createTableCommandText += @" CONSTRAINT [PK_audit_" + logTableName + @"] PRIMARY KEY CLUSTERED 
                            (
	                            [AuditId] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                            ) ON [PRIMARY]
 
                            ;";
                        //TODO: Check if PK constraint is existed before 

                        // Execute
                        dbConnectionString.InitialCatalog = targetDb;
                        var executeResult = await ExecuteSql(new DbQuery<DatabaseCommand>
                        {
                            Conn = dbConnectionString,
                            Parameter = new DatabaseCommand
                            {
                                Command = createTableCommandText
                            }
                        });

                        if (!executeResult.Result)
                        {

                            stringBuilder.AppendLine($"{logPrefix} Could not create {logTableName} database table! Error: {executeResult.ErrorMessage}");
                            return StopCleanAndReturn(stringBuilder);
                        }
                        else
                        {
                            stringBuilder.AppendLine($"{logPrefix} {logTableName} database table created!");
                        }
                    }
                    else
                    {
                        dbConnectionString.InitialCatalog = targetDb;
                        var targetColumnsResult = await GetDataBaseTableColumns(new DbQuery<DatabaseTable>
                        {
                            Conn = dbConnectionString,
                            Parameter = new DatabaseTable
                            {
                                Name = logTableName
                            }
                        });

                        if (!targetColumnsResult.Result)
                        {
                            stringBuilder.AppendLine($"{logPrefix} Could not get target {logTableName} database table columns! Error: {targetColumnsResult.ErrorMessage}");
                            return StopCleanAndReturn(stringBuilder);
                        }

                        var targetColumns = targetColumnsResult.Response;

                        // Check for  missed columns  
                        var missedColumns = tableColumns.Where(x => !targetColumns.Any(y => y.Name.EqualsInsensitive(x.Name))).ToList();
                        if (missedColumns.Any())
                        {
                            string command = $" USE {targetDb}; ";
                            foreach (var missedColumn in missedColumns)
                            {
                                command += $" ALTER TABLE {logTableName} ADD {missedColumn.Name} {missedColumn.DataType}{GetColumnLengthPhrase(missedColumn)};" + Environment.NewLine;
                            }


                            // Execute
                            dbConnectionString.InitialCatalog = targetDb;
                            var executeResult = await ExecuteSql(new DbQuery<DatabaseCommand>
                            {
                                Conn = dbConnectionString,
                                Parameter = new DatabaseCommand
                                {
                                    Command = command
                                }
                            });

                            if (!executeResult.Result)
                            {

                                stringBuilder.AppendLine($"{logPrefix} Could not add {logTableName} missed columnss! Error: {executeResult.ErrorMessage}");
                                return StopCleanAndReturn(stringBuilder);
                            }
                            else
                            {
                                stringBuilder.AppendLine($"{logPrefix} {logTableName} database table missed columns added!");
                            }

                        }
                    }

                    //stringBuilder.AppendLine($"Get source table  {sourceTable} columns ");
                    //// Check if it exists on Target (if new db created no need) ------------------------
                    //stringBuilder.AppendLine($"Check if it exists on Target (if new db created no need)");
                    //// Create new table if not exists OR Check if missed columns created and add them --
                    //stringBuilder.AppendLine($"Create new table if not exists OR Check if missed columns created and add them");
                    // Drop triggers and recreate them ------------------------------------------------- 
                    //stringBuilder.AppendLine($"Drop triggers and recreate them");

                    string columsScript = "";

                    for (int i = 0; i < tableColumns.Count; i++)
                    {
                        var tableColumn = tableColumns[i];

                        columsScript += ",";

                        columsScript += "[" + tableColumn.Name + "]" + Environment.NewLine;
                    }


                    string dropAllTriiggersScript = @"
                        USE " + sourceDb + @";
                        DROP TRIGGER IF EXISTS TR_" + sourceTable + @"_INSERT;
                        DROP TRIGGER IF EXISTS TR_" + sourceTable + @"_UPDATE;
                        DROP TRIGGER IF EXISTS TR_" + sourceTable + @"_DELETE;  ";

                    // Execute
                    dbConnectionString.InitialCatalog = sourceDb;
                    var executeResultDrop = await ExecuteSql(new DbQuery<DatabaseCommand>
                    {
                        Conn = dbConnectionString,
                        Parameter = new DatabaseCommand
                        {
                            Command = dropAllTriiggersScript
                        }
                    });

                    if (!executeResultDrop.Result)
                    {

                        stringBuilder.AppendLine($"Could not drop {sourceTable} triggers! Error: {executeResultDrop.ErrorMessage}");
                        return StopCleanAndReturn(stringBuilder);
                    }
                    else
                    {
                        stringBuilder.AppendLine($"{logPrefix} {sourceTable} triggers dropped!");
                    }

                    dbConnectionString.InitialCatalog = sourceDb;

                    string createInsertTriggerScript = @" 
                        CREATE TRIGGER TR_" + sourceTable + @"_INSERT
                            ON  " + sourceTable + @" 
                            FOR INSERT
                        AS 
                        BEGIN 
	                        SET NOCOUNT ON;

	                        INSERT INTO " + targetDb + @".[dbo]." + logTableName + @"
                                    ([AuditAction]
                                    ,[AuditDate]
                                    ,[AuditUser]
                                    ,[AuditApp]
                                    " + columsScript + @")
		                        SELECT 'I'
                                    ,GETDATE()
                                    ,'System'
                                    ,'' 
                                    " + columsScript + @"
		                        FROM INSERTED  
                        END 

                ";


                    var executeResultInsert = await ExecuteSql(new DbQuery<DatabaseCommand>
                    {
                        Conn = dbConnectionString,
                        Parameter = new DatabaseCommand
                        {
                            Command = createInsertTriggerScript
                        }
                    });

                    if (!executeResultInsert.Result)
                    {

                        stringBuilder.AppendLine($"{logPrefix} Could not create {sourceTable} Insert trigger! Error: {executeResultInsert.ErrorMessage}");
                        return StopCleanAndReturn(stringBuilder);
                    }
                    else
                    {
                        stringBuilder.AppendLine($"{logPrefix} {sourceTable} Insert trigger created!");
                    }

                    string createUpdateTriggerScript = @" 
                        
                        CREATE TRIGGER TR_" + sourceTable + @"_UPDATE
                            ON  " + sourceTable + @" 
                            FOR UPDATE
                        AS 
                        BEGIN 
	                        SET NOCOUNT ON;

	                        INSERT INTO " + targetDb + @".[dbo]." + logTableName + @"
                                    ([AuditAction]
                                    ,[AuditDate]
                                    ,[AuditUser]
                                    ,[AuditApp]
                                    " + columsScript + @")
		                        SELECT 'U'
                                    ,GETDATE()
                                    ,'System'
                                    ,'' 
                                    " + columsScript + @"
		                        FROM INSERTED  
                        END 

                ";


                    var executeResultUpdate = await ExecuteSql(new DbQuery<DatabaseCommand>
                    {
                        Conn = dbConnectionString,
                        Parameter = new DatabaseCommand
                        {
                            Command = createUpdateTriggerScript
                        }
                    });

                    if (!executeResultUpdate.Result)
                    {

                        stringBuilder.AppendLine($"{logPrefix} Could not create {sourceTable} Update trigger! Error: {executeResultUpdate.ErrorMessage}");
                        return StopCleanAndReturn(stringBuilder);
                    }
                    else
                    {
                        stringBuilder.AppendLine($"{logPrefix} {sourceTable} Update trigger created!");
                    }


                    string createDeleteTriggerScript = @" 
                        
                        CREATE TRIGGER TR_" + sourceTable + @"_DELETE
                            ON  " + sourceTable + @" 
                            FOR DELETE
                        AS 
                        BEGIN 
	                        SET NOCOUNT ON;

	                        INSERT INTO " + targetDb + @".[dbo]." + logTableName + @"
                                    ([AuditAction]
                                    ,[AuditDate]
                                    ,[AuditUser]
                                    ,[AuditApp]
                                    " + columsScript + @")
		                        SELECT 'D'
                                    ,GETDATE()
                                    ,'System'
                                    ,'' 
                                    " + columsScript + @"
		                        FROM DELETED  
                        END 

                ";


                    var executeResultDelete = await ExecuteSql(new DbQuery<DatabaseCommand>
                    {
                        Conn = dbConnectionString,
                        Parameter = new DatabaseCommand
                        {
                            Command = createDeleteTriggerScript
                        }
                    });

                    if (!executeResultDelete.Result)
                    {

                        stringBuilder.AppendLine($"{logPrefix} Could not create {sourceTable} Delete trigger! Error: {executeResultDelete.ErrorMessage}");
                        return StopCleanAndReturn(stringBuilder);
                    }
                    else
                    {
                        stringBuilder.AppendLine($"{logPrefix} {sourceTable} Delete trigger created!");
                    }

                    // ---------------------------------------------------------------------------------
                }


                return StopCleanAndReturn(stringBuilder, true);
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                return StopCleanAndReturn(stringBuilder);
            }
        }

        private OperationResult<string> StopCleanAndReturn(StringBuilder stringBuilder, bool succeeded = false)
        {
            return new OperationResult<string>(stringBuilder.ToString())
            {
                Result = succeeded,
                ErrorMessage = !succeeded ? "An error happened while processing current task. Please check logs to track the error!" : ""
            };
        }

        private string GetColumnLengthPhrase(DatabaseColumn tableColumn)
        {
            return (tableColumn.Length > 0 ? $"({tableColumn.Length})" : (tableColumn.Length < 0 ? $"(MAX)" : ""));
        }

        public async Task<OperationResult<string>> CreateTableBackup(DbQuery<BackupTablesRequest> query)
        {
            OperationResult<string> result = new OperationResult<string>();
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                var sourceDb = query.Parameter.SourceDatabase;

                DbConnectionString dbConnectionString = query.Conn;

                // For each table ------------------------------------------------------------------
                stringBuilder.AppendLine($"Checking Tables");

                List<string> checkedTables = new List<string>();

                dbConnectionString.InitialCatalog = sourceDb;

                var getSourceDbTables =
                    query.Parameter.LookupTablesOnly ?
                        await GetLookupDataBaseTables(new DbQuery<Database> { Conn = dbConnectionString }) :
                        await GetDataBaseTables(new DbQuery<Database> { Conn = dbConnectionString });

                using SqlConnection conn = new SqlConnection(dbConnectionString.GetConnectionString());
                await conn.OpenAsync();
                foreach (var checkedTable in getSourceDbTables.Response)
                {
                    try
                    {
                        var sourceTable = checkedTable.Name;

                        var logPrefix = "\t";

                        stringBuilder.AppendLine($"Creating backup file for table {sourceTable}");
                        // Get table columns from source -----------------------------------------------------------  

                        // Read the schema for the target table
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = $"SELECT * FROM {sourceTable}";
                        using var reader = cmd.ExecuteReader();
                        var tableSchema = reader.GetColumnSchema();

                        // apply the schema of the target SQL table to the CSV data reader
                        // this is require for the SqlBulkCopy to process the data as the
                        // correct type without manual conversions.
                        var options =
                            new CsvDataWriterOptions
                            {
                                Delimiter = ',',

                            };

                        string filePath = $"c:\\temp\\csv\\{sourceTable}.csv";

                        using var csv = CsvDataWriter.Create(filePath, options);
                        await csv.WriteAsync(reader);

                        // ---------------------------------------------------------------------------------

                    }
                    catch (Exception exInner)
                    {
                        stringBuilder.AppendLine(exInner.Message); 
                    } 
                } 

                return StopCleanAndReturn(stringBuilder, true);
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                return StopCleanAndReturn(stringBuilder);
            }
        }
    }
}
