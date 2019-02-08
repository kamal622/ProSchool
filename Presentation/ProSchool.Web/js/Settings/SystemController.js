ProApp.controller("SystemController", function ($scope, $http) {
   
    //var UploadFile = {};
    $scope.SchoolLogo_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        //UploadFile = JSON.parse(serverResponce);
        //var jsonResponse = JSON.parse($(serverResponce).text());
        //alert(jsonResponse.SysFileName);
    };

    $scope.OnBrowse = function () {
        $scope.SchoolLogo.browse();
    }

    //$scope.onSaveClick = function () {
    //    var System = $scope.System;

    $.ajax({
        url: "/Settings/GetByInstituteId",
        type: "GET",
        contentType: "application/json;",
        dataType: "json",
        success: function (data) {
            $scope.System = data;
          
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert('oops, something bad happened');
        }
    });
   
    $scope.onSaveClick = function () {
        
        var System = [];
        System.push({ Name: 'InstitutionName', Value: $scope.System.InstitutionName });
        System.push({ Name: 'Address', Value: $scope.System.Address });
        System.push({ Name: 'City', Value: $scope.System.City });
        System.push({ Name: 'State', Value: $scope.System.State });
        System.push({ Name: 'Pincode', Value: $scope.System.Pincode });
        System.push({ Name: 'Phone', Value: $scope.System.Phone });
        System.push({ Name: 'AboutSchool', Value: $scope.System.AboutSchool });
        // System.OriginalFileName = UploadFile.OriginalFileName;
        //System.SystemFileName = UploadFile.SystemFileName;
        $.ajax({
            cache: false,
            url: "/Settings/SaveSystem",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { system: System },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.openMessageBox('Success', response.Message, 400, 100);
                }
                else {
                    $scope.openMessageBox('Error', response.Message, 400, 100);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }
});
