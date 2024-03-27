
var Auditing = function () {

    var connection = {
        conn: {},
        databases: [],
        tables: []
    };
    var prepareEvents = function () {

        $('#connect-to-server').off('click').on('click', function () {

            $("#databases-container").addClass("d-none");
            $("#databases").val();
            clearTablesArea();
            connection = {
                conn: {},
                databases: [],
                tables: []
            };

            var dbServer = $('#db-server').val();

            if (dbServer.trim() == "") {
                General.notifyFailure("Please enter the database server to continue!");
                $('#db-server').focus();
                return false;
            }

            var useWindowsAuth = $('#db-windows-authentication').is(':checked');

            var dbUsername = "";
            var dbPassword = "";


            if (!useWindowsAuth) {
                dbUsername = $('#db-server-username').val();

                if (dbUsername.trim() == "") {
                    General.notifyFailure("Please enter the database server username to continue!");
                    $('#db-server-username').focus();
                    return false;
                }

                dbPassword = $('#db-server-password').val();

                if (dbPassword.trim() == "") {
                    General.notifyFailure("Please enter the database server password to continue!");
                    $('#db-server-password').focus();
                    return false;
                }
            }

            var conn = {
                DataSource: dbServer,
                WindowsAuthenticaton: useWindowsAuth,
                UserId: dbUsername,
                Password: dbPassword
            }

            getDatabases(conn, (databases) => {
                $("#databases").html(General.renderTemplate("#tmp-databases", { databases: databases }));
                $("#target-databases").html(General.renderTemplate("#tmp-target-databases", { databases: databases }));


                $("#databases-container").removeClass("d-none");
                connection.conn = conn;
                connection.databases = databases;
            });

        });


        $('#databases').off('change').on('change', function () {
           
            clearTablesArea();

            var database = $(this).val();

            connection.conn.InitialCatalog = database; 

            if (database.trim() == "" || database.trim() == "_Choose_Database_") {
                return false;
            } 

            getTables(connection.conn, (tables) => {
                $("#tables").html(General.renderTemplate("#tmp-tables", { tables: tables }));

                $("#tables-container").removeClass("d-none");
                connection.tables = tables;
            });

        });


        $('#check-all-tables').off('click').on('click', function () {

            $('.table-check').prop("checked", true);

        });

        $('#uncheck-all-tables').off('click').on('click', function () {

            $('.table-check').prop("checked", false);

        });

        //target-databases
        $('#target-databases').off('change').on('change', function () {

            $("#new-database-name-container").addClass("d-none");

            var targetDatabase = $(this).val();
            if (targetDatabase === "_New_Database_") {
                $("#new-database-name-container").removeClass("d-none");
            }
        });

        //CreateAuditTables
        $('#create-audit-tables').off('click').on('click', function () {
            $("#log-area").val('');

            var targetDatabase = $('#target-databases').val();
            var newDatabaseName = $('#new-database-name').val();

            if (targetDatabase.trim() == "" || targetDatabase.trim() == "_Choose_Database_") {
                General.notifyFailure("Please choose the target database or choose to create new database to continue!");
                $('#target-databases').focus();
                return false;
            }

            if (targetDatabase.trim() == "_New_Database_" && newDatabaseName == "") {
                General.notifyFailure("Please write new database name to continue!");
                $('#new-database-name').focus();
                return false;
            }


            var tables = [];

            $.each($('.table-check:checked'), (i, tableCheck) => {
                tables.push($(tableCheck).val());
            });

            if (tables.length < 1) {
                General.notifyFailure("Please choose tables to continue!");
                return false;
            }

            var tableSuffix = $('#table-suffix').val();

            var request = {
                Conn: connection.conn,
                Parameter: {
                    SourceDatabase: connection.conn.InitialCatalog,
                    TargetDatabase: targetDatabase,
                    CreateNewDatabase: targetDatabase.trim() == "_New_Database_",
                    NewDatabaseName: newDatabaseName,
                    Suffix: tableSuffix,
                    Tables: tables
                }
            }

            createAuditTables(request, (response) => {
                $("#log-area").val(response);
            }, (response) => {
                $("#log-area").val(response);
            });

        });
    }


    var clearTablesArea = function (conn, callback) {

        $("#tables-container").addClass("d-none");
        $("#new-database-name-container").addClass("d-none");
        $('#new-database-name').val('');
        $('#target-databases').val('_Choose_Database_');
        connection.conn.InitialCatalog = "";
    }

    var getDatabases = function (conn, callback) {

        General.ajax({
            url: "/auditing/GetDatabases",
            data: conn,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }

    var getTables = function (conn, callback) {

        General.ajax({
            url: "/auditing/GetTables",
            data: conn,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }


    var createAuditTables = function (model, callback, failureCallback) {

        General.ajax({
            url: "/auditing/CreateAuditTables",
            data: model,
            panel: $('body'),
            timeout: 10 * 60 * 1000,
            success: function (result) { 
                if (callback)
                    callback(result.response);
            },
            failure: function (result) {
                General.notifyFailure(result.errorMessage);
                if (failureCallback)
                    failureCallback(result.response);
            },
        });

    }



    return {
        init: function () {
            prepareEvents();
        }
    };
}();

$(document).ready(function () {

    Auditing.init();
});
