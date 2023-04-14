var Utils = function () {
    var idGenerator = 0;

    var requestType = {
        Get: "Get",
        Post: "POST",
        Put: "PUT",
        Delete: "DELETE"
    }

    var requestDataType = {
        Json: "json",
        Html: "html",
        Xml: "xml"
    }

    async function getData(key) {
        return $.ajax({
            'url': '/Common/GetMessageTranslation/' + key,
            'success': function (data) {
                return data.Response;
            },
            'error': function (error) {
                console.log("An error occurred:", error);
            }
        });
    }

    //var getMessage = async  function (key) {
    //    var result = key;
    //     $.when(General.ajax({
    //        url: "/Common/GetMessageTranslation/" + key,
    //         async:false,
    //        success: function (result) {
    //           result = result.Response;
    //        },
    //        failure: function (result) {
    //            //deferred.resolve(key);
    //         }
    //     })).done(function)


    //}  
    return {
        getRandId: function () {
            idGenerator++;
            return idGenerator;
        },
        deleteFlag: 99,
        requestType: requestType,
        requestDataType: requestDataType,
        /**
       * Gets highest z-index of the given element parents
       * @param {object} el jQuery element object
       * @returns {number}  
       */
        getHighestZindex: function (el) {
            var elem = $(el),
                position, value;

            while (elem.length && elem[0] !== document) {
                // Ignore z-index if position is set to a value where z-index is ignored by the browser
                // This makes behavior of this function consistent across browsers
                // WebKit always returns auto if the element is positioned
                position = elem.css("position");

                if (position === "absolute" || position === "relative" || position === "fixed") {
                    // IE returns 0 when zIndex is not specified
                    // other browsers return a string
                    // we ignore the case of nested elements with an explicit value of 0
                    // <div style="z-index: -10;"><div style="z-index: 0;"></div></div>
                    value = parseInt(elem.css("zIndex"), 10);
                    if (!isNaN(value) && value !== 0) {
                        return value;
                    }
                }
                elem = elem.parent();
            }
        },
        /**
      * Gets element actual/real width
      * @param {object} el jQuery element object
      * @returns {number}  
      */
        realWidth: function (el) {
            var clone = $(el).clone();
            clone.css("visibility", "hidden");
            clone.css('overflow', 'hidden');
            clone.css("height", "0");
            $('body').append(clone);
            var width = clone.outerWidth();
            clone.remove();

            return width;
        },
        getMessage: function (key) {
            var result = key;

            if ($("#global-message-code").attr("data-message-" + key) != undefined) {
                return $("#global-message-code").attr("data-message-" + key);
            } else {
                return result;
            }
            return result;
        },
        getMessageFromServer: async function (key) {
            var result = key;
            await $.when(getData(key).then(
                success =>
                    result = success.Response
            ));
            return result;
        },
        getPageMessage: function (key) {


            var result = $("#page-message-code").attr("data-message-" + key);

            //if (result == undefined || result == '')
            //    result = key.replace("-", " ");

            return result;
        },
        getDateName: function (day) {
            var result = $("#global-days").attr("data-day-" + day);
            return result;
        },
        formatDecimal: function (value, fractionDigits) {
            value = Utils.getDecimal(value);
            if (isNaN(value)) {
                return "";
            }
            fractionDigits = fractionDigits == undefined ? 2 : fractionDigits;
            return value.toLocaleString(undefined, { minimumFractionDigits: fractionDigits, maximumFractionDigits: fractionDigits });
        },
        getGlobalCultureInfo: function (name) {

            return $("#global-culture-info").attr("data-culture-" + name);

        },
        getDecimalSeparator: function () {
            return Utils.getGlobalCultureInfo("decimal-separator");
        },
        getDecimalRemote: function (value) {
            value += "";
            var found = false;
            var resultValue = "";
            for (var i = value.length - 1; i >= 0; i--) {
                if (!found && (value[i] == "," || value[i] == ".")) {
                    found = true;
                    resultValue = Utils.getDecimalSeparator() + resultValue;
                } else {
                    resultValue = value[i].replace(/[^0-9]/g, '') + resultValue;
                }
            }
            return resultValue;
        },
        getDecimal: function (value) {
            value += "";
            var found = false;
            var resultValue = "";
            for (var i = value.length - 1; i >= 0; i--) {
                if (!found && (value[i] == "," || value[i] == ".")) {
                    found = true;
                    resultValue = "." + resultValue;
                } else {
                    resultValue = value[i].replace(/[^0-9]/g, '') + resultValue;
                }
            }
            if (resultValue == "")
                resultValue = "0"
            var result = parseFloat(resultValue);
            if (result == NaN)
                result = 0
            return result
        },
        getDate: function (value) {
            if (value == undefined || value == "")
                return "";
            return new Date(parseInt(value.substr(6)));
        },
        getLocalDate: function (value) {
            if (value == undefined || value == "")
                return "";
            return Utils.getDate(value).toLocaleString();
        },
        getIsoDate: function (value) {
            if (value == undefined || value == "")
                return "";
            return Utils.getDate(value).toISOString();
        },
        formatDateTime: function (orginaldate) {
            var date = new Date(orginaldate);
            var day = date.getDate();
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            var h = date.getHours();
            var m = date.getMinutes();
            
            var date = day.padLeftZero(2) + "/" + month.padLeftZero(2) + "/" + year + " " + h.padLeftZero(2) + ":" + m.padLeftZero(2);
            return date;
        },
        formatDateTimeYearMoth: function (orginaldate) {
            var date = new Date(orginaldate);
            var month = date.getMonth() + 1;
            var year = date.getFullYear();
            if (month < 10) {
                month = "0" + month;
            }
            var date = year + "-" + month;
            return date;
        },
        getCurrentLanguage: function () {
            return $("head").attr("lang")
        },
        getClientId: function () {
            return $.connection.hub.id;
        },
        getPaggingObject: function (result) {



            var pages = [
                {
                    Page: 0,
                    Display: "1",
                    selected: result.PageIndex == 0
                }
            ];

            var totalPages = Math.ceil(result.TotalCount / result.PageSize);
            if (totalPages > 1) {
                // first 3 button 

                if (totalPages > 6 && result.PageIndex > 2) {
                    pages.push({
                        Page: -1,
                        Display: "..",
                        selected: false
                    });
                }

                //if (result.PageIndex >= 1 && (result.PageIndex < totalPages-1 )) {
                if (result.PageIndex - 1 > 0) {
                    pages.push({
                        Page: result.PageIndex - 1,
                        Display: result.PageIndex,
                        selected: false
                    });

                }

                if (result.PageIndex != 0 && (result.PageIndex + 1 != totalPages)) {
                    pages.push({
                        Page: result.PageIndex,
                        Display: result.PageIndex + 1,
                        selected: true
                    });
                }

                if (result.PageIndex + 1 < (totalPages - 1)) {
                    pages.push({
                        Page: result.PageIndex + 1,
                        Display: result.PageIndex + 2,
                        selected: false
                    });
                }
                //} 

                if (result.PageIndex + 2 < (totalPages - 1)) {
                    pages.push({
                        Page: -1,
                        Display: "..",
                        selected: false
                    });
                }

                pages.push({
                    Page: totalPages - 1,
                    Display: totalPages,
                    selected: result.PageIndex == (totalPages - 1)
                });

            }

            result.Pages = pages;
            return result;

        },

        getDataTableLanguage: function () {

            return {

                search: "",
                searchPlaceholder: "Search",
                sProcessing: "Processing...",
                sZeroRecords: "No data found!",
                sEmptyTable: "No data found!",
                "oPaginate": {
                    "sFirst": "First",
                    "sLast": "Last",
                    "sNext": "Next",
                    "sPrevious": "Previous"
                }
            }
        },

        showDataTableRowsInfo: function (tableId, dataTable) {

            var pagingInfo = dataTable.page.info();
            if (pagingInfo.recordsDisplay > 1) {
                var params = dataTable.ajax.params();
                var text = Utils.getMessage("Showing [X] to [Y] of [N] entries");


                text = text.replace("[N]", pagingInfo.recordsDisplay);
                text = text.replace("[X]", parseInt(pagingInfo.start) + 1);
                text = text.replace("[Y]", Math.min(parseInt(pagingInfo.start) + parseInt(pagingInfo.length), parseInt(pagingInfo.recordsDisplay)));

                if (params == undefined) {
                    $('#' + tableId + '_wrapper').find('.dataTables_paginate').closest('.row').find('div').first().html('<div class="float-left pt-2">' + text + '</div>');
                }
                else {
                    $('#' + tableId + '_wrapper').find('.dataTables_paginate').prepend('<div class="float-left"><b>' + text + '</b></div>');
                }


            }

        },
        getCodePatternExample: function (value, prevValue) {

            var date = new Date();

            value = value.replace('[YYYY]', date.getFullYear());
            value = value.replace('[YY]', date.getFullYear().toString().substring(2, 4));
            value = value.replace('[MM]', (date.getMonth() + 1).toString().padStart(2, '0'));
            value = value.replace('[DD]', date.getDate().toString().padStart(2, '0'));
            value = value.replace('[HH]', date.getHours().toString().padStart(2, '0'));
            value = value.replace('[mm]', date.getMinutes().toString().padStart(2, '0'));
            value = value.replace('[ss]', date.getSeconds().toString().padStart(2, '0'));
            value = value.replace('[mis]', date.getMilliseconds().toString().padStart(3, '0'));


            value = value.replace('[#]', "[*]");


            var hashValueLength = (value.match(/#/g) || []).length;

            if (prevValue != undefined && parseInt(prevValue) > -1) {

                var nextValue = parseInt(prevValue) + 1;

                var hashToReplace = "".padStart(hashValueLength, "#");

                if (hashToReplace.length > 0) {
                    value = value.replace(hashToReplace, nextValue.toString().padStart(hashValueLength, "0"));
                }
            }
            else {

                for (var i = 1; i < 10; i++) {

                    value = value.replace('#', i.toString());
                }
            }


            value = value.replace('[*]', "#");

            return {
                Value: value,
                HashValueLength: hashValueLength
            };
        },

        getCookie: function (name) {
            const value = `; ${document.cookie}`;
            const parts = value.split(`; ${name}=`);
            if (parts.length === 2) return parts.pop().split(';').shift();
        }
    };
}();

