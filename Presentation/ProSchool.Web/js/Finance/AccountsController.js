ProApp.controller("AccountsController", function ($scope, $http) {
    $scope.ShowFilter = false;
    $scope.IsPT = true;
    $scope.GenerateInvoice = false;
    var studProgramId = 0;
    $scope.Account = {};
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
    $scope.SubSearchSource = {
        //localdata: [{ Id: 0, Name: 'All', Description: 'test' }, { Id: 1, Name: 'Keyboard', Description: 'test' }, { Id: 2, Name: 'Guitar', Description: 'test' }],
        url: '/Academics/GetAllSub',
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
        //localdata: [{ Id: 1, Name: 'Quartly Fee ( 6000 )', Description: 'test' }, { Id: 2, Name: 'Yearly Fee ( 18000 )', Description: 'test' }],
        url: '/Academics/GetAllFees',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' }
            //{ name: 'Frequency', type: 'int' }//,
            //{ name: 'InvoiceAmount', type: 'int' },
            //{ name: 'InvoiceDate', type: 'date' },
            //{ name: 'Description', type: 'string' }
        ]
    };
    $scope.InvoiceFeeSource = {
        // localdata: [{ Id: 0, InvoiceDate: '31/05/2017', PaymentDate: '01/06/2017' ,SubjectName : 'keybord' , InvoiceAmount : '6000' ,PaymentStatus : 'paid' }, { Id: 1, InvoiceDate: '30/05/2017', PaymentDate: '02/06/2017' ,SubjectName : 'keybord' , InvoiceAmount : '6000' ,PaymentStatus : 'paid' }],
        url: '/Finance/ViewInvoiceDetails',
        data: { Id: 0 },
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'StudentProgramId', type: 'int' },
            { name: 'InvoiceDate', type: 'date' },
            { name: 'PaymentDate', type: 'date' },
            { name: 'SubjectName', type: 'string' },
            { name: 'InvoiceAmount', type: 'int' },
            { name: 'PaymentType', type: 'string' },
            { name: 'PaymentStatus', type: 'string' },
            { name: 'IsPaid', type: 'bool' },//
        ]
    };

    var r = 0;
    $scope.GenerateNewInvoice = function () {
        var studentPrograms = $scope.StudGrdInvoice.getrowdata(r);
        studProgramId = studentPrograms.Id;
        $scope.WinNewInvoice.open();
       
    }
   
    $scope.GenerateNew = function () {
        var row = $scope.InvoiceGrdFee.getrowdata();
        var fee = $scope.ddlFee.getSelectedItem().value //$scope.ddlFee.val();
        
        //alert(row.StudentProgramId);
            $.ajax({
                 url: "/Finance/GenerateNewInvoice",
                type: "POST",
                dataType: "json",
                data: { StudentProgramId: studProgramId , FeeId: fee },//row.StudentProgramId
                success: function (response) {
                    if (response.Status == 1) {
                        $scope.InvoiceGrdFee.updatebounddata();
                        $scope.GrdInvoice.updatebounddata();
                        $scope.WinNewInvoice.close();
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

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.OnRefresh = function () {
        $scope.GrdInvoice.updatebounddata();
        $scope.StudGrdInvoice.updatebounddata();
        $scope.InvoiceGrdFee.updatebounddata();

    }
    $scope.onPaymentTypeChange = function (event) {
        var item = $scope.ddlPaymentType.getSelectedItem();
        var PaymentType = item.label;
        if (PaymentType == "Cash") {
            $scope.IsPT = true;
        }
        else if (PaymentType == "Cheque") {
            $scope.IsPT = false;
        }
    }
    $scope.InvoiceSource = {
        url: '/Finance/GetAllInvoiceData',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'StudentName', type: 'string' },
            { name: 'PaymentStatus', type: 'string' },
            { name: 'IsPaid', type: 'bool' },
            { name: 'Mobile', type: 'string' },
            { name: 'Department', type: 'string' },
            { name: 'Class', type: 'string' },
            { name: 'InvoiceDate', type: 'date' },
            { name: 'PaymentDate', type: 'date' },
            { name: 'IsActive', type: 'bool' }
        ],
        data: { StatusId: 0, SubjectId: 0, StudentName:'' }
    };
    $scope.OnViewInvoices = function () {
        var Status = ($scope.ddlStatus.val() == "" ? 0 : $scope.ddlStatus.val());
        var Subject = ($scope.ddlSubject.val() == "" ? 0 : $scope.ddlSubject.val());
        var FirstName = $scope.ddlStudentName.val();
        $scope.InvoiceSource.data = { StatusId: Status, SubjectId: Subject, StudentName: FirstName };
        $scope.GrdInvoice.updatebounddata();
    }
    $scope.StudentInvoiceSource = {
        url: '/Finance/GetAllStudInvoice',
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
         ],
        data: { SubjectId: 0, StudentName: '' }
    };
    $scope.OnViewSubInvoices = function () {
        var Subject = ($scope.ddlSubjectName.val() == "" ? 0 : $scope.ddlSubjectName.val());
        var Fname = $scope.ddlStudName.val();
        //$scope.$apply(function (e) {
        $scope.StudentInvoiceSource.data.SubjectId = Subject;
        $scope.StudentInvoiceSource.data.StudentName = Fname;
        //});
        $scope.StudGrdInvoice.updatebounddata();
    }

    $scope.PayFeeCellRenderer = function (row, column, value) {
        return "Pay";
    };
    $scope.PayFeeNowCellRenderer = function (row, column, value) {
        return "PayNow";
    };
    $scope.ViewInvoiceCellRenderer = function (row, column, value) {
        return "View"; 
    };

    
    $scope.ViewInvoice = function (row) {
        r = row;
        var dataRecord = $scope.StudGrdInvoice.getrowdata(row);
        studProgramId = dataRecord.Id;
        // alert(dataRecord.Id);
        $scope.InvoiceFeeSource.data.Id = studProgramId;
        $scope.InvoiceGrdFee.updatebounddata();
        showButton();
        $scope.WinViewInvoice.open();
    }

    var showButton = function () {
        $.ajax({
            url: "/Finance/ShowInvoiceBtn",
            type: "GET",
            datatype: "json",
            data: { StudentProgramId: studProgramId },
            success: function (data) {
                $scope.$apply(function () {
                    if (data == true) {
                        $scope.GenerateInvoice = true;
                    }
                    else
                        $scope.GenerateInvoice = false;
                });
            },
            error: function (XMLHttpRequest, testStatus, errorThrown) {
                alert("oops, something bad happened");
            }
        });
    }

    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }
    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdInvoice.getrowdata(row);
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                $.ajax({
                    url: "/Finance/DeleteInvoice",
                    type: "POST",
                    dataType: "json",
                    data: { invoiceId: dataRecord.Id },
                    success: function (response) {
                        if (response.Status == 1) {
                            $scope.openMessageBox('Success', response.Message, 400, 100);
                            $scope.GrdInvoice.updatebounddata();
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

    var invoiceId = 0;
    $scope.PayFeeNow = function (row) {
        var dataRecord = $scope.InvoiceGrdFee.getrowdata(row);
        invoiceId = dataRecord.Id;
        $scope.WinPayFee.open();
        $.ajax({
                    url: "/Finance/GetInvoiceById",
                    type: "GET",
                    contentType: "application/json;",
                    dataType: "json",
                    data: { Id:dataRecord.Id },
                    success: function (data) {
                        $scope.$apply(function () {
                            $scope.Account = data;
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('oops, something bad happened');
                    }
                });
     }
  
    $scope.Pay = function () {
        //$scope.Account.Id = invoiceId;
        $scope.Account.FeeId = parseInt($scope.ddlFee.val());
        $scope.Account.PaymentType = parseInt($scope.ddlPaymentType.val());
        var IsChequeClear = true;

        if ($scope.Account.IsChequeClear == 1)
            IsChequeClear = true;

        var item = $scope.ddlPaymentType.getSelectedItem();
        var PaymentType = item.label;

        if (PaymentType == "Cash") {
            $scope.Account.BankName = null;
            $scope.Account.IFSCCode = null;
            var IsChequeClear = false;
        }
        $.ajax({
            url: "/Finance/UpdateFee",
            type: "POST",
            cache: false,
            //contentType: "application/json;", 
            dataType: "json",
            data: {
                Id: invoiceId, PaymentType: parseInt($scope.ddlPaymentType.val()), BankName: $scope.Account.BankName, //FeeId: parseInt($scope.ddlFee.val()),
                IFSCCode: $scope.Account.IFSCCode, IsChequeClear: IsChequeClear
            },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.WinPayFee.close();
                    $("#btnPaid").prop('checked', true);
                    $('#hdnId').val(invoiceId);
                    $('#frmReport').submit();
                    ClearData();
                    showButton();
                    $scope.GrdInvoice.updatebounddata();
                    $scope.InvoiceGrdFee.updatebounddata();
                    $scope.StudGrdInvoice.updatebounddata();
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
  
    $scope.CreateSwitchWidget = function (row, column, value, htmlElement) {
        var wSwitch = $("<div class='switch-sel' style='float:center;'></div>")
        wSwitch.prependTo(htmlElement);
        wSwitch.jqxSwitchButton({ onLabel: 'Paid', offLabel: 'Unpaid', theme: $scope.theme, width: '120' });
        wSwitch.jqxSwitchButton('val', value);
        $(wSwitch).attr('data-rowindex', row.boundindex);
        wSwitch.on('change', function (event) {
            var Index = parseInt($(event.currentTarget).attr('data-rowindex'));
            var rowID = $scope.GrdInvoice.getrowid(Index);
            var row = $scope.GrdInvoice.getrowdatabyid(rowID);
            if (row.IsPaid != event.args.checked) {
                row.IsPaid = event.args.checked;
                //
                if (event.args.checked) {
                    row.IsPaid = event.args.checked;
                    invoiceId = row.Id;
                    $scope.WinPayFee.open();
                    $scope.$apply(function () {
                        $scope.GrdInvoice.updatebounddata();
                     });
                }
                else {
                    $scope.openConfirm('Change payment status?', 'Do you want to change payment status paid to unpaid invoice?', 500, 100, function (flag) {
                        if(flag){
                    $.ajax({
                        url: "/Finance/UpdatePaymentStatus",
                        type: "GET",
                        contentType: "application/json;",
                        dataType: "json",
                        data: { Id: row.Id, PaymentStatus: event.args.checked },
                        success: function (response) {
                            if (response.Status == 1) {
                                $scope.$apply(function () {
                                    $scope.GrdInvoice.updatebounddata();
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
                     else
                            $scope.GrdInvoice.updatebounddata();
                            $scope.InvoiceGrdFee.updatebounddata();
                    });
                }
            }
        });
    };
    $scope.InitSwitchWidget = function (row, column, value, htmlElement) {
        $($(htmlElement).find('.switch-sel')).val(value);
    };

    studProgramId = 0;
    $scope.InvoiceCreateSwitchWidget = function (row, column, value, htmlElement) {
        //
        var wSwitchInvoice = $("<div class='switch-sel' style='float:center;'></div>");
        wSwitchInvoice.prependTo(htmlElement);
        wSwitchInvoice.jqxSwitchButton({ onLabel: 'Paid', offLabel: 'Unpaid', theme: $scope.theme, width: '120' });
        wSwitchInvoice.jqxSwitchButton('val', value);
        $(wSwitchInvoice).attr('data-rowindex', row.boundindex);
        wSwitchInvoice.on('change', function (event) {
            var Index = parseInt($(event.currentTarget).attr('data-rowindex'));
            var rowID = $scope.InvoiceGrdFee.getrowid(Index);
            var row = $scope.InvoiceGrdFee.getrowdatabyid(rowID);
            if (row.IsPaid != event.args.checked) {
               row.IsPaid = event.args.checked;
                if (event.args.checked) {
                    row.IsPaid = event.args.checked;
                    invoiceId = row.Id;
                    studProgramId = row.StudentProgramId;

                    $scope.WinPayFee.open();
                    $scope.$apply(function () {
                        showButton();
                        $scope.InvoiceGrdFee.updatebounddata();
                    });
                }
                else {
                    $scope.openConfirm('Change payment status?', 'Do you want to change payment status paid to unpaid invoice?', 500, 100, function (flag) {
                        if(flag){
                    $.ajax({
                        url: "/Finance/UpdatePaymentStatus",
                        type: "GET",
                        contentType: "application/json;",
                        dataType: "json",
                        data: { Id: row.Id, PaymentStatus: event.args.checked },
                        success: function (response) {
                            if (response.Status == 1) {
                                $scope.$apply(function () {
                                    showButton();
                                    $scope.InvoiceGrdFee.updatebounddata();
                                    $scope.GrdInvoice.updatebounddata();
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
                        else
                            $scope.InvoiceGrdFee.updatebounddata();
                            $scope.GrdInvoice.updatebounddata();
                    });
                }
            }
        });
    };

    $scope.InvoiceInitSwitchWidget = function (row, column, value, htmlElement) {
        $($(htmlElement).find('.switch-sel')).val(value);
    };

    $scope.ViewStudentInvoiceCellRenderer = function (row, column, value) {
        return "View";  
    }
    var rowdata = 0;
    invoiceId = 0;
    //$scope.SingleStudentInvoice = function (row, event) {
    //    rowdata = $scope.GrdInvoice.getrowdata(row);
    //    invoiceId = rowdata.Id;
    //    $('#hdnId').val(invoiceId);
    //    $('#frmReport').submit();
    //    $('#SwitchForPayementStatus').jqxSwitchButton({ onLabel: 'Paid', offLabel: 'Unpaid', theme: $scope.theme, width: '120' });
    //    $('#SwitchForPayementStatus').jqxSwitchButton('val',rowdata.IsPaid);
    //    $scope.WinStudentInvoice.open();
    //    $('#SwitchForPayementStatus').on('checked', function (event) {
    //        if (rowdata.IsPaid != event.args.checked) {
    //            rowdata.IsPaid = event.args.checked;
    //            if (event.args.checked) {
    //                rowdata.IsPaid = event.args.checked;
    //                $scope.$apply(function () {
    //                    $scope.WinPayFee.open();
    //                    $scope.GrdInvoice.updatebounddata();
    //                    $scope.InvoiceGrdFee.updatebounddata();
    //                    $scope.StudGrdInvoice.updatebounddata();
    //                });
    //            }
    //            else {
    //                $scope.openConfirm('Change payment status?', 'Do you want to change payment status paid to unpaid invoice?', 500, 100, function (flag) {
    //                    if (flag) {
    //                        $.ajax({
    //                            url: "/Finance/UpdatePaymentStatus",
    //                            type: "GET",
    //                            contentType: "application/json;",
    //                            cache: false,
    //                            dataType: "json",
    //                            data: { Id: invoiceId, PaymentStatus: event.args.checked },
    //                            success: function (response) {
    //                                if (response.Status == 1) {
    //                                    $('#hdnId').val(invoiceId);
    //                                    $('#frmReport').submit();
    //                                    $scope.$apply(function () {
    //                                        $scope.StudGrdInvoice.updatebounddata();
    //                                        $scope.GrdInvoice.updatebounddata();
    //                                        $scope.InvoiceGrdFee.updatebounddata();
    //                                    });
    //                                }
    //                                else {
    //                                    $scope.openMessageBox('Error', response.Message, 400, 100);
    //                                }
    //                            },
    //                            error: function (XMLHttpRequest, textStatus, errorThrown) {
    //                                alert('oops, something bad happened');
    //                            }
    //                        });
    //                    }
    //                    else
    //                    {
    //                        $scope.GrdInvoice.updatebounddata();
    //                        $scope.InvoiceGrdFee.updatebounddata();
    //                    }
                        
    //                });
    //            }
    //        }
    //    });

    //}
    
    var rowsData = 0;
    $scope.downloadCellRenderer = function (row, column, value) {
        return "View";
    };
    invoiceId = 0;
    //$scope.InvoiceByStudent = function (row) {
    //    rowsData = $scope.InvoiceGrdFee.getrowdata(row);
    //    invoiceId = rowsData.Id;
    //    $('#hdnId').val(invoiceId);
    //    $('#frmReport').submit();
    //    $('#SwitchForPayementStatus').jqxSwitchButton({ onLabel: 'Paid', offLabel: 'Unpaid', theme: $scope.theme, width: '120' });
    //    $('#SwitchForPayementStatus').jqxSwitchButton('val', rowsData.IsPaid);
    //    $scope.WinStudentInvoice.open();
    //    $('#SwitchForPayementStatus').on('change', function (event) {
    //        if (rowsData.IsPaid != event.args.checked) {
    //            rowsData.IsPaid = event.args.checked;
    //            if (event.args.checked) {
    //                rowsData.IsPaid = event.args.checked;
    //                $scope.WinPayFee.open();
    //                $scope.$apply(function () {
    //                     showButton();
    //                    $scope.InvoiceGrdFee.updatebounddata();
    //                    $scope.GrdInvoice.updatebounddata();
    //                    rowdata = 0;
    //                });
    //            }
    //            else {
    //                $scope.openConfirm('Change payment status?', 'Do you want to change payment status paid to unpaid invoice?', 500, 100, function (flag) {
    //                    if (flag) {
    //                        $.ajax({
    //                            url: "/Finance/UpdatePaymentStatus",
    //                            type: "GET",
    //                            cache: false,
    //                            contentType: "application/json;",
    //                            dataType: "json",
    //                            data: { Id: rowsData.Id, PaymentStatus: event.args.checked },
    //                            success: function (response) {
    //                                if (response.Status == 1) {
    //                                    $scope.$apply(function () {
    //                                        $('#hdnId').val(invoiceId);
    //                                        $('#frmReport').submit();
    //                                        showButton();
    //                                        $scope.StudGrdInvoice.updatebounddata();
    //                                        $scope.InvoiceGrdFee.updatebounddata();
    //                                        $scope.GrdInvoice.updatebounddata();
    //                                        rowdata = 0;
    //                                    });
    //                                }
    //                                else {
    //                                    $scope.openMessageBox('Error', response.Message, 400, 100);
    //                                }
    //                            },
    //                            error: function (XMLHttpRequest, textStatus, errorThrown) {
    //                                alert('oops, something bad happened');
    //                            }
    //                        });
    //                    }
    //                    else
    //                    {
    //                        $scope.StudGrdInvoice.updatebounddata();
    //                        $scope.GrdInvoice.updatebounddata();
    //                        $scope.InvoiceGrdFee.updatebounddata();
    //                    }
                           
    //                });
    //            }
    //        }
    //    });
        
    //}

    invoiceId = 0;
    $scope.InvoiceByStudent = function (row) {
        rowsData = $scope.InvoiceGrdFee.getrowdata(row);
        invoiceId = rowsData.Id;
        $('#hdnId').val(invoiceId);
        $('#frmReport').submit();
        if (rowsData.IsPaid == true) {
            $("#btnPaid").prop('checked', true);
        }
        else {
            $("#btnUnPaid").prop('checked', true);
        }
        $scope.WinStudentInvoice.open();
    }
    invoiceId = 0;
    $scope.SingleStudentInvoice = function (row, event) {   //for radio button click paid or unpaid
        rowdata = $scope.GrdInvoice.getrowdata(row);
        invoiceId = rowdata.Id;
        $('#hdnId').val(invoiceId);
        $('#frmReport').submit();
        if (rowdata.IsPaid == true) {
            $("#btnPaid").prop('checked', true);
        }
        else {
            $("#btnUnPaid").prop('checked', true);
        }
        $scope.WinStudentInvoice.open();
    }
    
    $scope.radioBtnStatus = function (value) {
        if (value == "Paid") {
            $scope.WinPayFee.open();
            $scope.GrdInvoice.updatebounddata();
            $scope.InvoiceGrdFee.updatebounddata();
            $scope.StudGrdInvoice.updatebounddata();
        }
        else {
            $scope.openConfirm('Change payment status?', 'Do you want to change payment status paid to unpaid invoice?', 500, 100, function (flag) {
                if (flag) {
                    $.ajax({
                        url: "/Finance/UpdatePaymentStatus",
                        type: "GET",
                        contentType: "application/json;",
                        cache: false,
                        dataType: "json",
                        data: { Id: invoiceId, PaymentStatus: false },
                        success: function (response) {
                            if (response.Status == 1) {
                                $('#hdnId').val(invoiceId);
                                $('#frmReport').submit();
                                $scope.$apply(function () {
                                    showButton();
                                    $scope.StudGrdInvoice.updatebounddata();
                                    $scope.GrdInvoice.updatebounddata();
                                    $scope.InvoiceGrdFee.updatebounddata();
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
                else {
                    $("#btnPaid").prop('checked', true);
                    $scope.GrdInvoice.updatebounddata();
                    $scope.InvoiceGrdFee.updatebounddata();
                }
            });
        }
    }
    
    $scope.OnWinPayFeeClose = function (event) {
        $("#btnUnPaid").prop('checked', true);
    };

    var ClearData=function(e)
    {
        $scope.ddlPaymentType.selectIndex(0);
        $scope.Account.BankName = null;
        $scope.Account.IFSCCode = null;
    }
   
});