var ProApp = angular.module("ProApp", ["jqwidgets"]).factory('$exceptionHandler', function () {
    return function (exception, cause) {
        exception.message += ' (caused by "' + cause + '")';
        //console.log(exception, cause);
        //alert(exception.message);
        //console.log(exception);
        throw exception;
    }
});

ProApp.controller("BaseController", function ($scope, $http) {
    //alert('Base');
    $scope.theme = 'ui-redmond';
    $scope.DisableSiteList = true;
    $scope.GridPageSizeOption = ['10', '20', '50', '100'];
    //$scope.GridHeight = 350;
    $scope.GridPageSize = 10;
    $scope.LoadingText = '<b>Please Wait...</b>'
    $scope.WindowAnimationType = 'slide';

    $scope.ActivateMenu = function (id) {
        //$('#sideMenu li .active').removeClass('active');
        //$(id).addClass('active');
    };

    // $scope.ToJavaScriptDate = function(value) {
    //    var pattern = /Date\(([^)]+)\)/;
    //    var results = pattern.exec(value);
    //    var dt = new Date(parseFloat(results[1]));
    //    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear()+" " +dt.getHours()+":"+dt.getMinutes();
    //};

    $scope.openConfirm = function (title, message, width, height, callBack) {
        var win = $('<div><div><b>' + title + '</b></div><div><div style="padding: 5px;">' + message + '</div><div></div></div></div>');
        // 
        var btnOk = $('<input type="button" value="Yes" style="margin-right: 10px;" />');
        var btnCancel = $('<input type="button" value="No" />');
        var lastChild = win[0].lastChild.lastChild;
        $(lastChild).append(btnOk);
        $(lastChild).append(btnCancel);
        $(lastChild).attr("class", "jqx-center-align");

        win.jqxWindow({
            height: height,
            width: width,
            theme: $scope.theme,
            autoOpen: true,
            isModal: true,
            resizable: false,
            okButton: btnOk,
            cancelButton: btnCancel,
            initContent: function () {
                btnOk.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });
                btnCancel.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });
                $('#ok').focus();
            }
        });

        win.on('close', function (event) {
            if (event.args.dialogResult.OK) {
                if (typeof (callBack) === 'function')
                    callBack(true);
            } else {
                if (typeof (callBack) === 'function')
                    callBack(false);
            }
        });
    }

    $scope.openMessageBox = function (title, message, width, height, callBack) {
        var win = $('<div><div><b>' + title + '</b></div><div><div style="padding: 5px;">' + message + '</div><div></div></div></div>');
        // 
        var btnOk = $('<input type="button" value="OK" style="margin-right: 10px" />');

        var lastChild = win[0].lastChild.lastChild;
        $(lastChild).append(btnOk);
        $(lastChild).attr("class", "jqx-center-align");

        win.jqxWindow({
            height: height,
            width: width,
            theme: $scope.theme,
            autoOpen: true,
            isModal: true,
            resizable: false,
            okButton: btnOk,
            initContent: function () {
                btnOk.jqxButton({
                    width: '65px',
                    theme: $scope.theme
                });

                $('#ok').focus();
            }
        });

        win.on('close', function (event) {
            if (event.args.dialogResult.OK) {
                if (typeof (callBack) === 'function')
                    callBack();
            }
        });
    }

    $scope.ShowLoader = function () {
        $('#jqxLoader').jqxLoader('open');
    }

    $scope.HideLoader = function () {
        $('#jqxLoader').jqxLoader('close');
    }
});

$(document).ready(function () {
    $.ajaxSetup({ cache: false });
});

$(document).ajaxError(function (e, x, settings, exception) {
    var message;
    var statusErrorMap = {
        '400': "Server understood the request, but request content was invalid.",
        '401': "Unauthorized access.",
        '403': "Forbidden resource can't be accessed.",
        '500': "Internal server error.",
        '503': "Service unavailable."
    };
    if (x.status) {
        message = statusErrorMap[x.status];
        if (!message) {
            message = "Unknown Error \n.";
        }
    } else if (exception == 'parsererror') {
        message = "Error.\nParsing JSON Request failed.";
    } else if (exception == 'timeout') {
        message = "Request Time out.";
    } else if (exception == 'abort') {
        message = "Request was aborted by the server";
    } else {
        message = "Unknown Error \n.";
    }
    $(this).css("display", "inline");
    $(this).html(message);
});

$(document).ajaxStart(function () {
    $('#jqxLoader').jqxLoader('open');
}).ajaxComplete(function () {
    $('#jqxLoader').jqxLoader('close');
});