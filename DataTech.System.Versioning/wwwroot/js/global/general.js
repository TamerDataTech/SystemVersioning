
var General = function () {

   


    var prepareComponents = function () {

        if ($.fn.dataTable) {
            $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
                General.notify(General.getSystemMessage({
                    message,
                    settings,
                    helpPage
                }), Component.state.danger, 5);

                if (settings && settings.jqXHR && settings.jqXHR.status == 401) {
                    window.location = '/user/logout';
                    return
                }
            };
        }



        $('.modal').on('shown.bs.modal',
            function () {
                var body = $(this).find(".modal-body");

                if (body.html().trim() == '')
                    General.block(body);
            });


        $('.check-password-strength').keyup(function () {
            var panel = $($(this).attr('data-checker'));
            if ($(this).closest('.modal').length > 0) {
                panel = $(this).closest('.modal').find($(this).attr('data-checker'));
            }
            $(panel).html(Component.checkStrength($(this).val(), panel))
        });

        $('.show-password span').off('click').on('click', function () {
            var status = $(this).attr('data-status');
            var input;

            if ($(this).closest('.modal').length > 0) {
                input = $(this).closest('.modal').find('#' + $(this).attr('data-input'));
            }
            else {
                input = $('#' + $(this).attr('data-input'));
            }

            if (status == 'hide') {

                input.attr('type', 'text');
                $(this).attr('data-status', 'show');
                $(this).text(Utils.getMessage('hide'));
            }
            else {
                input.attr('type', 'password');
                $(this).attr('data-status', 'hide');
                $(this).text(Utils.getMessage('show'));
            }
        });


        $('.change-culture').off('click').click(function () {
            var url = window.location.href;
            var newCulture = $(this).attr('data-value');

            if (url.indexOf('newCulture') > -1) {

                url = updateQueryStringParamAndGetUrl('newCulture', newCulture);
            }
            else {
                if (url.indexOf('?') > -1) {
                    url = url + '&newCulture=' + newCulture;
                }
                else {
                    url = url + '?newCulture=' + newCulture;

                }
            }


            window.location.href = url;

        });


        $('.input-parameter-tag').off('click').on('click', function () {
            var text = $(this).attr('data-text');
            var input = $(this).attr('data-input');
            if (text == undefined) return;
            General.insertAtCaret(input, text.trim() + ' ');
        });


        $('.az-user-address-checker').on('change', function () {

            var cntrl = $(this);


            $('.az-user-address-checker[data-id!="' + cntrl.attr('data-id') + '"]').prop('checked', false);
            $('.az-user-address-chooser').removeClass('selected');


            if (cntrl.is(':checked')) {
                cntrl.closest('.az-user-address-chooser').addClass('selected');
            }
        });




    }

    var updateQueryStringParamAndGetUrl = function (key, value) {
        var baseUrl = [location.protocol, '//', location.host, location.pathname].join(''),
            urlQueryString = document.location.search,
            newParam = key + '=' + value,
            params = '?' + newParam;

        // If the "search" string exists, then build params from it
        if (urlQueryString) {
            keyRegex = new RegExp('([\?&])' + key + '[^&]*');

            // If param exists already, update it
            if (urlQueryString.match(keyRegex) !== null) {
                params = urlQueryString.replace(keyRegex, "$1" + newParam);
            } else { // Otherwise, add it to end of query string
                params = urlQueryString + '&' + newParam;
            }
        }
        return baseUrl + params;
    }


    //TODO:
    var initDatePickers = function () {

        $('.input-daterange').datepicker({
            todayHighlight: true,
            clearBtn: true,
            //orientation: "bottom left",
            templates: {
                leftArrow: '<i class="la la-angle-left"></i>',
                rightArrow: '<i class="la la-angle-right"></i>'
            }
        });

    }

    //TODO:
    var initChecker = function () {
        $('.az-check-group .az-check').off('click').on('click', function () {
            $(this).toggleClass("active")
        });
    }


    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    };



    var initInputs = function () {


        $.each($(".numeric-input"), function (i, input) {

            var numberOfDigits = $(this).attr('data-digit-count') > 0 ? $(this).attr('data-digit-count') : General.defaultFractionDigits();

            $(input).val(Utils.formatDecimal($(input).val(), numberOfDigits));
        });


        $.each($(".numeric-text"), function (i, input) {

            var numberOfDigits = $(this).attr('data-digit-count') > 0 ? $(this).attr('data-digit-count') : General.defaultFractionDigits();

            $(input).text(Utils.formatDecimal($(input).text(), numberOfDigits));
        });



        $(".numeric-input").on('keydown',
            function (e) {
                -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) ||
                    (/65|67|86|88/.test(e.keyCode) && (e.ctrlKey === true || e.metaKey === true)) &&
                    (!0 === e.ctrlKey || !0 === e.metaKey) ||
                    35 <= e.keyCode && 40 >= e.keyCode ||
                    (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) &&
                    (96 > e.keyCode || 105 < e.keyCode) &&
                    e.preventDefault();
            }).change(function () {
                if ($(this).val().trim() == "" || !$.isNumeric($(this).val().trim().replace(',', '').replace('.', ''))) {
                    $(this).val('');
                }
                else {
                    var numberOfDigits = $(this).attr('data-digit-count') > 0 ? $(this).attr('data-digit-count') : General.defaultFractionDigits();

                    $(this).val(Utils.formatDecimal($(this).val(), numberOfDigits));
                }
            });

        $(".integer-input").on('keydown',
            function (e) {
                -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13]) ||
                    (/65|67|86|88/.test(e.keyCode) && (e.ctrlKey === true || e.metaKey === true)) &&
                    (!0 === e.ctrlKey || !0 === e.metaKey) ||
                    35 <= e.keyCode && 40 >= e.keyCode ||
                    (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) &&
                    (96 > e.keyCode || 105 < e.keyCode) &&
                    e.preventDefault();
            }).change(function () {
                if ($(this).val().trim() == "" || !$.isNumeric($(this).val().trim().replace(',', '').replace('.', ''))) {
                    $(this).val('');
                }
                   
            });

        $(".email-input").on('change',
            function () {

                $(this).removeClass("invalid-input-validation");
                if ($(this).val().trim() != "" && !isEmail($(this).val())) {
                    $(this).addClass("invalid-input-validation");
                    $(this).attr("data-original-title", "Invalid Email!");
                    $(this).attr("data-toggle", "m-tooltip");
                } else {
                    $(this).removeAttr("data-original-title");
                    $(this).removeAttr("data-toggle");
                }

                mApp.initTooltip($(this));
            });

        $(".id-input").on('change',
            function () {
                General.reinitiateDefaults();
                var input = $(this);
                $(input).removeClass("is-invalid");

                var value = $(input).val().trim().replace(/\D+/g, "");

                var isValid = General.validateIdNumber(value);


                if (!isValid) {

                    jQuery(input).addClass("is-invalid");
                    jQuery(input).attr('data-original-title', Utils.getMessage("invalid-id"));

                    jQuery(input).tooltip({ placement: 'bottom', trigger: 'manual' }).tooltip('show');


                }
            });
        $(".tax-input").on('change',
            function () {

                General.reinitiateDefaults();

                var input = $(this);
                $(input).removeClass("is-invalid");

                var value = $(input).val().trim().replace(/\D+/g, "");

                var isValid = General.validateIdNumber(value) || General.validateTaxNumber(value);


                if (!isValid) {

                    jQuery(input).addClass("is-invalid");
                    jQuery(input).attr('data-original-title', Utils.getMessage("invalid-tax-id"));

                    jQuery(input).tooltip({ placement: 'bottom', trigger: 'manual' }).tooltip('show');


                }
            });



        if (typeof autosize != 'undefined') {
            autosize($('textarea.autosize-textarea'));
        }



        $('.button-minus', $('.input-spinner')).off('click').on('click',
            function () {

                var container = $(this).closest('.input-spinner');

                var prevValue = parseInt(container.find('.quantity-input').val());

                var currentValue = prevValue > 1 ? prevValue - 1 : 1;
                
                container.find('.quantity-input').val(currentValue).change();
            }); 

        $('.button-plus', $('.input-spinner')).off('click').on('click',
            function () {

                var container = $(this).closest('.input-spinner');

                var prevValue = parseInt(container.find('.quantity-input').val());

                var currentValue = prevValue + 1;

                container.find('.quantity-input').val(currentValue).change();
            }); 

        $('.quantity-input', $('.input-spinner')).on('change',
            function () { 
                var prevValue = parseInt($(this).val());

                var currentValue = prevValue < 1 ? 1 : prevValue;

                $(this).val(currentValue);
            }); 

        $('.check-box').click(function () { 
            var currentInput = $(this).find('input'); 
            var relartedInputs = $('[name="' + currentInput.attr('name') + '"]').closest('.check-box').removeClass('active'); 
            currentInput.closest('.check-box').addClass('active');
        });
        
    }

    return {
        //main function to initiate the module
        init: function () {
            // initDatePickers();
            initInputs();
           
            prepareComponents();

            General.initMask();
            General.initExtentions();
            //TODO: To be revised
            //General.initScroller({
            //    height: '100%'
            //});
            //  General.initToolTips();
        },
        initToolTips: function () {
            $('[data-toggle="tooltip"]').tooltip();

        },

        initInputs: function () {
            initInputs();
        },
        initChecker: function () {
            initChecker();
        },
        //TODO: To be revised
        initPageSearch: function () {
            prepareSearch();
        },
        block: function (item, options) {
            //Component.block($(item), {});
            $(item).waitMe({})
        },
        unblock: function (item) {
            //Component.unblock($(item));
            $(item).waitMe('hide')

        },
        blockPage: function () {
            Component.blockPage();
        },
        unblockPage: function () {
            Component.unblockPage();
        },
        initComponents: function () {
            prepareComponents();
        },
        initDatePickers: function () {
            initDatePickers();
        },
        azNotify: function (options) {
            var message = options.Message;
            var type = options.Type;
            var timer = options.Timer;
            var title = options.Title;
            var showProgress = options.ShowProgress;


            Component.notify(message, type, timer, title, showProgress);
        },
        notify: function (message, type, timer) {
            // Example: General.notify("your massage", General.state.Danger, 2)
            General.azNotify({
                Message: message,
                Type: type,
                Timer: timer
            });
        },
        notifySuccess: function (message) {
            General.notify(message == undefined || message.trim() == '' ? Utils.getMessage("Success") : message, Component.state.success, 3);
        },
        notifyFailure: function (message) {
            General.notify(message == undefined || message.trim() == '' ? Utils.getMessage("Last Action Failed") : message, Component.state.danger, 3);
        },
        confirm: function (yesCallback, noCallBack, title, message) {
            //Example: General.confirm(function(){General.notify("OK")}, function(){General.notify("You were closed!")})
            //Example: General.confirm(function(){General.notify("OK")}, undefined, "Are you sure?")
            //Example: General.confirm(function(){General.notify("OK")}, undefined, "" , "You can not go back")

            if (message == undefined)
                message = '';

            if (message == '')
                message = Utils.getMessage("Are you sure") + "?";


            if (title == undefined)
                title = Utils.getMessage("Warning");



            var defaults = {
                type: "danger",
                message: message,
                title: title,
                time: 30,
                ok: "Yes",
                cancel: "No",
                yesCallback: yesCallback,
                noCallBack: noCallBack
            }
            var settings = $.extend({}, defaults);

            var modal = $(General.renderTemplate($("#tmp-confirm-model"), settings)).appendTo("body");

            $(modal).modal();
            $(".btn-cancel", modal).off("click").click(function () {
                if (settings.noCallBack) {
                    settings.noCallBack();
                }
                $(modal).modal("hide");

                setTimeout(500, function () {
                    $(modal).remove();
                })

            });
            $(".btn-ok", modal).off("click").click(function () {
                if (settings.yesCallback) {
                    settings.yesCallback();
                }
                $(modal).modal("hide");
                setTimeout(500, function () {
                    $(modal).remove();
                })
            });

        },

        playButton: function (el) {
            //Example: General.playButton($("#store-modal .btn-success")) 
            $('<span class="spinner-border spinner-border-sm mx-1" role="status" aria-hidden="true"></span>').prependTo($(el));
            $(el).prop("disabled", true);
        },

        stopButton: function (el) {
            //Example: General.stopButton($("#store-modal .btn-success"))

            $(".spinner-border", $(el)).remove();
            //$('<span class="spinner-border spinner-border-sm mx-1" role="status" aria-hidden="true"></span>').prependTo();
            $(el).prop("disabled", false);

        },
        alertPanel: function (pnl, message, type, timeOut, borderStyle) {
            //Example: General.stopPanel($("#store-list tbody tr:eq(0) td:last span"))

            if (type == undefined)
                type = 'info';

            if (timeOut == undefined)
                timeOut = 0;

            var icon = "";

            switch (type) {
                case Component.state.success:
                    icon = "check-double";
                    break;
                case Component.state.danger:
                    icon = "error";
                    break;
                case Component.state.warning:
                    icon = "error-circle";
                    break;
                default:
                    icon = "info-circle";
            }


            Component.alert({
                message: message,
                container: pnl,
                type: type,
                icon: icon,
                closeInSeconds: timeOut,
                borderStyle: borderStyle == undefined || borderStyle == true
            });
        },

        validateForm: function (form) {

            General.reinitiateDefaults();

            var inputs = $(form).find("input:required, textarea:required, select:required");
            var mobileInputs = $(form).find("input.mobile-number");

            var Idinput = $(form).find("input.id-input");
            var TaxIdInput = $(form).find("input.tax-input");

            var firstInput;

            var result = true; 
          

            $.each(inputs,
                function (i, input) {
                    $(input).removeClass("is-invalid");
                    var parent = $(input).closest(".form-group");

                    if ($(input).is('input') || $(input).is('textarea')) {
                        if ($(input).val().trim() == '') { 
                            $(input).addClass("is-invalid") 
                            result = false; 

                            if (firstInput == undefined || firstInput.length < 1)
                                firstInput = $(input);

                        } else if ($(input).attr("type").trim() == 'email' && !isEmail($(input).val())) {

                            $(input).addClass("is-invalid");

                            result = false;

                            if (firstInput == undefined || firstInput.length < 1)
                                firstInput = $(input);

                        } else {

                            $(input).removeClass("is-invalid")

                        }
                    } else if ($(input).is('select')) {

                        // if ($(input).val() == $(input).find("option:eq(0)").val()) {
                        if ($(input).val() == undefined || $(input).val().trim() == '') {

                            $(input).addClass("is-invalid")

                            result = false;

                            if (firstInput == undefined || firstInput.length < 1)
                                firstInput = $(input);


                        } else {

                            $(input).removeClass("is-invalid")


                        }
                    }


                });

            $.each(mobileInputs,
                function (i, input) {
                    $(input).removeClass("is-invalid");
                    var value = $(input).val().trim().replace(/\D+/g, "");
                    var isEmpty = value == '';

                    if (!isEmpty && (value.length != 10 || value[0] != '5')) {
                        $(input).addClass("is-invalid")
                        result = false;


                        if (firstInput == undefined || firstInput.length < 1)
                            firstInput = $(input);
                    }
                });

            $.each(Idinput,
                function (i, input) {
                    $(input).removeClass("is-invalid");

                    var value = $(input).val().trim().replace(/\D+/g, "");

                    var isValid = General.validateIdNumber(value);


                    if (!isValid && $(input).closest('.d-none').length < 1) {

                        jQuery(input).addClass("is-invalid");
                        jQuery(input).attr('data-original-title', Utils.getMessage("invalid-id"));

                        jQuery(input).tooltip({ placement: 'bottom', trigger: 'manual' }).tooltip('show');
                        result = false;

                        if (firstInput == undefined || firstInput.length < 1)
                            firstInput = $(input);

                    } 

                });

            $.each(TaxIdInput,
                function (i, input) {
                    $(input).removeClass("is-invalid");

                    var value = $(input).val().trim().replace(/\D+/g, "");

                    var isValid = General.validateIdNumber(value) || General.validateTaxNumber(value);


                    if (!isValid && $(input).closest('.d-none').length < 1) {

                        jQuery(input).addClass("is-invalid");
                        jQuery(input).attr('data-original-title', Utils.getMessage("invalid-tax-id"));

                        jQuery(input).tooltip({ placement: 'bottom', trigger: 'manual' }).tooltip('show');
                        result = false;

                        if (firstInput == undefined || firstInput.length < 1)
                            firstInput = $(input);

                    }




                });
            // form.addClass("m-form--state");


            if (firstInput != undefined && firstInput.length > 0)
                $(firstInput).focus();

            return result;


        },
        validateInput: function (input) {
            $(input).removeClass("is-invalid");
            if ($(input).val().trim() == '') {
                $(input).addClass("is-invalid");
                $(input).focus();
                return false;  
            }

            return true;  
        },
        validateIdNumber: function (tcno) {
            // geleni her zaman String'e çevirelim!
            tcno = String(tcno);

            // tcno '0' karakteri ile başlayamaz!
            if (tcno.substring(0, 1) === '0') {
                return false;
            }
            // Tcno 11 karakter uzunluğunda olmalı!
            if (tcno.length !== 11) {
                return false;
            }

            /**
                Aşağıdaki iki kontrol için toplamları hazır ediyoruz
                - o anki karakteri sayıya dönüştür
                - tek haneleri ayrıca topla (1,3,5,7,9)
                - çift haneleri ayrıca topla (2,4,6,8)
                - bütün haneleri ayrıca topla
            **/
            var ilkon_array = tcno.substr(0, 10).split('');
            var ilkon_total = hane_tek = hane_cift = 0;

            for (var i = j = 0; i < 9; ++i) {
                j = parseInt(ilkon_array[i], 10);
                if (i & 1) { // tek ise, tcnin çift haneleri toplanmalı!
                    hane_cift += j;
                } else {
                    hane_tek += j;
                }
                ilkon_total += j;
            }

            /**
                KONTROL 1:
                1. 3. 5. 7. ve 9. hanelerin toplamının 7 katından, 
                2. 4. 6. ve 8. hanelerin toplamı çıkartıldığında, 
                elde edilen sonucun Mod10'u bize 10. haneyi verir
            **/
            if ((hane_tek * 7 - hane_cift) % 10 !== parseInt(tcno.substr(-2, 1), 10)) {
                return false;
            }

            /**
                KONTROL 2:
                1. 2. 3. 4. 5. 6. 7. 8. 9. ve 10. hanelerin toplamından
                elde edilen sonucun Mod10'u bize 11. haneyi vermelidir.
                NOT: ilk 9 haneyi üstteki FOR döndüsünde zaten topladık!
            **/
            ilkon_total += parseInt(ilkon_array[9], 10);
            if (ilkon_total % 10 !== parseInt(tcno.substr(-1), 10)) {
                return false;
            }

            return true;

        },
        validateTaxNumber: function (kno) {

            if (kno.length === 10) {
                let v = []
                let lastDigit = Number(kno.charAt(9))
                for (let i = 0; i < 9; i++) {
                    let tmp = (Number(kno.charAt(i)) + (9 - i)) % 10
                    v[i] = (tmp * 2 ** (9 - i)) % 9
                    if (tmp !== 0 && v[i] === 0) v[i] = 9
                }
                let sum = v.reduce((a, b) => a + b, 0) % 10
                return (10 - (sum % 10)) % 10 === lastDigit
            }
            return false

        },
        getFormModel: function (form) {

            var inputs = $(form).find("input, select, textarea");
            var model = {};

            $.each(inputs,
                function (i, input) {
                    if ($(input).is(':checkbox'))
                        model[$(input).attr("name")] = $(input).prop("checked");
                    else if ($(input).attr('type') == 'radio')
                        model[$(input).attr("name")] = $(form).find('[name="' + $(input).attr("name") + '"]:checked').val();
                    else
                        model[$(input).attr("name")] = $(input).val().trim();
                });
            return model;
        },
        showModal: function (options) {
            //Example: General.showModal({ Body: 'test', ShowSave: false, Lock: true})
            options = $.extend(true,
                {
                    title: " ",
                    body: "",
                    lock: false,
                    buttons: [
                        {
                            style: "danger",
                            close: true,
                            icon: "times",
                            title: "Close",
                            class: "btn-cancel"
                        }
                    ]
                },
                options);

            $("#"+ options.id).remove();
            $(".modal-backdrop").remove();


            var modal = General.renderTemplate("#tmp-management-modal", options);
            modal = $(modal).appendTo("body");

            if (options.id != undefined && options.id != '') {
                $(modal).attr('id', options.id);
            }

           // $("body").append(modal);

            if (options.lock) {
                $(modal).modal({
                    backdrop: 'static',
                    keyboard: false
                });
            }
            else {
                $(modal).modal();
            }
           

            $.each(options.buttons, function (i, button) {
                if (button.class && button.onClick) {
                    $("." + button.class, $(modal)).off("click").click(function () {
                        if (button.close) {
                            $(modal).modal("hide");
                            setTimeout(function () { button.onClick($(modal)); }, 100);
                        } else {
                            button.onClick($(modal));
                        }

                    })
                }
            });
            if (options.init)
                options.init(modal);


            $(modal).on('shown.bs.modal',
                function () {

                    var body = $(this).find(".modal-body");

                    
                    if (options.modalCallback) {
                        options.modalCallback(body, modal);
                    }

                    if (options.focusControl) {
                        $((body).find(options.focusControl)).focus();
                    }

                    if (options.submitOnAnyInput) {
                        $((body).find('input')).on('keypress', function (e) {
                            if (e.which == 13) {
                                modal.find(".btn-save").click();
                            }
                        });
                    }

                    General.reinitiateDefaults();

                });

           
            return modal;

        },

        startModal: function (modal, state, body, isResult) {

            var modalBody = $(modal).find(".modal-body");

            if (state == false && !isResult)
                body =
                    '<div  class="m-alert m-alert--outline alert alert-danger fade show"><i class="fa-lg fa fa-warning"></i> ' +
                    body +
                    '</div>';

            if (isResult) {
                General.alertPanel(modalBody, body, Component.state.Danger, 5);
            } else {

                modalBody.html(body);

            }

            General.unblock(modalBody);

            if (state)
                $(modal).find(".btn-save").removeAttr("disabled");


        },
        closeModal: function () {

        },
        getSystemMessage: function (data) {
            console.log(data);
            return Utils.getMessage("exception");
        },
        ajax: function (options) {
            options = $.extend(true,
                {
                    dataType: Utils.requestDataType.Json,
                    type: Utils.requestType.Post,
                    timeout: 30 * 1000,
                    async: true,
                    activateSender: true
                },
                options);

            if (options.sender) {
                General.playButton(options.sender);
            }

            if (options.panel) {
                General.block(options.panel);
            }


            if (options.blockPage) {
                General.blockPage()
            }

            $.ajax({
                //headers: {
                //    "Authorization": "Basic " + btoa("a:a")
                //},
                url: options.url,
                data: options.data,
                type: options.type,
                dataType: options.dataType,
                processData: options.processData,
                contentType: options.contentType,
                timeout: options.timeout,
                async: options.async,
                success: function (data) {
                    if (options.blockPage) {
                        General.unblockPage();
                    }
                    if (data.result) {

                        if (data.message == undefined ||
                            data.message == null ||
                            data.message == 'null' ||
                            data.message == '')
                            data.message = Utils.getMessage("Success");

                        if (!options.success) {
                            if (options.modal) {

                                if (options.sender) {
                                    General.stopButton(options.sender);
                                }

                                if (options.closeModal) {

                                    General.notify(data.message, Component.state.success, 3);

                                    options.Modal.modal('hide');
                                } else {
                                    General.startModal(options.modal, true, data.Response);
                                }
                            }
                            else {
                                General.notify(data.message, Component.state.success, 3);
                            }
                        } else {

                            if (options.showOpertionResultMessage) {
                                General.notify(data.message, Component.state.Success, 3);
                            }
                            options.success(data);
                        }


                        if (options.done) {
                            options.done();
                        }

                        if (options.sender && options.activateSender) {
                            General.stopButton(options.sender);
                        }
                    } else {

                        if (data.errorMessage == undefined ||
                            data.errorMessage == null ||
                            data.errorMessage == 'null' ||
                            data.errorMessage == '')
                            data.errorMessage = Utils.getMessage("Last Action Failed");

                        if (options.sender) {
                            General.stopButton(options.sender);
                        }

                        if (data.ErrorCode == "NotAuthorized") { 
                            General.notify(data.errorMessage, Component.state.danger, 3);
                            setTimeout(function () { location.href = "/member/login?returnUrl=" + location.pathname; }, 3000);
                        }
                        else if (data.errorType == 'General.TimedOut') {
                            //TODO:
                        } else {

                            if (!options.failure) {
                                if (options.Modal) {
                                    General.startModal(options.modal, false, data.ErrorMessage, options.isResult);
                                } else {
                                    General.notify(data.errorMessage, Component.state.danger, 5);
                                }
                            } else {
                                if (options.showOpertionResultMessage) {
                                    General.notify(data.errorMessage, Component.state.danger, 5);
                                }

                                options.failure(data);

                            }


                        }
                    }

                    if (options.panel) {
                        General.unblock(options.panel);
                    }
                },
                error: function (data) {

                    if (options.blockPage) {
                        General.unblockPage();
                    }
                    if (options.panel) {
                        General.unblock(options.panel);
                    }

                    if (options.sender) {
                        General.stopButton(options.sender);
                    }

                    if (options.error) {



                        if (data == undefined)
                            data = {};

                        if (data.errorMessage == undefined)
                            data.errorMessage = General.getSystemMessage(data);

                        options.error(data);

                    } else {
                        if (options.modal) {
                            General.startModal(options.modal, false, General.getSystemMessage(data));
                        } else {
                            if (data.status == 403) {
                                General.notify(Utils.getMessage("not-authorized"), Component.state.warning, 5);
                                window.location.href = "/user/login"
                            } else {
                                General.notify(General.getSystemMessage(data), Component.state.danger, 5);
                            }
                        }


                    }


                }
            });
        },
        state: Component.state,
        scrollTo: function (cntrl, topMargin, speed) {
            if (topMargin == undefined || topMargin === '')
                topMargin = 0;

            if (speed == undefined || speed === '')
                speed = 'slow';

            // Scroll
            $('html,body').animate({
                scrollTop: $(cntrl).offset().top - topMargin
            },
                speed);
        },
        checkEmail: function (value) {
            return isEmail(value);
        },
        reinitiateDefaults: function () {
            General.initToolTips();
            General.clearDefaults();
            General.initInputs();
            General.initComponents();
            General.initMask();
        },
        clearDefaults: function () {
            $('.tooltip[role="tooltip"]').remove();
        },
        //TODO: to be revised
        initScroller: function () {
            if (!jQuery().slimScroll) {
                return;
            }
            $('.scrollable').slimScroll({ height: "auto" });
        },
        //TODO: to be revised
        initTableCheck: function (tabel) {

            jQuery(tabel).find('.group-check').change(function () {
                var set = jQuery(tabel).find(".row-check");
                var checked = jQuery(this).is(":checked");
                jQuery(set).each(function () {
                    if (checked) {
                        //  $(this).attr("checked", "checked");
                        $(this).prop("checked", true);

                    } else {
                        // $(this).removeAttr("checked");
                        $(this).prop("checked", false);
                    }
                });
            });

            jQuery(tabel).find('.row-check').change(function () {
                var set = jQuery(tabel).find(".row-check");
                var checkedSet = jQuery(tabel).find(".row-check:checked");

                if (set.length == checkedSet.length && set.length > 0) {
                    jQuery(tabel).find('.group-check').prop("checked", true);
                }
                else {
                    jQuery(tabel).find('.group-check').prop("checked", false);
                }
            });
        },
        loadJsFile: function (path, callback) {
            var done = false;

            if ($("script[src='" + path + "']").length > 0) {
                callback();
                return;
            }

            var scr = document.createElement('script');

            scr.onload = handleLoad;
            scr.onreadystatechange = handleReadyStateChange;
            scr.onerror = handleError;
            scr.src = path;
            document.body.appendChild(scr);

            function handleLoad() {
                if (!done) {
                    done = true;
                    callback(path, "ok");
                }
            }

            function handleReadyStateChange() {
                var state;

                if (!done) {
                    state = scr.readyState;
                    if (state === "complete") {
                        handleLoad();
                    }
                }
            }
            function handleError() {
                if (!done) {
                    done = true;
                    callback(path, "error");
                }
            }
        },
        loadStyleFile: function (path, callback) {
            var done = false;

            if ($("link[src='" + path + "']").length > 0) {
                callback();
                return;
            }

            var scr = document.createElement('link');

            scr.onload = handleLoad;
            scr.onreadystatechange = handleReadyStateChange;
            scr.onerror = handleError;
            scr.href = filename;
            scr.type = 'text/css';
            scr.rel = 'stylesheet';

            document.head.appendChild(scr);

            function handleLoad() {
                if (!done) {
                    done = true;
                    callback(path, "ok");
                }
            }

            function handleReadyStateChange() {
                var state;

                if (!done) {
                    state = scr.readyState;
                    if (state === "complete") {
                        handleLoad();
                    }
                }
            }
            function handleError() {
                if (!done) {
                    done = true;
                    callback(path, "error");
                }
            }
        },
        getPropValue: function (value) {
            if (value == undefined)
                return "";

            return ("" + value).trim();
        },
        createGuid: function () {
            function _p8(s) {
                var p = (Math.random().toString(16) + "000000000").substr(2, 8);
                return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
            }
            return _p8() + _p8(true) + _p8(true) + _p8();
        },

        //TODO: to be revised
        uploadFile: function (options) {



            options = $.extend(true,
                {
                    sizeLimit: 2
                },
                options);



            if (options.files.length > 0) {
                if (window.FormData !== undefined) {
                    var data = new FormData();
                    for (var x = 0; x < options.files.length; x++) {
                        if (options.files[x].size > (2 * 1024 * 1024)) {
                            return options.error('File size bigger than 2 mb');
                        }
                        data.append("file" + x, options.files[x]);
                    }
                    General.ajaxRequest({
                        Type: "POST",
                        Url: '/Common/UploadFile/' + options.fileType,
                        ContentType: false,
                        ProcessData: false,
                        Data: data,
                        Success: options.success,
                        Error: function (data) {
                            if (data)
                                options.error(data.ErrorMessage);
                        }
                    }

                    )
                } else {
                    options.error("This browser doesn't support HTML5 file uploads!")

                }
            }
        },
        //TODO: to be revised
        autoComplate: function (elment, options) {
            if ($(elment).hasClass("tt-input")) {
                $(elment).typeahead("destroy");
            }
            this.options = $.extend({}, {
                Template: {
                    empty: [
                        ''
                    ].join('\n'),
                    suggestion: Handlebars.compile('<div><strong>{{' + options.DisplayFieldName + '}}</strong> ' +
                        (options.ExtraInfoField != undefined && options.ExtraInfoField != '' ? ' – {{' + options.ExtraInfoField + '}} ' : '') + '</div>')
                }
            }, options);



            var bloodhound = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace(options.DisplayFieldName),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: options.QueryUrl,
                    replace: function (url, uriEncodedQuery) {
                        url = url + '?query=' + uriEncodedQuery;
                        if (options.QueryParameters != undefined) {
                            $.each(options.QueryParameters, function (index, param) {
                                url += '&' + param.Name + "=";
                                switch (param.Type) {
                                    case "value":
                                        url += $(param.Element).val();
                                        break;
                                    case "attribute":
                                        url += $(param.Element).attr(param.AttributeName);
                                        break;
                                    case "selected-attribute":
                                        url += $(param.Element).find('option:selected').attr(param.AttributeName);
                                        break;

                                    default:
                                        url += $(param.Element).val();
                                        break;
                                }

                            });
                        }
                        return url;
                    }
                }
            });
            var typehead = $(elment).typeahead(null, {
                name: options.DisplayFieldName,
                hint: true,
                highlight: true,
                minLength: 1,
                display: options.DisplayFieldName,
                source: bloodhound,
                templates: options.Template
            });
            if (typeof options.OnSelect === 'function') {

                $(elment).unbind('typeahead:select').bind('typeahead:select', function (ev, suggestion) {
                    options.OnSelect(ev, suggestion);
                    // $(elment).typeahead('val', suggestion[options.DisplayFieldName]);

                });

            }
        },
        insertAtCaret: function (areaId, text) {
            var txtarea = document.getElementById(areaId);
            if (!txtarea || text == undefined) {
                return;
            }

            var scrollPos = txtarea.scrollTop;
            var strPos = 0;
            var br = ((txtarea.selectionStart || txtarea.selectionStart == '0') ?
                "ff" : (document.selection ? "ie" : false));
            if (br == "ie") {
                txtarea.focus();
                var range = document.selection.createRange();
                range.moveStart('character', -txtarea.value.length);
                strPos = range.text.length;
            } else if (br == "ff") {
                strPos = txtarea.selectionStart;
            }

            var front = (txtarea.value).substring(0, strPos);
            var back = (txtarea.value).substring(strPos, txtarea.value.length);
            txtarea.value = front + text + back;
            strPos = strPos + text.length;
            if (br == "ie") {
                txtarea.focus();
                var ieRange = document.selection.createRange();
                ieRange.moveStart('character', -txtarea.value.length);
                ieRange.moveStart('character', strPos);
                ieRange.moveEnd('character', 0);
                ieRange.select();
            } else if (br == "ff") {
                txtarea.selectionStart = strPos;
                txtarea.selectionEnd = strPos;
                txtarea.focus();
            }

            txtarea.scrollTop = scrollPos;

            //if ($(txtarea).hasClass('autosize-textarea') && typeof autosize != 'undefined') {
            //    autosize.update($(txtarea));
            //}
        },
        renderTemplate: function (template, data) {
            var html = "";
            if ($(template).length) {
                var tmp = $(template)
                if (data)
                    html = tmp.render(data);
                else
                    html = tmp.render();
            }
            return html;

        },
        isValidGuid: function (guid) {
            var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
            var match = regex.exec(guid);


            return match != null
                && guid != "00000000-0000-0000-0000-000000000000"

        },
        addElemntwithSlide: function (el, parent) {
            var elx = $($(el).hide());

            return $(elx.appendTo($(parent))).slideDown();
        },
        isIdEmpty: function (value) {

            return value == undefined || value.trim() == '' || value == '00000000-0000-0000-0000-000000000000';
        },
        showUploader: function (options) {
            if (!options) {
                options = {
                    params: {
                        Folder: "MyFile/{user}/{date}"
                    }
                }
            }
            Uploader.show(options);
            //var options = {
            //    acceptedFiles: 'image/*',
            //    maxFilesize: 2,
            //    maxFiles: 1,
            //    uploadMultiple: true,
            //    params  :params : {Folder: "MyFile/{user}/{date}"} 
            //              OR function() { return object },
            //    onSave  :function(list<upladedfiles>){ },
            //}; 
        },
        defaultTableRowCount: function () { return 20; },
        defaultFractionDigits: function () { return 2; },
        getEditorContent: function (name) {
            var value = '';
            for (var i in CKEDITOR.instances) {
                /* this  returns each instance as object try it with alert(CKEDITOR.instances[i])*/
                if (CKEDITOR.instances[i].name == name) {
                    value = CKEDITOR.instances[i].getData();
                }

            }
            return value;
        },
        initMask: function () {

            //$('.phone-number').mask('+(00) 000 000 00 00');
            //$('.mobile-number').mask('(500) 000 00 00');
            //$('.tax-input').mask('000 000 00 000');
            //$('.id-input').mask('000 000 00 000');
        },
        firstLetters: function (str) {

            var text = '';
            var arr = str.split(' ');
            for (i = 0; i < arr.length; i++) {
                text += arr[i].substr(0, 1)
            }
            return text;
        },
        firstOrDefault: function (arr, fieldName, value) {
            var index = arr.findIndex(o => o[fieldName] == value);
            if (index > -1) {
                return arr[index];
            }
            return {};
        },
        initExtentions: function () {
            if (typeof String.prototype.replaceAll !== "function") {
                String.prototype.replaceAll = function (find, replace) {
                    var str = this;
                    while (str.indexOf(find) > -1) {
                        str = str.replace(find, replace);
                    }
                    return str;
                };
            }
            
            if (typeof String.prototype.isEmpty !== "function") {
                String.prototype.isEmpty = function () {
                    return this == undefined || this == null || this.trim() == '';
                };
            }

            if (typeof String.prototype.padLeftZero !== "function") {
                String.prototype.padLeftZero = function (size) {
                    var s = String(this);
                    while (s.length < (size || 2)) { s = "0" + s; }
                    return s;
                }
            }

            if (typeof Number.prototype.padLeftZero !== "function") {
                Number.prototype.padLeftZero = function (size) {
                    var s = String(this);
                    while (s.length < (size || 2)) { s = "0" + s; }
                    return s;
                }
            }
        },
        padLeftZero: function (s, size) {
            while (s.length < (size || 2)) { s = "0" + s; }
            return s;
        }


    };
}();

$(document).ready(function () {
    General.init();
     
});

$('img').on("error", function () {
    $(this).attr('src', '/images/missing.png');
});

