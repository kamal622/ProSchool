ProApp.controller("StudentController", function ($scope, $http) {
    $scope.ShowFilter = false;

    $scope.GenderSwitchButtonSettings = {
        width: 130,
        height: 32,
        onLabel: 'Male',
        offLabel: 'Female',
        checked: true,
        theme: $scope.theme
    };

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.DepartmentSearchSource = {
        // localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Rock & Pop', Description: 'test' }, { Id: 2, Name: 'Classical', Description: 'test' }],
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
        // localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Mon-Wed-Fri', Description: 'test' }, { Id: 2, Name: 'Tue-Thu-Sat', Description: 'test' }],
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
        // localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Keyboard', Description: 'test' }, { Id: 2, Name: 'Guitar', Description: 'test' }],
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

    $scope.FeeSource = {
        url: '/Finance/ViewFeeInfo',
        data: { Id: 0 },
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'InvoiceDate', type: 'date' },
            { name: 'PaymentDate', type: 'date' },
            { name: 'SubjectName', type: 'string' },
            { name: 'InvoiceAmount', type: 'int' },
            { name: 'PaymentStatus', type: 'string' }
        ]
    };

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
                //alert(row.bounddata.Id)
            var Index = parseInt($(event.currentTarget).attr('data-rowindex'));
            var rowID = $scope.GrdStudent.getrowid(Index);
            var row = $scope.GrdStudent.getrowdatabyid(rowID);
            if (row.IsActive != event.args.checked) {
                row.IsActive = event.args.checked;
                $.ajax({
                    url: "/Academics/UpdateStudentIsActive",
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
            }
        });
    };

    $scope.InitSwitchWidget = function (row, column, value, htmlElement) {
        $($(htmlElement).find('.switch-sel')).val(value);
    };

    $scope.EditCellRenderer = function (row, column, value) {
       return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    var studentId = 0;
    $scope.Edit = function (row, event) {
        var dataRecord = $scope.GrdStudent.getrowdata(row);
        studentId = dataRecord.Id;
        $scope.FeeSource.data.Id = studentId;
        $scope.GrdFee.updatebounddata();
        //alert(dataRecord.Id);
        $('#liOtherDetails').removeClass('active');
        $('#liFeeDetails').removeClass('active');
        $('#liStudentInfo').addClass('active');
        $('#tabOtherDetails').removeClass('active');
        $('#tabFeeDetails').removeClass('active');
        $('#tabStudentInfo').addClass('active');
        $scope.WinStudent.open();
        $.ajax({
            url: "/Academics/GetStudentById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: studentId },
            success: function (data) {
                $scope.$apply(function () {
                    if (typeof (data.Student.DOB) != 'undefined' && data.Student.DOB != null) {
                        $scope.dtDOB.setDate(new Date(data.Student.DOB));
                    }
                    $scope.Student = data.Student;
                    if (data.Student.Gender == 'M') {
                        $scope.GenderSwitchButtonSettings.jqxSwitchButton('val', true)
                    }
                    else
                        $scope.GenderSwitchButtonSettings.jqxSwitchButton('val', false)
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.StudentSource = {
        //localdata: [{ Id: 1, FirstName: 'Vatsal', LastName: 'Desai', PhoneNumber: '9879768671', Department: 'Rock & Pop', Class: 'Keyboard', Section: 'Tue-Thu-Sat', Active: true }],
        url: '/Academics/GetAllStudentData',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'StudentName', type: 'string' },
            { name: 'Mobile', type: 'string' },
            { name: 'Programs', type: 'string' },
            { name: 'IsActive', type: 'bool' }
        ],
        data: { DivisionId: 0, BatchId: 0, SubjectId: 0 }
    };

    $scope.OnRefresh = function () {
        $scope.GrdStudent.updatebounddata();
    }

    var UploadFile = {};
    $scope.StudentPhoto_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        UploadFile = JSON.parse(serverResponce);
        // $.merge(UploadFiles, jsonResponse);
    };
    $scope.OnSaveStudent = function () {
        var Student = $scope.Student;

        Student.DOB = $scope.dtDOB.getDate().toISOString();
        if ($scope.GenderSwitchButtonSettings.jqxSwitchButton('val'))
            Student.Gender = 'M'
        else
            Student.Gender = 'F'
        Student.OriginalFileName = UploadFile.OriginalFileName;
        Student.SystemFileName = UploadFile.SystemFileName;

        $.ajax({
            url: "/Academics/SaveStudent",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { student: Student },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.WinStudent.close();
                    $scope.GrdStudent.updatebounddata();
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
    $scope.OnViewStudent = function () {
        var Division = ($scope.ddlDivision.val() == "" ? 0 : $scope.ddlDivision.val());
        var Batch = ($scope.ddlBatch.val() == "" ? 0 : $scope.ddlBatch.val());
        var Subject = ($scope.ddlSubject.val() == "" ? 0 : $scope.ddlSubject.val());
        $scope.StudentSource.data = { DivisionId: Division, BatchId: Batch, SubjectId: Subject };
    }
});