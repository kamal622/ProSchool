ProApp.controller("HomeController", function ($scope, $http) {

    $scope.ShowFilter = false;
    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.OnRefresh = function () {
        $scope.GrdStudent.refresh();
    }
    $scope.StudentSource = {
        //localdata: [{ Id: 1, FirstName: 'Vatsal', LastName: 'Desai', PhoneNumber: '9879768671', Department: 'Rock & Pop', Class: 'Keyboard', Section: 'Tue-Thu-Sat', Active: true }],
        url: '/Home/GetDashboardData',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'StudentName', type: 'string' },
            { name: 'PhoneNumber', type: 'string' },
            { name: 'Department', type: 'string' },
            { name: 'Class', type: 'string' },
            { name: 'Section', type: 'string' },
            { name: 'NextDueDate', type: 'date' },
            { name: 'Status', type: 'string' },
            { name: 'Active', type: 'bool' }
        ]
    };

    $scope.CreateSwitchWidget = function (row, column, value, htmlElement) {
        var wSwitch = $("<div class='switch-sel' style='float:center;'></div>")
        wSwitch.prependTo(htmlElement);
        wSwitch.jqxSwitchButton({ onLabel: 'Yes', offLabel: 'No', theme: $scope.theme });

        wSwitch.jqxSwitchButton('val', value);
        wSwitch.on('change', function (event) {

            if (row.bounddata[column] != event.args.checked) {
                row.bounddata[column] = event.args.checked;
                // Save Active/Inactive here...
                // alert(row.bounddata.Id);
                // alert(event.args.checked);
                $.ajax({
                    url: "/Academics/UpdateIsActive",
                    type: "GET",
                    contentType: "application/json;",
                    dataType: "json",
                    data: { Id: row.bounddata.Id, Checked: event.args.checked },
                    success: function (response) {
                        if (response.Status == 1) {
                            $scope.$apply(function () {
                                $scope.GrdStudent.updatebounddata();
                            });
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
    };

    $scope.InitSwitchWidget = function (row, column, value, htmlElement) {
        $($(htmlElement).find('.switch-sel')).val(value);
    };
    $scope.GenerateInvoiceCellRenderer = function (row, column, value) {

        return "Pay";
    };
    $scope.GenerateInvoice = function (row) {
        var dataRecord = $scope.GrdStudent.getrowdata(row);

        $scope.StudentProgramId = dataRecord.Id;

        $.ajax({
            url: "/Home/SaveInvoice",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { StudentProgramId: $scope.StudentProgramId },
            success: function (response) {
                if (response.Status == 1) {
                    //$scope.openMessageBox('Success', response.Message, 400, 100);
                }
                else if (response.Status == 3) {
                    //$scope.openMessageBox('Warning', response.Message, 400, 100);
                    $scope.openConfirm('Invoice already generated', 'Do you want to go to account page?', 300, 100, function (flag) {
                        if (flag) {
                                            window.location = '/Finance/Accounts';
                        }
                    });
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