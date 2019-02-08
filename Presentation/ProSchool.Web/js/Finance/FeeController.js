ProApp.controller("FeeController", function ($scope, $http) {
    $scope.ShowFilter = false;

    $scope.Rules = [
                      { input: '#txtFeeName', message: 'Fee name is required!', action: 'keyup,blur', rule: 'required' },
                      { input: '#txtAmount', message: 'Fee amount is required!', action: 'keyup,blur', rule: 'required' },
                      {
                          input: '#txtFeeName', message: 'Name already exists!', action: 'keyup,blur,valueChanged', rule: function (input, commit) {
                              var name = '';
                              if (typeof ($scope.Fee) != 'undefined')
                                  name = $scope.Fee.Name;
                              if (name == '')
                                  return true;

                              var isValidate = false;
                              $.ajax({
                                  async: false,
                                  url: "/Finance/IsFeeNameExists",
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
                      }
    ];

    $scope.OnFilterClick = function () {
        $scope.ShowFilter = !$scope.ShowFilter;
    }

    $scope.OnAddNew = function () {
        $scope.FeeValidator.hide();
        $scope.WinAddNew.open();
    }

    $scope.OnRefresh = function () {
        $scope.GrdFee.updatebounddata();
    }

    $scope.FrequencySource = {
        datatype: "json",
        datafields: [
            { name: 'Id', type: 'int' },
            { name: 'Description', type: 'string' }
        ],
        url: '/Finance/GetAllFrequency'
    };

    $scope.FeeSource = {
        //localdata: [{ Id: 1, Name: 'Quarterly Fees', Frequency: 'Quarterly', Amount: 6000 }],
        url: '/Finance/GetAllFee',
        datatype: "json",
        datafields:
        [
            { name: 'Id', type: 'int' },
            { name: 'Name', type: 'string' },
            { name: 'FrequencyName', type: 'string' },
            { name: 'Amount', type: 'int' }
        ]
    };

    $scope.OnSaveFee = function () {

        var IsValidate = $scope.FeeValidator.validate();
        if (!IsValidate)
            return;

        var Fee = $scope.Fee;
        $scope.Fee.Frequency = parseInt($scope.ddlFrequency.val());
        $.ajax({
            url: "/Finance/SaveFee",
            type: "POST",
            //contentType: "application/json;",
            dataType: "json",
            data: { fee:Fee },
            success: function (response) {
                if (response.Status == 1) {
                    $scope.WinAddNew.close();
                    $scope.GrdFee.updatebounddata();
                    Fee = [];
                    ClearData();
                }
                else {
                    $scope.openMessageBox('Error', respond.Message, 400, 100);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }
    var ClearData = function (e) {
        if (typeof ($scope.Fee) != 'undefined') {
            $scope.Fee.Id = null;
            $scope.Fee.Name = null;
            $scope.Fee.Amount = null;
            $scope.ddlFrequency.selectIndex(0);
        }
    }

    $scope.EditCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Edit(" + row + ", event)' class='fa fa-pencil fa-2' href='javascript:;'></a></div>";
    }

    $scope.DeleteCellRenderer = function (row, column, value) {
        return "<div class='text-center pad' style='width:100%;'><a ng-click='Delete(" + row + ", event)' class='fa fa-trash fa-2' href='javascript:;'></a></div>";
    }

    $scope.Edit = function (row, event) {
        var dataRecord = $scope.GrdFee.getrowdata(row);
        $scope.FeeValidator.hide();
        $scope.WinAddNew.open();
        $.ajax({
            url: "/Finance/GetFeeById",
            type: "GET",
            contentType: "application/json;",
            dataType: "json",
            data: { Id: dataRecord.Id },
            success: function (data) {
                $scope.$apply(function () {
                    $scope.Fee = data;
                });
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('oops, something bad happened');
            }
        });
    }

    $scope.Delete = function (row, event) {
        var dataRecord = $scope.GrdFee.getrowdata(row);
        $scope.openConfirm('Confirm delete?', 'Are you sure want to delete this item?', 300, 100, function (flag) {
            if (flag) {
                
                $.ajax({
                    url: "/Finance/DeleteFee",
                    type: "GET",
                    dataType: "json",
                    data: { Id: dataRecord.Id },
                    success: function (response) {
                        if (response.Status == 1) {
                            $scope.GrdFee.updatebounddata();
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
});