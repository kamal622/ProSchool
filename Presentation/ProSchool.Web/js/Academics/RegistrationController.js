ProApp.controller("RegistrationController", function ($scope, $http) {
    $scope.ShowFilter = false;
    $scope.Program = {};
    $scope.DepartmentSearchSource = {
        //localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Rock & Pop', Description: 'test' }, { Id: 2, Name: 'Classical', Description: 'test' }],
        url: '/Academics/GetAllDivisions',
        data: { sendDefaultItem: true },
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.SectionSearchSource = {
        //localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Mon-Wed-Fri', Description: 'test' }, { Id: 2, Name: 'Tue-Thu-Sat', Description: 'test' }],
        url: '/Academics/GetAllBatches',
        data: { sendDefaultItem: true },
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.SubjectSearchSource = {
        //localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Keyboard', Description: 'test' }, { Id: 2, Name: 'Guitar', Description: 'test' }],
        url: '/Academics/GetAllSubjects',
        data: { sendDefaultItem: true },
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.OnRefresh = function () {
        $scope.GrdStudent.updatebounddata();
    }

    $scope.PopupDepartmentSource = {
        url: '/Academics/GetAllDivisions',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.PopupClassSource = {
        url: '/Academics/GetAllSubjects',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.PopupSectionSource = {
        url: '/Academics/GetAllBatches',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.PopupFeeSource = {
        url: '/Academics/GetAllFees',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.EditCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    $scope.Edit = function (row, event) {
        var dataRecord = $scope.GrdStudent.getrowdata(row);
        studentprogramId = dataRecord.Id;
        $.ajax({
            url: "/Academics/GetProgramById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: studentprogramId },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.Program = data;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.WinProgram.open();
    }

    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }

    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdStudent.getrowdata(row);
        //StudentId = dataRecord.Id;
        //alert(dataRecord.Id);
         $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                $.ajax({
                    url: "/Academics/DeleteRegStudentByID",
                    type: "POST",
                    dataType: "json",
                    data: { Id: dataRecord.Id },
                    success: function (response) {
                            if (response.Status == 1) {
                            $scope.openMessageBox('Success', response.Message, 400, 100);
                            $scope.GrdStudent.updatebounddata();
                        }
                        else if (response.Status == 3) {
                            $scope.openMessageBox('Warning', response.Message, 400, 100);
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
    }

    $scope.StudentSource = {
        //localdata: [{ Id: 1, FirstName: 'Vatsal', LastName: 'Desai', PhoneNumber: '9879768671', Department: 'Rock & Pop', Class: 'Keyboard', Section: 'Tue-Thu-Sat', Active: true }],
        url: '/Academics/GetAllRegistrationData',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'StudentId', type: 'int' },
            { name: 'StudentName', type: 'string' },
            { name: 'PhoneNumber', type: 'string' },
            { name: 'Department', type: 'string' },
            { name: 'Class', type: 'string' },
            { name: 'Section', type: 'string' },
            { name: 'NextDueDate', type: 'date' },
            { name: 'Active', type: 'bool' }
        ],
        data: { DivisionId: 0, BatchId: 0, SubjectId: 0 }
    };
    $scope.ExtendDueDate = function (row, columnfield, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Extend(" + row + ", event)'style='margin: 4px;text-decoration:underline;' href='javascript:;'>GracePeriod</a></div>";
    }
    var programId = 0;
    $scope.Extend = function (row, event) {
        var dataRecord = $scope.GrdStudent.getrowdata(row);
        programId = dataRecord.Id;
        $scope.Extendwindow.open();
    }
    $scope.SaveExtendDate = function () {
        var ExtendDate = $scope.dtExtend.getDate().toISOString();
        $.ajax({
            url: "/Academics/UpdateDueDate",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { Id: programId, extendDate: ExtendDate },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.Extendwindow.close();
                    $scope.GrdStudent.updatebounddata();
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
    $scope.OnSaveProgram = function () {
        var Program = $scope.Program;
        $scope.Program.DivisionId = parseInt($scope.divDivision.val());
        $scope.Program.SubjectId = parseInt($scope.divSubject.val());
        $scope.Program.BatchId = parseInt($scope.divBatch.val());
        $scope.Program.FeeId = parseInt($scope.divFee.val());
       
        $.ajax({
            url: "/Academics/UpdateProgram",
            type: "POST",
            dataType: "json",
            data: { studentprogram: Program },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.WinProgram.close();
                    $scope.GrdStudent.updatebounddata();
                }
                else if (response.Status == 3) {
                    $scope.openMessageBox('Warning', response.Message, 400, 100);
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
    $scope.OnSearChclick = function () {
        var Division = ($scope.ddlDivision.val() == "" ? 0 : $scope.ddlDivision.val());
        var Batch = ($scope.ddlBatch.val() == "" ? 0 : $scope.ddlBatch.val());
        var Subject = ($scope.ddlSubject.val() == "" ? 0 : $scope.ddlSubject.val());
        $scope.StudentSource.data = { DivisionId: Division, BatchId: Batch, SubjectId: Subject };
    }

    $scope.CreateSwitchWidget = function (row, column, value, htmlElement) {
        var wSwitch = $("<div class='switch-sel' style='float:center;'></div>")
        wSwitch.prependTo(htmlElement);
        wSwitch.jqxSwitchButton({ onLabel: 'Yes', offLabel: 'No', theme: $scope.theme });
        wSwitch.jqxSwitchButton('val', value);
        $(wSwitch).attr('data-rowindex', row.boundindex);
        wSwitch.on('change', function (event) {
            //if (row.bounddata[column] != event.args.checked) {
            //    row.bounddata[column] = event.args.checked;
                // Save Active/Inactive here...
                // alert(row.bounddata.Id);
            // alert(event.args.checked);

            var Index = parseInt($(event.currentTarget).attr('data-rowindex'));
            var rowID = $scope.GrdStudent.getrowid(Index);
            var row = $scope.GrdStudent.getrowdatabyid(rowID);
            if (row.Active != event.args.checked) {
                row.Active = event.args.checked;
                
                    $.ajax({
                        url: "/Academics/UpdateIsActive",
                        type: "GET",
                        contentType: "application/json;",
                        dataType: "json",
                        data: { Id: row.Id, Checked: event.args.checked }, //Id: row.bounddata.Id,
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
                //}
            }
        });
    };
    $scope.InitSwitchWidget = function (row, column, value, htmlElement) {
        $($(htmlElement).find('.switch-sel')).val(value);
    };
});