// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var Login = function () {

    return {
        init: function () {


            $("input", $("#form-auth")).off("keydown").keydown(function (e) {
                if (e.which == 13) {
                    var username = $("#username").val();
                    var password = $("#password").val();
                    if (username.trim() != "" && password.trim() != "") {
                        $(".btn-login").click();
                    }
                }
            });

            $(".btn-login").off("click").click(function () {

                var panel = $('#login-panel');

                if (!General.validateForm(panel)) {
                    return false;
                }

                var login = General.getFormModel(panel);

                General.ajax({
                    type: Utils.requestType.Post,
                    url: "/user/DoLogin",
                    data: login,
                    panel: panel,
                    sender: $(".btn-login"),
                    success: function (data) {

                        window.location = login.returnUrl == null || login.returnUrl == '' ? "/home/index" : login.returnUrl;
                    },
                    failure: function (data) {
                        General.notifyFailure(data.errorMessage);
                    }
                });

            });
        }
    };
}();

$(document).ready(function () {
    Login.init();

    setTimeout(function () {
        // after 1000 ms we add the class animated to the login/register card
        $('.card').removeClass('card-hidden');
    }, 300);
});
