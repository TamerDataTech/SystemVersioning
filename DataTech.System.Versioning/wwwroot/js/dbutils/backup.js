
var Backup = function () {

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

 
       
        $('#create-backup').off('click').on('click', function () {
            $("#log-area").val('');

             

            var request = {
                Conn: connection.conn,
                Parameter: {
                    SourceDatabase: connection.conn.InitialCatalog, 
                    LookupTablesOnly: $('#lookup-tables').is(':checked')
                }
            }

            createBackup(request, (result) => {
                window.location = result.message;
                $("#log-area").val(result.response);
            }, (response) => {
                $("#log-area").val(response);
            });

        });
    }


    var clearTablesArea = function () {

        $("#tables-container").addClass("d-none"); 
        connection.conn.InitialCatalog = "";
    }

    var getDatabases = function (conn, callback) {

        General.ajax({
            url: "/DbUtils/GetDatabases",
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


    var createBackup = function (model, callback, failureCallback) {

        General.ajax({
            url: "/DbUtils/CreateTableBackup",
            data: model,
            panel: $('body'),
            timeout: 10* 60 * 1000,
            success: function (result) { 
                if (callback)
                    callback(result);
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

    Backup.init();
});
