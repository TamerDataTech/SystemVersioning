
var Systems = function () {
    var systemsTable;
    var curentSystem = { logs: [] };
    var prepareEvents = function () {

        $('#toggle-advanced-filter').off('click').on('click', function () {

            $('.advanced-filter-inputs').toggleClass('d-none');
        });


        $('.search-systems').off('click').on('click', function () {
            systemsTable.draw();
        });




        if ($(".pickadate").length) {
            $(".pickadate").pickadate(Component.getDatePickerLocale());
        }

        $('#add-new-system').off('click').on('click', function () {
            showSystem({});
        });

        //

    }


    var prepareModalEvents = function () { 
      
        $('#system-log-release').off('change').on('change', function () {
            var id = $(this).val();
            var release = id == undefined || id == "" ? {} : curentSystem.logs.find(x => x.id == id);
            $('#release-description').val(release.description);
        });
    }

    var showSystem = function (system) {

        var title = system.id == undefined || system.id.trim() == '' ?
            "Add System" :
           "Update System";

        var options = {
            id: "set-system-modal",
            title: title,
            body: General.renderTemplate("#tmp-set-system", system),
            lock: true,
            buttons: [{
                style: "light-secondary",
                close: true,
                icon: "x",
                title: Utils.getMessage("Close")
            },
            {
                style: "success",
                icon: "save",
                title: "Save",
                class: 'save-system',
                eventClass: 'save-system',
                onClick: function () {

                    var panel = $('#set-system-form');

                    if (General.validateForm(panel)) {
                        var modal = General.getFormModel(panel); 

                        setSystem(modal, function () {

                            General.notifySuccess();
                            $('#set-system-modal').modal('hide');
                            Systems.refreshTable();
                        });
                    }
                }
            }],
            modalCallback: function () {
                 

            }
        };

        General.showModal(options);
    }

    var showSystemLog = function (systemLog) {

        var title = systemLog.id == undefined || systemLog.id.trim() == '' ?
            "Add System Release" :
            "Update System Release";

        var options = {
            id: "set-system-log-modal",
            title: title,
            body: General.renderTemplate("#tmp-set-system-log", systemLog),
            lock: true,
            buttons: [{
                style: "light-secondary",
                close: true,
                icon: "x",
                title: Utils.getMessage("Close")
            },
            {
                style: "success",
                icon: "save",
                title: "Save",
                class: 'save-system-log',
                eventClass: 'save-system-log',
                onClick: function () {

                    var panel = $('#set-system-log-form');

                    if (General.validateForm(panel)) {
                        var modal = General.getFormModel(panel);

                        setSystemLog(modal, function () { 
                            General.notifySuccess();
                            $('#set-system-log-modal').modal('hide');
                            Systems.refreshTable();
                        });
                    }
                }
            }],
            modalCallback: function () { 
                 
            }
        };

        General.showModal(options);
    } 
    
    var showReleases = function (system) {

        $.each(system.logs, function (i, log) {
            log.releaseText = log.releaseIndex.padLeftZero(3);
        });

        curentSystem = system;
       
        var options = {
            id: "system-releases-modal",
            title: "Releases",
            body: General.renderTemplate("#tmp-system-releases", system),
            lock: true,
            buttons: [{
                style: "light-secondary",
                close: true,
                icon: "x",
                title: Utils.getMessage("Close")
            },
            {
                style: "success",
                icon: "save",
                title: "Save",
                class: 'save-system-log',
                eventClass: 'save-system-log',
                onClick: function () {

                    var panel = $('#system-releases-form');

                    var releaseId = $('#system-log-release').val();

                    if (releaseId.isEmpty()) {
                        General.notifyFailure("Please choose a release to continue!");
                        return false;
                    }

                    if (General.validateForm(panel)) {
                        var modal = {
                            Id : releaseId,
                            Description: $('#release-description').val()
                        } 
                        editSystemLog(modal, function () {
                            General.notifySuccess(); 
                            $('#system-releases-modal').modal('hide');
                        });
                    }
                }
            }],
            modalCallback: function () { 
                prepareModalEvents();
            }
        };

        General.showModal(options);
    }

    var setSystem = function (model, callback) {


        General.ajax({
            url: "/app/setSystem",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }

    var setSystemLog = function (model, callback) { 

        General.ajax({
            url: "/app/AddNewRelease",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }

    var editSystemLog = function (model, callback) {

        General.ajax({
            url: "/app/EditRelease",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }

    var getSystem = function (id, callback) {

        General.ajax({
            url: "/app/getSystem",
            data: { id: id },
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }

 
    var getSystemReleases = function (id, callback) {

        General.ajax({
            url: "/app/GetSystemLogs",
            data: { id: id },
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }

    var prepareSystemsEvents = function () {
        $(".delete-system").off("click").click(function () {

            var id = $(this).attr("data-id");

            var model = {
                Id: id
            }

            General.confirm(function () {
                General.ajax({
                    "url": "/app/DeleteSystem",
                    "type": "POST", 
                    "data": model,
                    "success": function (data) {
                        General.notifySuccess("System Deleted Successfully!")
                        Systems.refreshTable();
                    } 
                })
            });


            //edit-system

        });

        $('.edit-system').off('click').on('click', function () {

            var id = $(this).attr("data-id");

            getSystem(id, function (system) {

                showSystem(system);
            });

        });

        $('.new-release-system').off('click').on('click', function () {

            var id = $(this).attr("data-id");
            var release = $(this).attr("data-release");
            showSystemLog({ appSystemId: id, newRelease: (parseInt(release) + 1).padLeftZero(3)});

        });    

       
        $('.show-releases').off('click').on('click', function () {

            var id = $(this).attr("data-id"); 
            getSystemReleases(id, function (system) { 
                showReleases(system); 
            });
            
        });    

    }



    return {
        init: function () {
            prepareEvents();
        },
        initTable: function () {

            systemsTable = $("#systems-table").DataTable({
                "processing": true, // for show progress bar
                "serverSide": true, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "pageLength": 15, // General.defaultTableRowCount(),
                "ajax": {
                    "url": "/app/SearchSystems",
                    "type": "POST",
                    "datatype": "json",
                    "data": function (d) {

                        $.each($(".table-filter"), function (i, el) {
                            d[$(el).attr("name")] = $(el).val();
                        });
                    },

                    "error": function (xhr, error, thrown) {
                        if (xhr.status == "401") {
                            window.location = "/user/Login";
                        }
                    }
                },
                dom:
                    '<"top d-flex flex-wrap"<"action-filters flex-grow-1"f><"actions action-btns d-flex align-items-center">><"clear">rt<"bottom"p>',
                language: Utils.getDataTableLanguage(),
                'order': [[2, 'desc']],
                responsive: {
                    details: {
                        type: "column",
                        target: 2
                    }
                },
                "columnDefs":
                    [{
                        "targets": [0, 1, 2],
                        responsivePriority: -1
                    }],
                "columns": [  
                    { 
                        "name": "name",
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return General.renderTemplate("#tmp-name", full)
                        }
                    }, 
                    { 
                        "name": "releaseIndex",
                        "className": "text-center", "render": function (data, type, full, meta) {
                            return full.releaseIndex.padLeftZero(3);
                        }
                    }, 
                    {

                        "name": "creationTime",
                        "className": "text-center",
                        "render": function (data, type, full, meta) {
                            return Utils.formatDateTime(full.creationTime);
                        }
                    },  
                    {
                        "width": "10%",
                        "orderable": false,
                        "render": function (data, type, full, meta) {
                            return General.renderTemplate("#tmp-system-actions", full)
                        }
                    }
                ],
                "drawCallback": function (settings) {

                    Utils.showDataTableRowsInfo('systems-table', systemsTable);
                    General.reinitiateDefaults(); 
                    prepareSystemsEvents();

                }
            });

            // To append actions dropdown inside action-btn div
            var advancedFilterAction = $(".advanced-filter-action");
            $(".action-btns").append(advancedFilterAction);

            var advancedFilterInputs = $(".advanced-filter-inputs");
            $(".action-filters").parent().after(advancedFilterInputs);

            $(".table-filter").off("change").change(function () {
                systemsTable.draw();
            });

            // Grab the datatables input box and alter how it is bound to events
            $(".dataTables_filter input")
                .unbind() // Unbind previous default bindings
                .on('change', function (e) {
                    systemsTable.search(this.value).draw();
                });

        },
        getTableDT: function () {
            return systemsTable;
        },
        reloadTable: function () {
            systemsTable.ajax.reload();
        },
        refreshTable: function () {
            systemsTable.ajax.reload(null, false);
        }
    };
}();

$(document).ready(function () {
    Systems.initTable();
    Systems.init();
});
