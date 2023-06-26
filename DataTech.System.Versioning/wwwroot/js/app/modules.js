
var Modules = function () {
    var modulesTable;
    var curentModule = { logs: [] };
    var prepareEvents = function () {

        $('#toggle-advanced-filter').off('click').on('click', function () {

            $('.advanced-filter-inputs').toggleClass('d-none');
        });


        $('.search-modules').off('click').on('click', function () {
            modulesTable.draw();
        });




        if ($(".pickadate").length) {
            $(".pickadate").pickadate(Component.getDatePickerLocale());
        }

        $('#add-new-module').off('click').on('click', function () {

            showModule({});
        });

        //

    }


    var prepareModalEvents = function () {

        $('#module-log-version').off('change').on('change', function () {
            var id = $(this).val();

            var updates = curentModule.logs.filter(function (el) {
                return el.versionIndex == id;
            });

            $('#module-version-update')
                .find('option')
                .remove()
                .end()
                .append('<option value="">Choose Update</option>');


            $.each(updates, function (i, update) {
                $('#module-version-update').append('<option value="' + update.id + '">' + update.updateIndex.padLeftZero(3) + '</option>');
            });

            $('#module-version-update').val('').change();

        });

        $('#module-version-update').off('change').on('change', function () {
            var id = $(this).val();
            var release = id == undefined || id == "" ? {} : curentModule.logs.find(x => x.id == id);
            $('#version-description').val(release.description);
        });
    }

    var showModule = function (module) {

        getAllSystems((systems) => {

            module.appSystems = systems;

            var title = module.id == undefined || module.id.trim() == '' ?
                "Add Module" :
                "Update Module";

            var options = {
                id: "set-module-modal",
                title: title,
                body: General.renderTemplate("#tmp-set-module", module),
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
                    class: 'save-module',
                    eventClass: 'save-module',
                    onClick: function () {

                        var panel = $('#set-module-form');

                        if (General.validateForm(panel)) {
                            var modal = General.getFormModel(panel);

                            setModule(modal, function () {

                                General.notifySuccess();
                                $('#set-module-modal').modal('hide');
                                Modules.refreshTable();
                            });
                        }
                    }
                }],
                modalCallback: function () {


                }
            };

            General.showModal(options);
        });

    }

    var showModuleLog = function (moduleLog) {

        var title = moduleLog.id == undefined || moduleLog.id.trim() == '' ?
            "Add Module Version" :
            "Update Module Version";

        var options = {
            id: "set-module-log-modal",
            title: title,
            body: General.renderTemplate("#tmp-set-module-log", moduleLog),
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
                class: 'save-module-log',
                eventClass: 'save-module-log',
                onClick: function () {

                    var panel = $('#set-module-log-form');

                    if (General.validateForm(panel)) {
                        var modal = General.getFormModel(panel);

                        setModuleLog(modal, function () {
                            General.notifySuccess();
                            $('#set-module-log-modal').modal('hide');
                            Modules.refreshTable();
                        });
                    }
                }
            }],
            modalCallback: function () {

            }
        };

        General.showModal(options);
    }
    var showModuleEnhancement = function (moduleLog) {

        var title = moduleLog.id == undefined || moduleLog.id.trim() == '' ?
            "Add Module Enhancement" :
            "Update Module Enhancement";

        var options = {
            id: "set-module-log-modal",
            title: title,
            body: General.renderTemplate("#tmp-set-module-enhancement", moduleLog),
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
                class: 'save-module-enhancement',
                eventClass: 'save-module-enhancement',
                onClick: function () {

                    var panel = $('#set-module-enhancement-form');

                    if (General.validateForm(panel)) {
                        var modal = General.getFormModel(panel);

                        setModuleEnhancement(modal, function () {
                            General.notifySuccess();
                            $('#set-module-log-modal').modal('hide');
                            Modules.refreshTable();
                        });
                    }
                }
            }],
            modalCallback: function () {

            }
        };

        General.showModal(options);
    }

    //showModuleFix
    var showModuleFix = function (moduleLog) {

        var title = moduleLog.id == undefined || moduleLog.id.trim() == '' ?
            "Add Module Fix" :
            "Update Module Fix";

        var options = {
            id: "set-module-log-modal",
            title: title,
            body: General.renderTemplate("#tmp-set-module-fix", moduleLog),
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
                class: 'save-module-fix',
                eventClass: 'save-module-fix',
                onClick: function () {

                    var panel = $('#set-module-fix-form');

                    if (General.validateForm(panel)) {
                        var modal = General.getFormModel(panel);

                        setModuleFix(modal, function () {
                            General.notifySuccess();
                            $('#set-module-log-modal').modal('hide');
                            Modules.refreshTable();
                        });
                    }
                }
            }],
            modalCallback: function () {

            }
        };

        General.showModal(options);
    }

    var showVersions = function (module) {

        var versions = [...new Set(module.logs.map(item => item.versionIndex))]; 

        module.versions = [];
        $.each(versions, function (i, version) {
            module.versions.push({
                versionIndex: version,
                versionText: version //.padLeftZero(3)
            });
        }); 

        //var enhamcements = [...new Set(module.logs.map(item => item.EnhancementIndex))];

        //module.enhamcements = [];
        //$.each(versions, function (i, version) {
        //    module.versions.push({
        //        versionIndex: version,
        //        versionText: version.padLeftZero(3)
        //    });
        //}); 

        curentModule = module;

        var options = {
            id: "module-versions-modal",
            title: "Versions, Enhamcements & Fixes",
            body: General.renderTemplate("#tmp-module-versions", module),
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
                class: 'save-module-log',
                eventClass: 'save-module-log',
                onClick: function () {

                    var panel = $('#module-versions-form');

                    var logId = $('#module-version-fix').val();

                    if (logId.isEmpty()) {
                        General.notifyFailure("Please choose a version to continue!");
                        return false;
                    }

                    if (General.validateForm(panel)) {
                        var modal = {
                            Id: logId,
                            Description: $('#version-description').val()
                        }
                        editModuleLog(modal, function () {
                            General.notifySuccess();
                            $('#module-versions-modal').modal('hide');
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

    var setModule = function (model, callback) {


        General.ajax({
            url: "/app/setModule",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }

    var setModuleLog = function (model, callback) {

        General.ajax({
            url: "/app/AddNewVersion",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }
    var setModuleEnhancement = function (model, callback) {

        General.ajax({
            url: "/app/AddNewEnhancement",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }

    var setModuleFix = function (model, callback) {

        General.ajax({
            url: "/app/AddNewFix",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }


    var editModuleLog = function (model, callback) {

        General.ajax({
            url: "/app/EditVersion",
            data: model,
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });

    }


    var getAllSystems = function (callback) {

        General.ajax({
            url: "/app/getAllSystems",
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }


    var getModule = function (id, callback) {

        General.ajax({
            url: "/app/getModule",
            data: { id: id },
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }


    var getModuleVersions = function (id, callback) {

        General.ajax({
            url: "/app/GetModuleLogs",
            data: { id: id },
            panel: $('body'),
            success: function (result) {

                if (callback)
                    callback(result.response);
            }
        });
    }

    var prepareModulesEvents = function () {
        $(".delete-module").off("click").click(function () {

            var id = $(this).attr("data-id");

            var model = {
                Id: id
            }

            General.confirm(function () {
                General.ajax({
                    "url": "/app/DeleteModule",
                    "type": "POST",
                    "data": model,
                    "success": function (data) {
                        General.notifySuccess("Module Deleted Successfully!")
                        Modules.refreshTable();
                    }
                })
            });


            //edit-module

        });

        $('.edit-module').off('click').on('click', function () {

            var id = $(this).attr("data-id");

            getModule(id, function (module) {

                showModule(module);
            });

        });

        $('.new-version-module').off('click').on('click', function () {

            var id = $(this).attr("data-id");
            var version = $(this).attr("data-version");
            showModuleLog({ appModuleId: id, newVersion: (parseInt(version) + 1).padLeftZero(3) });

        });

        $('.new-enhancement-module').off('click').on('click', function () {

            var id = $(this).attr("data-id");
            var version = $(this).attr("data-version");
            var enhancement = $(this).attr("data-enhancement");
            showModuleEnhancement({ appModuleId: id, versionText: version, newEnhancement: (parseInt(enhancement) + 1).padLeftZero(3) }); 
        });


        $('.new-fix-module').off('click').on('click', function () {

            var id = $(this).attr("data-id");
            var version = $(this).attr("data-version");
            var enhancement = $(this).attr("data-enhancement");
            var fix = $(this).attr("data-fix");
            showModuleFix({ appModuleId: id, versionText: version, enhancementText: enhancement.padLeftZero(3), newFix: (parseInt(fix) + 1).padLeftZero(3) }); 
        });

        $('.show-versions').off('click').on('click', function () {

            var id = $(this).attr("data-id");
            getModuleVersions(id, function (module) {
                showVersions(module);
            });

        });

    }



    return {
        init: function () {
            prepareEvents();
        },
        initTable: function () {

            modulesTable = $("#modules-table").DataTable({
                "processing": true, // for show progress bar
                "serverSide": true, // for process server side
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "pageLength": 15, // General.defaultTableRowCount(),
                "ajax": {
                    "url": "/app/SearchModules",
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
                'order': [[3, 'desc']],
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
                        "name": "versionIndex",
                        "className": "text-center", "render": function (data, type, full, meta) {
                            return full.currentVersion;
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
                            return General.renderTemplate("#tmp-module-actions", full)
                        }
                    }
                ],
                "drawCallback": function (settings) {

                    Utils.showDataTableRowsInfo('modules-table', modulesTable);
                    General.reinitiateDefaults();
                    prepareModulesEvents();

                }
            });

            // To append actions dropdown inside action-btn div
            var advancedFilterAction = $(".advanced-filter-action");
            $(".action-btns").append(advancedFilterAction);

            var advancedFilterInputs = $(".advanced-filter-inputs");
            $(".action-filters").parent().after(advancedFilterInputs);

            $(".table-filter").off("change").change(function () {
                modulesTable.draw();
            });

            // Grab the datatables input box and alter how it is bound to events
            $(".dataTables_filter input")
                .unbind() // Unbind previous default bindings
                .on('change', function (e) {
                    modulesTable.search(this.value).draw();
                });

        },
        getTableDT: function () {
            return modulesTable;
        },
        reloadTable: function () {
            modulesTable.ajax.reload();
        },
        refreshTable: function () {
            modulesTable.ajax.reload(null, false);
        }
    };
}();

$(document).ready(function () {
    Modules.initTable();
    Modules.init();
});
