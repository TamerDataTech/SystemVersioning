var Component = function () {

    var state = {
        success: "success",
        danger: "danger",
        warning: "warning",
        info: "info",
        primary: "primary",
        brand: "brand"
    }

    var uploadFileType = {
        Image: 1,
        File: 2
    }
    return {
        //main function to initiate the module
        init: function () {

        },
        uploadFileType: uploadFileType,
        state: state,
        alert: function (options) {
            options = $.extend(true, {
                container: $(".page-content").eq(0), // alerts parent container(by default placed after the page breadcrumbs)
                place: "prepend", // "append" or "prepend" in container 
                type: 'success', // alert's type
                message: "", // alert's message
                close: true, // make alert closable
                reset: true, // close all previouse alerts first
                focus: true, // auto scroll to the alert after shown
                closeInSeconds: 0, // auto close after defined seconds
                icon: "", // put icon before the message
                borderStyle: true
            }, options);

            var id = Utils.getRandId(); //  mUtil.getUniqueID("App_alert");

            var html = '<div id="' + id + '" class="alert custom-alerts ' + (options.borderStyle ? "border" : "alert") + '-' + options.type + (options.close ? ' alert-dismissible' : ' ') + ' ">' +
                (options.close ? '<button type="button" class="close" data-dismiss="alert" aria-label="Close"> <span aria-hidden="true" >×</span ></button>' : '') +
                (options.icon !== "" ?
                    '<div class="d-flex align-items-center">  <i class="bx bx-' + options.icon + '"></i>  <span> ' + options.message + ' </span></div>'
                    : options.message) + '</div>';




            if (options.reset) {
                $('.custom-alerts').remove();
            }

            if (!options.container) {
                if ($('.page-fixed-main-content').size() === 1) {
                    $('.page-fixed-main-content').prepend(html);
                } else if (($('body').hasClass("page-container-bg-solid") || $('body').hasClass("page-content-white")) && $('.page-head').size() === 0) {
                    $('.page-title').after(html);
                } else {
                    if ($('.page-bar').size() > 0) {
                        $('.page-bar').after(html);
                    } else {
                        $('.page-breadcrumb, .breadcrumbs').after(html);
                    }
                }
            } else {
                if (options.place == "append") {
                    $(options.container).append(html);
                } else {
                    $(options.container).prepend(html);
                }
            }



            //if (options.focus) {
            //    General.scrollTo($('#' + id), 100);
            //}

            if (options.closeInSeconds > 0) {
                setTimeout(function () {
                    $('#' + id).remove();
                }, options.closeInSeconds * 1000);
            }

            return id;
        },
        block: function (target, options) {
            var el = $(target);


            if (target == 'body') {
                $.blockUI({
                    overlayCSS: {
                        background: 'rgba(142, 159, 167, 0.3)',
                        opacity: 1,
                        cursor: 'wait'
                    },
                    css: {
                        width: 'auto',
                        top: '50%',
                        left: '50%'
                    },
                    message: '<div class="blockui-default-message"><i class="fa fa-circle-o-notch fa-spin"></i></div>',
                    blockMsgClass: 'block-msg-message-loader'
                });
            } else {
                el.block({
                    message: '<div class="bx bx-revision icon-spin font-medium-2"></div>',
                    overlayCSS: {
                        backgroundColor: 'FFFFFF',
                        opacity: 0.8,
                        cursor: 'wait'
                    },
                    css: {
                        border: 0,
                        padding: 0,
                        backgroundColor: 'transparent'
                    }
                });
            }
        },
        /**
       * Un-blocks the blocked element 
       * @param {object} target jQuery element object
       */
        unblock: function (target) {
            if (target && target != 'body') {
                $(target).unblock();
            } else {
                $.unblockUI();
            }
        },

        /**
        * Blocks the page body element with loading indicator
        * @param {object} options 
        */
        blockPage: function (options) {
            return Component.block('body', options);
        },

        /**
        * Un-blocks the blocked page body element
        */
        unblockPage: function () {
            return Component.unblock('body');
        },
        notify: function (message, type, timer, title, showProgress, position) {


            if (type == undefined || type == "") {
                type = state.Primary;
            }

            if (timer == undefined || timer == "0" || timer < 1) {
                timer = 3;
            }

            if (position == undefined || position == "") {
                position = "toast-top-center";
            }

           

            toastr.options.showMethod = "slideDown";
            toastr.options.hideMethod = "slideUp";
            toastr.options.closeButton = true;
            toastr.options.positionClass = position;
            toastr.options.progressBar = showProgress;
            toastr.options.timeOut = (timer * 1000) + ""; 


            switch (type) {
                case state.success:
                    toastr["success"](message, title);
                    break;
                case state.danger:
                    toastr["error"](message, title);
                    break;
                case state.warning:
                    toastr["warning"](message, title);
                    break;
                case state.info:
                    toastr["info"](message, title);
                    break;
                default:
                    toastr["info"](message, title);
                    break;
            }


            //notify


        },
        checkStrength: function (password, panel) {
            var strength = 0

            if (password.length < 1) {
                $(panel).removeClass('short-password').removeClass('weak-password').removeClass('good-password').removeClass('strong-password');
                return '';
            }

            if (password.length < 6) {
                $(panel).removeClass('short-password').removeClass('weak-password').removeClass('good-password').removeClass('strong-password');
                $(panel).addClass('short-password')
                return Utils.getMessage('too-short');
            }
            if (password.length > 7) strength += 1
            // If password contains both lower and uppercase characters, increase strength value.
            if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
            // If it has numbers and characters, increase strength value.
            if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1
            // If it has one special character, increase strength value.
            if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
            // If it has two special characters, increase strength value.
            if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
            // Calculated strength value, we can return messages
            // If value is less than 2
            if (strength < 2) {
                $(panel).removeClass('short-password').removeClass('weak-password').removeClass('good-password').removeClass('strong-password');
                $(panel).addClass('weak-password')
                return Utils.getMessage('weak');
            } else if (strength == 2) {
                $(panel).removeClass('short-password').removeClass('weak-password').removeClass('good-password').removeClass('strong-password');
                $(panel).addClass('good-password')
                return Utils.getMessage('good');
            } else {
                $(panel).removeClass('short-password').removeClass('weak-password').removeClass('good-password').removeClass('strong-password');
                $(panel).addClass('strong-password')
                return Utils.getMessage('strong');
            }
        },
        GetDataTableLanguageUrl: function () {
            return Utils.getCurrentLanguage() == 'tr-TR' ? "//cdn.datatables.net/plug-ins/1.10.19/i18n/Turkish.json" : "";
        },
        blinkIt: function (item, className) {
            if (className == undefined || className == "") {
                className = "blink-bg";
            }


            setTimeout(function () { $(item).addClass(className); setTimeout(function () { $(item).removeClass(className); }, 1000); }, 1000);

        },
        largeSelect: function (parent, items, template, placeHolder, valueProp, selectedValue) {
            var a = document.createElement("div");
            a.setAttribute("class", "select-selected");
            a.innerHTML = placeHolder; 
            $(parent).addClass("custom-menu-selector");
            parent[0].appendChild(a);
            /*for each element, create a new DIV that will contain the option list:*/
            var b = document.createElement("div");
            b.setAttribute("class", "select-items select-hide");
            for (var j = 0; j < items.length; j++) {
                /*for each option in the original select element,
                create a new DIV that will act as an option item:*/
                var c = document.createElement("div");
                c.setAttribute("class", "select-item");

                var item = items[j];
                c.innerHTML = $(template).render(item);


                $(c).attr("data-json", JSON.stringify(item));
                if (valueProp != undefined && valueProp != null && valueProp.trim() != '') {
                    $(c).attr("data-id", item[valueProp]); 
                }

                c.addEventListener("click", function (e) {
                    var selectorParent = $(this).closest('.custom-menu-selector');
                    selectorParent.find('.select-selected').html(this.innerHTML); 
                    selectorParent.attr('data-id', $(this).attr("data-id")); 
                    $(document).trigger(valueProp+"-choice-selected");
                });

                b.appendChild(c);
            }
            parent[0].appendChild(b);
            a.addEventListener("click", function (e) {
                /*when the select box is clicked, close any other select boxes,
                and open/close the current select box:*/
                e.stopPropagation();
                Component.closeLargeSelect(this);
                this.nextSibling.classList.toggle("select-hide");
                this.classList.toggle("select-arrow-active");
                
            });

            /*if the user clicks anywhere outside the select box,
            then close all select boxes:*/
            document.addEventListener("click", Component.closeLargeSelect);
        },
        closeLargeSelect: function (elmnt) {
            /*a function that will close all select boxes in the document,
             except the current select box:*/
            var x, y, i, xl, yl, arrNo = [];
            x = document.getElementsByClassName("select-items");
            y = document.getElementsByClassName("select-selected");
            xl = x.length;
            yl = y.length;
            for (i = 0; i < yl; i++) {
                if (elmnt == y[i]) {
                    arrNo.push(i)
                } else {
                    y[i].classList.remove("select-arrow-active");
                }
            }
            for (i = 0; i < xl; i++) {
                if (arrNo.indexOf(i)) {
                    x[i].classList.add("select-hide");
                }
            }
        }

    };
}();

$(document).ready(function () {
    Component.init();

});