var that = {};
ProApp.controller("InfraController", function ($scope, $http) {
    $scope.ShowFilter = false;

    $scope.Rules = [
                       { input: '#txtName', message: 'Name is required!', action: 'keyup,blur', rule: 'required' },
                       {
                           input: '#txtName', message: 'Name already exists!', action: 'keyup,blur,valueChanged', rule: function (input, commit) {
                               var name = '';
                               if (typeof ($scope.Division) != 'undefined')
                                   name = $scope.Division.Name;
                               if (name == '')
                                   return true;

                               var isValidate = false;
                               $.ajax({
                                   async: false,
                                   url: "/Settings/IsDivisionNameExists",
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
                               //$.ajaxSetup({ async: false });
                               //$.get("/Settings/IsDivisionNameExists", { name: name }, function (response) {
                               //    if (response.Status == 1) {
                               //        commit(!response.Data);
                               //    }
                               //    else {
                               //        $scope.openMessageBox('Error', respond.Message, 400, 100);
                               //        commit(false);
                               //    }
                               //});
                               //$.ajaxSetup({ async: true });
                               // call commit with false, when you are doing server validation and you want to display a validation error on this field. 
                               //return result;
                           }
                       }
    ];


    $scope.DepartmentSource ={
        //localdata: [{ Id: 1, Name: 'Rock & Pop', Description: 'test' }, { Id: 2, Name: 'Classical', Description: 'test' }],
        url: '/Settings/GetAllDivisions',
        type: 'GET',
        datatype: "json",
        updaterow: function (rowid, rowdata, commit) {
            // synchronize with the server - send update command
            // call commit with parameter true if the synchronization with the server is successful 
            // and with parameter false if the synchronization failder.
            alert(JSON.parse(rowdata));
            commit(isUpdate);
        },
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'Description', type: 'string' }
        ]
    };

    $scope.DepartmentDA = new $.jqx.dataAdapter($scope.DepartmentSource, { loadComplete: function (records) { }, loadError: function (jqXHR, status, error) { } });

    $scope.EditCellRenderer = function (row, column, value) {
        if (row === that.editrow) {
            return "<div class='text-center pad' style='width:100%'><a ng-click='Update(" + row + ", event)' class='fa fa-check fa-2' href='javascript:;'></a>&nbsp;&nbsp;&nbsp;<a ng-click='Cancel(" + row + ", event)' class='fa fa-ban fa-2' href='javascript:;'></a></div>";
        }

        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }

    $scope.Edit = function (row, event) {
        that.editrow = row;
        isUpdate = false;
        $scope.GrdDivision.beginrowedit(row);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        return false;
    }

    $scope.Delete = function (row, event) {
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                var dataRecord = $scope.GrdDivision.getrowdata(row);
                $.post("/Settings/DeleteDivision", { Id: dataRecord.Id }, function (response) {
                    if (response.Status == 1) {
                        $scope.OnRefresh();
                        $scope.openMessageBox('Success', response.Message, 400, 100);
                    }
                    else if(response.Status == 3){
                        $scope.openMessageBox('Warning', response.Message, 400, 100);
                    }
                    else {
                        //$scope.openMessageBox('Error', respond.Message, 400, 100);
                        $scope.openMessageBox("Error", "You can't delete this division because this division is already used by student",510,100);
                    }
                });
            }
        });
    }

    var isUpdate = false;
    $scope.Update = function (row, event) {
        var dataRecord = $scope.GrdDivision.getrowdata(row);
        that.editrow = -1;
        isUpdate = true;
        $scope.GrdDivision.endrowedit(row, false);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }

        $.ajax({
            url: "/Settings/UpdateDivision",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { Id: dataRecord.Id, name: dataRecord.Name, desc: dataRecord.Description },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.GrdDivision.updatebounddata();
                }
                else if (response.Status == 3) {
                    $scope.GrdDivision.updatebounddata();
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

    $scope.Cancel = function (row, event) {
        that.editrow = -1;
        isUpdate = false;
        $scope.GrdDivision.endrowedit(row, true);
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        $scope.GrdDivision.refresh();
        return false;
    }

    $scope.OnCellEndEdit = function (event) {
        //alert('E-' + that.editrow);
        if (that.editrow > -1) {
            $scope.GrdDivision.endrowedit(that.editrow, true);
            that.editrow = -1;
            $scope.GrdDivision.refresh();
        }
    }

    $scope.OnRefresh = function () {
        $scope.GrdDivision.endrowedit(that.editrow, true);
        that.editrow = -1;
        $scope.GrdDivision.updatebounddata();
    }

    $scope.OnAddNew = function () {
        ClearData();
        $scope.DivisionValidator.hide();
        $scope.WinAddNew.open();
    }

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.handleKeys = function (event) {
        var key = event.charCode ? event.charCode : event.keyCode ? event.keyCode : 0;
        //$scope.GrdDivision.endrowedit(that.editrow, true);
        //that.editrow = -1;
        //$scope.GrdDivision.refresh();
        if (event) {
            if (event.preventDefault) {
                event.preventDefault();
            }
        }
        return false;
    }

    $scope.OnSaveDivision = function () {
        var IsValidate = $scope.DivisionValidator.validate()
        if (!IsValidate)
            return;
        $.post("/Settings/InsertDivision", { division: $scope.Division }, function (response) {
            if (response.Status == 1) {
                $scope.WinAddNew.close();
                $scope.Division.Name = '';
                $scope.Division.Description = '';
                $scope.OnRefresh();
            }
            else {
                $scope.openMessageBox('Error', respond.Message, 400, 100);
            }
        });
    }
    var ClearData = function (e) {
        if (typeof ($scope.Division) != 'undefined') {
            $scope.Division.Name = null;
           
        }
    }
});
