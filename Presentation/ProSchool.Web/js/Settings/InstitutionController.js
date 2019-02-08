ProApp.controller("InstitutionController", function ($scope, $http) {
    var that = {};
    var isClassUpdate = false;
    var isSectionUpdate = false;
    $scope.ShowClassFilter = false;
    $scope.ShowSectionFilter = false;
    $scope.Batche = { Capacity: 1 };

    $scope.onBindingSubject = function (event) {
        if (typeof ($scope.Batche.SubjectId) == 'undefined')
            $scope.ddlSubject.val($scope.Batche.SubjectId);
        else
            $scope.Batche.SubjectId = parseInt($scope.ddlSubject.val());
    }

    $scope.SubjectRules = [{ input: '#txtName', message: 'Name is required!', action: 'blur', rule: 'required' },
    {
        input: '#txtName', message: 'Name already exists!', action: 'keyup,blur,valueChanged', rule: function (input, commit) {
            var name = '';
            if (typeof ($scope.Subject) != 'undefined')
                name = $scope.Subject.Name;
            if (name == '')
                return true;

            var isValidate = false;
            $.ajax({
                async: false,
                url: "/Settings/IsSubjectNameExists",
                type: "GET",
                contentType: "application/json;",
                dataType: "json",
                data: { name: name },
                success: function (response) {
                    if (response.Status == 1) {
                        isValidate = !response.Data;
                    }
                    else {
                        $scope.openMessageBox('Error', respond.Message, 400, 100);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('oops, something bad happened');
                }
            });
            commit(isValidate);
            if (!isValidate)
                return false;
        }
    }];

    $scope.BatcheRules = [

        { input: '#txtBatchName', message: 'Name is required!', action: 'blur', rule: 'required' },
        //{
        //    input: '#txtsub', message: 'Subject is required!', action: 'blur', rule: function (input, commit) {
        //        var index = $scope.ddlSubject.getSelectedIndex();
        //        return index != -1;
        //    }
        //},
        {
            input: '#txtBatchName', message: 'Name already exists!', action: 'keyup,blur,valueChanged', rule: function (input, commit) {
                var name = '';
                if (typeof ($scope.Batche) != 'undefined')
                    name = $scope.Batche.Name;
                if (name == '')
                    return true;

                var IsValidate = false;
                $.ajax({
                    async: false,
                    url: "/Settings/IsBatcheNameExists",
                    type: "GET",
                    contentType: "application/json;",
                    dataType: "json",
                    data: { name: name },
                    success: function (response) {
                        if (response.Status == 1) {
                            IsValidate = !response.Data;
                        }
                        else {
                            $scope.openMessageBox('Error', respond.Message, 400, 100);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('oops, something bad happened');
                    }
                });
                commit(IsValidate);
                if (!IsValidate)
                    return false;
            }
        }
    ];

    $scope.ClassSource = {
        //localdata: [{ Id: 1, Name: 'Keyboard', Description: 'test' }, { Id: 2, Name: 'Vocal', Description: 'test' }],
        url: '/Settings/GetAllSubjects',
        datatype: "json",
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.
            //alert('U-' + rowid);
            commit(isClassUpdate);
        },
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.ClassEditCellRenderer = function (row, column, value) {
        if (row === that.classeditrow) {
            return "<div class='text-center pad' style='width:100%'><a ng-click='ClassUpdate(" + row + ", event)' class='fa fa-check fa-2' href='javascript:;'></a>&nbsp;&nbsp;&nbsp;<a ng-click='ClassCancel(" + row + ", event)' class='fa fa-ban fa-2' href='javascript:;'></a></div>";
        }

        return "<div class='text-center pad' style='width:100%;'><a ng-click='ClassEdit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    $scope.ClassDeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='ClassDelete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }

    $scope.ClassEdit = function (row, event) {
        that.classeditrow = row;
        isClassUpdate = false;
        $scope.GrdClass.beginrowedit(row);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        return false;
    }

    $scope.ClassDelete = function (row, event) {
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                var dataRecord = $scope.GrdClass.getrowdata(row);
                $.post("/Settings/DeleteSubject", { Id: dataRecord.Id }, function (response) {
                    if (response.Status == 1) {
                        $scope.OnRefreshClass();
                        $scope.openMessageBox('Success', response.Message, 400, 100);
                    }
                    else if(response.Status == 3){
                        $scope.openMessageBox('Warning', response.Message, 400, 100);
                    }
                    else {
                        //$scope.openMessageBox('Error', respond.Message, 400, 100);
                        $scope.openMessageBox("Error", "You can't delete this subject because student is already register with subject ", 510, 100);
                    }
                });
            }
        });
    }

    $scope.ClassUpdate = function (row, event) {
        var dataRecord = $scope.GrdClass.getrowdata(row);
        that.classeditrow = -1;
        isClassUpdate = true;
        $scope.GrdClass.endrowedit(row, false);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        $.ajax({
            url: "/Settings/UpdateSubject",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { Id: dataRecord.Id, name: dataRecord.Name, desc: dataRecord.Description },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.GrdClass.updatebounddata();
                }
                 else if (response.Status == 3) {
                     $scope.GrdClass.updatebounddata();
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

        // return false;
    }

    $scope.ClassCancel = function (row, event) {
        that.classeditrow = -1;
        isClassUpdate = false;
        $scope.GrdClass.endrowedit(row, true);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        $scope.GrdClass.refresh();
        return false;
    }

    $scope.OnClassCellEndEdit = function (event) {
        //alert(that.classeditrow);
        if (that.classeditrow > -1) {
            $scope.GrdClass.endrowedit(that.classeditrow, true);
            that.classeditrow = -1;
            $scope.GrdClass.refresh();
        }
    }

    $scope.OnRefreshClass = function () {
        $scope.GrdClass.endrowedit(that.classeditrow, true);
        that.classeditrow = -1;
        $scope.GrdClass.refresh();
        $scope.GrdClass.updatebounddata();
    }

    $scope.OnAddNewClass = function () {
        ClearData();
        $scope.SubjectValidator.hide();
        $scope.WinAddNewClass.open();
    }

    $scope.OnFilterClassClick = function () {
        $scope.ShowClassFilter = !$scope.ShowClassFilter;
    }

    $scope.SectionSource = {
        // localdata: [{ Id: 1, Name: 'Mon-Wed-Fri', Subject: 'Keyboard', Capacity: 5, Description: 'test' }, { Id: 2, Name: 'Tue-Thu-Sat', Subject: 'Vocal', Capacity: 5, Description: 'test' }],
        url: '/Settings/GetAllBatche',
        datatype: "json",
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.
            //alert('U-' + rowid);
            commit(isSectionUpdate);
        },
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'SubjectName', type: 'string' },
            { name: 'Capacity', type: 'number' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.SectionEditCellRenderer = function (row, column, value) {
        if (row === that.sectioneditrow) {
            return "<div class='text-center pad' style='width:100%'><a ng-click='SectionUpdate(" + row + ", event)' class='fa fa-check fa-2' href='javascript:;'></a>&nbsp;&nbsp;&nbsp;<a ng-click='SectionCancel(" + row + ", event)' class='fa fa-ban fa-2' href='javascript:;'></a></div>";
        }

        return "<div class='text-center pad' style='width:100%;'><a ng-click='SectionEdit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    $scope.SectionDeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='SectionDelete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }

    $scope.SectionEdit = function (row, event) {
        that.sectioneditrow = row;
        isSectionUpdate = false;
        $scope.GrdSection.beginrowedit(row);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        return false;
    }

    $scope.SectionDelete = function (row, event) {
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                var dataRecord = $scope.GrdSection.getrowdata(row);
                $.post("/Settings/DeleteBatche", { Id: dataRecord.Id }, function (response) {
                    if (response.Status == 1) {
                        $scope.OnRefreshSection();
                        $scope.openMessageBox('Success', response.Message, 400, 100);
                    }
                    else if (response.Status == 3) {
                        $scope.openMessageBox('Warning', response.Message, 400, 100);
                    }
                    else {
                        //$scope.openMessageBox('Error', respond.Message, 400, 100);
                        $scope.openMessageBox("Error", "You can't delete this batch because, student is already register with this batch", 520, 100);
                    }
                });
            }
        });
    }

    $scope.SectionUpdate = function (row, event) {
        var dataRecord = $scope.GrdSection.getrowdata(row);
        that.sectioneditrow = -1;
        isSectionUpdate = true;
        $scope.GrdSection.endrowedit(row, false);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
       
        $.ajax({
            url: "/Settings/UpdateBatche",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { Id: dataRecord.Id, name: dataRecord.Name, capacity: dataRecord.Capacity, desc: dataRecord.Description },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.GrdSection.updatebounddata();
                }
                else if (response.Status == 3) {
                    $scope.GrdSection.updatebounddata();
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
        //return false;
    }

    $scope.SectionCancel = function (row, event) {
        that.sectioneditrow = -1;
        isSectionUpdate = false;
        $scope.GrdSection.endrowedit(row, true);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        $scope.GrdSection.refresh();
        return false;
    }

    $scope.OnSectionCellEndEdit = function (event) {
        //alert(that.sectioneditrow);
        if (that.sectioneditrow > -1) {
            $scope.GrdSection.endrowedit(that.sectioneditrow, true);
            that.sectioneditrow = -1;
            $scope.GrdSection.refresh();
        }
    }

    $scope.OnRefreshSection = function () {
        $scope.GrdSection.endrowedit(that.sectioneditrow, true);
        that.sectioneditrow = -1;
        $scope.GrdSection.refresh();
        $scope.GrdSection.updatebounddata();
    }

    $scope.OnAddNewSection = function () {
        ClearData();
        $scope.BatcheValidator.hide();
        $scope.WinAddNewSection.open();
    }

    $scope.OnFilterSectionClick = function () {
        $scope.ShowSectionFilter = !$scope.ShowSectionFilter;
    }

    $scope.CapacityValidation = function (cell, value) {
        if (!$.isNumeric(value) || parseInt(value) <= 0) {
            return { result: false, message: "Capacity should be positive value." };
        }
        return true;
    };

    $scope.CapacityCreateEditor = function (row, cellvalue, editor) {
        editor.jqxNumberInput({ decimalDigits: 0, digits: 3, theme: $scope.theme, spinButtons: true });
    };

    $scope.OnSaveSubject = function () {
        var IsValidate = $scope.SubjectValidator.validate()
        if (!IsValidate)
            return;
        $.post("/Settings/InsertSubject", { subject: $scope.Subject }, function (response) {
            if (response.Status == 1) {
                $scope.WinAddNewClass.close();
                $scope.Subject.Name = '';
                $scope.Subject.Description = '';
                $scope.OnRefreshClass();
                $scope.GrdSection.updatebounddata();
            }
            else {
                $scope.openMessageBox('Error', respond.Message, 400, 100);
            }
        });
    }

    $scope.OnSaveBatche = function () {
        var IsValidate = $scope.BatcheValidator.validate()
        if (!IsValidate)
            return;
        $scope.Batche.SubjectId = parseInt($scope.ddlSubject.val());
        $.post("/Settings/InsertBatche", { batche: $scope.Batche }, function (response) {
            if (response.Status == 1) {
                $scope.WinAddNewSection.close();
                $scope.Batche.Name = '';
                $scope.ddlSubject.selectIndex(0);
                $scope.Batche.Capacity = 1;
                $scope.Batche.Description = '';
                $scope.OnRefreshSection();
            }
            else {
                $scope.openMessageBox('Error', response.Message, 600, 400);
            }
        });
    }

    var ClearData = function (e) {
        if (typeof ($scope.Subject) != 'undefined' && typeof ($scope.Batche) != 'undefined') {
            $scope.Subject.Name = null;
            $scope.Batche.Name = null;
        }
    }
});