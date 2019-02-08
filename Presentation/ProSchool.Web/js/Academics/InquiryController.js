ProApp.controller("InquiryController", function ($scope, $http) {

    $scope.RegisterStatusId = 2;
   
    $scope.Rules = [
                        { input: '#txtFName', message: 'First name is required!', action: 'valuechanged,blur,change', rule: 'required' },
                        { input: '#txtLName', message: 'Last name is required!', action: 'blur', rule: 'required' },
                        { input: '#txtRNumber', message: 'Registration number is required!', action: 'blur', rule: 'required' },
                        { input: '#txtAdd', message: 'Address is required!', action: 'blur', rule: 'required' },
                        { input: '#txtMobile', message: 'Mobile is required!', action: 'blur', rule: 'required' }
    ];
    
    $scope.ShowFilter = false;
    $scope.IsRegistered = false;
    $scope.IsProgram = false;
    $scope.IsEducation = false;
    var StudentProgram = {};
    var StudentId = 0;
    var IsRegisterCheck = true;

    $scope.GenderSwitchButtonSettings = {
        width: 130,
        height: 32,
        onLabel: 'Male',
        offLabel: 'Female',
        checked: true,
        theme: $scope.theme
    };
    $scope.OnAddNew = function () {
        ClearData();
        $scope.WindowTitle = 'Program Inquiry';
        $scope.IsRegistered = false;
        $scope.IsProgram = false;
        $scope.IsEducation = false;
        $('#liStudentInfo').addClass('active');
        $('#liOtherDetails').removeClass('active');
        $('#tabStudentInfo').addClass('active');
        $('#tabOtherDetails').removeClass('active');
        $scope.InquiryValidator.hide()
        $scope.WinAddNew.open();
    }
    $scope.InquirySource = {
        //localdata: [{ Id: 1, Name: 'Desai Vatsal', PhoneNumber: '9879768671', Department: 'Rock & Pop', Class: 'Keyboard', InquiryDate: new Date() }],
        url: '/Academics/GetAllInquiry',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'PhoneNumber', type: 'string' },
            { name: 'Department', type: 'string' },
            { name: 'Class', type: 'string' },
            { name: 'InquiryDate', type: 'date' }
        ],
        data: { registerStatusId: 2 },
    };
    $scope.DepartmentSource = {
        //localdata: [{ Id: 1, Name: 'Rock & Pop', Description: 'test' }, { Id: 2, Name: 'Classical', Description: 'test' }],
        url: '/Academics/GetAllDivisions',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.ClassSource = {
        //localdata: [{ Id: 1, Name: 'Keyboard', Description: 'test' }, { Id: 2, Name: 'Vocal', Description: 'test' }],
        url: '/Academics/GetAllSubjects',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.SectionSource = {
        //localdata: [{ Id: 1, Name: 'Mon-Wed-Fri [4/5]', Description: 'test' }, { Id: 2, Name: 'Tue-Thu-Sat [5/5]', Description: 'test' }],
        url: '/Academics/GetAllBatches',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.FeeSource = {
        //localdata: [{ Id: 1, Name: 'Quartly Fee ( 6000 )', Description: 'test' }, { Id: 2, Name: 'Yearly Fee ( 18000 )', Description: 'test' }],
        url: '/Academics/GetAllFees',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };
    $scope.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes();
    };
    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }
    $scope.OnRefresh = function () {
        $scope.GrdInquiry.updatebounddata();
    }
    $scope.OnSearChclick = function (event) {
        $scope.InquirySource.data = { registerStatusId: $scope.RegisterStatusId }
    }
    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }
    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdInquiry.getrowdata(row);
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                $.ajax({
                    url: "/Academics/DeleteInquiry",
                    type: "POST",
                    dataType: "json",
                    data: { Id: dataRecord.Id },
                    success: function (response) {
                        if (response.Status == 1) {
                            $scope.openMessageBox('Success', response.Message, 400, 100);
                            $scope.GrdInquiry.updatebounddata();
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
  
    $scope.EditCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)'  href='javascript:;'>Edit</a></div>";
    }
  
    $scope.Edit = function (row, event) {
        var dataRecord = $scope.GrdInquiry.getrowdata(row);
        $scope.WindowTitle = 'Student Information';
        $scope.IsRegistered = true;
        $scope.IsProgram = false;
        $scope.IsEducation = false;
        IsRegisterCheck = false;
        StudentId = dataRecord.Id;
        $('#liProgramDetails').removeClass('active');
        $('#liEducationDetails').removeClass('active');
        $('#liStudentInfo').addClass('active');
        $('#tabProgramDetails').removeClass('active');
        $('#tabStudentInfo').addClass('active');
        $('#tabEducationDetails').removeClass('active');
        $.ajax({
            url: "/Academics/GetStudentByEditId",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            cache: false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                if (typeof (data.Student.DOB) != 'undefined' && data.Student.DOB != null) {
                    //data.DOB = new Date($scope.ToJavaScriptDate(data.DOB));
                    $scope.dtDOB.setDate(new Date(data.Student.DOB));
                }
                $scope.$apply(function () {
                    $scope.Student = data.Student;
                    //$scope.Inquiry = data.Inquiry;
                    $scope.GrdInquiry.updatebounddata();
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.InquiryValidator.hide()
        $scope.WinAddNew.open();
    }
    $scope.RegisterCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Register(" + row + ", event)' class='fa fa-edit fa-2' href='javascript:;'></a></div>";
    }
    $scope.Register = function (row, event) {
        var dataRecord = $scope.GrdInquiry.getrowdata(row);
        $scope.WindowTitle = 'Student Registration';
        $scope.IsRegistered = true;
        $scope.IsProgram = true;
        $scope.IsEducation = true;
        IsRegisterCheck = true;
        StudentId = dataRecord.Id;
        $('#liStudentInfo').removeClass('active');
        $('#liProgramDetails').addClass('active');
        $('#liEducationDetails').removeClass('active');
        $('#tabStudentInfo').removeClass('active');
        $('#tabProgramDetails').addClass('active');
        $('#tabEducationDetails').removeClass('active');
        $.ajax({
            url: "/Academics/GetStudentById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            //  cache:false,
            data: { Id: dataRecord.Id },
            success: function (data) {
                if (typeof (data.Student.DOB) != 'undefined' && data.Student.DOB != null) {
                    //data.DOB = new Date($scope.ToJavaScriptDate(data.DOB));
                    $scope.dtDOB.setDate(new Date(data.Student.DOB));

                }
                $scope.$apply(function () {
                    $scope.Student = data.Student;
                    //$scope.Inquiry = data.Inquiry;
                });
                $scope.GrdInquiry.updatebounddata();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
        $scope.InquiryValidator.hide()
        $scope.WinAddNew.open();
    }
    var UploadFile = {};
    $scope.StudentPhoto_UploadEnd = function (event) {
        var args = event.args;
        var fileName = args.file;
        var serverResponce = args.response;
        UploadFile = JSON.parse(serverResponce);
        // $.merge(UploadFiles, jsonResponse);
    };
    $scope.OnBrowse = function () {
        $scope.StudentPhoto.browse();
    }
    $scope.BatchBindingComplete = function (e) {
        $scope.DDLBatch.disableItem(2);
    };
    $scope.OnSaveInquiry = function () {
        var IsValidate = $scope.InquiryValidator.validate();
        if (!IsValidate)
            return;
        var Inquiry = $scope.Inquiry;
        var Student = $scope.Student;
      
        //var StudentProgram = {};
        if (Inquiry == null || typeof (Inquiry) == 'undefined')
            Inquiry = { Remark: Student.Remarks, SubjectId: 0, DivisionId: 0 };
        Inquiry.Remark = Student.Remarks;
        Student.DOB = $scope.dtDOB.getDate().toISOString();
        if ($scope.GenderSwitchButtonSettings.jqxSwitchButton('val'))
            Student.Gender = 'M'
        else
            Student.Gender = 'F'
        Student.OriginalFileName = UploadFile.OriginalFileName;
        Student.SystemFileName = UploadFile.SystemFileName;

        Inquiry.SubjectId = parseInt($scope.ddlSubject.val());
        Inquiry.DivisionId = parseInt($scope.ddlDivision.val());
       
        if ($scope.IsRegistered) {
            if (StudentProgram == null)
                StudentProgram = {};
            StudentProgram.SubjectId = parseInt($scope.ddlSubject.val());
            StudentProgram.DivisionId = parseInt($scope.ddlDivision.val());
            StudentProgram.BatchId = parseInt($scope.ddlBatch.val());
            StudentProgram.FeeId = parseInt($scope.ddlFee.val());
            StudentProgram.StudentId = StudentId;

        }
        else
            StudentProgram = null;
        $.ajax({
            cache: false,
            url: "/Academics/SaveInquiry",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { inquiry: Inquiry, student: Student, StudentProgram: StudentProgram, IsRegisterCheck: IsRegisterCheck },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.WinAddNew.close();
                    ClearData();

                    StudentId = response.Data.StudentId;
                    var studentProgramId = response.Data.StudentProgramId;
                    if (studentProgramId != undefined)
                        //$scope.openConfirm('Generate invoice?', 'Student registered successfully. Do you want to generate invoice?', 500, 100, function (flag) {

                        $.ajax({
                            url: "/Home/SaveInvoice",
                            type: "POST",
                            //contentType: "application/json;",
                            dataType: "json",
                            data: { StudentProgramId: studentProgramId },
                            success: function (response) {
                                if (response.Status == 1) {
                                    $scope.GrdInquiry.updatebounddata();
                                    ClearData();
                                    //$scope.openMessageBox('Success', response.Message, 400, 100);
                                    //    window.location = '/Finance/Accounts';
                                    //});
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

                    // });
                    $scope.GrdInquiry.updatebounddata();
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

    var ClearData = function (e) {
        if (typeof ($scope.Student) != 'undefined') {
            $scope.Student.FirstName = null;
            $scope.Student.LastName = null;
            $scope.Student.MiddleName = null;
            $scope.Student.RegistrationNo = null;
            $scope.Student.Mobile = null;
            $scope.Student.Email = null;
            $scope.Student.Remarks = null;
            $scope.Student.Address = null;
            $scope.Student.DOB = new Date();
            $scope.ddlDivision.selectIndex(0);
            $scope.ddlSubject.selectIndex(0);
            $scope.ddlBatch.selectIndex(0);
            $scope.ddlFee.selectIndex(0);
            $scope.Student.Id = 0;
            $scope.Student.InquiryId = 0;
            $scope.InquiryValidator.hide();
        }
    }
});